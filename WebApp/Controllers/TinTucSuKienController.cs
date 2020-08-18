using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.Common;
using PagedList;

namespace WebApp.Controllers
{
    public class TinTucSuKienController : Controller
    {
        //
        // GET: /TinTucSuKien/

        public PartialViewResult TinTucSuKien(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                
                HomeController hController = new HomeController();
                var listData = hController.GetCacheTinBai().Where(t=>t.CateKey.Contains("dt-")).OrderByDescending(s => s.CREATEDATE).ToList();
                //var listData = hController.GetCacheTinBai().Where(t => t.CateKey.Contains("dt-")).OrderByDescending(s => s.CREATEDATE).ToList();
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult DSTinKHCNCuaBo(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                HomeController hController = new HomeController();
                var listData = hController.GetCacheTinBai().Where(s=>s.CateKey==category).ToList();
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
        public PartialViewResult DSTinKHCNTrongNuoc(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                HomeController hController = new HomeController();
                var listData = hController.GetCacheTinBai().Where(s => s.CateKey == category).ToList();
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
        public PartialViewResult DSTinKHCNQuocTe(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                HomeController hController = new HomeController();
                var listData = hController.GetCacheTinBai().Where(s => s.CateKey == category).ToList();
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
        public PartialViewResult DSSuKienKHCN(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                HomeController hController = new HomeController();
                var listData = hController.GetCacheTinBai().Where(s => s.CateKey == category).ToList();
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

    }
}
