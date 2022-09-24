using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Filtery.Exceptions;
using Filtery.Models;
using Filtery.Models.Order;
using ImMicro.Business.User.Abstract;
using ImMicro.Common.Auth;
using ImMicro.Common.Auth.Concrete;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Data.Abstract;
using ImMicro.Contract.Service.User;
using ImMicro.Data.BaseRepositories;
using ImMicro.Model.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImMicro.BusinessTests.Services.Concrete;

[TestClass]
public class UserServiceTests : TestBase
{
    private readonly IUserService _userService;
    private readonly IGenericRepository<Model.User.User> _userRepository;

    public UserServiceTests()
    {
        _userService = Container.Resolve<IUserService>();
        _userRepository = Container.Resolve<IGenericRepository<Model.User.User>>();
    }

    [TestMethod]
    public async Task GetAsyncTest_NO_DATA()
    {
        //arrange - act
        var response = await _userService.GetAsync(Guid.NewGuid(), CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }

    [TestMethod]
    public async Task GetAsyncTest_OK()
    {
        //arrange
        var user = new Model.User.User {Id = Guid.NewGuid(), Email = "test@test.com", Password = "Password", Type = UserType.Root};
        await _userRepository.InsertAsync(user, CancellationToken.None);

        //act
        var response = await _userService.GetAsync(user.Id, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }

    [TestMethod]
    public async Task CreateAsync_VALIDATION_ERROR()
    {
        //arrange
        var user = new CreateUserRequestServiceRequest {FirstName = ""};

        //act
        var response = await _userService.CreateAsync(user, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.InvalidInput, response.Status);
    }
    
    [TestMethod]
    public async Task CreateAsync_DUPLICATE_EMAIL_ERROR()
    {
        //arrange
        var user = new Model.User.User {Id = Guid.NewGuid(), Email = "test@test.com", Password = "Password", Type = UserType.Root};
        await _userRepository.InsertAsync(user, CancellationToken.None);
        
        var userCreateRequest = new CreateUserRequestServiceRequest {Email = "test@test.com", FirstName = "test", LastName = "test", Password = "123456789.tY"};

        //act
        var response = await _userService.CreateAsync(userCreateRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.InvalidInput, response.Status);
    }
    
    [TestMethod]
    public async Task CreateAsync_OK()
    {
        //arrange
        var userCreateRequest = new CreateUserRequestServiceRequest {Email = "test1@test.com", FirstName = "test", LastName = "test", Password = "123456789.tY"};

        //act
        var response = await _userService.CreateAsync(userCreateRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task UpdateAsync_VALIDATION_ERROR()
    {
        //arrange
        var user = new UpdateUserRequestServiceRequest {FirstName = ""};

        //act
        var response = await _userService.UpdateAsync(user, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.InvalidInput, response.Status);
    }
    
    [TestMethod]
    public async Task UpdateAsync_USER_NOT_EXIST()
    {
        //arrange
        var user = new UpdateUserRequestServiceRequest {Id = Guid.NewGuid() ,Email = "test@test.com", FirstName = "test", LastName = "test", Password = "123456789.tY"};

        //act
        var response = await _userService.UpdateAsync(user, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task UpdateAsync_DUPLICATE_EMAIL_ERROR()
    {
        //arrange
        var user1 = new Model.User.User {Id = Guid.NewGuid(), Email = "test@test.com", Password = "Password", Type = UserType.Root};
        var user2 = new Model.User.User {Id = Guid.NewGuid(), Email = "test2@test.com", Password = "Password", Type = UserType.Root};
        await _userRepository.InsertAsync(user1, CancellationToken.None);
        await _userRepository.InsertAsync(user2, CancellationToken.None);
        
        var userUpdateRequest = new UpdateUserRequestServiceRequest {Id = user1.Id ,Email = "test2@test.com", FirstName = "test", LastName = "test", Password = "123456789.tY"};

        //act
        var response = await _userService.UpdateAsync(userUpdateRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.InvalidInput, response.Status);
    }
    
    [TestMethod]
    public async Task UpdateAsync_OK()
    {
        //arrange
        var user = new Model.User.User {Id = Guid.NewGuid(), Email = "tes5t@test.com", Password = "Password", Type = UserType.Root};
        await _userRepository.InsertAsync(user, CancellationToken.None);
        
        var userUpdateRequest = new UpdateUserRequestServiceRequest {Id = user.Id ,Email = "test6@test.com", FirstName = "test", LastName = "test", Password = "123456789.tY"};

        //act
        var response = await _userService.UpdateAsync(userUpdateRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task DeleteAsync_USER_NOT_FOUND()
    {
        //arrange - act
        var response = await _userService.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task DeleteAsync_OK()
    {
        //arrange
        var user = new Model.User.User {Id = Guid.NewGuid(), Email = "test@test.com", Password = "Password", Type = UserType.Root};
        await _userRepository.InsertAsync(user, CancellationToken.None);
        
        //act
        var response = await _userService.DeleteAsync(user.Id, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task SearchAsync_OK()
    {
        //arrange
        var user = new Model.User.User {Id = Guid.NewGuid(), Email = "test@test.com", Password = "Password", Type = UserType.Root};
        await _userRepository.InsertAsync(user, CancellationToken.None);
        
        //act
        var response = await _userService.SearchAsync(new FilteryRequest
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
            await _userService.SearchAsync(new FilteryRequest
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
     
    [TestMethod]
    public async Task GetTokenAsync_VALIDATION_ERROR()
    {
        //arrange
        var tokenRequest = new GetTokenContractServiceRequest {Email = "test@test.com", Password = "Password"};
        
        //act
        var response = await _userService.GetTokenAsync(tokenRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.InvalidInput, response.Status);
    }
    
    [TestMethod]
    public async Task GetTokenAsync_USER_NOT_FOUND()
    {
        //arrange
        var tokenRequest = new GetTokenContractServiceRequest {Email = "test@4test.com", Password = "123456789.tY"};
        
        //act
        var response = await _userService.GetTokenAsync(tokenRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task GetTokenAsync_PASSWORD_NOT_MATCH()
    {
        //arrange
        var user = new Model.User.User {Id = Guid.NewGuid(), Email = "test3@test.com", Password = BCrypt.Net.BCrypt.HashPassword("123456789.tY"), Type = UserType.Root};
        await _userRepository.InsertAsync(user, CancellationToken.None);

        var tokenRequest = new GetTokenContractServiceRequest {Email = "test3@test.com", Password = "123456789.Ty"};
        
        //act
        var response = await _userService.GetTokenAsync(tokenRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task GetTokenAsync_OK()
    {
        //arrange
        var user = new Model.User.User {Id = Guid.NewGuid(), Email = "test7@test.com", Password = BCrypt.Net.BCrypt.HashPassword("123456789.tY"), Type = UserType.Root};
        await _userRepository.InsertAsync(user, CancellationToken.None);

        var tokenRequest = new GetTokenContractServiceRequest {Email = "test7@test.com", Password = "123456789.tY"};
        
        //act
        var response = await _userService.GetTokenAsync(tokenRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
    
    [TestMethod]
    public async Task RefreshTokenAsync_VALIDATION_ERROR()
    {
        //arrange
        var tokenRequest = new RefreshTokenContractServiceRequest {Token = ""};
        
        //act
        var response = await _userService.RefreshTokenAsync(tokenRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.InvalidInput, response.Status);
    }
    
    [TestMethod]
    public async Task RefreshTokenAsync_USER_NOT_FOUND()
    {
        //arrange
        var tokenRequest = new RefreshTokenContractServiceRequest {Token = "token"};
        
        //act
        var response = await _userService.RefreshTokenAsync(tokenRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task RefreshTokenAsync_OK()
    {
        //arrange
        var user = new Model.User.User {Id = Guid.NewGuid(), Email = "test@test.com", Password = BCrypt.Net.BCrypt.HashPassword("123456789.tY"), Type = UserType.Root};
        var token = new JwtManager().GenerateToken(new JwtContract
        {
            Id = user.Id,
            Name = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            UserType = user.Type
        });
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(1);
        
        await _userRepository.InsertAsync(user, CancellationToken.None);
         
        var tokenRequest = new RefreshTokenContractServiceRequest {Token = token.RefreshToken};
        
        //act
        var response = await _userService.RefreshTokenAsync(tokenRequest, CancellationToken.None);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
}