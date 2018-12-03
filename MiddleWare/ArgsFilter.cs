using Common;
using SoEasy.Common;
using SoEasy.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MiddleWare
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ArgsFilter : ActionFilterAttribute
    {
        private CommonBL comBL = CommonBL.CreateInstance();

        /// <summary>
        /// 方法执行前被调用
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool flag = false;

            OPResult opRes = new OPResult { State = Enums.OPState.Success };

            try {

                var systemId = "systemid";
                Dictionary<string, object> dic = new Dictionary<string, object>();
                var args = context.ActionParameters;
                if (args != null) {

                    foreach (var item in args.Keys) {
                        dic[item.ToLower()] = args[item];
                    }

                    if (dic.ContainsKey(systemId)) {

                        var id = dic[systemId];

                        if (CommonVarsEx.SystemTable.Select($"id='{id}'").Count() == 1) {

                            flag = true;
                        }

                    }

                }


            }
            catch (Exception ex) {

                Utility.Logger.Error("检查参数时,检测业务系统ID时发生异常:" + ex.Message, ex);
            }

            if (!flag) {

                opRes.State = Enums.OPState.Fail;
                opRes.Data = "非法的系统ID";
                var res = new JsonResult();
                res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                res.Data = opRes;
                context.Result = res;

            }


        }

    }
}
