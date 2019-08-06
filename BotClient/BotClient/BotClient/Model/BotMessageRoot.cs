using System;
using System.Collections.Generic;
using System.Text;

namespace BotClient.Model
{
    public class BotMessageRoot
    {
        public List<BotMessage> Messages { get; set; }
        public string Watermark { get; set; }
    }
}
