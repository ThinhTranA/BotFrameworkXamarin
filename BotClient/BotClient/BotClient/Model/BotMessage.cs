using System;
using System.Collections.Generic;
using System.Text;

namespace BotClient.Model
{
    public class BotMessage
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public DateTime Created { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public string ChannelData { get; set; }
        public string[] vs { get; set; }
        public Attachment[] Attachments { get; set; }
        public string ETag { get; set; }
    }

    public class Attachment
    {
        public string Url { get; set; }
        public string ContentType { get; set; }
    }
}
