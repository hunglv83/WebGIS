using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core.EF;
using System.IO;
using WebApp.Core.DAO;
using WebApp.Common;
using System.Reflection;
using PagedList;
using WebApp.App_Start;
namespace WebApp.Areas.Admin.Controllers.QuanLyBanDo
{
    public class CMS_TypeOfMapController : BaseController
    {
        //
        // GET: /Admin/CMS_Categories/

        [CheckPermission]
        public ActionResult Index(string keyWord, int? page)
        {
            try
            {
                CMS_TypeOfMap_DAO objDAO = new CMS_TypeOfMap_DAO();
                keyWord = keyWord != null ? keyWord : "";
                var data = objDAO.Search(keyWord);
                ViewBag.SearchString = keyWord;
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
        public ActionResult Edit(int id)
        {
            try
            {
                CMS_TypeOfMap_DAO objDAO = new CMS_TypeOfMap_DAO();
                var Partial = objDAO.GetTypeOfMapByID(id);
                return View(Partial);
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), AlertType.Error);
                Logs.WriteLog(ex);
                return View();
            }
        }


        [CheckPermission]
        public ActionResult Create()
        {
            try
            {

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
        public ActionResult Create(FormCollection fc, CMS_TypeOfMap obj, HttpPostedFileBase file)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_TypeOfMap_DAO objDAO = new CMS_TypeOfMap_DAO();
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm loại bản đồ thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_TypeOfMap");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm loại bản đồ không thành công");
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
        public JsonResult Delete(int id)
        {
            try
            {
                CMS_TypeOfMap_DAO objDAO = new CMS_TypeOfMap_DAO();
                if (objDAO.CheckDelete(id))
                {
                    if (objDAO.Delete(id))
                    {
                        SetAlert("Xóa loại bản đồ thành công", AlertType.Success);
                        return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "Lỗi xóa loại bản đồ" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Loại bản đồ đang được sử dụng không được phép xóa." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return Json(new { status = false, message = "Lỗi: " + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [CheckPermission]
        [HttpPost]
        public ActionResult Edit(FormCollection fc, CMS_TypeOfMap obj, HttpPostedFileBase file)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_TypeOfMap_DAO objDAO = new CMS_TypeOfMap_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật loại bản đồ thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_TypeOfMap");
                    }
                    else
                    {
                        SetAlert("Cập nhật loại bản đồ không thành công", AlertType.Error);
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

    }
}
