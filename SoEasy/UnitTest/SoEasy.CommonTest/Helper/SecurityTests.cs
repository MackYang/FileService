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
    public class SecurityTests
    {
        string input = "杨JY";
        string outPut = "1E8808ABDB28BD2F";
        [TestMethod()]
        public void EncryptTest()
        {
            outPut=SecurityHelper.Encrypt(input,null);
            Assert.IsTrue(outPut.Length>0);
        }

        [TestMethod()]
        public void DecryptTest()
        {
            Assert.IsTrue(SecurityHelper.Decrypt(outPut,null)==input);
        }

        [TestMethod()]
        public void MD5Test()
        {
            Assert.IsTrue(SecurityHelper.MD5(input,null).Length == 32);
        }

        
    }
}
