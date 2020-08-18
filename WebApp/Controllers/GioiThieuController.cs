using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Common;
using WebApp.Core.DAO;
using WebApp.Core.EF;

namespace WebApp.Controllers
{
    public class GioiThieuController : Controller
    {
        //
        // GET: /GioiThieu/
        HomeController objHomeController = new HomeController();
        public PartialViewResult GioiThieu(string title, string category)
        {
            try
            {
                ViewBag.CategoryTitle = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result> lstGioiThieu = new List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result>();
                lstGioiThieu = objHomeController.GetCacheTinBai().Where(s => s.CateKey == "gioi-thieu-chung").ToList();
                if (lstGioiThieu !=null && lstGioiThieu.Count>0)
                {
                    ViewBag.TITLE = lstGioiThieu[0].TITLE;
                    ViewBag.EXCERPT = lstGioiThieu[0].EXCERPT;
                    ViewBag.CONTENTS = lstGioiThieu[0].CONTENTS;    
                }
                
                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult GioiThieuChung(string title, string category)
        {
            try
            {
                ViewBag.CategoryTitle = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result> lstGioiThieu = new List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result>();
                lstGioiThieu = objHomeController.GetCacheTinBai().Where(s => s.CateKey == category).ToList();
                if (lstGioiThieu != null && lstGioiThieu.Count > 0)
                {
                    ViewBag.TITLE = lstGioiThieu[0].TITLE;
                    ViewBag.EXCERPT = lstGioiThieu[0].EXCERPT;
                    ViewBag.CONTENTS = lstGioiThieu[0].CONTENTS;
                }

                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult CoCauToChuc(string title, string category)
        {
            try
            {
                ViewBag.CategoryTitle = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result> lstGioiThieu = new List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result>();
                lstGioiThieu = objHomeController.GetCacheTinBai().Where(s => s.CateKey == category).ToList();
                if (lstGioiThieu != null && lstGioiThieu.Count > 0)
                {
                    ViewBag.TITLE = lstGioiThieu[0].TITLE;
                    ViewBag.EXCERPT = lstGioiThieu[0].EXCERPT;
                    ViewBag.CONTENTS = lstGioiThieu[0].CONTENTS;
                }

                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ChucNangNhiemVu(string title, string category)
        {
            try
            {
                ViewBag.CategoryTitle = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result> lstGioiThieu = new List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result>();
                lstGioiThieu = objHomeController.GetCacheTinBai().Where(s => s.CateKey == category).ToList();
                if (lstGioiThieu != null && lstGioiThieu.Count > 0)
                {
                    ViewBag.TITLE = lstGioiThieu[0].TITLE;
                    ViewBag.EXCERPT = lstGioiThieu[0].EXCERPT;
                    ViewBag.CONTENTS = lstGioiThieu[0].CONTENTS;
                }

                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult LichSuPhatTrien(string title, string category)
        {
            try
            {
                ViewBag.CategoryTitle = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result> lstGioiThieu = new List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result>();
                lstGioiThieu = objHomeController.GetCacheTinBai().Where(s => s.CateKey == category).ToList();
                if (lstGioiThieu != null && lstGioiThieu.Count > 0)
                {
                    ViewBag.TITLE = lstGioiThieu[0].TITLE;
                    ViewBag.EXCERPT = lstGioiThieu[0].EXCERPT;
                    ViewBag.CONTENTS = lstGioiThieu[0].CONTENTS;
                }

                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

    }
}
