using SoEasy.Common;
using SoEasy.Model;
using SoEasy.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoEasy.Logic
{
    public class LogBL
    {
        static LogBL instance = null;
        static CommonBL comBL = CommonBL.CreateInstance();
        static object locker = new object();

        private LogBL()
        {

        }
        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns></returns>
        public static LogBL CreateInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance = new LogBL();
                }
            }
            return instance;
        }

        /// <summary>
        /// 删除某段时间内某级别的日志
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="logLevel"></param>
        /// <param name="opRes"></param>
        /// <returns></returns>
        public bool DeleteLog(string beginDate, string endDate, Enums.LogLevel logLevel, OPResult opRes)
        {
            bool flag = false;

            SysLogModel model = new SysLogModel();

            NotEqualCondition nec = new NotEqualCondition();

            if (!string.IsNullOrWhiteSpace(beginDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    nec.AddCondition("logtime>=:beginDate", "beginDate", beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    nec.AddCondition("logtime<=:endDate", "endDate", endDate);
                }
                
                if (logLevel != Enums.LogLevel.All)
                {
                    nec.AddCondition("Loglevel=:logLevel", "logLevel", logLevel.ToString());
                }
                model.OtherCondition = nec;

                flag = comBL.Delete(model, opRes);
            }

            return flag;
        }


        /// <summary>
        /// 日志查询
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="platform">平台类型</param>
        /// <param name="logLevel">日志级别</param>
        /// <param name="pager">分页对象</param>
        /// <param name="opRes"></param>
        /// <returns></returns>
        public DataTable QueryLoginfo(string keyword, string beginDate, string endDate, int platform, Enums.LogLevel logLevel, Pager pager, OPResult opRes)
        {
            SysLogModel model = new SysLogModel();

            NotEqualCondition nec = new NotEqualCondition();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                nec.ConditionSQL = comBL.GetKeywordSQLStrict(keyword, nec.ArgsArr, "Logmessage", "Logger");
            }
            if (!string.IsNullOrWhiteSpace(beginDate))
            {
                nec.AddCondition("logtime>=:beginDate", "beginDate", beginDate);
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                nec.AddCondition("logtime<=:endDate", "endDate", endDate);
            }
            if (platform != -1)
            {
                model.Platform = platform;
            }
            if (logLevel != Enums.LogLevel.All)
            {
                nec.AddCondition("Loglevel=:logLevel", "logLevel", logLevel.ToString());
            }

            model.OtherCondition = nec;

            return comBL.SelectPage(model, pager, null, opRes, "LogTime");
        }

        /// <summary>
        /// 统计日志总记录条数,主要是为了在写返回值为void类型的方法时,通过此数据来判断在方法运行中是否发生错误
        /// </summary>
        /// <param name="opRes"></param>
        /// <returns></returns>
        public long GetLogCount(OPResult opRes)
        {
            SysLogModel m = new SysLogModel();
            return comBL.Count(m, opRes);
        }

        /// <summary>
        /// 写入日志,供客户端写入日志用,后台写日志不要调用此方法,直接用Utility.Logger.Error(...)就可以
        /// </summary>
        /// <param name="logMessage">日志内容</param>
        /// <param name="platformType">平台类型</param>
        /// <param name="level">日志级别</param>
        /// <returns></returns>
        public bool WriteLog(string logMessage,int platformType, OPResult opRes, string level = "Error")
        {
            SysLogModel log = new SysLogModel();
            log.Logger = "客户端日志";
            log.Loglevel = level;
            log.Logmessage = logMessage;
            log.Logtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            log.Platform = platformType;
            return comBL.Insert(log, opRes);
        }
    }
}
