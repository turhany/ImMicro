using Filtery.Models;
using ImMicro.Common.Pager;

namespace ImMicro.Common.Extensions
{
    public static class FilteryExtensions
    {
        public static Page GetPageInfo<T>(this FilteryResponse<T> response) => new()
        {
            PageNumber = response.PageNumber,
            PageSize = response.PageSize,
            TotalItemCount = response.TotalItemCount
        };
    }
}