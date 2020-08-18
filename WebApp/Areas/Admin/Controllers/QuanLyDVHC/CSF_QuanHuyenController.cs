using WebApp.App_Start;
using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers.QuanLyDVHC
{
    public class CSF_QuanHuyenController : BaseController
    {
        private string maDVHC = System.Configuration.ConfigurationManager.AppSettings["MaDVHC"].ToString();
        private DT_WebGISEntities db = new DT_WebGISEntities();
        // GET: Admin/CMS_QuanHuyen
        [CheckPermission]
        public ActionResult Index(string strSeach)
        {
            try
            {
                strSeach = strSeach ?? "";
                if (string.IsNullOrEmpty(strSeach))
                {
                    var model = db.CSF_HCHuyen.ToList()
                    .Where(m => m.IsDelete == false && m.MaTinh.Equals(maDVHC))
                    .OrderBy(m => m.SortOrder)
                    .ToList();
                    ViewBag.SearchString = strSeach;
                    return View("Index", model);
                }
                else
                {
                    var model = db.CSF_HCHuyen.ToList()
                    .Where(m => m.IsDelete == false && m.MaTinh.Equals(maDVHC) && m.TenQuanHuyen.ToLower().Contains(strSeach.ToLower()))
                    .OrderBy(m => m.SortOrder)
                .ToList();
                    ViewBag.SearchString = strSeach;
                    return View("Index", model);
                }
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return View("Index");
            }
        }
        public CSF_HCHuyen SelectByMaQuanHuyen(string strMaQuanHuyen)
        {
            var obj = db.CSF_HCHuyen.Where(m => m.IsDelete == false && m.MaTinh.Equals(maDVHC)).SingleOrDefault(m => m.MaQuanHuyen == strMaQuanHuyen);
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
        public ActionResult Create(CSF_HCHuyen obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = SelectByMaQuanHuyen(obj.MaQuanHuyen);
                    if (model != null)
                    {
                        ModelState.AddModelError("", "Đã tồn tại quận huyện có mã: " + obj.MaQuanHuyen);
                    }
                    else
                    {
                        obj.IsDelete = false;
                        obj.CreatedBy = User.Identity.Name;
                        obj.CreatedDate = DateTime.Now;
                        db.CSF_HCHuyen.Add(obj);
                        if (db.SaveChanges() > 0)
                        {
                            SetAlert("Thêm dữ liệu thành công", AlertType.Success);
                            return RedirectToAction("Index", "CSF_QuanHuyen");
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
            var model = SelectByMaQuanHuyen(id);
            return View(model);
        }

        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CSF_HCHuyen obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var model = SelectByMaQuanHuyen(obj.MaQuanHuyen);
                        if (model != null)
                        {
                            if (Update(obj, User.Identity.Name))
                            {
                                SetAlert("Cập nhật dữ liệu thành công", AlertType.Success);
                                return RedirectToAction("Index", "CSF_QuanHuyen");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Thêm dữ liệu không thành công");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không tìm thấy Quận huyện có mã: " + obj.MaQuanHuyen);
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
                if (!CheckTonTaiPhuongXa(id))
                {
                    if (DVHC_QuanHuyen_Delete(id))
                    {
                        SetAlert("Cập nhật dữ liệu thành công", AlertType.Success);
                        return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { status = false, message = "Lỗi: Xóa dữ liệu không thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { status = false, message = "Lỗi: Quận huyện đã tồn tại phường xã. Không thể xóa!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return Json(new { status = false, message = "Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool Insert(CSF_HCHuyen obj, string userName)
        {
            obj.CreatedBy = userName;
            obj.CreatedDate = DateTime.Now;
            db.CSF_HCHuyen.Add(obj);
            if (db.SaveChanges() > 0)
                return true;
            return false;
        }

        public bool Update(CSF_HCHuyen obj, string userName)
        {
            var model = db.CSF_HCHuyen.Where(m => m.MaQuanHuyen == obj.MaQuanHuyen).FirstOrDefault();
            if (model != null)
            {
                model.TenQuanHuyen = obj.TenQuanHuyen;
                model.IsDelete = false;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = userName;
                if (db.SaveChanges() > 0)
                    return true;
                return false;
            }
            return false;
        }
        public bool DVHC_QuanHuyen_Delete(string strMaQuanHuyen)
        {
            var obj = SelectByMaQuanHuyen(strMaQuanHuyen);
            db.CSF_HCHuyen.Remove(obj);
            if (db.SaveChanges() > 0)
                return true;
            return false;
        }

        public bool DVHC_QuanHuyen_Delete(CSF_HCHuyen obj)
        {
            db.CSF_HCHuyen.Remove(obj);
            if (db.SaveChanges() > 0)
                return true;
            return false;
        }

        public bool CheckTonTaiPhuongXa(string maQuanHuyen)
        {
            var result = db.CSF_HCXa.Where(m => m.MaQuanHuyen.ToUpper().Equals(maQuanHuyen.ToUpper())).ToList();
            if (result.Count > 0)
                return true;
            return false;
        }
    }
}