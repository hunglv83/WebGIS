using WebApp.Core.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApp.Common;
using WebApp.Core.EF;
using WebApp.App_Start;
using System.IO;

namespace WebApp.Areas.Admin.Controllers.QuanLyBanDo
{
    public class CMS_MapsController : BaseController
    {
        //
        // GET: /Admin/CMS_Maps/

        [CheckPermission]
        public ActionResult Index(string searchString, int? TypeOfMap, int? page)
        {
            try
            {
                //int TypeOfMapID = TypeOfMap != null ? Convert.ToInt32(TypeOfMap) : 0;
                int TypeOfMapID = 0;
                searchString = searchString != null ? searchString : "";
                CMS_Maps_DAO objDAO = new CMS_Maps_DAO();
                var data = objDAO.GetAll();
                ViewBag.SearchString = searchString;

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
        public ActionResult EditBanDo(int id)
        {
            try
            {
                CMS_Maps_DAO objDAO = new CMS_Maps_DAO();
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                CMS_MapService_DAO objMapSeveDAO = new CMS_MapService_DAO();
                CMS_Services_DAO objSeverDAO = new CMS_Services_DAO();
                var Partial = objDAO.GetMapByID(id);
                var list = MyContext.CMS_TypeOfMap.ToList();
                TempData["TypeOfMap"] = new SelectList(list, "ID", "Name", (int)Partial.TypeOfMapID);
                TempData.Keep("TypeOfMap");

                var list1 = MyContext.CMS_MapService_ByMapID(id).ToList();
                TempData["MapServices"] = list1.ToList();
                TempData.Keep("MapServices");
                var list2 = MyContext.CMS_Services.Where(s => s.Publish == true).ToList();
                TempData["Services"] = list2;
                TempData.Keep("Services");
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
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditBanDo(FormCollection fc, CMS_Maps obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string strThumbnailDir = System.Configuration.ConfigurationManager.AppSettings["ThumbnailFileUrl"].ToString();
                    var item = Request.Files.AllKeys.Where(m => m.StartsWith("Thumbnail")).FirstOrDefault();
                    var imgThumbnail = Request.Files[item];
                    var fileName = "thumbnail_" + DateTime.Now.ToString("yyyyMMddhhmmsstt") + Guid.NewGuid().ToString() + Path.GetExtension(imgThumbnail.FileName);

                    //if (imgThumbnail != null && imgThumbnail.ContentLength > 0)
                    //{
                    //    obj.Thumbnail = fileName;
                    //}

                    CMS_MapService objMapService = new CMS_MapService();
                    CMS_Maps_DAO objDAO = new CMS_Maps_DAO();
                    CMS_MapService_DAO objDAOMapService = new CMS_MapService_DAO();
                    string strIDService = "0";
                    if (fc["ddlServices"] != null)
                    {
                        strIDService = fc["ddlServices"].ToString();
                    }
                    if (strIDService == "0")
                    {
                        SetAlert("Chọn Services", AlertType.Error);
                        return View();
                    }
                    if (objDAO.Update(obj))
                    {
                        // Save file
                        var path = Path.Combine(Server.MapPath(strThumbnailDir), fileName);
                        imgThumbnail.SaveAs(path);

                        objDAOMapService.DeleteByIDMap(obj.ID);
                        objMapService.MapID = obj.ID;
                        objMapService.Orders = 0;
                            objMapService.ServiceID = Convert.ToInt32(strIDService);
                        objDAOMapService.Insert(objMapService);
                        SetAlert("Cập nhật bản đồ thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Maps");
                    }
                    else
                    {
                        SetAlert("Cập nhật bản đồ không thành công", AlertType.Error);
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
        public ActionResult CreateBanDo()
        {
            try
            {

                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                TempData["TypeOfMap"] = MyContext.CMS_TypeOfMap.ToList();
                TempData.Keep("TypeOfMap");
                TempData["Services"] = MyContext.CMS_Services.Where(s=>s.Publish==true).ToList();
                TempData.Keep("Services");
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
        public ActionResult CreateBanDo(FormCollection fc, CMS_Maps obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string strThumbnailDir = System.Configuration.ConfigurationManager.AppSettings["ThumbnailFileUrl"].ToString();
                    var item = Request.Files.AllKeys.Where(m => m.StartsWith("Thumbnail")).FirstOrDefault();
                    var imgThumbnail = Request.Files[item];
                    if (imgThumbnail != null && imgThumbnail.ContentLength > 0)
                    {
                        //var fileName = "thumbnail_" + DateTime.Now.ToString("yyyyMMddhhmmsstt") + Guid.NewGuid().ToString() + Path.GetExtension(imgThumbnail.FileName);
                        //obj.Thumbnail = fileName;

                        // Save file
                        //var path = Path.Combine(Server.MapPath(strThumbnailDir), fileName);
                        //imgThumbnail.SaveAs(path);
                    }
                    else
                    {
                        //obj.Thumbnail = "thumbnail_DEFAULT.jpg";
                    }

                    CMS_MapService objMapService = new CMS_MapService();
                    CMS_Maps_DAO objDAO = new CMS_Maps_DAO();
                    CMS_MapService_DAO objDAOMapService = new CMS_MapService_DAO();
                    string strIDService = "0";
                    if (fc["ddlServices"] != null)
                    {
                        strIDService = fc["ddlServices"].ToString();
                    }
                    if (strIDService == "0")
                    {
                        SetAlert("Chọn Services", AlertType.Error);
                        return View();
                    }
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int intUserID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);

                    obj.UserCreate = intUserID;
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        objMapService.MapID = ReturnID;
                        objMapService.Orders = 0;
                        objMapService.ServiceID = Convert.ToInt32(strIDService);
                        objDAOMapService.Insert(objMapService);
                        SetAlert("Thêm bản đồ thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Maps");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm bản đồ không thành công");
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
                CMS_Maps_DAO objDAO = new CMS_Maps_DAO();
                CMS_MapService_DAO objDAOMapService = new CMS_MapService_DAO();
                objDAOMapService.DeleteByIDMap(id);
                if (objDAO.Delete(id))
                {
                    SetAlert("Xóa bản đồ thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi xóa bản đồ" }, JsonRequestBehavior.AllowGet);
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
