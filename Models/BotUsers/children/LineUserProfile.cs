using isRock.LineBot;

public class LineUserProfile : LineProfile {
    public LineUserProfile (Source eventSource, string channelAccessToken) : base (eventSource, channelAccessToken) { }

    public override LineUserInfo GetUserProfile () {
        return Utility.GetUserInfo (_src.userId, _token);
    }

    public override string Leave () {
        return string.Empty;
    }
}