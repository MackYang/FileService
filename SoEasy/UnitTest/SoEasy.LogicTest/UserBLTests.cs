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
    public class UserBLTests
    {
        CommonBL comBL = null;
        UserBL userBL = null;
        public UserBLTests()
        {
            InitConfigData.InitSettings("Set.config", null);
            comBL = CommonBL.CreateInstance();
            userBL = UserBL.CreateInstance();

        }
        [TestMethod()]
        public void GetUserByXTest()
        {
            string x = "553030761@qq.com";
           SysUserModel user=userBL.GetUserByX(x, null);
           Assert.IsNotNull(user);
        }

        [TestMethod()]
        public void IsExistsUserNameTest()
        {
            string userName = "admin";
            Assert.IsTrue(userBL.IsExistsUserName(userName,null));
        }

        [TestMethod()]
        public void IsExistsPhoneNumTest()
        {
            Assert.IsTrue(userBL.IsExistsPhoneNum("15687051030", null));
        }

        [TestMethod()]
        public void IsExistsEmailTest()
        {
            Assert.IsTrue(userBL.IsExistsEmail("553030761@qq.com", null));
        }
    }
}
