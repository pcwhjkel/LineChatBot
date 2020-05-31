using System.Collections.Generic;

public class LineEncouragingIntentReplier : LineIntentReplier {
    public LineEncouragingIntentReplier (IDictionary<string, object> entities) : base (entities) { }

    public override string GetProcMessage (string appendMessage = "") {
        return appendMessage + "親!您的鼓勵是我們的動力！謝謝支持乾蝦乾蝦！";
    }
}