using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoEasy.Model;
using SoEasy.Common;
using SoEasy.Init;
namespace SoEasy.Logic.Tests
{
    [TestClass()]
    public class MailBLTests
    {
        MailBL bl = null;

        CommonBL comBL = null;
        public MailBLTests()
        {
            InitConfigData.InitSettings("Set.config", null);
            bl = MailBL.CreateInstance();
            comBL = CommonBL.CreateInstance();
        }

        [TestMethod()]
        public void RecordSendedMailTest()
        {
            SysRecMailModel m = new SysRecMailModel();
            long l = comBL.Count(m, null);
            MailInfo mi = new MailInfo();
            mi.Content = "unit mail";
            mi.OperaterID = "xxrec";
            mi.Title = "mileTitle";
            mi.ToMail = "xdfe";
            MailBL.RecordSendedMail(mi);
            long l2 = comBL.Count(m, null);

            Assert.IsTrue(l < l2);

        }
    }
}
