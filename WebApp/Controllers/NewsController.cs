using WebApp.Core.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using WebApp.Common;
using WebApp.Core.EF;

namespace WebApp.Controllers
{
    public class NewsController : BaseController
    {
        //
        // GET: /News/
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult ChiTietTinTuc(string title, string id)
        {
            try
            {
                ViewBag.TITLE = title;
                int newsid = Convert.ToInt32(id);
                using (DT_WebGISEntities MyContext = new DT_WebGISEntities())
                {
                    var news = MyContext.CMS_News.Find(newsid);
                    if (news != null)
                    {
                        int cateid = (int)news.ID_CATEGORIES;
                        var cate = MyContext.CMS_Categories.Find(cateid);
                        string catekey = cate.KEY;
                        ViewBag.CateName = cate.NAME;
                        HomeController hController = new HomeController();
                        var listData = hController.GetCacheTinBai().Where(s => s.CateKey == catekey).Where(x => x.ID != newsid).Take(5).ToList();
                        if (listData != null && listData.Count() > 0)
                        {
                            TempData["tinbaikhac"] = listData;
                            TempData.Keep("tinbaikhac");
                        }
                    }
                    return PartialView(news);
                }

            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult DSChienLuocQHKH(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                HomeController hController = new HomeController();
                var listData = hController.GetCacheTinBai().Where(x => x.CateKey == category).ToList();
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult DSTinTucTheoChuyenMuc(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                HomeController hController = new HomeController();
                var listData = hController.GetCacheTinBai().Where(x => x.CateKey == category).ToList();
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult DSDuAnHangMuc(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                //if (category.Trim()=="du-an-nhiem-vu")
                //{
                //    category = "du-an-hang-muc-dau-tu";
                //}
                HomeController hController = new HomeController();
                var listData = hController.GetCacheTinBai().Where(x => x.CateKey == category).ToList();
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
        public PartialViewResult DSDuAn(string title, string category, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                HomeController hController = new HomeController();
                var listData = hController.GetCacheTinBai().Where(x => x.CateKey == category).ToList();
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return PartialView(listData.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult DSThongBao(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult DSHoiDap(string title, string category, string key, string type, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                int typeID = type != null ? Convert.ToInt32(type) : 0;
                key = key != null ? key : "";
                CMS_Questions_DAO objDAO = new CMS_Questions_DAO();
                var data = objDAO.Search(key, typeID, true);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                using (DT_WebGISEntities MyContext = new DT_WebGISEntities())
                {
                    TempData["LoaiCauHoi"] = MyContext.CMS_TypeOfQuestion.ToList();
                    TempData.Keep("LoaiCauHoi");
                }
                ViewBag.KEY = key;
                ViewBag.TYPE = typeID;
                return PartialView(data.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult TimKiem(string title, string category, string key, string type, int? page)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;
                int typeID = type != null ? Convert.ToInt32(type) : 0;
                key = key != null ? key : "";
                CMS_News_DAO objDAO = new CMS_News_DAO();
                var data = objDAO.Search(key, typeID, 5);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                var listData = MyContext.CMS_Categories.Where(x => x.PUBLISH == true).ToList();
                List<SubSelectBox> listCate = new List<SubSelectBox>();
                SubSelectBox sc;
                var listData0 = listData.Where(x => x.PARENTCATE == 0).OrderBy(x => x.ORDERS);
                string tag = "";
                foreach (var item in listData0)
                {
                    sc = new SubSelectBox();
                    sc.id = item.ID;
                    sc.name = item.NAME;
                    listCate.Add(sc);
                    BuildSubCate(listCate, item.ID, listData, tag);
                }
                TempData["category"] = listCate;
                TempData.Keep("category");
                ViewBag.KEY = key;
                ViewBag.TYPE = typeID;
                return PartialView(data.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public void BuildSubCate(List<SubSelectBox> listCate, int ParentID, List<CMS_Categories> listData, string tag)
        {
            try
            {
                SubSelectBox sc;
                var listChild = listData.Where(x => x.PARENTCATE == ParentID);
                if (listChild != null && listChild.Count() > 0)
                {
                    tag += "--- ";
                    foreach (var item in listChild)
                    {
                        sc = new SubSelectBox();
                        sc.id = item.ID;
                        sc.name = tag + item.NAME;
                        listCate.Add(sc);
                        BuildSubCate(listCate, item.ID, listData, tag);
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
