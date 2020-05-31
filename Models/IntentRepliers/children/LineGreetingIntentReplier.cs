using System.Collections.Generic;

public class LineGreetingIntentReplier : LineIntentReplier {
    public LineGreetingIntentReplier (IDictionary<string, object> entities) : base (entities) { }

    public override string GetProcMessage (string appendMessage = "") {
        return appendMessage + "親!真正高興來見到您！";
    }
}