using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Common
{
    public class CalcHelper
    {

        /// <summary>
        /// 计算增减率返回%比形式字符串
        /// </summary>
        /// <param name="a">本期</param>
        /// <param name="b">同期</param>
        /// <returns></returns>
        public static string CalcDifferenceRate(decimal a, decimal b, int decimals = 2)
        {
            if (b == 0)
            {
                if (a == b)
                {
                    return "0.00%";
                }
                if (a > b)
                {
                    return "100.00%";
                }
                else
                {
                    return "-100.00%";
                }

            }
            string tmp = (((a - b) / b) * 100).ToString();
            if (tmp.IndexOf(".") != -1)
            {
                if (tmp.IndexOf(".") + decimals < tmp.Length)
                {
                    tmp = tmp.Substring(0, tmp.IndexOf(".") + 1 + decimals);
                }
                else
                {
                    tmp += "0";
                }

            }
            else
            {
                tmp += ".00";
            }
            if (a - b <= 0 && a < 0)
            {

                return ("-" + tmp + "%").Replace("--", "-");
            }
            return tmp + "%";

        }


        /// <summary>
        /// 计算增减率返回转成百分比后的decimal
        /// </summary>
        /// <param name="a">本期</param>
        /// <param name="b">同期</param>
        /// <returns></returns>
        public static decimal CalcDifferenceRateReturnDecimal(decimal a, decimal b, int decimals = 2)
        {
            if (b == 0)
            {
                if (a == b)
                {
                    return 0;
                }
                if (a > b)
                {
                    return 100;
                }
                else
                {
                    return -100;
                }

            }
            string tmp = (((a - b) / b) * 100).ToString();
            if (tmp.IndexOf(".") != -1)
            {
                if (tmp.IndexOf(".") + decimals < tmp.Length)
                {
                    tmp = tmp.Substring(0, tmp.IndexOf(".") + 1 + decimals);
                }
                else
                {
                    tmp += "0";
                }
            }
            if (a - b <= 0 && a < 0)
            {
                return decimal.Parse(("-" + tmp).Replace("--", "-"));
            }
            return decimal.Parse(tmp);

        }

        /// <summary>
        /// 除法计算后以百分比形式返回（比率）
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns></returns>
        public static string CalcDivisionAndRate(decimal a, decimal b, int decimals = 2)
        {
            if (b == 0)
            {
                return "0.00%";
            }
            string tmp = ((a / b) * 100).ToString();
            if (tmp.IndexOf(".") != -1)
            {
                if (tmp.IndexOf(".") + decimals < tmp.Length)
                {
                    tmp = tmp.Substring(0, tmp.IndexOf(".") + 1 + decimals);
                }
                else
                {
                    tmp += "0";
                }
            }
            else
            {
                tmp += ".00";
            }

            return tmp + "%";

        }

        /// <summary>
        /// 除法计算后以百分比形式返回（比率）,返回小数
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns></returns>
        public static decimal CalcDivisionAndRateReturnDecimal(decimal a, decimal b, int decimals = 2)
        {
            if (b == 0)
            {
                return 0.00M;
            }
            string tmp = ((a / b) * 100).ToString();
            if (tmp.IndexOf(".") != -1)
            {
                if (tmp.IndexOf(".") + decimals < tmp.Length)
                {
                    tmp = tmp.Substring(0, tmp.IndexOf(".") + 1 + decimals);
                }
                else
                {
                    tmp += "0";
                }

            }
            else
            {
                tmp += ".00";
            }

            return decimal.Parse(tmp);

        }

        /// <summary>
        /// 除法计算
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns></returns>
        public static decimal CalcDivision(decimal a, decimal b, int decimals = 2)
        {
            if (b == 0)
            {
                return 0.00M;
            }
            string tmp = (a / b).ToString();
            if (tmp.IndexOf(".") != -1)
            {
                if (tmp.IndexOf(".") + decimals < tmp.Length)
                {
                    tmp = tmp.Substring(0, tmp.IndexOf(".") + 1 + decimals);
                }
                else
                {
                    tmp += "0";
                }

            }
            else
            {
                tmp += ".00";
            }

            return decimal.Parse(tmp);

        }
    }
}
