﻿using System;
using System.Threading.Tasks;
using ImMicro.Business.Product.Abstract;
using ImMicro.Common.Data.Abstract;
using Autofac;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Contract.Service.Product;
using ImMicro.Model.Category;
using ImMicro.Model.Product;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        var response = await _productService.GetAsync(Guid.NewGuid());

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task GetAsync_OK()
    {
        //arrange
        var product = new Model.Product.Product { Id = Guid.NewGuid(), Title = "Product Name"};
        await _productRepository.InsertAsync(product);
            
        //act
        var response = await _productService.GetAsync(product.Id);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }

    [TestMethod]
    public async Task CreateAsync_VALIDATION_ERROR()
    {
        //arrange
        var product = new CreateProductRequestServiceRequest { Title = "Product Name"}; 
            
        //act
        var response = await _productService.CreateAsync(product);

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
        var response = await _productService.CreateAsync(product);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task CreateAsync_OK()
    {
        //arrange
        var category = await _categoryRepository.InsertAsync(new Category() {Id = Guid.NewGuid(), Name = "Category"});
        var product = new CreateProductRequestServiceRequest
        {
            Title = "Product Name",
            Description = "Description",
            CategoryId = category.Id
        }; 
            
        //act
        var response = await _productService.CreateAsync(product);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task UpdateAsync_VALIDATION_ERROR()
    {
        //arrange
        var product = new UpdateProductRequestServiceRequest { Title = "Product Name"}; 
            
        //act
        var response = await _productService.UpdateAsync(product);

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
        var response = await _productService.UpdateAsync(product);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    
    [TestMethod]
    public async Task UpdateAsync_CATEGORY_NOT_FOUND()
    {
        //arrange
        var category = await _categoryRepository.InsertAsync(new Category() {Id = Guid.NewGuid(), Name = "Category"});
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Product Name",
            Description = "Description",
            CategoryId = category.Id
        };
        await _productRepository.InsertAsync(product);
        
        var productForUpdate = new UpdateProductRequestServiceRequest
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            CategoryId = Guid.NewGuid()
        };
        
        //act
        var response = await _productService.UpdateAsync(productForUpdate);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
}