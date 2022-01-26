using System.Collections.Generic;

namespace ImMicro.Common.BaseModels.Mail
{
    public class MailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string PlainTextContent { get; set; }
        public IEnumerable<string> CC { get; set; } = new List<string>();
        public IEnumerable<string> BCC { get; set; } = new List<string>();
    }
}