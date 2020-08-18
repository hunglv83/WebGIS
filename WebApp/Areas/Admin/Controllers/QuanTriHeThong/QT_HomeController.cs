using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Common;
using WebApp.Core.EF;
using WebApp.Core.DAO;
using WebApp.App_Start;
using WebApp.Core;

namespace WebApp.Areas.Admin.Controllers
{
    public class QT_HomeController : BaseController
    {
        [CheckPermission]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        #region MENU
        public PartialViewResult AdminMenu()
        {
            try
            {
                DT_WebGISEntities entities = new DT_WebGISEntities();
                CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                string username = HttpContext.User.Identity.Name;
                int intGuestGroup = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IDGuestGroup"]);
                List<int> lRoleID = objUserDao.GetRoleIDByUserName(username, intGuestGroup);
                string stringRoleID = String.Join(",", lRoleID);
                //
                List<CSF_Pages> listAllPage = new List<CSF_Pages>();
                if (username.Trim().ToLower() != "host")
                {
                    var lPageActiveID = entities.CSF_Pages_GetPageByRoleID(stringRoleID).Select(x => (int)x.id).ToList();
                    listAllPage = entities.CSF_Pages
                        .Where(x => lPageActiveID.Contains(x.ID) && x.IsAdmin == true)
                        .OrderBy(x => x.Order)
                        .Join(entities.CSF_Modules,
                                pg => pg.ModuleID, // Primary Key
                                md => md.ID, // Foreign Key
                                (pg, md) => new { CSF_Pages = pg, CSF_Modules = md })
                        .Where(pgm => pgm.CSF_Modules.IsActive == true)
                        .Select(pgm => pgm.CSF_Pages).ToList();
                }
                else
                {
                    //listAllPage = entities.CSF_Pages.Where(x => x.IsAdmin == true && x.IsBlank == false && x.IsActive == true).OrderBy(x => x.Order).ToList();
                    listAllPage = entities.CSF_Pages
                        .Where(x => x.IsAdmin == true && x.IsBlank == false && x.IsActive == true)
                        .OrderBy(x => x.Order)
                        .Join(entities.CSF_Modules,
                                pg => pg.ModuleID, // Primary Key
                                md => md.ID, // Foreign Key
                                (pg, md) => new { CSF_Pages = pg, CSF_Modules = md })
                        .Where(pgm => pgm.CSF_Modules.IsActive == true)
                        .Select(pgm => pgm.CSF_Pages).ToList();
                }
                //
                string stringMenu = BuildTreeMenu(listAllPage);
                MainMenu mainMenu = new MainMenu();
                mainMenu.stringMenu = stringMenu;
                return PartialView(mainMenu);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        private string BuildTreeMenu(List<CSF_Pages> listAllMenu)
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"] + "/admin/";
                url = url.ToLower();
                string stringMenu = "<ul class='nav menu nav-pills nav-sidebar flex-column' data-widget='treeview' role='menu' data-accordion='false'>";
                var parentMenu = listAllMenu.Where(x => x.ParentID == 0 || x.ParentID == null).OrderBy(m => m.Order).ToList();
                int level = 1;
                foreach (var menu in parentMenu)
                {
                    level = 1;
                    var childMenu = listAllMenu.Where(x => x.ParentID == menu.ID).OrderBy(m => m.Order).ToList();
                    if (childMenu.Any())
                    {
                        stringMenu += "<li class='nav-item has-treeview'>";
                        stringMenu += "<a href='#' class='nav-link navParent'>";
                        stringMenu += menu.Icon;
                        stringMenu += "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "<i class='right fas fa-angle-left'></i></p>";
                        stringMenu += "</a>";
                        stringMenu += GetSubMenu(childMenu, listAllMenu, url, level);
                        stringMenu += "</li>";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(menu.Key))
                            stringMenu += "<li class='nav-item'><a class='nav-link' href='" + url + menu.Key + "'>" + menu.Icon + "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "</p></a></li>";
                        else
                            stringMenu += "<li class='nav-item'><a class='nav-link' style='cursor: pointer'>" + menu.Icon + "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "</p></a></li>";
                    }
                }
                stringMenu += "</ul>";
                return stringMenu;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetSubMenu(List<CSF_Pages> childMenu, List<CSF_Pages> listAllMenu, string url, int level)
        {
            try
            {
                level++;
                string submenu = "";
                if (level == 2)
                {
                    submenu += "<ul class='nav nav-treeview'>";
                }
                else
                {
                    submenu += "<ul class='nav nav-treeview'>";
                }
                foreach (var menu in childMenu)
                {
                    var xChildPage = listAllMenu.Where(x => x.ParentID == menu.ID).OrderBy(m => m.Order).ToList();
                    if (xChildPage.Any())
                    {
                        submenu += "<li class='nav-item has-treeview'>";
                        submenu += "<a href='#' class='nav-link navParent'>";
                        submenu += menu.Icon;
                        submenu += "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "<i class='right fas fa-angle-left'></i></p>";
                        submenu += "</a>";
                        submenu += GetSubMenu(xChildPage, listAllMenu, url, level);
                        submenu += "</li>";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(menu.Key))
                            submenu += "<li class='nav-item'><a class='nav-link' href='" + url + menu.Key + "'>" + menu.Icon + "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "</p></a></li>";
                        else
                            submenu += "<li class='nav-item'><a class='nav-link' style='cursor: pointer'>" + menu.Icon + "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "</p></a></li>";
                    }
                }
                submenu += "</ul>";
                return submenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
