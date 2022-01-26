using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ImMicro.Common.Data
{
    public class Entity
    {
        public Guid Id { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; } 
    }
}