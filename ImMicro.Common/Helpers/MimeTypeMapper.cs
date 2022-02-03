using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace ImMicro.Common.Helpers;

public static class MimeTypeMapper
{
    public static string GetMimeType(string file)
    {
        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(file, out var contentType))
            contentType = "application/octet-stream";

        if (Path.GetExtension(file).Equals(".csv"))
            return "text/csv";

        return contentType;
    }
}