using System;
using System.Threading.Tasks;
using ImMicro.Business.User.Abstract;
using ImMicro.BusinessTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using ImMicro.Common.BaseModels.Service;

namespace ImMicro.Business.Services.Concrete.Tests;

[TestClass]
public class UserServiceTests : TestBase
{
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userService = Container.Resolve<IUserService>();
    }
}