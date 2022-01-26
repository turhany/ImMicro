using System.Collections.Generic;

namespace ImMicro.Common.BaseModels.Service
{
    public class ServiceResult<T> where T : class
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> ValidationMessages { get; set; } = new List<string>();
    }
}