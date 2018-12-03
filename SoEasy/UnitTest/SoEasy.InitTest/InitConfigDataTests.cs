using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Init;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace SoEasy.Init.Tests
{
    [TestClass()]
    public class InitConfigDataTests
    {
        string file = "Set.config";
        [TestMethod()]
        public void InitVarsTest()
        {
            //log4net.Config.DOMConfigurator.Configure();
            //Utility.Logger.Error("测试日志");//在上行配置后日志已成功写入
            Assert.Fail("涉及web请求,请在外层测试");

        }
    }
}
