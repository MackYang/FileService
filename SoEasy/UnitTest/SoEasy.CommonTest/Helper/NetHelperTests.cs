using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoEasy.Init;
namespace SoEasy.Common.Tests
{
    [TestClass()]
    public class NetHelperTests
    {

        static NetHelperTests()
        {
            InitConfigData.InitSettings("set.config", null);
        }

        [TestMethod()]
        public void GetClientIPTest()
        {
            Assert.Fail("涉及web请求,请在外层去测试");
        }


        [TestMethod()]
        public void QueryIPInfoIP138Test()
        {
            Assert.IsNotNull(NetHelper.QueryIPInfoIP138("43.226.30.21", null));
        }

        [TestMethod()]
        public void QueryIPInfoBaiDuTest()
        {
            Assert.IsNotNull(NetHelper.QueryIPInfoBaiDu("43.226.30.21", null));
        }

        [TestMethod()]
        public void AsyncSendEmailTest()
        {
            try
            {
                MailInfo mi = new MailInfo();
                mi.ToMail = "553030761@qq.com";
                mi.Content = "UnitTest..<a href='bjhhsc.com'>登录</a>";
                mi.OperaterID = "yh";
                mi.Title = "UnitTest";

                OPResult opRes = new OPResult { State = Enums.OPState.Success };
                NetHelper.AsyncSendEmail(mi, opRes);

                Assert.IsTrue(opRes.State==Enums.OPState.Success);
            }
            catch (Exception)
            {
                Assert.Fail();
            }


        }

        [TestMethod()]
        public void AsyncSendSMSTest()
        {
            try
            {
                SMSInfo s = new SMSInfo();
                s.OperaterID = "yh";
                s.SMSContent = "unitTest";
                s.ToPhone = "15687051030";

                OPResult opRes = new OPResult { State = Enums.OPState.Success };
                NetHelper.AsyncSendSMS(s, opRes);

                Assert.IsTrue(opRes.State == Enums.OPState.Success);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

    }
}
