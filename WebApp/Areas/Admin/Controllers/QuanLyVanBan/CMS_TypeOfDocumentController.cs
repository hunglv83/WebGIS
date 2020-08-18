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

namespace WebApp.Areas.Admin.Controllers.QuanLyVanBan
{
    public class CMS_TypeOfDocumentController : BaseController
    {
        //
        // GET: /Admin/CMS_TypeOfDocument/

        [CheckPermission]
        public ActionResult Index(string keyWord, int? page)
        {
            try
            {
                CMS_TypeOfDocument_DAO objDAO = new CMS_TypeOfDocument_DAO();
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
                CMS_TypeOfDocument_DAO objDAO = new CMS_TypeOfDocument_DAO();
                var Partial = objDAO.GetTypeOfDocumentByID(id);
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
        public ActionResult Create(FormCollection fc, CMS_TypeOfDocument obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_TypeOfDocument_DAO objDAO = new CMS_TypeOfDocument_DAO();
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm loại văn bản thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_TypeOfDocument");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm loại văn bản không thành công");
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
                CMS_TypeOfDocument_DAO objDAO = new CMS_TypeOfDocument_DAO();
                if (objDAO.CheckDelete(id))
                {
                    if (objDAO.Delete(id))
                    {
                        SetAlert("Xóa loại văn bản thành công", AlertType.Success);
                        return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "Lỗi xóa loại văn bản" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Loại văn bản đang được sử dụng không được phép xóa" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(FormCollection fc, CMS_TypeOfDocument obj, HttpPostedFileBase file)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_TypeOfDocument_DAO objDAO = new CMS_TypeOfDocument_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật loại văn bản thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_TypeOfDocument");
                    }
                    else
                    {
                        SetAlert("Cập nhật loại văn bản không thành công", AlertType.Error);
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
