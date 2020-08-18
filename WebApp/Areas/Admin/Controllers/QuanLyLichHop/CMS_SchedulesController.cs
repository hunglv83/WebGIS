using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApp.Common;
using WebApp.Core.EF;
using WebApp.Core.DAO;
using WebApp.App_Start;
using WebApp.Controllers;

namespace WebApp.Areas.Admin.Controllers.QuanLyLichHop
{
    public class CMS_SchedulesController : BaseController
    {
        //
        // GET: /Admin/CMS_Schedules/

        [CheckPermission]
        public ActionResult Index(int? page, string date)
        {
            try
            {
                ViewBag.DATE = date;
                if (date != null)
                {
                    date = Convert.ToDateTime(date).ToShortDateString();
                }
                else
                {
                    date = DateTime.Now.ToShortDateString();
                    ViewBag.DATE = DateTime.Now.ToString("yyyy-MM-dd");
                }
                HomeController home = new HomeController();
                List<LichHop> listLH = home.getDataLH(date);
                return View(listLH);
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
                CMS_Schedules_DAO objDAO = new CMS_Schedules_DAO();
                var Partial = objDAO.GetSchedulesByID(id);
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
        public ActionResult Create(FormCollection fc, CMS_Schedules obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_Schedules_DAO objDAO = new CMS_Schedules_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int userID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                    obj.UserCreate = userID;
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm lịch họp thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Schedules");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm lịch họp không thành công");
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
                CMS_Schedules_DAO objDAO = new CMS_Schedules_DAO();
                if (objDAO.Delete(id))
                {
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
        public ActionResult Edit(FormCollection fc, CMS_Schedules obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_Schedules_DAO objDAO = new CMS_Schedules_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật thông báo nội bộ thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Schedules");
                    }
                    else
                    {
                        SetAlert("Cập nhật thông báo nội bộs không thành công", AlertType.Error);
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
