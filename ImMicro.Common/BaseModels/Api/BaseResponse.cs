using System.Collections.Generic;

namespace ImMicro.Common.BaseModels.Api
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            ValidationMessages = new List<string>();
        }

        public string Status { get; set; }
        public string Message { get; set; }
        public List<string> ValidationMessages { get; set; }
    }
}