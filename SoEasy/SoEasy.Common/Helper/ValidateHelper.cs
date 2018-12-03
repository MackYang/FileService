using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Common
{
    /// <summary>
    /// 验证相关的辅助类
    /// </summary>
    public class ValidateHelper
    {
        /// <summary>
        /// 对验证码进行验证,通过返回true
        /// </summary>
        /// <param name="vCode">验证码</param>
        static public bool ValidateVCode(string vCode)
        {
            if (!string.IsNullOrWhiteSpace(vCode))
            {
                string code = SessionHelper<string>.GetSessionObject(Constants.SessionKey_VCode);

                if (code != null)
                {
                    return code.ToString().ToLower() == vCode.Trim().ToLower();
                }
            }

            return false;
        }

        /// <summary>
        /// 获取验证码字符串
        /// </summary>
        static public string GetVCodeString()
        {
            string vCode = StringHelper.CreateValidCode(6);
            SessionHelper<string>.SetSessionObject(Constants.SessionKey_VCode, vCode);
            return vCode;
        }

        /// <summary>
        /// 获取验证码图片的base64字符串
        /// </summary>
        static public string GetVCodeImgString(OPResult opRes)
        {
            string vCode = StringHelper.CreateValidCode(6);
            string vImageString = ImageHelper.CreateVerificationImage(vCode, 100, 32, opRes);
            if (string.IsNullOrWhiteSpace(vImageString))
            {
                Utility.Logger.Error("生成验证码图片base64字符串失败");
            }
            else
            {
                SessionHelper<string>.SetSessionObject(Constants.SessionKey_VCode, vCode);
            }
            return vImageString;
        }

    }
}
