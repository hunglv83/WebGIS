using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using WebApp.Core.DAO;
using WebApp.Common;
using System.Reflection;
using PagedList;
using WebApp.App_Start;
using WebApp.Core.EF;

namespace WebApp.Areas.Admin.Controllers.Khac
{
    public class CMS_ContactController : BaseController
    {
        //
        // GET: /Admin/CMS_Contact/

        [CheckPermission]
        public ActionResult Index(string keyWord, int? page)
        {
            try
            {
                CMS_Contact_DAO objDAO = new CMS_Contact_DAO();
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
                CMS_Contact_DAO objDAO = new CMS_Contact_DAO();
                var Partial = objDAO.GetContactByID(id);
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
        [ValidateInput(false)]
        public ActionResult Create(FormCollection fc, CMS_Contact obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_Contact_DAO objDAO = new CMS_Contact_DAO();
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm thông tin liên hệ thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Contact");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm thông tin liên hệ không thành công");
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
                CMS_Contact_DAO objDAO = new CMS_Contact_DAO();
            
                    if (objDAO.Delete(id))
                    {
                        SetAlert("Xóa thông tin liên hệ thành công", AlertType.Success);
                        return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "Lỗi xóa thông tin liên hệ" }, JsonRequestBehavior.AllowGet);
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
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection fc, CMS_Contact obj, HttpPostedFileBase file)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_Contact_DAO objDAO = new CMS_Contact_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật thông tin liên hệ thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Contact");
                    }
                    else
                    {
                        SetAlert("Cập nhật thông tin liên hệ không thành công", AlertType.Error);
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
