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
        private HttpClient httpClient;
        private Conversation lastConversation;
        private string _directLineKey = "get key from Azure portal bot";

        public async Task Setup()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://directline.botframeworks.com/api/conversations");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", _directLineKey);
            var response = await httpClient.GetAsync("/api/tokens/");

            if(response.IsSuccessStatusCode)
            {
                var conversation = new Conversation();
                HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(conversation), Encoding.UTF8,
                    "application/json");
                response = await httpClient.PostAsync("/api/conversations/", contentPost);
                if(response.IsSuccessStatusCode)
                {
                    var conversationInfo = await response.Content.ReadAsStringAsync();
                    lastConversation = JsonConvert.DeserializeObject<Conversation>(conversationInfo);
                }
            }
        }

        public async Task<BotMessage> SendMessage(string username, string messageText)
        {
            var messageToSend = new BotMessage() { From = username, Text = messageText, ConversationId = lastConversation.ConversationId };
            var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
            var conversationUrl = "https://directline.botframeworks.com/api/conversations/" + lastConversation.ConversationId + "/messages/"

            var response = await httpClient.PostAsync(conversationUrl, contentPost);
            var messageInfo = await response.Content.ReadAsStringAsync();

            var messageReceived = await httpClient.GetAsync(conversationUrl);
            var messageReceivedData = await messageReceived.Content.ReadAsStringAsync();
            var messageRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messageReceivedData);
            var messages = messageRoot.Messages;

            return messages.FirstOrDefault(m => m.From == "FirstBot10");
        }
    }
}
