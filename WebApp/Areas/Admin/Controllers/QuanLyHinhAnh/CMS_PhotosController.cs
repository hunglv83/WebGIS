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
    public class CMS_PhotosController : BaseController
    {
        //
        // GET: /Admin/CMS_Photos/

        [CheckPermission]
        public ActionResult Index(string keyword, string type, int? page)
        {
            try
            {
                int typeID = type != null ? Convert.ToInt32(type) : 0;
                keyword = keyword != null ? keyword : "";
                CMS_Photos_DAO objDAO = new CMS_Photos_DAO();
                var data = objDAO.Search(keyword, typeID);
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
                CMS_Photos_DAO objDAO = new CMS_Photos_DAO();
                DT_WebGISEntities MVCContext = new DT_WebGISEntities();
                TempData["TypeOfPhotoID"] = MVCContext.CMS_TypeOfPhoto.ToList();
                TempData.Keep("TypeOfPhotoID");
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
        public ActionResult Create(FormCollection fc, CMS_Photos obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                TempData.Keep("TypeOfPhotoID");
                if (ModelState.IsValid)
                {

                    CMS_Photos_DAO objDAO = new CMS_Photos_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int intUserID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                    obj.UserCreate = intUserID;
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm hình ảnh thành công", AlertType.Success);
                        return RedirectToAction("index", "CMS_Photos");
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
                TempData["TypeOfPhotoID"] = entity.CMS_TypeOfPhoto.ToList();
                TempData.Keep("TypeOfPhotoID");
                var doc = entity.CMS_Photos.Find(id);
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
        public ActionResult Edit(FormCollection fc, CMS_Photos obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                TempData.Keep("TypeOfPhotoID");
                if (ModelState.IsValid)
                {
                    CMS_Photos_DAO objDAO = new CMS_Photos_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Photos");
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
                CMS_Photos_DAO objDAO = new CMS_Photos_DAO();
                if (objDAO.Delete(id))
                {
                    SetAlert("Xóa thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi xóa hình ảnh" }, JsonRequestBehavior.AllowGet);
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
