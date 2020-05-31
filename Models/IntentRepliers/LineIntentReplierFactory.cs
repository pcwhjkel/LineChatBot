using System.Collections.Generic;

public class LineIntentReplierFactory {
    public LineIntentReplier GetReplier(string intent, IDictionary<string, object> entities) {
        LineIntentReplier result = null;

        switch (intent) {
            case "Greeting":
                result = new LineGreetingIntentReplier(entities);
                break;

            case "Complaining":
                result = new LineOrderingIntentReplier(entities);
                break;

            case "Encouraging":
                result = new LineEncouragingIntentReplier(entities);
                break;

            case "MixedReview":
                result = new LineMixedReviewIntentReplier(entities);
                break;

            case "Ordering":
                result = new LineOrderingIntentReplier(entities);
                break;

            case "None":
                result = new LineNoneIntentReplier(entities);
                break;

            default:
                break;
        }

        return result;
    }

}