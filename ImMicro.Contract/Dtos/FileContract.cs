using System;
using Microsoft.AspNetCore.Http;

namespace ImMicro.Contract.Dtos
{
    [Serializable]
    public class FileContract
    {
        public IFormFile FileData { get; set; }
    }
}