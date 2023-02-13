using KhanVidBot.Modals;
using KhanVidBot.Properties;
using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace KhanVidBot;

internal partial class KhanClient : IDisposable
{
    private readonly HttpClient _client;

    private string? _kaVer, _xsrfToken;
    private readonly string _coachKaid, _studentListId;

    private const string F_PROXY = "http://localhost:8888";

    public KhanClient(
        string url,
        string cookies,
        bool debug = false
        )
    {
        (_coachKaid, _studentListId) = ExtractDataFromUrlUnsafe(url);

        if (
            string.IsNullOrEmpty(_coachKaid) ||
            string.IsNullOrEmpty(_studentListId)
            ) throw new Exception("Failed parse data from the url!");

        var handler = new HttpClientHandler
        {
            UseCookies = false,
            Proxy = debug ? new WebProxy(F_PROXY) : null
        };

        _client = new(
             handler,
             true
             );

        _client.DefaultRequestHeaders.Add(
            "User-Agent",
            Resources.UserAgent
            );

        if (GenOrExtractXSRFToken(cookies))
        {
            if (!cookies.EndsWith(';'))
                cookies += "; ";

            cookies += "fkey=" + _xsrfToken;
        }

        _client.DefaultRequestHeaders.Add(
            "x-ka-fkey",
             _xsrfToken
            );

        _client.DefaultRequestHeaders.Add(
            "Cookie",
            cookies
            );
    }

    public async Task<VideoItemProgress?> ReportProgress(
        AssignmentContent assignment,
        int secWatched = 0,
        int lastSecWatched = 0
        )
    {
        const string op = "updateUserVideoProgress";

        var queryBody = new QueryBody
        {
            OperationName = op,
            Variables = new QueryVariablesV2
            {
                Input = new()
                {
                    ContentId = assignment.ContentId,
                    SecondsWatched = secWatched,
                    LastSecondWatched = lastSecWatched,
                    DurationSeconds = assignment.Duration
                }
            },
            Query = Resources.UpdateUserVideoProgress,
        };

        var res = await _client.PostAsJsonAsync(
            GenGqlUrl(op, true),
            queryBody
            );

        if (!res.IsSuccessStatusCode)
            return null;

        var content = res.Content;
        var str = await content.ReadAsStringAsync();

        if (str.Contains("CHEATING"))
        {
            await Task.Delay(10 * 1000);
            return null;
        }

        try
        {
            var json = await content.ReadFromJsonAsync<VideoProgressResp>();
            return json?.Data?.UpdateUserVideoProgress?.VideoItemProgress;
        }
        catch { return null; }
    }

    public async Task<AssignmentContent[]?> GetAssignments()
    {
        const string op = "UserAssignments";

        var queryBody = new QueryBody
        {
            OperationName = op,
            Query = Resources.UserAssignmentsQuery,
            Variables = new QueryVariables
            {
                StudentListId = _studentListId,
                CoachKaid = _coachKaid
            }
        };

        var res = await _client.PostAsJsonAsync(
            GenGqlUrl(op),
            queryBody
            );

        if (!res.IsSuccessStatusCode)
            return null;

        var json = await res.Content.ReadFromJsonAsync<AssignmentsPageResp>();
        var assignments = json?.Data?.User?.AssignmentsPage?.Assignments;

        if (assignments == null)
            return null;

        var result = new List<AssignmentContent>();

        foreach (var i in assignments)
        {
            var contents = i.Contents;
            var comStates = i.TotalCompletionStates;

            if (contents == null || comStates == null)
                continue;

            for (int j = 0; j < contents.Length; j++)
            {
                var content = contents[j];

                if (
                    content == null ||
                    content.Kind != "Video"
                    ) continue;


                switch (comStates[j].State)
                {
                    case "completed":
                        continue;

                    case "unstarted":
                    default:
                        break;
                }

                result.Add(content);
            }
        }

        return
            result.Count == 0 ?
            null :
            result.ToArray();
    }

    public async Task<bool> TryGetKAStaticVersion()
    {
        var res = await _client.GetAsync(
            "https://khanacademy.org/profile/me/assignments/teacher/" +
            $"{_coachKaid}/class/{_studentListId}"
            );

        if (!res.IsSuccessStatusCode)
            return false;

        string? str = await res.Content.ReadAsStringAsync();
        _kaVer = KAStaticRegex().Match(str).Groups[1].Value;

        return !string.IsNullOrEmpty(_kaVer);
    }

    private string GenGqlUrl(string path, bool mt = false)
    {
        if (string.IsNullOrEmpty(_kaVer))
            throw new Exception("Client not initialized!");

        const string url = "https://www.khanacademy.org/api/internal/";

        return
            url + (mt ? "_mt/" : "") +
            $"graphql/{path}?lang=en&_={_kaVer}_{Funcs.GetTs()}";
    }

    private bool GenOrExtractXSRFToken(string cookie)
    {
        bool GenRet()
        {
            _xsrfToken = GenXSRFToken();
            return true;
        }

        if (!FKeyRegex().IsMatch(cookie))
            return GenRet();

        var matches = FKeyRegex().Matches(cookie);

        foreach (Match i in matches.Cast<Match>())
            if (i.Success)
            {
                _xsrfToken = i.Groups[1].Value;
                return false;
            }

        return GenRet();
    }

    private static string GenXSRFToken()
    {
        static uint Rnd32()
        {
            var rnd = new Random();
            return (uint)rnd.Next(1 << 30) << 2 | (uint)rnd.Next(1 << 2);
        }

        string token = string.Empty;

        for (int i = 0; i < 10; i++)
            token += Funcs.UintToStr(Rnd32());

        return $"1.0_{token}_{Funcs.GetTs()}";
    }

    private static (string, string) ExtractDataFromUrlUnsafe(string? input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input));
        else if (
            !input.Contains("http") ||
            !input.Contains("khanacademy.org")
            ) throw new Exception("Invalid link!");

        string[]? data = input[
            input.IndexOf("kaid_")..
            ].Split("/class/");

        return (data[0], data[1]);
    }

    public void Dispose()
    {
        _client.CancelPendingRequests();
        _client.Dispose();
    }

    [GeneratedRegex("X-KA-Static-Version\":\"(.*?)\",")]
    private static partial Regex KAStaticRegex();

    [GeneratedRegex("fkey=(.*?);")]
    private static partial Regex FKeyRegex();
}