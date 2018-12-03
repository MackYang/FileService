using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace System.Tests
{
    [TestClass()]
    public class StringExtensionTests
    {
        [TestMethod()]
        public void isIntTest()
        {
            string a = "2";
            string b = "2.3";
            Assert.IsTrue(a.isInt());
            Assert.IsFalse(b.isInt());
        }

        [TestMethod()]
        public void IsEmailTest()
        {
            string a = "xxx@qq.com";
            string b = "cwqfs";
            Assert.IsTrue(a.IsEmail());
            Assert.IsFalse(b.IsEmail());
        }

        [TestMethod()]
        public void IsWebUrlTest()
        {
            string a = "http://www.bjhhsc.com?ID=xxx";
            string b = "cwqfs";
            Assert.IsTrue(a.IsWebUrl());
            Assert.IsFalse(b.IsWebUrl());
        }

        [TestMethod()]
        public void IsIPAddressTest()
        {
            string a = "192.168.1.1";
            string b = "cwqfs";
            Assert.IsTrue(a.IsIPAddress());
            Assert.IsFalse(b.IsIPAddress());
        }

        [TestMethod()]
        public void IsNumberTest()
        {
            string a = "23.25";
            string b = "cwqfs";
            Assert.IsTrue(a.IsNumber());
            Assert.IsFalse(b.IsNumber());
        }

        [TestMethod()]
        public void IsNumberMoreThanZeroTest()
        {
            string a = "23.25";
            string b = "-5";
            string c = "fe";
            Assert.IsTrue(a.IsNumberMoreThanZero());
            Assert.IsFalse(b.IsNumberMoreThanZero());
            Assert.IsFalse(c.IsNumberMoreThanZero());
        }

        [TestMethod()]
        public void NOSQLTest()
        {
            string a = "update a set i=3 where id=5 or id=9";
            Assert.IsTrue(!a.NOSQL().Contains("or"));
        }

        [TestMethod()]
        public void BxSubstringTest()
        {
            string a = "update a set i=3 where id=5 or id=9";
            Assert.IsTrue(a.BxSubstring(10,true).Length==13);
        }

        [TestMethod()]
        public void GetChineseSpellTest()
        {
            string a = "杨家勇";
            Assert.IsTrue(a.GetChineseSpell()=="YJY");
        }

        [TestMethod()]
        public void NoHTMLTest()
        {
            string a = "<html>update<div style='xxx' class='33'>2fasd</div></html> a set i=3 where id=5 or id=9";
            Assert.IsTrue(!a.NoHTML().Contains("div"));
        }

        [TestMethod()]
        public void ContainElementTest()
        {
            string a = "abc";
            Assert.IsTrue(a.LikeElement(new string[]{"f","c","x"}));
        }

        [TestMethod()]
        public void GetContainFirstElementTest()
        {
            string a = "abc";
            Assert.IsTrue(a.GetLikeFirstElement(new string[] { "f", "c", "x" })=="c");
        }

        [TestMethod()]
        public void ContainsULTest()
        {
            string a = "abc";
            Assert.IsTrue(a.ContainsUL("B"));
        }

        [TestMethod()]
        public void SqlLikeArgsProcessTest()
        {
            string a = "fe%e[a,b,c]i_rs[^d,e,f]";
            string res = a.SqlLikeArgsProcess();
            Assert.IsTrue(res == @"fe\%e\[a,b,c\]i\_rs\[\^d,e,f\]");
        }
    }
}
