using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using isRock.LineBot;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Extensions.Configuration;

namespace LineWebHook.Controllers {

    [Route ("api/[controller]")]
    [ApiController]
    public class LineBotWebHookController : ControllerBase {
        string _channelAccessToken { get; set; }
        string _channelSecret { get; set; }
        string _toUserId { get; set; }
        string _luisSubscriptionKey { get; set; }
        string _luisPredictionEndpoint { get; set; }
        string _luisAppId { get; set; }

        Bot bot { get; set; }
        ApiKeyServiceClientCredentials credentials { get; set; }
        //IBfStoreData dataRepository { get; set; }

        public LineBotWebHookController (IConfiguration config/*, IBfStoreData repo*/) {
            _channelSecret          = config.GetSection ("LineMessageAPI:channelSecret").Value;
            _channelAccessToken     = config.GetSection ("LineMessageAPI:channelAccessToken").Value;
            _toUserId               = config.GetSection ("LineMessageAPI:toUserId").Value;
            _luisSubscriptionKey    = config.GetSection ("LUIS:key").Value;
            _luisPredictionEndpoint = config.GetSection ("LUIS:endpoint").Value;
            _luisAppId              = config.GetSection ("LUIS:appId").Value;

            bot            = new Bot (_channelAccessToken);
            credentials    = new ApiKeyServiceClientCredentials (_luisSubscriptionKey);
            //dataRepository = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Post () {
            StatusCodeResult actionResult = Unauthorized ();

            var postData = await getPostDataAsync (Request, _channelSecret);

            if (!string.IsNullOrEmpty (postData)) {
                var received = Utility.Parsing (postData);

                //should enumerate each event in reveived.events, but this is just a practice, do it simple.
                var lineEvent = received.events.FirstOrDefault ();

                // get replyToken from lineEvent
                var replyToken = lineEvent.replyToken;

                // "0x32" is token for line verify
                if (lineEvent.replyToken != "00000000000000000000000000000000") {
                    switch (lineEvent.type) {
                        case "message":
                            await replyMessage (lineEvent, replyToken);
                            actionResult = Ok ();
                            break;

                        // case "postback":
                        //     bot.ReplyMessage (replyToken, lineEvent.postback.data == "yes" ? "答對了!" : "答錯了!");
                        //     actionResult = Ok ();
                        //     break;

                        case "join":
                            bot.ReplyMessage (replyToken, $"有人把我加入 {lineEvent.source.type} 中了，大家好啊～");
                            actionResult = Ok ();
                            break;

                        default:
                            break;
                    }
                } else {
                    actionResult = Ok ();
                }
            }

            return actionResult;
        }

        private async Task<string> getPostDataAsync (HttpRequest request, string secret) {
            var postData = string.Empty;

            using (var reader = new StreamReader (request.Body)) {
                postData = await reader.ReadToEndAsync ();
            }

            var utf8       = new UTF8Encoding ();
            var dataBuffer = utf8.GetBytes (postData);
            var key        = utf8.GetBytes (secret);
            var digest     = string.Empty;

            using (var hmacSha256 = new HMACSHA256 (key)) {
                var hash   = hmacSha256.ComputeHash (dataBuffer);
                    digest = Convert.ToBase64String (hash);
            };

            if (request.Headers["X-Line-Signature"] != digest) {
                postData = string.Empty;
            }

            return postData;
        }

        private async Task replyMessage (Event lineEvent, string replyToken) {
            var eventMessage = lineEvent.message;
            var eventSource  = lineEvent.source;

            switch (lineEvent.message.type) {
                case "text":
                    var messageText = eventMessage.text;
                    var sourceType  = eventSource.type.ToLower ();

                    switch (messageText) {
                        case "bye":
                            bot.ReplyMessage (replyToken, "bye-bye");

                            var profileFactory = new LineProfileFactory ();
                            var userInfo       = profileFactory.GetUserProfile (eventSource, sourceType, _channelAccessToken);

                            userInfo.Leave ();
                            break;

                        // case "quiz":
                        //     createQuiz (replyToken);
                        //     break;

                        default:
                            await replyUserMessageAsync (eventSource, replyToken, sourceType, messageText);
                            break;
                    }

                    break;

                case "sticker":
                    bot.ReplyMessage (replyToken, eventMessage.packageId, eventMessage.stickerId);
                    break;

                default:
                    break;
            }
        }

        private void createQuiz (string replyToken) {
            var message = new TextMessage ("醫生給了你三顆藥丸要你每半個小時吃一顆,請問吃完需要多長時間？");

            var actions = new List<QuickReplyPostbackAction> () {
                new QuickReplyPostbackAction ("1小時", "yes", null, "1小時"),
                new QuickReplyPostbackAction ("1.5小時", "err", null, "1.5小時")
            };

            message.quickReply.items.AddRange (actions);

            bot.ReplyMessage (replyToken, message);
        }

        private async Task replyUserMessageAsync (Source eventSource, string replyToken, string sourceType, string messageText) {
            var prediction = (await getLUISPredictionAsync (messageText)).Prediction;
            var topIntent  = prediction.TopIntent;

            // 回應 top intent
            var replierFactory = new LineIntentReplierFactory ();
            var intentReplier  = replierFactory.GetReplier(topIntent, prediction.Entities);

            var replyMessage = intentReplier.GetProcMessage ();

            // 回應其他高分的 intent
            var skipList = new List<string> () { "None", topIntent };
            var highScoreIntents = prediction.Intents.Where(m => !skipList.Contains(m.Key) && m.Value.Score >= 0.5);

            foreach(var t in highScoreIntents) {
                intentReplier = replierFactory.GetReplier(t.Key, prediction.Entities);
                replyMessage  = intentReplier.GetProcMessage(replyMessage + "\n");
            }

            bot.ReplyMessage(replyToken, replyMessage);
        }

        private async Task<PredictionResponse> getLUISPredictionAsync (string query) {
            using (var client = new LUISRuntimeClient (credentials, new DelegatingHandler[] { }) { Endpoint = _luisPredictionEndpoint }) {
                var appId    = Guid.Parse (_luisAppId);
                var slotName = "production";

                var options = new PredictionRequestOptions () {
                    DatetimeReference      = DateTime.Parse ("2020-01-01"),
                    PreferExternalEntities = true
                };

                var predictionRequest = new PredictionRequest (query, options);

                var result = await client.Prediction.GetSlotPredictionAsync (appId, slotName, predictionRequest, true, true, true);

                return result;
            }
        }
    }
}