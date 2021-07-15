using System.Collections.Generic;
using System.Linq;

namespace ImMicro.Common.Validation
{
    public static class MimeValidation
    {
        /// <summary>
        /// Json
        /// </summary>
        private static readonly byte[] Json = {91, 13, 10, 32};


        private static readonly Dictionary<string, string> MimeTypesDictionary = new()
        {
            {"json", "application/json"}
        };

        private static string GetMimeType(byte[] file, string fileName)
        {
            string mime = "application/octet-stream";

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return mime;
            }

            if (file.Take(4).SequenceEqual(Json))
            {
                mime = "application/json";
            }

            return mime;
        }

        public static bool IsValidMime(byte[] file, string fileName, params string[] allowedExtensions)
        {
            var mime = GetMimeType(file, fileName);
            return MimeTypesDictionary.ContainsValue(mime) && MimeTypesDictionary.Any(mimeItem => mimeItem.Value == mime && allowedExtensions.Contains(mimeItem.Key));
        }
    }
}