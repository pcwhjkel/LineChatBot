using System.Collections.Generic;

public abstract class LineIntentReplier {
    public IDictionary<string, object> Entities { get; set; }
    public LineIntentReplier (IDictionary<string, object> entities) {
        this.Entities = entities;
    }

    public abstract string GetProcMessage(string appendMessage = "");
}