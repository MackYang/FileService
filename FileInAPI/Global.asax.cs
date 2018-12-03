using SoEasy.Common;
using SoEasy.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace FileInAPI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.DOMConfigurator.Configure();
            InitConfigEx.InitSettings(Server.MapPath("~/Set.config"), null);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        //public override void Init()
        //{
        //    base.Init();
        //    string appName = "CW";
        //    IHttpModule module = this.Modules["Session"];
        //    SessionStateModule ssm = module as SessionStateModule;
        //    if (ssm != null)
        //    {
        //        FieldInfo storeInfo = typeof(SessionStateModule).GetField("_store", BindingFlags.Instance | BindingFlags.NonPublic);
        //        SessionStateStoreProviderBase store = (SessionStateStoreProviderBase)storeInfo.GetValue(ssm);
        //        if (store == null)//In IIS7 Integrated mode, module.Init() is called later
        //        {
        //            FieldInfo runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.Static | BindingFlags.NonPublic);
        //            HttpRuntime theRuntime = (HttpRuntime)runtimeInfo.GetValue(null);
        //            FieldInfo appNameInfo = typeof(HttpRuntime).GetField("_appDomainAppId", BindingFlags.Instance | BindingFlags.NonPublic);
        //            appNameInfo.SetValue(theRuntime, appName);
        //        }
        //        else
        //        {
        //            Type storeType = store.GetType();
        //            if (storeType.Name.Equals("OutOfProcSessionStateStore"))
        //            {
        //                FieldInfo uribaseInfo = storeType.GetField("s_uribase", BindingFlags.Static | BindingFlags.NonPublic);
        //                uribaseInfo.SetValue(storeType, appName);
        //            }
        //        }

        //    }

        //}
    }
}