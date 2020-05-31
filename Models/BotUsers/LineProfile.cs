using isRock.LineBot;

public abstract class LineProfile {
    protected Source _src { get; set; }
    protected string _token { get; set; }

    public LineProfile(Source eventSource, string channelAccessToken) {
        _src   = eventSource;
        _token = channelAccessToken;
    }

    public abstract LineUserInfo GetUserProfile();

    public abstract string Leave();
}