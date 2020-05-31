using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

public class LineOrderingIntentReplier : LineIntentReplier {
    public LineOrderingIntentReplier (IDictionary<string, object> entities) : base (entities) { }

    public override string GetProcMessage (string appendMessage = "") {
        foreach(var entity in Entities) {
            if (entity.Key == "DrinkSet") {
                var drinkSet = JsonConvert.DeserializeObject<IList<DrinkSet>>(entity.Value.ToString());

                if (drinkSet.Count > 0) {
                    foreach (var d in drinkSet) {
                        var name        = d.drinkName;
                        var temperature = d.drinkTemperature;
                        var amount      = d.drinkAmount;

                        if (name != null && name.Count > 0) {
                            appendMessage += "收到訂單!\n";
                            appendMessage += "飲料：" + name.First() + "\n";
                            appendMessage += "溫度：" + (temperature != null && temperature.Count > 0 ? temperature.First() : "冰") + "\n";
                            appendMessage += "數量：" + (amount != null && amount.Count > 0 ? amount.First() : "1") + "杯\n";
                        }
                    }
                }
            }

            if (entity.Key == "FoodSet") {
                var foodSet = JsonConvert.DeserializeObject<IList<FoodSet>>(entity.Value.ToString());

                if (foodSet.Count > 0) {
                    foreach (var f in foodSet) {
                        var name   = f.foodName;
                        var amount = f.foodAmount;

                        if (name != null && name.Count > 0) {
                            appendMessage += "收到訂單！\n";
                            appendMessage += "餐點：" + name.First() + "\n";
                            appendMessage += "數量：" + (amount != null && amount.Count > 0 ? amount.First() : "1") + "份\n";
                        }
                    }
                }
            }
        }

        return string.IsNullOrEmpty(appendMessage) ? "不太明瞭您想點什麼？" : appendMessage;
    }
}