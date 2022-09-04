using ImMicro.Common.Data;
using ImMicro.Data.BaseModels;

namespace ImMicro.Model.RequestLog
{
    public class RequestLog : Entity
    {
        public string Request { get; set; }
        public string Response { get; set; }
        public string StatusCode { get; set; }
        public string RequestPath { get; set; }
    }
}