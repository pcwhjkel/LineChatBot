using System.Collections.Generic;

public class LineComplainingIntentReplier : LineIntentReplier {
    public LineComplainingIntentReplier (IDictionary<string, object> entities) : base (entities) { }

    public override string GetProcMessage (string appendMessage = "") {
        return appendMessage + "親!您的抱怨我聽到了,就不用那麼大聲囉！";
    }
}