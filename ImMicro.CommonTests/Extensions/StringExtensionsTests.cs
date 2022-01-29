using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var expected = "39b8d37fc187c87933353c57275090239c70f9ac";
            
            //act
            var hash = "ImMicro".ComputeHashSha(AppConstants.HashKey);
            
            //assert
            Assert.AreEqual(expected, hash);
        }
    }
}