using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using WebApp.Core.EF;
using WebApp.Common;

namespace WebApp.Controllers
{
    public class ServicesController : Controller
    {
        public PartialViewResult IndexService(string title, string lv, string q, int? page)
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                ViewBag.TITLE = title;
                q = q == null ? "" : q;
                ViewBag.Q = q;
                q = q.ToLower().Trim();
                int iLV = lv == null ? 0 : Convert.ToInt32(lv);
                var ldata = MyContext.CMS_Services.Where(x => x.Name.ToLower().Contains(q) && (x.TypeOfMapID == iLV || iLV == 0) && x.Publish==true).ToList();

                TempData["LV"] = MyContext.CMS_TypeOfMap.ToList();
                TempData.Keep("LV");
                ViewBag.LV = iLV;

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return PartialView(ldata.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ChiTietDichVu(string title, string id)
        {
            try
            {
                ViewBag.TITLE = title;
                int newsid = Convert.ToInt32(id);
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                var news = MyContext.CMS_Services.Find(newsid);
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
