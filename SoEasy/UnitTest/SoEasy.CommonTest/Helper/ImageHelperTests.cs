using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoEasy.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Drawing.Imaging;
namespace SoEasy.Common.Tests
{
    [TestClass()]
    public class ImageHelperTests
    {
        [TestMethod()]
        public void CreateThumbnailTest()
        {
            Assert.IsTrue(ImageHelper.CreateThumbnail("img.jpg", "img_T.jpg", 100, 70,null));
        }

        [TestMethod()]
        public void IsImageTest()
        {
            Assert.IsTrue(ImageHelper.IsImage("abc.bmp"));
            Assert.IsFalse(ImageHelper.IsImage("abc.bvsmp"));
            
        }

        [TestMethod()]
        public void AddWatermarkTextTest()
        {
            Assert.IsTrue(ImageHelper.AddWatermarkText("img.jpg", "img_W.jpg", 16, "水印哦",null));
        }

        [TestMethod()]
        public void ImageToBase64Test()
        {
            Image img = Image.FromFile("img.jpg");
            Assert.IsTrue(SerializeHelper.SerializeObject(img,null).Length>0);
        }

        [TestMethod()]
        public void Base64ToImageTest()
        {
            Image img = Image.FromFile("img.jpg");
            string base64 = SerializeHelper.SerializeObject(img,null);
            Image imgNew = SerializeHelper.Desrialize<Image>(base64,null);
            Assert.IsTrue(imgNew != null && imgNew.Width > 0);
        }

        [TestMethod()]
        public void CreateVerificationImageTest()
        {
            string imgStr = ImageHelper.CreateVerificationImage("123Acd", 100, 50,null);
            Assert.IsNotNull(imgStr);
        }
    }
}
