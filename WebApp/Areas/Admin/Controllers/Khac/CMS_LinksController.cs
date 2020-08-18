using WebApp.Common;
using WebApp.Core.DAO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApp.App_Start;
using WebApp.Core.EF;

namespace WebApp.Areas.Admin.Controllers.QuanLyVanBan
{
    public class CMS_LinksController : BaseController
    {
        //
        // GET: /Admin/CMS_Links/

        [CheckPermission]
        public ActionResult Index(string keyword, int? page)
        {
            try
            {
                keyword = keyword != null ? keyword : "";
                CMS_Links_DAO objDAO = new CMS_Links_DAO();
                var data = objDAO.Search(keyword);
                ViewBag.SearchString = keyword;
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                ViewBag.URLIMG = System.Configuration.ConfigurationManager.AppSettings["UrlImage"];
                return View(data.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return View();
            }
        }

        [CheckPermission]
        public ActionResult Create()
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                return View();
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), AlertType.Error);
                Logs.WriteLog(ex);
                return View();
            }
        }

        [CheckPermission]
        [HttpPost]
        public ActionResult Create(FormCollection fc, CMS_Links obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                if (ModelState.IsValid)
                {

                    CMS_Links_DAO objDAO = new CMS_Links_DAO();
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm liên kết thành công", AlertType.Success);
                        return RedirectToAction("index", "CMS_Links");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm mới không thành công");
                    }
                    return View("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), AlertType.Error);
                Logs.WriteLog(ex);
                return View();
            }
        }

        [CheckPermission]
        public ActionResult Edit(int id)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                ViewBag.URLIMG = System.Configuration.ConfigurationManager.AppSettings["UrlImage"];
                DT_WebGISEntities entity = new DT_WebGISEntities();
                var doc = entity.CMS_Links.Find(id);
                if (doc != null)
                {
                    return View(doc);
                }
                return View();
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), AlertType.Error);
                Logs.WriteLog(ex);
                return View();
            }
        }

        [CheckPermission]
        [HttpPost]
        public ActionResult Edit(FormCollection fc, CMS_Links obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                if (ModelState.IsValid)
                {
                    CMS_Links_DAO objDAO = new CMS_Links_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Links");
                    }
                    else
                    {
                        SetAlert("Cập nhật không thành công", AlertType.Error);
                    }

                }
                return View(obj);
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), AlertType.Error);
                Logs.WriteLog(ex);
                return View();
            }
        }

        [CheckPermission]
        public JsonResult Delete(int id)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                CMS_Links_DAO objDAO = new CMS_Links_DAO();
                if (objDAO.Delete(id))
                {
                    SetAlert("Xóa thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi xóa liên kết" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return Json(new { status = false, message = "Lỗi: " + ex }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
