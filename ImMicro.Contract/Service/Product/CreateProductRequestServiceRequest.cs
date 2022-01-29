using System;

namespace ImMicro.Contract.Service.Product;

public class CreateProductRequestServiceRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
}