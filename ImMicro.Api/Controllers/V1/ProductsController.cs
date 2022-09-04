using System;
using System.Threading;
using System.Threading.Tasks;
using Filtery.Models;
using ImMicro.Business.Product.Abstract;
using ImMicro.Common.BaseModels.Api;
using ImMicro.Contract.App.Product; 
using ImMicro.Contract.Service.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImMicro.Api.Controllers.V1;

/// <summary>
/// Products Controller
/// </summary>
[ApiVersion("1.0")]
public class ProductsController : BaseController
{
    private readonly IProductService _productService; 

    /// <summary>
    /// Products Controller
    /// </summary>
    /// <param name="productService"></param>
    public ProductsController(IProductService productService)
    {
        _productService = productService; 
    }

    /// <summary>
    /// Get Product
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Root")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductView))]
    public async Task<ActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productService.GetAsync(id, cancellationToken);
        return ApiResponse.CreateResult(result);
    }

    /// <summary>
    /// Create Product
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Root")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        if (request == null) return ApiResponse.InvalidInputResult;

        var result = await _productService.CreateAsync(Mapper.Map<CreateProductRequestServiceRequest>(request), cancellationToken);
        return ApiResponse.CreateResult(result);
    }

    /// <summary>
    /// Update Product
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Root")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductRequest request, Guid id, CancellationToken cancellationToken)
    {
        if (request == null) return ApiResponse.InvalidInputResult;
        var model = Mapper.Map<UpdateProductRequestServiceRequest>(request);
        model.Id = id;

        var result = await _productService.UpdateAsync(model, cancellationToken);
        return ApiResponse.CreateResult(result);
    }

    /// <summary>
    /// Delete Product
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Root")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return ApiResponse.InvalidInputResult;

        var result = await _productService.DeleteAsync(id, cancellationToken);
        return ApiResponse.CreateResult(result);
    }

    /// <summary>
    /// Product Search
    /// </summary>
    /// <returns></returns>
    [HttpPost("search")]
    [Authorize(Roles = "Root")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Search([FromBody] FilteryRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.SearchAsync(request, cancellationToken);
        return ApiResponse.CreateResult(result);
    }
    
    /// <summary>
    /// Get Product
    /// </summary>
    [HttpGet("GetWithDapper/{id:guid}")]
    [Authorize(Roles = "Root")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductView))]
    public async Task<ActionResult> GetWithDapper(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productService.GetWithDapperAsync(id, cancellationToken);
        return ApiResponse.CreateResult(result);
    }
}