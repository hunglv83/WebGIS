using WebApp.Common;
using WebApp.Core.DAO;
using WebApp.Core.EF;
using WebApp.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class SystemController : BaseController
    {
        //
        // GET: /System/

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult MainMenu()
        {
            try
            {
                int intGuestGroup = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IDGuestGroup"]);
                DT_WebGISEntities entities = new DT_WebGISEntities();
                CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                string username = HttpContext.User.Identity.Name;
                List<int> lRoleID = objUserDao.GetRoleIDByUserName(username, intGuestGroup);
                string stringRoleID = String.Join(",", lRoleID);
                //
                List<CSF_Pages> listAllPage = new List<CSF_Pages>();
                if (username.Trim().ToLower() != "host")
                {
                    //var lPageActiveID = entities.CSF_Pages_GetPageByRoleID(stringRoleID).Select(x => (int)x.id).ToList();
                    //listAllPage = entities.CSF_Pages.Where(x => lPageActiveID.Contains(x.ID) && x.IsAdmin == false).OrderBy(x => x.Order).ToList();

                    var lPageActiveID = entities.CSF_Pages_GetPageByRoleID(stringRoleID).Select(x => (int)x.id).ToList();
                    listAllPage = entities.CSF_Pages
                        .Where(x => lPageActiveID.Contains(x.ID) && x.IsAdmin == false)
                        .Join(entities.CSF_Modules,
                                pg => pg.ModuleID, // Primary Key
                                md => md.ID, // Foreign Key
                                (pg, md) => new { CSF_Pages = pg, CSF_Modules = md })
                        .Where(pgm => pgm.CSF_Modules.IsActive == false)
                        .Select(pgm => pgm.CSF_Pages)
                        .OrderBy(x => x.Order)
                        .ToList();
                }
                else
                {
                    //listAllPage = entities.CSF_Pages.Where(x => x.IsAdmin == false && x.IsBlank == false && x.IsActive == true).OrderBy(x => x.Order).ToList();

                    listAllPage = entities.CSF_Pages
                        .Where(x => x.IsAdmin == false && x.IsBlank == false && x.IsActive == true)
                        .Join(entities.CSF_Modules,
                                pg => pg.ModuleID, // Primary Key
                                md => md.ID, // Foreign Key
                                (pg, md) => new { CSF_Pages = pg, CSF_Modules = md })
                        .Where(pgm => pgm.CSF_Modules.IsActive == false)
                        .Select(pgm => pgm.CSF_Pages)
                        .OrderBy(x => x.Order)
                        .ToList();
                }
                //
                string stringMenu = buildTreeMenu(listAllPage);
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

        private string buildTreeMenu(List<CSF_Pages> listAllPage)
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"] + "/";
                string stringMenu = "<ul class='nav navbar-nav'>";
                var parentPage = listAllPage.Where(x => x.ParentID == 0).ToList();
                string rootKey = "";
                int rootID = 0;
                foreach (var page in parentPage)
                {
                    var childPage = listAllPage.Where(x => x.ParentID == page.ID).ToList();
                    if (childPage.Any())
                    {
                        rootKey = page.Key;
                        rootID = page.ID;
                        stringMenu += "<li class='dropdown' id='page-" + rootID + "'>";
                        stringMenu += "<a class='dropdown-toggle disabled' data-toggle='dropdown' href='" + url + rootKey.ToLower() + "'>" + page.Name + " <span class='caret'></span></a>";
                        stringMenu += getSubMenu(childPage, listAllPage, rootKey, url);
                        stringMenu += "</li>";
                    }
                    else
                    {
                        stringMenu += "<li id='page-" + page.ID + "'><a href='" + url + page.Key.ToLower() + "'>" + page.Name + "</a></li>";
                    }
                }
                stringMenu += "</ul>";
                return stringMenu;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        private string getSubMenu(List<CSF_Pages> childPage, List<CSF_Pages> listAllPage, string rootKey, string url)
        {
            try
            {
                string submenu = "<ul class='dropdown-menu' style='display: none;'>";
                foreach (var page in childPage)
                {
                    var xChildPage = listAllPage.Where(x => x.ParentID == page.ID).ToList();
                    if (xChildPage.Any())
                    {
                        submenu += "<li class='dropdown-submenu' id='page-" + page.ID + "'>";
                        submenu += "<a class='mutiple-menu' tabindex='-1' href='" + url + rootKey.ToLower() + "/" + page.Key.ToLower() + "'>" + page.Name + " <i class='fas fa-caret-right'></i></a>";
                        submenu += getSubMenu(xChildPage, listAllPage, rootKey, url);
                        submenu += "</li>";
                    }
                    else
                    {
                        submenu += "<li id='page-" + page.ID + "'><a href='" + url + rootKey.ToLower() + "/" + page.Key.ToLower() + "'>" + page.Name + "</a></li>";
                    }
                }
                submenu += "</ul>";
                return submenu;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw ex;
            }
        }
    }
}
