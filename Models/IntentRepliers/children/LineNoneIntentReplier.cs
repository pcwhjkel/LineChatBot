using System.Collections.Generic;

public class LineNoneIntentReplier : LineIntentReplier {
    public LineNoneIntentReplier (IDictionary<string, object> entities) : base (entities) { }

    public override string GetProcMessage (string appendMessage = "") {
        return appendMessage + "不太清楚您到底有何意圖！";
    }
}