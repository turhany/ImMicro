using System;

namespace ImMicro.Contract.App.Product;

public class ProductView
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
}