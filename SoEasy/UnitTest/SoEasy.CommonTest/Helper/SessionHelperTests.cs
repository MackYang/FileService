using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoEasy.LogicTest.Model;
namespace SoEasy.Common.Tests
{
    [TestClass()]
    public class SessionHelperTests
    {
        ProductInfo p = new ProductInfo();
        public SessionHelperTests()
        {
            p.Op_Time = DateTime.Now;
            p.Name = "IPhone";
            p.Price = 5288.23M;
        }

        [TestMethod()]
        public void GetSessionObjectTest()
        {
            Assert.Fail("涉及session,请在外层去测试");
        }

        [TestMethod()]
        public void SetSessionObjectTest()
        {
            Assert.Fail("涉及session,请在外层去测试");
            
        }
    }
}
