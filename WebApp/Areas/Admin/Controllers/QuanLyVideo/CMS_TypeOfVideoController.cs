using WebApp.Common;
using WebApp.Core.DAO;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApp.App_Start;

namespace WebApp.Areas.Admin.Controllers.QuanLyVanBan
{
    public class CMS_TypeOfVideoController : BaseController
    {
        //
        // GET: /Admin/CMS_TypeOfVideo/

        [CheckPermission]
        public ActionResult Index(string keyword, int? page)
        {
            try
            {
                keyword = keyword != null ? keyword : "";
                CMS_TypeOfVideo_DAO objDAO = new CMS_TypeOfVideo_DAO();
                var data = objDAO.Search(keyword);
                ViewBag.SearchString = keyword;
                int pageSize = 10;
                int pageNumber = (page ?? 1);
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
        public ActionResult Create(FormCollection fc, CMS_TypeOfVideo obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                if (ModelState.IsValid)
                {

                    CMS_TypeOfVideo_DAO objDAO = new CMS_TypeOfVideo_DAO();
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm bản ghi thành công", AlertType.Success);
                        return RedirectToAction("index", "CMS_TypeOfVideo");
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
                DT_WebGISEntities entity = new DT_WebGISEntities();
                var doc = entity.CMS_TypeOfVideo.Find(id);
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
        public ActionResult Edit(FormCollection fc, CMS_TypeOfVideo obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                if (ModelState.IsValid)
                {
                    CMS_TypeOfVideo_DAO objDAO = new CMS_TypeOfVideo_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_TypeOfVideo");
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
                CMS_TypeOfVideo_DAO objDAO = new CMS_TypeOfVideo_DAO();
                if (objDAO.Delete(id))
                {
                    SetAlert("Xóa thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi xóa bản ghi" }, JsonRequestBehavior.AllowGet);
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
