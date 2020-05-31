using isRock.LineBot;

public class LineRoomUserProfile : LineProfile {
    public LineRoomUserProfile (Source eventSource, string channelAccessToken) : base (eventSource, channelAccessToken) { }

    public override LineUserInfo GetUserProfile () {
        return Utility.GetRoomMemberProfile (_src.roomId, _src.userId, _token);
    }

    public override string Leave () {
        return Utility.LeaveRoom(_src.roomId, _token);
    }
}