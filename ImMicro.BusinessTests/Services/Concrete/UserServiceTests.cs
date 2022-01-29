using System;
using System.Threading.Tasks;
using ImMicro.Business.User.Abstract;
using ImMicro.BusinessTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Data.Abstract;
using ImMicro.Model.User;

namespace ImMicro.Business.Services.Concrete.Tests;

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
        var response = await _userService.GetAsync(Guid.NewGuid());

        //assert 
        Assert.AreEqual(ResultStatus.ResourceNotFound, response.Status);
    }
    
    [TestMethod]
    public async Task GetAsyncTest_OK()
    {
        //arrange
        var user = new Model.User.User { Id = Guid.NewGuid(), Email = "test@test.com", Password = "Password", Type = UserType.Root};
        await _userRepository.InsertAsync(user);
            
        //act
        var response = await _userService.GetAsync(user.Id);

        //assert 
        Assert.AreEqual(ResultStatus.Successful, response.Status);
    }
}