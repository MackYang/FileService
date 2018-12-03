using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoEasy.Init;
using SoEasy.Model;
using SoEasy.Common;
namespace SoEasy.Logic.Tests
{
    [TestClass()]
    public class SMSBLTests
    {
         SMSBL bl = null;

        CommonBL comBL = null;
        public SMSBLTests()
        {
            InitConfigData.InitSettings("Set.config", null);
            bl = SMSBL.CreateInstance();
            comBL = CommonBL.CreateInstance();
        }

        [TestMethod()]
        public void RecordSendedSMSTest()
        {
            SysRecSmsModel m = new SysRecSmsModel();
            long l = comBL.Count(m, null);
            SMSInfo s=new SMSInfo ();
            s.OperaterID="test";
            s.SMSContent="unit record";
            s.ToPhone="1222222";
            SMSBL.RecordSendedSMS(s);
            long l2 = comBL.Count(m, null);

            Assert.IsTrue(l < l2);
        }
    }
}
