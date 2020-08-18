using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers.QuanTriHeThong
{
    public class QT_TemplatesController : BaseController
    {
        //
        // GET: /Admin/QT_Templates/

        public ActionResult Index()
        {
            DT_WebGISEntities MyContext = new DT_WebGISEntities();
            ViewBag.URLIMAGE = System.Configuration.ConfigurationManager.AppSettings["UrlImage"];
            var temp = MyContext.CSF_Templates.FirstOrDefault();
            if (temp != null)
            {
                return View(temp);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(CSF_Templates obj)
        {
            ViewBag.URLIMAGE = System.Configuration.ConfigurationManager.AppSettings["UrlImage"];
            if (ModelState.IsValid)
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                MyContext.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                MyContext.SaveChanges();
                SetAlert("Lưu giao diên thành công", AlertType.Success);
            }
            return View(obj);
        }

    }
}
