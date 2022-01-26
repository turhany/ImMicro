using System;

namespace ImMicro.Common.Pager
{
    public class Page
    {
        public int TotalItemCount { get;  set; }
        public int PageSize { get;  set; }
        public int PageNumber { get;  set; }
        public int TotalPageCount => (int)Math.Ceiling(TotalItemCount / (double)PageSize);
    }
}