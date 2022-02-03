using Exporty.Models;
using Filtery.Models;

namespace ImMicro.Contract.App;

public class ExportRequest
{
    public FilteryRequest SearchRequest { get; set; }
    public ExportType ExportType { get; set; }
}