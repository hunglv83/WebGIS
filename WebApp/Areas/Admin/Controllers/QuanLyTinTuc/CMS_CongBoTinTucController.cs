
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApp.Common;
using WebApp.Core.EF;
using WebApp.Core.DAO;
using WebApp.App_Start;
namespace WebApp.Areas.Admin.Controllers.QuanLyTinTuc
{
    public class CMS_CongBoTinTucController : BaseController
    {
        //
        // GET: /Admin/CMS_CongBoTinTuc/
         
        [CheckPermission]
        public ActionResult Index(string keyWord,  int? page)
        {
            try
            {
              
                keyWord = keyWord != null ? keyWord : "";
                CMS_News_DAO objDAO = new CMS_News_DAO();
                var data = objDAO.LayDanhSachDeCongBo(keyWord, 0);
                ViewBag.SearchString = keyWord;
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                TempData["DsTinTucNgungCongBo"] = objDAO.Search(keyWord, 0, 5).ToList().ToPagedList(pageNumber, pageSize);
                TempData.Keep("DsTinTucNgungCongBo");
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
        public JsonResult SetCongBo(string[] arrChecked, string[] checkedTieuDiem)
        {
            try
            {
                int ikq = 0;
                string[] arrIDTinTuc = arrChecked;
                string[] arrIDTinTucTieuDiem = checkedTieuDiem;
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
                        Int16 iTieuDiem = 0;
                        if (arrIDTinTucTieuDiem != null && arrIDTinTucTieuDiem.Length > 0)
                        {
                            for (int j = 0; j < arrIDTinTucTieuDiem.Length; j++)
                            {
                                if (arrIDTinTuc[i].ToString()==arrIDTinTucTieuDiem[j].ToString())
                                {
                                    iTieuDiem = 1;
                                    break;
                                }
                            }
                        }
                        iIDNews = Convert.ToInt32(arrIDTinTuc[i].ToString());
                        objNewsDao.UpdateTrangThai(iIDNews, 5, iTieuDiem);
                        ikq = 1;
                    }
                }
                if (ikq > 0)
                {
                    //cache lai tin bai
                    WebApp.Controllers.HomeController hController = new WebApp.Controllers.HomeController();
                    hController.Cache_TinBai();
                    return Json(new { status = true, message = "Công bố tin tức thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi công bố tin tức" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return Json(new { status = false, message = "Lỗi: " + ex }, JsonRequestBehavior.AllowGet);
            }
        }
        [CheckPermission]
        public JsonResult SetNgungCongBo(string[] arrChecked, string abc)
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
                        objNewsDao.UpdateTrangThai(iIDNews, 6,0);
                        objApp.ID_News = iIDNews;
                        objApp.ID_News_Status = 6;
                        CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                        int intUserID = objUserDao.GetUserIDByUserName(HttpContext.User.Identity.Name);
                        objApp.ID_User = intUserID;
                        objApp.Comment = abc;
                        ikq = objApproverDao.Insert(objApp);
                    }

                }
                if (ikq > 0)
                {
                    //cache lai tin bai
                    WebApp.Controllers.HomeController hController = new WebApp.Controllers.HomeController();
                    hController.Cache_TinBai();
                    return Json(new { status = true, message = "Ngừng công bố tin tức thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Lỗi ngừng công bố tin tức" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return Json(new { status = false, message = "Lỗi: " + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [CheckPermission]
        public ActionResult DSNgungCongBo(string keyWord1,  int? page1)
        {
            try
            {
                //int categoryID = category != null ? Convert.ToInt32(category) : 0;

                keyWord1 = keyWord1 != null ? keyWord1 : "";
                CMS_News_DAO objDAO = new CMS_News_DAO();
                var data = objDAO.Search(keyWord1, 0, 5).ToList();
                ViewBag.SearchString = keyWord1;
                int pageSize1 = 10;
                int pageNumber1 = (page1 ?? 1);

                return PartialView(data.ToPagedList(pageNumber1, pageSize1));
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
    }
}
