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


namespace WebApp.Areas.Admin.Controllers.QuanLyVanBan
{
    public class CMS_OrganizationController : BaseController
    {
        //
        // GET: /Admin/CMS_Organization/

        [CheckPermission]
        public ActionResult Index(string keyWord, int? page)
        {
            try
            {
                CMS_Organization_DAO objDAO = new CMS_Organization_DAO();
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
                CMS_Organization_DAO objDAO = new CMS_Organization_DAO();
                var Partial = objDAO.GetOrganizationByID(id);
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                List<SubSelectBox> listTree = new List<SubSelectBox>();
                SubSelectBox sc;
                var listData = MyContext.CMS_Organization.ToList();
                var listData0 = listData.Where(x => x.ParentID == 0).ToList();
                string tag = "";
                foreach (var item in listData0)
                {
                    sc = new SubSelectBox();
                    sc.id = item.ID;
                    sc.name = item.Name;
                    listTree.Add(sc);
                    BuildSubTree(listTree, item.ID, listData, tag);
                }
                TempData["Organization"] = listTree;
                TempData.Keep("Organization");
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
                TempData["Organization"] = MyContext.CMS_Organization.ToList();
                TempData.Keep("Organization");

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
        public ActionResult Create(FormCollection fc, CMS_Organization obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_Organization_DAO objDAO = new CMS_Organization_DAO();
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm cơ quan/tổ chức thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Organization");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm cơ quan/tổ chức không thành công");
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
                CMS_Organization_DAO objDAO = new CMS_Organization_DAO();
                if (objDAO.CheckDelete(id))
                {
                    if (objDAO.Delete(id))
                    {
                        SetAlert("Xóa cơ quan/tổ chức thành công", AlertType.Success);
                        return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "Lỗi xóa cơ quan/tổ chức" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Cơ quan/tổ chức đang được sử dụng không được phép xóa." }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(FormCollection fc, CMS_Organization obj, HttpPostedFileBase file)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CMS_Organization_DAO objDAO = new CMS_Organization_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật cơ quan/tổ chức thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Organization");
                    }
                    else
                    {
                        SetAlert("Cập nhật cơ quan/tổ chức không thành công", AlertType.Error);
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

        public JsonResult GetParentID()
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                List<SubSelectBox> listTree = new List<SubSelectBox>();
                SubSelectBox sc;
                var listData = MyContext.CMS_Organization.ToList();
                var listData0 = listData.Where(x => x.ParentID == 0).ToList();
                string tag = "";
                foreach (var item in listData0)
                {
                    sc = new SubSelectBox();
                    sc.id = item.ID;
                    sc.name = item.Name;
                    listTree.Add(sc);
                    BuildSubTree(listTree, item.ID, listData, tag);
                }
                var jsonResults = new { listTree, state = true };
                return Json(jsonResults, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
        public void BuildSubTree(List<SubSelectBox> listTree, int ParentID, List<CMS_Organization> listData, string tag)
        {
            try
            {
                SubSelectBox sc;
                var listChild = listData.Where(x => x.ParentID == ParentID);
                if (listChild != null && listChild.Count() > 0)
                {
                    tag += "--- ";
                    foreach (var item in listChild)
                    {
                        sc = new SubSelectBox();
                        sc.id = item.ID;
                        sc.name = tag + item.Name;
                        listTree.Add(sc);
                        BuildSubTree(listTree, item.ID, listData, tag);
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
