using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.App_Start;
using WebApp.Common;
using WebApp.Core.EF;

namespace WebApp.Areas.Admin.Controllers.QuanLyDVHC
{
    public class CSF_TinhThanhPhoController : BaseController
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
                var model = db.CSF_HCTinh.ToList()
                .Where(m => m.IsDelete == false && m.Ten.ToLower().Contains(strSeach.ToLower()))
                .OrderBy(m => m.SortOrder)
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
        public CSF_HCTinh SelectByMa(string strMa)
        {
            var obj = db.CSF_HCTinh.Where(m => m.IsDelete == false).SingleOrDefault(m => m.Ma == strMa);
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
        public ActionResult Create(CSF_HCTinh obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = SelectByMa(obj.Ma);
                    if (model != null)
                    {
                        ModelState.AddModelError("", "Đã tồn tại tỉnh có mã: " + obj.Ma);
                    }
                    else
                    {
                        obj.IsDelete = false;
                        obj.CreatedBy = User.Identity.Name;
                        obj.CreatedDate = DateTime.Now;
                        db.CSF_HCTinh.Add(obj);
                        if (db.SaveChanges() > 0)
                        {
                            SetAlert("Thêm dữ liệu thành công", AlertType.Success);
                            return RedirectToAction("Index", "CSF_HCTinh");
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
            var model = SelectByMa(id);
            return View(model);
        }

        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CSF_HCTinh obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var model = SelectByMa(obj.Ma);
                        if (model != null)
                        {
                            if (Update(obj, User.Identity.Name))
                            {
                                SetAlert("Cập nhật dữ liệu thành công", AlertType.Success);
                                return RedirectToAction("Index", "CSF_HCTinh");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Thêm dữ liệu không thành công");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không tìm thấy Tỉnh có mã: " + obj.Ma);
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
                if (!CheckTonTaiQuanHuyen(id))
                {
                    if (CSF_Tinh_Delete(id))
                    {
                        SetAlert("Cập nhật dữ liệu thành công", AlertType.Success);
                        return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { status = false, message = "Lỗi: Xóa dữ liệu không thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { status = false, message = "Lỗi: Tỉnh đã tồn tại phường xã. Không thể xóa!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return Json(new { status = false, message = "Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool Insert(CSF_HCTinh obj, string userName)
        {
            obj.CreatedBy = userName;
            obj.CreatedDate = DateTime.Now;
            db.CSF_HCTinh.Add(obj);
            if (db.SaveChanges() > 0)
                return true;
            return false;
        }

        public bool Update(CSF_HCTinh obj, string userName)
        {
            var model = db.CSF_HCTinh.Where(m => m.Ma == obj.Ma).FirstOrDefault();
            if (model != null)
            {
                model.Ten = obj.Ten;
                model.TenTiengAnh = obj.TenTiengAnh;
                model.IsDelete = false;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = userName;
                if (db.SaveChanges() > 0)
                    return true;
                return false;
            }
            return false;
        }
        public bool CSF_Tinh_Delete(string strMa)
        {
            var obj = SelectByMa(strMa);
            db.CSF_HCTinh.Remove(obj);
            if (db.SaveChanges() > 0)
                return true;
            return false;
        }

        public bool CSF_Tinh_Delete(CSF_HCTinh obj)
        {
            db.CSF_HCTinh.Remove(obj);
            if (db.SaveChanges() > 0)
                return true;
            return false;
        }

        public bool CheckTonTaiQuanHuyen(string maTinh)
        {
            var result = db.CSF_HCHuyen.Where(m => m.MaTinh.ToUpper().Equals(maTinh.ToUpper())).ToList();
            if (result.Count > 0)
                return true;
            return false;
        }
    }
}