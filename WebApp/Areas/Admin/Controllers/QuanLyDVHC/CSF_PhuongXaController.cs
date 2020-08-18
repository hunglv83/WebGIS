using WebApp.App_Start;
using WebApp.Common;
using WebApp.Core.DAO;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers.QuanLyDVHC
{
    public class CSF_PhuongXaController : BaseController
    {
        private string maDVHC = System.Configuration.ConfigurationManager.AppSettings["MaDVHC"].ToString();
        private DT_WebGISEntities db = new DT_WebGISEntities();
        // GET: Admin/CMS_PhuongXa
        [HttpPost]
        public JsonResult ChangeQuanHuyen(string strMaQuanHuyen)
        {
            var phuongXa = db.CSF_HCXa.ToList()
                .Where(m => m.IsDelete == false && m.MaQuanHuyen.ToLower().Contains(strMaQuanHuyen.ToLower()))
                .OrderBy(m => m.SortOrder)
                .ToList();
            var result = new { phuongXa = phuongXa };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<CSF_HCXa> DVHC_QuanHuyen_SelectAll()
        {
            var objs = db.CSF_HCXa
                .Where(m => m.IsDelete == false && m.MaTinh.Equals(maDVHC))
                 .OrderBy(m => m.SortOrder)
                .ToList();
            return objs;
        }
        public List<CSF_HCXa> DVHC_PhuongXa_SelectAll()
        {
            var objs = db.CSF_HCXa.ToList()
                .Where(m => m.IsDelete == false && m.MaTinh.Equals(maDVHC))
                .OrderBy(m => m.SortOrder)
                .ThenBy(m => m.SortOrder)
                .ToList();
            return objs;
        }
        public List<CSF_HCXa> DVHC_PhuongXa_SelectAll(string strSearch)
        {
            var objs = new List<CSF_HCXa>();
            if (string.IsNullOrEmpty(strSearch))
                objs = db.CSF_HCXa.ToList()
                    .Where(m => m.IsDelete == false && m.CSF_HCHuyen.MaTinh.Equals(maDVHC))
                    .OrderBy(m => m.CSF_HCHuyen.SortOrder)
                    .ThenBy(m => m.SortOrder)
                    .ToList();
            else
                objs = db.CSF_HCXa.ToList()
                    .Where(m => m.IsDelete == false && m.CSF_HCHuyen.MaTinh.Equals("40") && m.TenPhuongXa.ToLower().Contains(strSearch.ToLower()))
                    .OrderBy(m => m.CSF_HCHuyen.SortOrder)
                    .ThenBy(m => m.SortOrder)
                    .ToList();
            return objs;
        }
        [CheckPermission]
        public ActionResult Index(string strSeach)
        {
            try
            {
                strSeach = strSeach != null ? strSeach : "";
                var model = DVHC_PhuongXa_SelectAll(strSeach).ToList();
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
        [CheckPermission]
        public ActionResult TraCuu(string strSeach)
        {
            try
            {
                strSeach = strSeach != null ? strSeach : "";
                var model = DVHC_PhuongXa_SelectAll(strSeach).ToList();
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
        [CheckPermission]
        public ActionResult Create()
        {
            ViewBag.QuanHuyen = DVHC_QuanHuyen_SelectAll();
            return View();
        }
        public CSF_HCXa SelectByMaPhuongXa(string strMaPhuongXa)
        {
            var obj = db.CSF_HCXa.ToList().Where(m => m.IsDelete == false).SingleOrDefault(m => m.MaPhuongXa == strMaPhuongXa);
            return obj;
        }
        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CSF_HCXa obj)
        {
            ViewBag.QuanHuyen = DVHC_QuanHuyen_SelectAll();
            try
            {
                if (ModelState.IsValid)
                {
                    var model = SelectByMaPhuongXa(obj.MaPhuongXa);
                    if (model != null)
                    {
                        ModelState.AddModelError("", "Đã tồn tại phường xã có mã: " + obj.MaPhuongXa);
                    }
                    else
                    {
                        obj.CreatedBy = User.Identity.Name;
                        obj.CreatedDate = DateTime.Now;
                        db.CSF_HCXa.Add(obj);
                        obj.IsDelete = false;
                        if (db.SaveChanges() > 0)
                        {
                            SetAlert("Thêm dữ liệu thành công", AlertType.Success);
                            return RedirectToAction("Index", "CSF_PhuongXa");
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
            ViewBag.QuanHuyen = DVHC_QuanHuyen_SelectAll();
            var model = SelectByMaPhuongXa(id);
            return View(model);
        }
        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CSF_HCXa obj)
        {
            ViewBag.QuanHuyen = DVHC_QuanHuyen_SelectAll();
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var model = SelectByMaPhuongXa(obj.MaPhuongXa);
                        if (model != null)
                        {
                            if (Update(obj, User.Identity.Name))
                            {
                                SetAlert("Cập nhật dữ liệu thành công", AlertType.Success);
                                return RedirectToAction("Index", "CSF_PhuongXa");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Thêm dữ liệu không thành công");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không tìm thấy Phường xã có mã: " + obj.MaPhuongXa);
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
                var obj = SelectByMaPhuongXa(id);
                if (DVHC_PhuongXa_Delete(obj))
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
        public bool DVHC_PhuongXa_Delete(CSF_HCXa obj)
        {
            var model = db.CSF_HCXa.ToList().Where(m => m.MaPhuongXa == obj.MaPhuongXa).FirstOrDefault();
            if (model != null)
            {
                model.IsDelete = true;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = User.Identity.Name; ;
                if (db.SaveChanges() > 0)
                    return true;
                return false;
            }
            return false;
        }
        public bool Update(CSF_HCXa obj, string userName)
        {
            var model = db.CSF_HCXa.ToList().Where(m => m.MaPhuongXa == obj.MaPhuongXa).FirstOrDefault();
            if (model != null)
            {
                model.MaPhuongXa = obj.MaPhuongXa;
                model.TenPhuongXa = obj.TenPhuongXa;
                model.MaQuanHuyen = obj.MaQuanHuyen;
                model.IsDelete = false;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = userName;
                if (db.SaveChanges() > 0)
                    return true;
                return false;
            }
            return false;
        }
        public JsonResult GetPhuongXa()
        {
            try
            {
                List<SubSelectBoxString> listTree = new List<SubSelectBoxString>();
                SubSelectBoxString sc;
                var listData = DVHC_PhuongXa_SelectAll();
                var listData0 = DVHC_QuanHuyen_SelectAll();
                string tag = "";
                foreach (var item in listData0)
                {
                    sc = new SubSelectBoxString();
                    sc.Code = item.MaQuanHuyen;
                    sc.Name = item.TenQuanHuyen;
                    listTree.Add(sc);
                    BuildSubTree(listTree, item.MaQuanHuyen, listData, tag);
                }
                var jsonResults = new { listTree, state = true };
                return Json(jsonResults, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
        public void BuildSubTree(List<SubSelectBoxString> listTree, string ParentCode, List<CSF_HCXa> listData, string tag)
        {
            try
            {
                SubSelectBoxString sc;
                var listChild = listData.Where(x => x.MaQuanHuyen == ParentCode);
                if (listChild != null && listChild.Count() > 0)
                {
                    tag += "--- ";
                    foreach (var item in listChild)
                    {
                        sc = new SubSelectBoxString();
                        sc.Code = item.MaPhuongXa;
                        sc.Name = tag + item.TenPhuongXa;
                        listTree.Add(sc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}