using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp.Common
{
    public class BaseController : Controller
    {
        //public string strHoSoDirectory = System.Configuration.ConfigurationManager.AppSettings["HoSoDirectory"].ToString();
        //public string strKhuVucCamDirectory = System.Configuration.ConfigurationManager.AppSettings["KhuVucCamDirectory"].ToString();
        //public string strKhuVucDuTruKSQGDirectory = System.Configuration.ConfigurationManager.AppSettings["KhuVucDuTruKSQGDirectory"].ToString();
        private string siteUrl = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"];

        #region protected Variables

        protected bool HasPermission { get; private set; }
        protected string ControllerTitle { get; private set; }
        protected string ControllerAction { get; private set; }
        protected string ControllerName { get; private set; }

        #endregion protected Variables

        #region Public Constructor Functions

        public BaseController()
        {
            HasPermission = false;
            ControllerTitle = string.Empty;
            ControllerName = string.Empty;
            ControllerAction = string.Empty;
        }

        #endregion Public Constructor Functions

        protected void ClearTempData()
        {
            foreach (var key in TempData.Keys.ToList())
            {
                TempData.Remove(key);
            }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            try
            {
                base.Initialize(requestContext);
                RouteData routedata = requestContext.RouteData;
                object routes;
                if (routedata.Values.TryGetValue("MS_DirectRouteMatches", out routes))
                {
                    routedata = (routes as List<RouteData>).FirstOrDefault();
                }
                if (routedata == null)
                    return;
                Func<string, string> getValue = (s) =>
                {
                    object o;
                    return routedata.Values.TryGetValue(s, out o) ? o.ToString() : String.Empty;
                };

                this.ControllerAction = getValue("action");
                this.ControllerName = getValue("controller");

                //SetPermissions();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetPermissions()
        {
            try
            {
                ////Kiểm tra user login hay chưa?
                //DT_WebGISEntities MyContext = new DT_WebGISEntities();
                //CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                //string username = HttpContext.User.Identity.Name;
                //List<int> listUserRole = objUserDao.GetRoleIDByUserName(username);
                //string ControllerAction = this.ControllerName + "-" + this.ControllerAction;
                //var ListPermission = (from a in MyContext.CSF_RoleFunction
                //                      join b in MyContext.CSF_Functions on a.FunctionID equals b.ID
                //                      where listUserRole.Contains(a.RoleID)
                //                      select new { ca = b.Controller_Action.Trim() }).ToList();
                //var permission = ListPermission.Where(x => x.ca.Contains(ControllerAction.Trim())).FirstOrDefault();
                //if (permission != null)
                //{
                //    this.HasPermission = true;
                //}
                //else
                //{
                //    this.HasPermission = false;
                //}
                //return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ////Set Alert
        protected void SetAlert(string message, string type)
        {
            @TempData["AlertMessage"] = message;
            if (type.ToLower().Trim() == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type.ToLower().Trim() == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type.ToLower().Trim() == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}