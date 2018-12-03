using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LitJson;
using SoEasy.Common;
namespace System.Tests
{
    [TestClass()]
    public class StringHelperTests
    {
        [TestMethod()]
        public void ToJsonDataTest()
        {
            string str="{'Name':'IPhone','Rev':[4,5,6],'Price':2222}";
            JsonData data= StringHelper.ToJsonData(str);
            Assert.IsNotNull(data);
        }

        [TestMethod()]
        public void GetMaxSameStringTest()
        {
            string res = StringHelper.GetMaxSameString("jweabcdrwa", "iabcabcdlkw").First();
            Assert.IsTrue(res=="abcd");
        }

        [TestMethod()]
        public void CreateValidCodeTest()
        {
            Assert.IsTrue(StringHelper.CreateValidCode(5).Length==5);
        }
    }
}
