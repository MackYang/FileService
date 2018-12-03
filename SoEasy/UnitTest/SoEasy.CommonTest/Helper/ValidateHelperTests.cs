using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
namespace SoEasy.Common.Tests
{
    [TestClass()]
    public class ValidateHelperTests
    {
        [TestMethod()]
        public void ValidateVCodeTest()
        {
            //涉及session,只好在外层去测试
            Assert.Fail("涉及session,请在外层去测试");
        }

        [TestMethod()]
        public void GetVCodeTest()
        {
            ////涉及session,只好在外层去测试
            //Image img = SerializeHelper.Desrialize<Image>(ValidateHelper.GetVCode(), null);
            //Assert.IsNotNull(img);
            Assert.Fail("涉及session,请在外层去测试");
            
        }
    }
}
