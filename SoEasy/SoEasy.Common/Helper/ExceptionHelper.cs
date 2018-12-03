using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Common
{
    /// <summary>
    /// 异常辅助类
    /// </summary>
    public class ExceptionHelper
    {
        /// <summary>
        /// 没有参数,且返回值为T类型的委托
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <returns></returns>
        public delegate T ExDelegate<T>();
        /// <summary>
        /// 记录异常信息,并可控制是否要抛出异常
        /// </summary>
        /// <typeparam name="T">code返回的值</typeparam>
        /// <param name="code">要执行的代码</param>
        /// <param name="throwException">出现异常时是否要抛出</param>
        /// <returns></returns>
        public static T ExceptionRecord<T>(ExDelegate<T> code, OPResult opRes, bool throwException)
        {
            //如果检测到已经处于异常状态就不往下执行了
            if (opRes != null && opRes.State == Enums.OPState.Exception)
            {
                return default(T);
            }

            try
            {
                return code.Invoke();
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(GetExceptionMessage(ex).TrimEnd(':'), ex);
                if (opRes != null)
                {
#if DEBUG
                    opRes.Data = ex.Message;
#else
                opRes.Data = "操作异常,请重试.(我们已收到此次异常信息,将尽快解决.)";
#endif
                    opRes.State = Enums.OPState.Exception;
                }
#if !DEBUG
                SendExceptionNotifyMail(ex);
#endif

                if (throwException)
                {
                    throw ex;
                }
                return default(T);
            }

        }

        /// <summary>
        /// 记录异常信息,并可控制是否要抛出异常
        /// </summary>
        /// <param name="code">要执行的代码</param>
        /// <param name="throwException">出现异常时是否要抛出</param>
        /// <returns></returns>
        public static void ExceptionRecord(Action code, OPResult opRes, bool throwException)
        {
            //如果检测到已经处于异常状态就不往下执行了
            if (opRes != null && opRes.State == Enums.OPState.Exception)
            {
                return;
            }

            try
            {
                code.Invoke();
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(GetExceptionMessage(ex).TrimEnd(':'), ex);
                if (opRes != null)
                {
#if DEBUG
                    opRes.Data = ex.Message;
#else
                opRes.Data = "操作异常,请重试.(我们已收到此次异常信息,将尽快解决.)";
#endif
                    opRes.State = Enums.OPState.Exception;
                }
#if !DEBUG
                SendExceptionNotifyMail(ex);
#endif

                if (throwException)
                {
                    throw ex;
                }

            }

        }

        /// <summary>
        /// 发送异常通知邮件
        /// </summary>
        /// <param name="ex">异常信息</param>
        private static void SendExceptionNotifyMail(Exception ex)
        {
            MailInfo mi = new MailInfo();
            mi.ToMail = Vars.ExceptionNotifyEmail;
            mi.Title = Vars.SiteName + " 站点于 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 发生了异常";
            mi.Content = GetExceptionMessage(ex);
            mi.OperaterIP = "127.0.0.1";
            NetHelper.AsyncSendEmail(mi,Constants.OK);
        }

        /// <summary>
        /// 获取异常信息,包括内部异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string GetExceptionMessage(Exception ex)
        {
            if (ex != null)
            {
                return ex.Message + ":" + GetExceptionMessage(ex.InnerException);
            }
            return "";
        }
    }
}
