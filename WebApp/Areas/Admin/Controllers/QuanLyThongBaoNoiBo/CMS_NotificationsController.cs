using WebApp.Common;
using WebApp.Core.DAO;
using System;
using System.Web.Mvc;

using PagedList;
using WebApp.Core.EF;
using WebApp.App_Start;
using System.Linq;

namespace WebApp.Areas.Admin.Controllers.QuanLyThongBaoNoiBo
{
    public class CMS_NotificationsController : BaseController
    {
        //
        // GET: /Admin/CMS_Notifications/

        [CheckPermission]
        public ActionResult Index(int? page,string keyWord)
        {
            try
            {
                keyWord = keyWord != null ? keyWord : "";
                CMS_Notifications_DAO objDAO = new CMS_Notifications_DAO();
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
                CMS_Notifications_DAO objDAO = new CMS_Notifications_DAO();
                var Partial = objDAO.GetNotificationsByID(id);
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
        public ActionResult Create(FormCollection fc, CMS_Notifications obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_Notifications_DAO objDAO = new CMS_Notifications_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int userID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                    obj.UserCreate = userID;
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        //cache lai thong bao
                        WebApp.Controllers.HomeController hController = new WebApp.Controllers.HomeController();
                        hController.Cache_ThongBao();
                        SetAlert("Thêm thông báo nội bộ thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Notifications");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm thông báo nội bộ không thành công");
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
                CMS_Notifications_DAO objDAO = new CMS_Notifications_DAO();
                if (objDAO.Delete(id))
                {
                    // cache lai thong bao
                    WebApp.Controllers.HomeController hController = new WebApp.Controllers.HomeController();
                    hController.Cache_ThongBao();
                    SetAlert("Xóa thông báo nội bộ thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi xóa thông báo nội bộ" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(FormCollection fc, CMS_Notifications obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_Notifications_DAO objDAO = new CMS_Notifications_DAO();
                    if (objDAO.Update(obj))
                    {
                        //cache lai thong bao
                        WebApp.Controllers.HomeController hController = new WebApp.Controllers.HomeController();
                        hController.Cache_ThongBao();
                        SetAlert("Cập nhật thông báo nội bộ thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Notifications");
                    }
                    else
                    {
                        SetAlert("Cập nhật thông báo nội bộ không thành công", AlertType.Error);
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
        public ActionResult Detail(int id)
        {
            try
            {
                DT_WebGISEntities entity = new DT_WebGISEntities();
                WebApp.Controllers.HomeController homeController = new WebApp.Controllers.HomeController();
                var tb = homeController.GetCacheThongBao().Where(x => x.ID == id).FirstOrDefault();
                return View(tb);
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
