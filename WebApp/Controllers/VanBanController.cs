using WebApp.Core.DAO;
using System;
using System.Linq;
using PagedList;
using System.Web.Mvc;
using WebApp.Common;
using WebApp.Core.EF;

namespace WebApp.Controllers
{
    public class VanBanController : Controller
    {
        //
        // GET: /VanBan/

        public PartialViewResult Index(string title, string category, string key, string type, string area, string org, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                int typeID = type != null ? Convert.ToInt32(type) : 0;
                int areaID = area != null ? Convert.ToInt32(area) : 0;
                int orgID = org != null ? Convert.ToInt32(org) : 0;
                key = key != null ? key : "";
                #region DATA-SEARCH
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                TempData["LoaiVanBan"] = MyContext.CMS_TypeOfDocument.ToList();
                TempData["LinhVuc"] = MyContext.CMS_AreaOfDocument.ToList();
                TempData["CoQuanBanHanh"] = MyContext.CMS_Organization.ToList();
                TempData.Keep("LoaiVanBan");
                TempData.Keep("LinhVuc");
                TempData.Keep("CoQuanBanHanh");
                ViewBag.KEY = key;
                ViewBag.TYPE = typeID;
                ViewBag.AREA = areaID;
                ViewBag.ORG = orgID;
                #endregion
                CMS_Documents_DAO objDAO = new CMS_Documents_DAO();
                HomeController homeController = new HomeController();
                var listData = homeController.GetCacheVanBan().Where(x => (typeID == 0 || x.TypeOfDocumentID == typeID)
                                                                        && (areaID == 0 || x.AreaOfDocument == areaID)
                                                                        && (orgID == 0 || x.OrganizationID == orgID)
                                                                        && x.Abstract.ToLower().Contains(key.ToLower())
                                                                        ).ToList();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ChiTietVanBan(string title, string id)
        {
            try
            {
                ViewBag.TITLE = title;
                int vbid = Convert.ToInt32(id);
                HomeController homeController = new HomeController();
                var news = homeController.GetCacheVanBan().Where(x => x.ID == vbid).FirstOrDefault();
                return PartialView(news);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

    }
}
