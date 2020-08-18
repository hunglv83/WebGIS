using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;
using WebApp.Core.EF;
using WebApp.Common;
using System.IO;

namespace WebApp.Controllers
{
    public class MapController : Controller
    {

        private DT_WebGISEntities MyContext = new DT_WebGISEntities();

        //public PartialViewResult MapView(int mapid)
        //{

        //    return PartialView();
        //}
        public ActionResult MapView(int? serviceid)
        {
            try
            {
                DT_WebGISEntities db = new DT_WebGISEntities();
                var services = db.CMS_Services.Find(serviceid);
                return PartialView(services);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Leafletjs()
        {
            return PartialView();
        }
        public PartialViewResult DiemMo()
        {
            return PartialView();
        }
        public string LayDiemMo(string input)
        {
            string strDSDiemMo = string.Empty;
            return strDSDiemMo;
        }

        public PartialViewResult DanhSachBanDo(string lv, string q, int? page)
        {
            try
            {
                TempData["LV"] = MyContext.CMS_TypeOfMap.ToList();
                TempData.Keep("LV");
                q = q == null ? "" : q;
                ViewBag.Q = q;
                q = q.ToLower().Trim();
                int iLV = lv == null ? 0 : Convert.ToInt32(lv);
                ViewBag.LV = iLV;
                var lData = MyContext.CMS_Maps_LayTatCa(iLV, q).ToList();

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return PartialView(lData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public ActionResult Bando()
        {
            try
            {
                DT_WebGISEntities db = new DT_WebGISEntities();
                var id = Session["idMap"];
                var GT = db.CMS_Maps.Find(id);
                return View(GT);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return View();
            }
        }

        public ActionResult EsriMap()
        {
            return View();
        }

        public JsonResult getIdMap(int id)
        {
            Session["idMap"] = id;
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult editUrlMap(string url)
        {
            var id = Session["idMap"];
            DT_WebGISEntities db = new DT_WebGISEntities();
            var GT = db.CMS_Maps.Find(id);
            var service = db.CMS_MapService.Where(x => x.MapID == GT.ID).FirstOrDefault();
            var serviceMap = db.CMS_Services.Where(x => x.ID == service.ServiceID).FirstOrDefault();
            serviceMap.URL = url;
            db.Entry(serviceMap).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getService()
        {
            var mapSv = "";
            int idMap = Int32.Parse(Session["idMap"].ToString());
            if (Session["idMap"] != null)
            {
                var mycontext = new DT_WebGISEntities();
                var idservice = mycontext.CMS_MapService.Where(b => b.MapID == idMap).Select(b => b.ServiceID).FirstOrDefault();
                mapSv = mycontext.CMS_Services.Where(b => b.ID == idservice).Select(b => b.URL).FirstOrDefault();
            }
            return Json(mapSv, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getExtent()
        {
            var xmax = "";
            var xmin = "";
            var ymax = "";
            var ymin = "";
            List<string> termsList = new List<string>();
            int idMap = Int32.Parse(Session["idMap"].ToString());
            if (Session["idMap"] != null)
            {
                var mycontext = new DT_WebGISEntities();
                var idservice = MyContext.CMS_MapService.Where(b => b.MapID == idMap).Select(b => b.ServiceID).FirstOrDefault();
                xmin = mycontext.CMS_Services.Where(b => b.ID == idservice).Select(b => b.XMin).FirstOrDefault();
                ymin = mycontext.CMS_Services.Where(b => b.ID == idservice).Select(b => b.YMin).FirstOrDefault();
                xmax = mycontext.CMS_Services.Where(b => b.ID == idservice).Select(b => b.XMax).FirstOrDefault();
                ymax = mycontext.CMS_Services.Where(b => b.ID == idservice).Select(b => b.YMax).FirstOrDefault();
                termsList.Add(xmin);
                termsList.Add(ymin);
                termsList.Add(xmax);
                termsList.Add(ymax);
            }
            return Json(termsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult putExtent(string xMin, string yMin, string xMax, string yMax)
        {
            var jsonResults = new Object();
            int idMap = Int32.Parse(Session["idMap"].ToString());
            var idservice = MyContext.CMS_MapService.Where(b => b.MapID == idMap).Select(b => b.ServiceID).FirstOrDefault();
            CMS_Services objService = MyContext.CMS_Services.SingleOrDefault(s => s.ID == idservice);
            objService.XMin = xMin;
            objService.YMin = yMin;
            objService.XMax = xMax;
            objService.YMax = yMax;
            WebApp.Core.DAO.CMS_Services_DAO objDAO = new WebApp.Core.DAO.CMS_Services_DAO();
            objDAO.Update(objService);
            return Json(jsonResults, JsonRequestBehavior.AllowGet);

        }

        public JsonResult checkAdmin(string name)
        {
            WebApp.Core.DAO.CSF_Users_DAO objDAO = new WebApp.Core.DAO.CSF_Users_DAO();
            var users = objDAO.GetUserIDByUserName(name);
            var usersRole = MyContext.CSF_UserRole.Where(b => b.UserID == users && b.RoleID == 1).FirstOrDefault();
            if (usersRole != null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}
