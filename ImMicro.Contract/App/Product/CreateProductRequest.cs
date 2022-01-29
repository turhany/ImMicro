using System;

namespace ImMicro.Contract.App.Product;

public class CreateProductRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
}