using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class VideoController : Controller
    {
        //
        // GET: /Video/
        public PartialViewResult VideoIndex(string title)
        {
            try
            {
                ViewBag.TITLE = title;
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                var videos = MyContext.CMS_Videos.Where(x => x.Publish == true).ToList();
                return PartialView(videos);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
        public PartialViewResult TinVideo(string title, string category)
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                ViewBag.TITLE = title;
                var listData = MyContext.CMS_Videos.Where(s => s.Publish == true).OrderByDescending(s => s.CreateDate).Take(3).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ChiTietVideo(string title, string id)
        {
            try
            {
                ViewBag.TITLE = title;
                int newsid = Convert.ToInt32(id);
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                var news = MyContext.CMS_Videos.Find(newsid);
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
