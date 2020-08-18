using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;
using WebApp.Common;
using WebApp.Core.EF;

namespace WebApp.Areas.Admin.Controllers.DT_Modules
{
    public class DT_KhuVucController : BaseController
    {
        // GET: Admin/DT_QuanLyKhuVuc
        private DT_WebGISEntities db = new DT_WebGISEntities();
        [CheckPermission]
        public ActionResult Index(string strSeach)
        {
            try
            {
                strSeach = strSeach ?? "";
                var model = db.DT_KhuVuc.ToList()
                .Where(m => m.IsActive == false && m.Ten.ToLower().Contains(strSeach.ToLower()))
                .ToList();
                ViewBag.SearchString = strSeach;
                return View("Index", model);
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return View("Index");
            }
        }
        public DT_KhuVuc SelectByTen(string strTen)
        {
            var obj = db.DT_KhuVuc.Where(m => m.IsActive == false).SingleOrDefault(m => m.Ten == strTen);
            return obj;
        }
        [CheckPermission]
        public ActionResult Create()
        {
            return View();
        }

        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DT_KhuVuc obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = SelectByTen(obj.Ten);
                    if (model != null)
                    {
                        ModelState.AddModelError("", "Đã tồn tại thời điểm có tên: " + obj.Ten);
                    }
                    else
                    {
                        obj.IsActive = false;
                        obj.CreatedBy = User.Identity.Name;
                        obj.CreatedDate = DateTime.Now;
                        db.DT_KhuVuc.Add(obj);
                        if (db.SaveChanges() > 0)
                        {
                            SetAlert("Thêm dữ liệu thành công", AlertType.Success);
                            return RedirectToAction("Index", "DT_KhuVuc");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Thêm dữ liệu không thành công");
                        }
                    }

                    return View("Create");
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
        public ActionResult Edit(string id)
        {
            var model = db.DT_KhuVuc.Where(x => x.Id.Equals(id)).FirstOrDefault();
            return View(model);
        }

        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DT_KhuVuc obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var model = SelectByTen(obj.Ten);
                        if (model != null)
                        {
                            if (Update(obj, User.Identity.Name))
                            {
                                SetAlert("Cập nhật dữ liệu thành công", AlertType.Success);
                                return RedirectToAction("Index", "DT_KhuVuc");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Thêm dữ liệu không thành công");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không tìm thấy Khu vực có tên: " + obj.Ten);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Cập nhật dữ liệu không thành công");
                    }
                    return View("Edit");
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
        public JsonResult Delete(string id)
        {
            try
            {
                var obj = db.DT_KhuVuc.Where(x => x.Id.Equals(id)).FirstOrDefault();
                db.DT_KhuVuc.Remove(obj);
                if (db.SaveChanges() > 0)
                {
                    SetAlert("Cập nhật dữ liệu thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { status = false, message = "Lỗi: Xóa dữ liệu không thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return Json(new { status = false, message = "Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool Insert(DT_KhuVuc obj, string userName)
        {
            obj.CreatedBy = userName;
            obj.CreatedDate = DateTime.Now;
            db.DT_KhuVuc.Add(obj);
            if (db.SaveChanges() > 0)
                return true;
            return false;
        }

        public bool Update(DT_KhuVuc obj, string userName)
        {
            var model = db.DT_KhuVuc.Where(m => m.Ten == obj.Ten).FirstOrDefault();
            if (model != null)
            {
                model.Ten = obj.Ten;
                model.MoTa = obj.MoTa;
                model.IsActive = false;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = userName;
                if (db.SaveChanges() > 0)
                    return true;
                return false;
            }
            return false;
        }
    }
}