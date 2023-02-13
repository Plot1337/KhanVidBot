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
    public DateTime? DueAfter { get; } = DateTime.UtcNow;

    [JsonPropertyName("orderBy")]
    public string OrderBy { get; } = "DUE_DATE_ASC";

    [JsonPropertyName("dueBefore")]
    public DateTime? DueBefore { get; } = null;
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
