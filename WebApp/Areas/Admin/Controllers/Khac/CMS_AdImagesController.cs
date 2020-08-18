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
    public class CMS_AdImagesController : BaseController
    {
        //
        // GET: /Admin/CMS_AdImages/
        [CheckPermission]
        public ActionResult Index(string keyWord, int? page)
        {
            try
            {
                CMS_AdImages_DAO objDAO = new CMS_AdImages_DAO();
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
        public ActionResult Create(FormCollection fc, CMS_AdImages obj)
        {
            try
            {
          
                if (ModelState.IsValid)
                {

                    CMS_AdImages_DAO objDAO = new CMS_AdImages_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int userID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                    obj.UserCreate = userID;

                    int ReturnID = objDAO.Insert(obj);
                   
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm ảnh quảng cáo thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_AdImages");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm ảnh quảng cáo không thành công");
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
               ViewBag.URLIMAGE = System.Configuration.ConfigurationManager.AppSettings["UrlImage"];
               CMS_AdImages_DAO objDAO = new CMS_AdImages_DAO();
               var Partial = objDAO.GetAdImagesID(id);
             
               return View(Partial);
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), AlertType.Error);
                Logs.WriteLog(ex);
                return View();
            }
        }

        [HttpPost]
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection fc, CMS_AdImages obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CMS_AdImages_DAO objDAO = new CMS_AdImages_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_AdImages");
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
                CMS_AdImages_DAO objDAO = new CMS_AdImages_DAO();
                if (objDAO.Delete(id))
                {
                    SetAlert("Xóa ảnh quảng cáo thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi xóa ảnh quảng cáo" }, JsonRequestBehavior.AllowGet);
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
