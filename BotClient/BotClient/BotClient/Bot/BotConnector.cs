using BotClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BotClient.Bot
{
    public class BotConnector
    {
        private HttpClient _httpClient;
        private Conversation _lastConversation;
        private string _directLineKey = "get key from Azure portal bot";

        public async Task Setup()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://directline.botframeworks.com/api/conversations");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", _directLineKey);
            var response = await _httpClient.GetAsync("/api/tokens/");

            if(response.IsSuccessStatusCode)
            {
                var conversation = new Conversation();
                HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(conversation), Encoding.UTF8,
                    "application/json");
                response = await _httpClient.PostAsync("/api/conversations/", contentPost);
                if(response.IsSuccessStatusCode)
                {
                    var conversationInfo = await response.Content.ReadAsStringAsync();
                    _lastConversation = JsonConvert.DeserializeObject<Conversation>(conversationInfo);
                }
            }
        }

        public async Task<BotMessage> SendMessage(string username, string messageText)
        {
            var messageToSend = new BotMessage() { From = username, Text = messageText, ConversationId = _lastConversation.ConversationId };
            var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
            var conversationUrl = "https://directline.botframeworks.com/api/conversations/" + _lastConversation.ConversationId + "/messages/";

            var response = await _httpClient.PostAsync(conversationUrl, contentPost);
            var messageInfo = await response.Content.ReadAsStringAsync();

            var messageReceived = await _httpClient.GetAsync(conversationUrl);
            var messageReceivedData = await messageReceived.Content.ReadAsStringAsync();
            var messageRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messageReceivedData);
            var messages = messageRoot.Messages;

            return messages.FirstOrDefault(m => m.From == "FirstBot10");
        }
    }
}
