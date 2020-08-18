using WebApp.Core.DAO;

using WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApp.App_Start;
using WebApp.Core.EF;

namespace WebApp.Areas.Admin.Controllers
{
    public class CMS_QuestionsController : BaseController
    {
        //
        // GET: /Admin/News/

        [CheckPermission]
        public ActionResult Index(string keyWord, string type, int? page)
        {
            try
            {
                int typeID = type != null ? Convert.ToInt32(type) : 0;
                keyWord = keyWord != null ? keyWord : "";
                CMS_Questions_DAO objDAO = new CMS_Questions_DAO();
                var data = objDAO.Search(keyWord, typeID, null);
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
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                CMS_Questions_DAO objDAO = new CMS_Questions_DAO();
                DT_WebGISEntities MVCContext = new DT_WebGISEntities();
                TempData["TypeOfQuestionID"] = MVCContext.CMS_TypeOfQuestion.ToList();
                TempData.Keep("TypeOfQuestionID");
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
        public ActionResult Create(FormCollection fc, CMS_Questions obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                TempData.Keep("TypeOfQuestionID");
                if (ModelState.IsValid)
                {

                    CMS_Questions_DAO objDAO = new CMS_Questions_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int userID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                    obj.UserCreate = userID;
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Thêm câu hỏi thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Questions");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm không thành công");
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
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                ViewBag.URLIMAGE = System.Configuration.ConfigurationManager.AppSettings["UrlImage"];
                DT_WebGISEntities entity = new DT_WebGISEntities();
                TempData["TypeOfQuestionID"] = entity.CMS_TypeOfQuestion.ToList();
                TempData.Keep("TypeOfQuestionID");
                var news = entity.CMS_Questions.Find(id);
                if (news != null)
                {
                    return View(news);
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
        public ActionResult Edit(FormCollection fc, CMS_Questions obj)
        {
            try
            {
                //if (!this.HasPermission) return RedirectToAction("Unauthorized", "Home");
                TempData.Keep("TypeOfQuestionID");
                if (ModelState.IsValid)
                {
                    CMS_Questions_DAO objDAO = new CMS_Questions_DAO();
                    CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                    int userID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                    obj.UserCreate = userID;
                    if (objDAO.Update(obj))
                    {
                        SetAlert("Cập nhật thành công", AlertType.Success);
                        return RedirectToAction("Index", "CMS_Questions");
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
                CMS_Questions_DAO objDAO = new CMS_Questions_DAO();
                if (objDAO.Delete(id))
                {
                    SetAlert("Xóa câu hỏi thành công", AlertType.Success);
                    return Json(new { status = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi xóa câu hỏi" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return Json(new { status = false, message = "Lỗi: " + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Detail(string idquestion)
        {
            try
            {
                DT_WebGISEntities entity = new DT_WebGISEntities();
                int questionID = Convert.ToInt32(idquestion);
                var q = (from a in entity.CMS_Questions where a.ID == questionID select new {a.Title, a.Contents, a.Answer, a.FileName }).FirstOrDefault();
                if (q != null)
                {
                    return Json(new { state = true, objQ = q}, JsonRequestBehavior.AllowGet);
                }
                return Json(new { state = false, mess = "Không tìm thấy dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
