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
    public class CMS_ServicesController : BaseController
    {
        //
        // GET: /Admin/CMS_Categories/

        [CheckPermission]
        public ActionResult Index(string keyWord, int? page)
        {
            try
            {
                CMS_Services_DAO objDAO = new CMS_Services_DAO();
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
                CMS_Services_DAO objDAO = new CMS_Services_DAO();
                var Partial = objDAO.GetMapByID(id);
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                TempData["TypeOfMap"] = MyContext.CMS_TypeOfMap.ToList();
                TempData.Keep("TypeOfMap");
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
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                TempData["TypeOfMap"] = MyContext.CMS_TypeOfMap.ToList();
                TempData.Keep("TypeOfMap");
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
        public ActionResult Create(FormCollection fc, CMS_Services obj, HttpPostedFileBase file)
        {
            try
            {
                TempData.Keep("TypeOfMap");
                if (ModelState.IsValid)
                {
                    CMS_Services_DAO objDAO = new CMS_Services_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int intUserID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                    obj.UserCreate = intUserID;
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm services thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Services");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm services không thành công");
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
                CMS_Services_DAO objDAO = new CMS_Services_DAO();
                if (objDAO.CheckDelete(id))
                {
                    if (objDAO.Delete(id))
                    {
                        SetAlert("Xóa services thành công", AlertType.Success);
                        return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "Lỗi xóa services" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Services đang được sử dụng không được phép xóa" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(FormCollection fc, CMS_Services obj, HttpPostedFileBase file)
        {
            try
            {
                TempData.Keep("TypeOfMap");
                if (ModelState.IsValid)
                {
                    CMS_Services_DAO objDAO = new CMS_Services_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật services thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Services");
                    }
                    else
                    {
                        SetAlert("Cập nhật services không thành công", AlertType.Error);
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
