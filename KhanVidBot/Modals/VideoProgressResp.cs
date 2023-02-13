using System.Text.Json.Serialization;

namespace KhanVidBot.Modals;

internal class VideoProgressResp
{
    [JsonPropertyName("data")]
    public ProgressRespData? Data { get; set; }
}

internal class ProgressRespData
{
    [JsonPropertyName("updateUserVideoProgress")]
    public UpdateUserVideoProgress? UpdateUserVideoProgress { get; set; }
}

internal class UpdateUserVideoProgress
{
    [JsonPropertyName("videoItemProgress")]
    public VideoItemProgress? VideoItemProgress { get; set; }
}

internal class VideoItemProgress
{
    [JsonPropertyName("completed")]
    public bool Completed { get; set; }

    [JsonPropertyName("lastSecondWatched")]
    public int LastSecondWatched { get; set; }

    [JsonPropertyName("secondsWatched")]
    public int SecondsWatched { get; set; }
}