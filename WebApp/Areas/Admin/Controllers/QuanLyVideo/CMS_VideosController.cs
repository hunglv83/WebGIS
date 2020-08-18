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
    public class CMS_VideosController : BaseController
    {
        //
        // GET: /Admin/CMS_Videos/

        [CheckPermission]
        public ActionResult Index(string keyword, string type, int? page)
        {
            try
            {
                int typeID = type != null ? Convert.ToInt32(type) : 0;
                keyword = keyword != null ? keyword : "";
                CMS_Videos_DAO objDAO = new CMS_Videos_DAO();
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
                CMS_Videos_DAO objDAO = new CMS_Videos_DAO();
                DT_WebGISEntities MVCContext = new DT_WebGISEntities();
                TempData["TypeOfVideoID"] = MVCContext.CMS_TypeOfVideo.ToList();
                TempData.Keep("TypeOfVideoID");
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
        public ActionResult Create(FormCollection fc, CMS_Videos obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                TempData.Keep("TypeOfVideoID");
                if (ModelState.IsValid)
                {

                    CMS_Videos_DAO objDAO = new CMS_Videos_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int intUserID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                    obj.UserCreate = intUserID;
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm video thành công", AlertType.Success);
                        return RedirectToAction("index", "CMS_Videos");
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


        #region CreateThumbnailImageFromVideo
        //[CheckPermission]
        //public void CreateThumbnailImage(string videoPath)
        //{
        //    try
        //    {
        //        videoPath = videoPath.Replace("/csf_mvc", "");
        //        string[] arrFileName = videoPath.Split('/');
        //        string fileName = arrFileName[arrFileName.Length - 1];
        //        string thumbnailPath = Server.MapPath("~/data/thumbVideo/");
        //        string thumb = thumbnailPath + fileName + ".jpg";
        //        videoPath = Server.MapPath("~" + videoPath);
        //        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        //        proc.StartInfo.FileName = Server.MapPath("~/data/files/ffmpeg.exe");
        //        proc.StartInfo.Arguments = " -i \"" + videoPath + "\" -f image2 -ss 00:00:05.050 -vframes 1 -s 250x180 -an \"" + thumb + "\"";
        //        //The command which will be executed
        //        proc.StartInfo.UseShellExecute = false;
        //        proc.StartInfo.CreateNoWindow = true;
        //        proc.StartInfo.RedirectStandardOutput = false;
        //        proc.Start();
        //        proc.WaitForExit();
        //        proc.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logs.WriteLog(ex);
        //    }
        //}
        #endregion

        [CheckPermission]
        public ActionResult Edit(int id)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                ViewBag.URLIMG = System.Configuration.ConfigurationManager.AppSettings["UrlImage"];
                DT_WebGISEntities entity = new DT_WebGISEntities();
                TempData["TypeOfVideoID"] = entity.CMS_TypeOfVideo.ToList();
                TempData.Keep("TypeOfVideoID");
                var doc = entity.CMS_Videos.Find(id);
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
        public ActionResult Edit(FormCollection fc, CMS_Videos obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                TempData.Keep("TypeOfVideoID");
                if (ModelState.IsValid)
                {
                    CMS_Videos_DAO objDAO = new CMS_Videos_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Videos");
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
                CMS_Videos_DAO objDAO = new CMS_Videos_DAO();
                if (objDAO.Delete(id))
                {
                    SetAlert("Xóa thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi xóa video" }, JsonRequestBehavior.AllowGet);
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
