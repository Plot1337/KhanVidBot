using System.Text.Json.Serialization;

namespace KhanVidBot.Modals;

internal class QueryBody
{
    [JsonPropertyName("operationName")]
    public string? OperationName { get; set; }

    [JsonPropertyName("variables")]
    public object? Variables { get; set; }

    [JsonPropertyName("query")]
    public string? Query { get; set; }
}

internal class QueryVariables
{
    [JsonPropertyName("after")]
    public object? After { get; } = null;

    [JsonPropertyName("pageSize")]
    public int PageSize { get; } = 100;

    [JsonPropertyName("studentListId")]
    public string? StudentListId { get; set; }

    [JsonPropertyName("coachKaid")]
    public string? CoachKaid { get; set; }

    [JsonPropertyName("dueAfter")]
    public DateTime? DueAfter { get; set; } = null;

    [JsonPropertyName("orderBy")]
    public string OrderBy { get; set; }

    [JsonPropertyName("dueBefore")]
    public DateTime? DueBefore { get; set; } = null;
}

internal class QueryVariablesV2
{
    [JsonPropertyName("input")]
    public VarInput? Input { get; set; }
}

internal class VarInput
{
    [JsonPropertyName("contentId")]
    public string? ContentId { get; set; }

    [JsonPropertyName("secondsWatched")]
    public double SecondsWatched { get; set; }

    [JsonPropertyName("lastSecondWatched")]
    public double LastSecondWatched { get; set; }

    [JsonPropertyName("durationSeconds")]
    public int DurationSeconds { get; set; }

    [JsonPropertyName("captionsLocale")]
    public string CaptionsLocale { get; } = string.Empty;

    [JsonPropertyName("fallbackPlayer")]
    public bool FallbackPlayer { get; } = false;

    [JsonPropertyName("localTimezoneOffsetSeconds")]
    public int LocalTimezoneOffsetSeconds { get; } = 36000;

    [JsonPropertyName("gaReferrer")]
    public string? GaReferrer { get; } = string.Empty;
}

// fkey=1.0_1sbcic1s1j7nb1mqkcah2rd9t963p1j9gl244r1qc37rqoj41v378ko101klrh3mpus0k_1673948601671; G_ENABLED_IDPS=google; G_AUTHUSER_H=1; GOOGAPPUID=x; KAAC=$DRt3xdJPRx9pVYgTdJdUXty9lIWrXpyobtQfhoYOuY4.~rrpnlp$a2FpZF82ODMxMTA3MDQ5NDI4NzQ1NjIxODA5MjM*$a2FpZF82ODMxMTA3MDQ5NDI4NzQ1NjIxODA5MjM!0!0!0~2
// fkey=1.0_1sbcic1s1j7nb1mqkcah2rd9t963p1j9gl244r1qc37rqoj41v378ko101klrh3mpus0k_1673948601671; G_ENABLED_IDPS=google; G_AUTHUSER_H=1; KAAS=NLvffyIB0hslRbZ7a3i_uw; GOOGAPPUID=x; KAAL=$F3iaJvKgVKFiGikW2QQmQgLifBNB-t-dBFmenTvIARs.~rrpno7$a2FpZF82ODMxMTA3MDQ5NDI4NzQ1NjIxODA5MjM*; KAAC=$COCk2-y0ceVF-9P9TMM-Dae3tTe0F8TuGjFk9y7lHRc.~rrpno7$a2FpZF82ODMxMTA3MDQ5NDI4NzQ1NjIxODA5MjM*$a2FpZF82ODMxMTA3MDQ5NDI4NzQ1NjIxODA5MjM!0!0!0~2