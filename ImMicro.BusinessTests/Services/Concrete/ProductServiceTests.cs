using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImMicro.Business.Product.Abstract; 
using Autofac;
using Filtery.Exceptions;
using Filtery.Models;
using Filtery.Models.Order;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Contract.Service.Product;
using ImMicro.Model.Category;
using ImMicro.Model.Product;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using ImMicro.Data.BaseRepositories;

namespace ImMicro.BusinessTests.Services.Concrete;

[TestClass]
public class ProductServiceTests : TestBase
{
    private readonly IProductService _productService;
    private readonly IGenericRepository<Model.Product.Product> _productRepository;
    private readonly IGenericRepository<Model.Category.Category> _categoryRepository;

    public ProductServiceTests()
    {
        _productService = Container.Resolve<IProductService>();
        _productRepository = Container.Resolve<IGenericRepository<Model.Product.Product>>();
        _categoryRepository = Container.Resolve<IGenericRepository<Model.Category.Category>>();
    }
    
    [TestMethod]
    public async Task GetAsync_NO_DATA()
    {
        //arrange - act
        var response = await _productService.GetAsync(Guid.NewGuid(), CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task GetAsync_OK()
    {
        //arrange
        var product = new Model.Product.Product { Id = Guid.NewGuid(), Title = "Product Name"};
        await _productRepository.InsertAsync(product, CancellationToken.None);
            
        //act
        var response = await _productService.GetAsync(product.Id, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }

    [TestMethod]
    public async Task CreateAsync_VALIDATION_ERROR()
    {
        //arrange
        var product = new CreateProductRequestServiceRequest { Title = "Product Name"}; 
            
        //act
        var response = await _productService.CreateAsync(product, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.InvalidInput, response.Status);
    }
    
    [TestMethod]
    public async Task CreateAsync_CATEGORY_NOT_FOUND()
    {
        //arrange
        var product = new CreateProductRequestServiceRequest
        {
            Title = "Product Name",
            Description = "Description",
            CategoryId = Guid.NewGuid()
        }; 
            
        //act
        var response = await _productService.CreateAsync(product, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task CreateAsync_OK()
    {
        //arrange
        var category = await _categoryRepository.InsertAsync(new Category() {Id = Guid.NewGuid(), Name = "Category"}, CancellationToken.None);
        var product = new CreateProductRequestServiceRequest
        {
            Title = "Product Name",
            Description = "Description",
            CategoryId = category.Id
        }; 
            
        //act
        var response = await _productService.CreateAsync(product, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task UpdateAsync_VALIDATION_ERROR()
    {
        //arrange
        var product = new UpdateProductRequestServiceRequest { Title = "Product Name"}; 
            
        //act
        var response = await _productService.UpdateAsync(product, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.InvalidInput, response.Status);
    }
    
    [TestMethod]
    public async Task UpdateAsync_PRODUCT_NOT_FOUND()
    {
        //arrange
        var product = new UpdateProductRequestServiceRequest
        {
            Id = Guid.NewGuid(),
            Title = "Product Name",
            Description = "Description",
            CategoryId = Guid.NewGuid()
        }; 
            
        //act
        var response = await _productService.UpdateAsync(product, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task UpdateAsync_CATEGORY_NOT_FOUND()
    {
        //arrange
        var category = await _categoryRepository.InsertAsync(new Category() {Id = Guid.NewGuid(), Name = "Category"}, CancellationToken.None);
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product Name",
            Description = "Description",
            CategoryId = category.Id
        };
        await _productRepository.InsertAsync(product, CancellationToken.None);
        
        var productForUpdate = new UpdateProductRequestServiceRequest
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            CategoryId = Guid.NewGuid()
        };
        
        //act
        var response = await _productService.UpdateAsync(productForUpdate, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task UpdateAsync_OK()
    {
        //arrange
        var category = await _categoryRepository.InsertAsync(new Category() {Id = Guid.NewGuid(), Name = "Category"}, CancellationToken.None);
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product Name",
            Description = "Description",
            CategoryId = category.Id
        };
        await _productRepository.InsertAsync(product, CancellationToken.None);
        
        var productForUpdate = new UpdateProductRequestServiceRequest
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            CategoryId = product.CategoryId
        };
        
        //act
        var response = await _productService.UpdateAsync(productForUpdate, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task DeleteAsync_PRODUCT_NOT_FOUND()
    {
        //arrange - act
        var response = await _productService.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task DeleteAsync_OK()
    {
        //arrange
        var category = await _categoryRepository.InsertAsync(new Category() {Id = Guid.NewGuid(), Name = "Category"}, CancellationToken.None);
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product Name",
            Description = "Description",
            CategoryId = category.Id
        };
        await _productRepository.InsertAsync(product, CancellationToken.None);
        
        //act
        var response = await _productService.DeleteAsync(product.Id, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task SearchAsync_OK()
    {
        //arrange
        var category = await _categoryRepository.InsertAsync(new Category() {Id = Guid.NewGuid(), Name = "Category"}, CancellationToken.None);
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product Name",
            Description = "Description",
            CategoryId = category.Id
        };
        await _productRepository.InsertAsync(product, CancellationToken.None);
        
        //act
        var response = await _productService.SearchAsync(new FilteryRequest()
        {
            PageNumber = 1,
            PageSize = 10
        }, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task SearchAsync_NOK()
    {
        //INFO: I can not use "Assert.ThrowsExceptionAsync" because it's not catch base exception type
        
        //arrange - act - assert 
        try
        {
            await _productService.SearchAsync(new FilteryRequest
            {
                OrderOperations = new Dictionary<string, OrderOperation>()
                {
                    {"errorkey", OrderOperation.Ascending}
                },
                PageNumber = 1,
                PageSize = 10
            }, CancellationToken.None);
        }
        catch (FilteryBaseException)
        {
           Assert.IsTrue(true);
        }
        catch(Exception)
        {
            Assert.IsTrue(false);
        }
    }
}