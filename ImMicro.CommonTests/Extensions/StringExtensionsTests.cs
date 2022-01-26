using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImMicro.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpersToolbox.Extensions;
using ImMicro.Common.Constans;

namespace ImMicro.Common.Extensions.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        public void ComputeHashShaTest()
        {
            //arrange
            var expected = "94c36ff77cb9c37e52b0eb45b8885a78f429168c";
            
            //act
            var hash = "ImMicro".ComputeHashSha(AppConstants.HashKey);
            
            //assert
            Assert.AreEqual(expected, hash);
        }
    }
}