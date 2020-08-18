using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.Common;
using PagedList;
namespace WebApp.Controllers
{
    public class ThongBaoController : Controller
    {
        //
        // GET: /ThongBao/

        public PartialViewResult ThongBao(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                HomeController homeController = new HomeController();
                var listData = homeController.GetCacheThongBao().Where(s => s.Publish == true).OrderByDescending(s => s.UserCreate).Take(5).ToList();
                //var listData = objDAO.GetAll().Where(s => s.Publish==true).OrderByDescending(s => s.UserCreate).Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
        public PartialViewResult DSThongBao(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                HomeController homeController = new HomeController();
                var listData = homeController.GetCacheThongBao().Where(s => s.Publish == true).OrderByDescending(s => s.UserCreate).ToList();
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

        public PartialViewResult ChiTietThongBao(string title, string id)
        {
            try
            {
                ViewBag.TITLE = title;
                int tbid = Convert.ToInt32(id);
                HomeController homeController = new HomeController();
                var tb = homeController.GetCacheThongBao().Where(x => x.ID == tbid).FirstOrDefault();
                return PartialView(tb);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
    }
}
