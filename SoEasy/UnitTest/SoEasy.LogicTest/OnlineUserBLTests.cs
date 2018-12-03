using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoEasy.Init;
using SoEasy.Model;
namespace SoEasy.Logic.Tests
{
    [TestClass()]
    public class OnlineUserBLTests
    {
       OnlineUserBL bl = null;

        CommonBL comBL = null;
        public OnlineUserBLTests()
        {
            InitConfigData.InitSettings("Set.config", null);
            bl = OnlineUserBL.CreateInstance();
            comBL = CommonBL.CreateInstance();
        }

        [TestMethod()]
        public void AddOnlineUserTest()
        {
            SysOnlineUserModel m = new SysOnlineUserModel();
            long l = comBL.Count(m, null);
            bl.AddOnlineUser("192.168.1.1", null, "Lib.unit");
            long l2 = comBL.Count(m, null);

            Assert.IsTrue(l < l2);
        }

        [TestMethod()]
        public void DeleteOnlineUserTest()
        {
            SysOnlineUserModel m = new SysOnlineUserModel();
            long l = comBL.Count(m, null);
            bl.DeleteOnlineUser(null, "192.168.1.1");
            long l2 = comBL.Count(m, null);

            Assert.IsTrue(l >= l2);

        }

        [TestMethod()]
        public void ClearAllOnlineUserTest()
        {
            bl.ClearAllOnlineUser();
            SysOnlineUserModel m = new SysOnlineUserModel();
            long l = comBL.Count(m, null);
            Assert.IsTrue(l==0);
        }
    }
}
