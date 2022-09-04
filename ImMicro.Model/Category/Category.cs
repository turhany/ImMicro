using ImMicro.Data.BaseModels;

namespace ImMicro.Model.Category;

public class Category : SoftDeleteEntity
{
    public string Name { get; set; }
    public int MinStockQuantity { get; set; }
}