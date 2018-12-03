using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace SoEasy.Common.Tests
{
    [TestClass()]
    public class PageHelperTests
    {
        [TestMethod()]
        public void GetPagesLinkTest()
        {
            Assert.IsTrue(PageHelper.GetPagesLink("http://www.bjhhsc.com", 5, 1, 20, 10).Contains("3"));
        }

        [TestMethod()]
        public void GetPagesActionTest()
        {
            Assert.IsTrue(PageHelper.GetPagesAction("toPage", 5, 1, 20, 4).Contains("3"));
        }
    }
}
