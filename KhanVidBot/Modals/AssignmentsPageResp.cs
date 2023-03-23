using System.Text.Json.Serialization;

namespace KhanVidBot.Modals;

internal class AssignmentsPageResp
{
    [JsonPropertyName("data")]
    public AssignmentsPageData? Data { get; set; }
}

internal class AssignmentsPageData
{
    [JsonPropertyName("user")]
    public AssignmentsPageUser? User { get; set; }
}

internal class AssignmentsPageUser
{
    [JsonPropertyName("assignmentsPage")]
    public AssignmentsPage? AssignmentsPage { get; set; }
}

internal class AssignmentsPage
{
    [JsonPropertyName("assignments")]
    public Assignment[]? Assignments { get; set; }

    [JsonPropertyName("pageInfo")]
    public PageInfo? PageInfo { get; set; }
}

internal class PageInfo
{
    [JsonPropertyName("nextCursor")]
    public string? NextCursor { get; set; }
}

internal class Assignment
{
    [JsonPropertyName("__typename")]
    public string? Typename { get; set; }

    [JsonPropertyName("assignedDate")]
    public DateTime AssignedDate { get; set; }

    [JsonPropertyName("contents")]
    public AssignmentContent[]? Contents { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime DueDate { get; set; }

    [JsonPropertyName("totalCompletionStates")]
    public TotalCompletionState[]? TotalCompletionStates { get; set; }
}

internal class AssignmentContent
{
    [JsonPropertyName("contentId")]
    public string? ContentId { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("kind")]
    public string? Kind { get; set; }
}

internal class TotalCompletionState
{
    [JsonPropertyName("state")]
    public string? State { get; set; }
}