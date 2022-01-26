using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImMicro.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpersToolbox.Extensions;

namespace ImMicro.Common.Extensions.Tests
{
    [TestClass()]
    public class DictionaryExtensionMethodsTests
    {
        [TestMethod()]
        public void MergeTest()
        {
            //arrange
            var dic1 = new Dictionary<string, string>()
            {
                {"item1", "itemv1alue"}
            };
            var dic2 = new Dictionary<string, string>()
            {
                {"item2", "item2value"}
            };
            
            //act
            dic1.Merge(dic2);
            
            //assert
            Assert.IsTrue(dic1.ContainsKey("item2"));
        }
    }
}