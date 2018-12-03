using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoEasy.Common;
using SoEasy.Init;
namespace SoEasy.Logic.Tests
{
    [TestClass()]
    public class AddressBLTests
    {

        AddressBL bl = null;
        public AddressBLTests()
        {
            InitConfigData.InitSettings("Set.config", null);
            bl = AddressBL.CreateInstance();
        }


        [TestMethod()]
        public void GetSubAddressListTest()
        {
            Assert.IsTrue(bl.GetSubAddressList("+86").Rows.Count == 34);
        }
    }
}
