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
    public class JsonHelperTests
    {

        public class OPResult
        {
            public int State { get; set; }
            public object Data { get; set; }
        }

        [TestMethod()]
        public void ToJsonStringTest()
        {
            OPResult x = new OPResult();
            x.State = 1;
            x.Data = new OPResult {  State=0,Data="操作失败"};
            Assert.IsTrue(JsonHelper.ToJsonString(x).Length>0);
        }

        [TestMethod()]
        public void FromJsonStringTest()
        {
            OPResult x = new OPResult();
            x.State = 1;
            x.Data = new OPResult { State = 2, Data = "操作失败" };
            string res=JsonHelper.ToJsonString(x);

            OPResult xx = JsonHelper.FromJsonString<OPResult>(res);
            Assert.IsTrue(xx!=null);

            var xxx = JsonHelper.FromJsonString(res);

            OPResult n = JsonHelper.FromJsonString<OPResult>(JsonHelper.ToJsonString(xxx.Data));
            Assert.IsTrue(xxx.Data.State==2);
        }

        
    }
}
