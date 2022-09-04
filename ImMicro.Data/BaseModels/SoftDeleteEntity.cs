using System;

namespace ImMicro.Data.BaseModels
{
    public class SoftDeleteEntity : Entity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}