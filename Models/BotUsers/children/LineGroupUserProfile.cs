using isRock.LineBot;

public class LineGroupUserProfile : LineProfile {
    public LineGroupUserProfile (Source eventSource, string channelAccessToken) : base (eventSource, channelAccessToken) { }

    public override LineUserInfo GetUserProfile () {
        return Utility.GetGroupMemberProfile (_src.groupId, _src.userId, _token);
    }

    public override string Leave () {
        return Utility.LeaveGroup(_src.roomId, _token);
    }
}