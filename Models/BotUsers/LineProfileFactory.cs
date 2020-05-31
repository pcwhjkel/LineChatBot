using isRock.LineBot;

public class LineProfileFactory {
    public LineProfile GetUserProfile(Source eventSource, string sourceType, string channelAccessToken) {
        LineProfile result = null;

        switch (sourceType) {
            case "group":
                result = new LineGroupUserProfile(eventSource, channelAccessToken);
                break;

            case "room":
                result = new LineRoomUserProfile(eventSource, channelAccessToken);
                break;

            case "user":
                result = new LineUserProfile(eventSource, channelAccessToken);
                break;

            default:
                break;
        }

        return result;
    }

}