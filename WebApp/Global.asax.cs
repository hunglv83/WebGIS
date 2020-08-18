using AutoMapper;
using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

            Application["Visitors"] = 0;
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //Automapper
            Mapper.Initialize(m => m.AddProfile<MappingProfile>());
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var httpContext = ((HttpApplication)sender).Context;
            httpContext.Response.Clear();
            httpContext.ClearError();

            if (new HttpRequestWrapper(httpContext.Request).IsAjaxRequest())
            {
                return;
            }
        }

        protected void Session_Start()
        {
            try
            {
                string source_xml = Server.MapPath("~/SoLanTruyCap.xml");//Utilities.SiteURL() + "/SoLanTruyCap.xml";

                Application.Lock();
                int hits = Convert.ToInt32(Application["Visitors"]);
                Application["Visitors"] = hits + 1;

                DataSet tmpDs = new DataSet();
                tmpDs.ReadXml(source_xml);

                int hitsx = Int32.Parse(tmpDs.Tables[0].Rows[0]["hits"].ToString());

                hitsx += 1;

                tmpDs.Tables[0].Rows[0]["hits"] = hitsx.ToString();
                int today_hit = Int32.Parse(tmpDs.Tables[0].Rows[0]["today_hit"].ToString());
                DateTime today = DateTime.Today;
                DateTime datetime = Convert.ToDateTime(tmpDs.Tables[0].Rows[0]["Datetime"].ToString());
                if (today.Date != datetime.Date)
                    today_hit = 1;
                else
                    today_hit += 1;
                tmpDs.Tables[0].Rows[0]["today_hit"] = today_hit.ToString();
                tmpDs.Tables[0].Rows[0]["Datetime"] = today.ToString();
                tmpDs.WriteXml(source_xml);
                Application.UnLock();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                Application.Lock();
                int hits = Convert.ToInt32(Application["Visitors"]);
                Application["Visitors"] = hits - 1;
                Application.UnLock();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
            }
        }
    }
}