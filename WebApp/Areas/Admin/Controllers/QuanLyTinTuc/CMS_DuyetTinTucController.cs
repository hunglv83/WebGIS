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
namespace WebApp.Areas.Admin.Controllers.QuanLyTinTuc
{
    public class CMS_DuyetTinTucController : BaseController
    {
        //
        // GET: /Admin/CMS_DuyetTinTuc/

        [CheckPermission]
        public ActionResult Index(string keyWord, string category, string newsStatus, int? page)
        {
            try
            {
                int categoryID = category != null ? Convert.ToInt32(category) : 0;
                int newsStatusID = newsStatus != null ? Convert.ToInt32(newsStatus) : 0;
                keyWord = keyWord != null ? keyWord : "";
                CMS_News_DAO objDAO = new CMS_News_DAO();
                var data = objDAO.Search(keyWord, categoryID, 2);
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
        [HttpPost]
        public JsonResult SetDuyet(string[] arrChecked, string abc)
        {
            try
            {
                int ikq = 0;
                string[] arrIDTinTuc = arrChecked;
                CMS_News_DAO objNewsDao = new CMS_News_DAO();
                CMS_Approves_DAO objApproverDao = new CMS_Approves_DAO();
                CMS_Approves objApp = new CMS_Approves();
                int iIDNews = 0;
                //int intRoleID = Convert.ToInt32(RoleID);
                //int intUserID = 0;
                //CSF_UserRole_DAO objUserRoleDAO = new CSF_UserRole_DAO();
                if (arrIDTinTuc != null && arrIDTinTuc.Length > 0)
                {
                    for (int i = 0; i < arrIDTinTuc.Length; i++)
                    {
                        iIDNews = Convert.ToInt32(arrIDTinTuc[i].ToString());
                        objNewsDao.UpdateTrangThai(iIDNews, 4,0);
                        objApp.ID_News = iIDNews;
                        objApp.ID_News_Status = 4;
                        CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                        int intUserID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                        objApp.ID_User = intUserID;
                        objApp.Comment = abc;
                        ikq = objApproverDao.Insert(objApp);
                    }

                }
                if (ikq > 0)
                {

                    return Json(new { status = true, message = "Duyệt tin tức thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi duyệt tin tức" }, JsonRequestBehavior.AllowGet);
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
        public JsonResult SetTuChoi(string[] arrChecked, string abc)
        {
            try
            {
                int ikq = 0;
                string[] arrIDTinTuc = arrChecked;
                CMS_News_DAO objNewsDao = new CMS_News_DAO();
                CMS_Approves_DAO objApproverDao = new CMS_Approves_DAO();
                CMS_Approves objApp = new CMS_Approves();
                int iIDNews = 0;
                if (arrIDTinTuc != null && arrIDTinTuc.Length > 0)
                {
                    for (int i = 0; i < arrIDTinTuc.Length; i++)
                    {
                        iIDNews = Convert.ToInt32(arrIDTinTuc[i].ToString());
                        objNewsDao.UpdateTrangThai(iIDNews, 3,0);
                        objApp.ID_News = iIDNews;
                        objApp.ID_News_Status = 3;
                        CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                        int intUserID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                        objApp.ID_User = intUserID;
                        objApp.Comment = abc;
                        ikq = objApproverDao.Insert(objApp);
                    }

                }
                if (ikq > 0)
                {
                    return Json(new { status = true, message = "Không duyệt tin tức thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi không duyệt tin tức" }, JsonRequestBehavior.AllowGet);
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
