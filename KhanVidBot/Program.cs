using KhanVidBot.ConsoleUI;
using KhanVidBot.Modals;
using System.Drawing;
using Console = Colorful.Console;

namespace KhanVidBot;

internal partial class Program
{
    private static void Main(string[] args)
    {
        try { MainAsync(args).Wait(); }
        catch (Exception ex) { Logger.Error(ex); }
        finally { Exit(); }
    }

    private static async Task MainAsync(string[] args)
    {
        Console.Title = "Khan Academy Video Bot - github.com/Plot1337/KhanVidBot";

        bool debugProxy = false;

        if (args.Length > 0)
            foreach (string arg in args)
                switch (arg)
                {
                    case "-d":
                        debugProxy = true;
                        Logger.Warn("Using debug proxy!");
                        break;
                }

        using var client = new KhanClient(
            GetUrl(),
            GetCookies(),
            debugProxy
            );

        Logger.WriteTitle();
        Logger.Output("Press any key to start...");

        Console.ReadKey(true);
        Logger.WriteTitle();

        Logger.Info("Starting...");

        if (!await client.TryGetKAStaticVersion())
        {
            Logger.Error("Something went wrong! Please try again.");
            return;
        }

        Logger.Info("Fetching assignments...");

        var assignments = await client.GetAssignments();

        if (assignments == null || assignments.Length == 0)
        {
            Logger.Warn("No assignments found!");
            Logger.Info(
                "If you think this was a mistake, perhaps your cookies are invalid?"
                );

            return;
        }

        Logger.Output($"Found {assignments.Length} assignments!...");

        foreach (var assignment in assignments)
            await TryWatchVid(client, assignment);
    }

    private static async Task TryWatchVid(
       KhanClient client,
       AssignmentContent assignment
       )
    {
        if (assignment == null)
            return;

        for (int i = 0; i < 3; i++)
        {
            try
            {
                if (
                    await WatchVid(client, assignment)
                    ) break;
            }
            catch (Exception ex) { Logger.Error(ex); }
        }
    }

    private static async Task<bool> WatchVid(
        KhanClient client,
        AssignmentContent assignment
        )
    {
        Logger.Info("Watching: " + assignment.Title);

        var progress = await client.ReportProgress(assignment);

        if (progress == null)
            return false;
        else if (progress.Completed)
        {
            Logger.Warn(
                "Assignment already completed!",
                false
                );

            return true;
        }

        int secWatched = 25;

        while (!progress?.Completed ?? true)
        {
            await Task.Delay(3 * 1000);

            int lsw = progress?.LastSecondWatched ?? (secWatched + 1);

            if (secWatched > lsw)
                secWatched = lsw / 2;

            progress = await client.ReportProgress(
                assignment,
                secWatched,
                (lsw > 0 ? lsw : 1) + 25
                );

            if (progress == null)
                continue;
            else if (progress.Completed)
                break;
            else secWatched += 50;
        }

        Logger.Output("Done watching: " + assignment.Title);
        await Task.Delay(5 * 1000);
        return true;
    }

    private static string GetUrl()
    {
        while (true)
        {
            Logger.WriteTitle();
            string? url = Logger.InputStr("URL");

            if (string.IsNullOrEmpty(url))
            {
                Logger.Error("Invalid/null input!");
                continue;
            }
            else if (
                !url.Contains("khanacademy.org") ||
                !url.Contains("kaid_")
                )
            {
                Logger.Error("URL does not match syntax!");
                continue;
            }

            return url;
        }
    }

    private static string GetCookies()
    {
        while (true)
        {
            Logger.WriteTitle();
            string? val = Logger.InputStr("Cookies");

            if (string.IsNullOrEmpty(val))
            {
                Logger.Error("Invalid/null input!");
                continue;
            }

            return val;
        }
    }

    public static void Exit(int exitCode = 0)
    {
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("  --", Color.Silver);

        Logger.Output("Press any key to exit...");

        Console.ReadKey(true);
        Console.ResetColor();

        Environment.Exit(exitCode);
    }

    public static bool IsDebug()
    {
#if DEBUG
        return true;
#else
            return false;
#endif
    }
}