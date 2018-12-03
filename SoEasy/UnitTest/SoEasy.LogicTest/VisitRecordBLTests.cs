using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoEasy.Init;
using SoEasy.Common;
using SoEasy.Model;
namespace SoEasy.Logic.Tests
{
    [TestClass()]
    public class VisitRecordBLTests
    {
        VisitRecordBL bl = null;
        
        CommonBL comBL = null;
        public VisitRecordBLTests()
        {
            InitConfigData.InitSettings("Set.config", null);
            bl = VisitRecordBL.CreateInstance();
            comBL = CommonBL.CreateInstance();
        }

        [TestMethod()]
        public void GetIpInfoFromDBTest()
        {
            SysRecVisitModel m = new SysRecVisitModel();
            m = comBL.SelectFirst(m, null);
            m.Create_Time = DateTime.Now;
            comBL.Update(m, null);

            string ip = "127.0.0.1";
            IPInfo info= bl.GetIpInfoFromDB(ip);
            Assert.IsNotNull(info);
        }

        [TestMethod()]
        public void AddVisitRecordTest()
        {
            SysRecVisitModel m = new SysRecVisitModel();
            long l = comBL.Count(m,null);
            bl.AddVisitRecord("192.168.1.4", 1);
            long l2 = comBL.Count(m, null);

            Assert.IsTrue(l<l2);
        }
    }
}
