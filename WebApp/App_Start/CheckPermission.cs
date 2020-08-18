using WebApp.Common;
using WebApp.Core.DAO;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;              
using System.Web.Routing;

namespace WebApp.App_Start
{
    //HOANGND
    public class CheckPermissionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var descriptor = filterContext.ActionDescriptor;
                var actionName = descriptor.ActionName;
                var controllerName = descriptor.ControllerDescriptor.ControllerName;
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                string username = filterContext.HttpContext.User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    int intGuestGroup = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IDGuestGroup"]);
                    List<int> listUserRole = objUserDao.GetRoleIDByUserName(username, intGuestGroup);
                    string ControllerAction = controllerName + "-" + actionName;
                    var ListPermission = (from a in MyContext.CSF_RoleFunction
                                          join b in MyContext.CSF_Functions on a.FunctionID equals b.ID
                                          where listUserRole.Contains(a.RoleID)
                                          select new { ca = b.Controller_Action.ToLower() }).ToList();
                    //write log
                    int intUserID = objUserDao.GetUserIDByUserName(username);
                    if (intUserID > 0)
                    {
                        CSF_Logs objLog = new CSF_Logs();
                        objLog.Controller_Action = controllerName + "-" + actionName;
                        objLog.CreateDate = System.DateTime.Now;
                        objLog.UserCreate = intUserID;
                        //objLog.Content = descriptor.ToString();
                        MyContext.CSF_Logs.Add(objLog);
                        MyContext.SaveChanges();
                    }
                    var permission = ListPermission.Where(x => x.ca.Contains(ControllerAction.ToLower())).FirstOrDefault();
                    if (permission == null && username.ToLower().Trim() != "host")
                    {
                        base.OnActionExecuting(filterContext);
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "home" }, { "action", "index" }, { "area", "" } });
                    }
                }
                else
                {
                    //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "home" }, { "action", "login" }, { "area", "" } });

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                        { "Area", "" },
                        { "Controller", "home" },
                        { "Action", "login" },
                        { "RedirectUrl", filterContext.HttpContext.Request.Url }
                   }
           );
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
            }
        }
    }
}
