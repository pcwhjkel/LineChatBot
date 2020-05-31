using System.Collections.Generic;

public class LineMixedReviewIntentReplier : LineIntentReplier {
    public LineMixedReviewIntentReplier (IDictionary<string, object> entities) : base (entities) { }

    public override string GetProcMessage (string appendMessage = "") {
        return appendMessage + "親!您的鼓勵是我們的動力,您的抱怨我們聽到後就不用那麼大聲!";
    }
}