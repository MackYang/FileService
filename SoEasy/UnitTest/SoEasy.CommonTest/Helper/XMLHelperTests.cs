using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
namespace SoEasy.Common.Tests
{
    [TestClass()]
    public class XMLHelperTests
    {
        string file = "test.xml";
        [TestMethod()]
        public void GetXmlNodeAttributeTest()
        {
            SetXmlNodeAttributeTest();
            Assert.IsTrue(XMLHelper.GetXmlNodeAttribute(file, "Root/Property", "name",null) == "属性1");
        }

        [TestMethod()]
        public void SetXmlNodeAttributeTest()
        {
            Assert.IsTrue(XMLHelper.SetXmlNodeAttribute(file, "Root/Property", "name", "属性1",null));
        }

        [TestMethod()]
        public void GetXmlNodeValueTest()
        {
            SetXmlNodeValueTest();
            Assert.IsTrue(XMLHelper.GetXmlNodeValue(file, "Root/Node",null) == "this is Node Data1");
            
        }

        [TestMethod()]
        public void SetXmlNodeValueTest()
        {
            Assert.IsTrue(XMLHelper.SetXmlNodeValue(file, "Root/Node", "this is Node Data1",null));
        }

        [TestMethod()]
        public void GetXmlNodeCollectValueTest()
        {
            List<string> list = XMLHelper.GetXmlNodeCollectValue(file, "Root/SSOSites/SSOURL", null);
            Assert.IsTrue(list!=null&&list.Count==4);
        }

        [TestMethod()]
        public void GetXmlNodeCollectValueTest1()
        {
            Hashtable ht = XMLHelper.GetHashtableFromXmlNodeCollectAttr(file, "Root/LoginTargetURL/TargetURL", "UserType", "TargetURL", null);
            Assert.IsTrue(ht!=null&&ht.Count==3);
        }
    }
}
