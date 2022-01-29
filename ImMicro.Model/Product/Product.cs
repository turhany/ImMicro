using System;
using ImMicro.Common.Data;

namespace ImMicro.Model.Product;

public class Product : SoftDeleteEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public Guid CategoryId { get; set; }
    public virtual Category.Category Category { get; set; }
}