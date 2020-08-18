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
    public class CMS_DocumentsController : BaseController
    {
        //
        // GET: /Admin/CMS_Documents/

        [CheckPermission]
        public ActionResult Index(string keyWord, string type, string area, string org, int? page)
        {
            try
            {
                int typeID = type != null ? Convert.ToInt32(type) : 0;
                int areaID = area != null ? Convert.ToInt32(area) : 0;
                int orgID = org != null ? Convert.ToInt32(org) : 0;
                keyWord = keyWord != null ? keyWord : "";
                CMS_Documents_DAO objDAO = new CMS_Documents_DAO();
                var data = objDAO.Search(keyWord, typeID, areaID, orgID);
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
                CMS_Documents_DAO objDAO = new CMS_Documents_DAO();
                DT_WebGISEntities MVCContext = new DT_WebGISEntities();
                TempData["TypeOfDocumentID"] = MVCContext.CMS_TypeOfDocument.ToList();
                TempData.Keep("TypeOfDocumentID");
                TempData["AreaOfDocument"] = MVCContext.CMS_AreaOfDocument.ToList();
                TempData.Keep("AreaOfDocument");
                TempData["OrganizationID"] = MVCContext.CMS_Organization.ToList();
                TempData.Keep("OrganizationID");
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
        public ActionResult Create(FormCollection fc, CMS_Documents obj)
        {
            try
            {
                TempData.Keep("TypeOfDocumentID");
                TempData.Keep("AreaOfDocument");
                TempData.Keep("OrganizationID");
                if (ModelState.IsValid)
                {

                    CMS_Documents_DAO objDAO = new CMS_Documents_DAO();
                    if (objDAO.CheckExist_DocumentNumber(obj))
                    {
                        SetAlert("Số hiệu đã tồn tại!", AlertType.Error);
                        return View();
                    }
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int intUserID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                    obj.UserCreate = intUserID;
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        //cache lai van ban
                        WebApp.Controllers.HomeController hController = new WebApp.Controllers.HomeController();
                        hController.Cache_VanBan();
                        SetAlert("Thêm văn bản thành công", AlertType.Success);
                        return RedirectToAction("index", "cms_documents");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm văn bản không thành công");
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
                DT_WebGISEntities entity = new DT_WebGISEntities();
                TempData["TypeOfDocumentID"] = entity.CMS_TypeOfDocument.ToList();
                TempData.Keep("TypeOfDocumentID");
                TempData["AreaOfDocument"] = entity.CMS_AreaOfDocument.ToList();
                TempData.Keep("AreaOfDocument");
                TempData["OrganizationID"] = entity.CMS_Organization.ToList();
                TempData.Keep("OrganizationID");
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
                var doc = entity.CMS_Documents.Find(id);
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
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection fc, CMS_Documents obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CMS_Documents_DAO objDAO = new CMS_Documents_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    if (objDAO.CheckExist_DocumentNumber(obj))
                    {
                        SetAlert("Số hiệu đã tồn tại!", AlertType.Error);
                        return View();
                    }
                    if (objDAO.Update(obj))
                    {
                        //cache lai van ban
                        WebApp.Controllers.HomeController hController = new WebApp.Controllers.HomeController();
                        hController.Cache_VanBan();
                        SetAlert("Cập nhật văn bản thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Documents");
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
                CMS_Documents_DAO objDAO = new CMS_Documents_DAO();
                if (objDAO.Delete(id))
                {
                    //cache lai van ban
                    WebApp.Controllers.HomeController hController = new WebApp.Controllers.HomeController();
                    hController.Cache_VanBan();
                    SetAlert("Xóa văn bản thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi xóa văn bản" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return Json(new { status = false, message = "Lỗi: " + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [CheckPermission]
        public ActionResult Detail(int id)
        {
            try
            {
                DT_WebGISEntities entity = new DT_WebGISEntities();
                var vb = entity.CMS_Documents.Find(id);
                return View(vb);
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
