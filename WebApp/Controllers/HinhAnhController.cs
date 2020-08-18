using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HinhAnhController : Controller
    {
        //
        // GET: /HinhAnh/
        // cate: loai hinh anh
        public PartialViewResult IndexHinhAnh(string title, string category, string cate)
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                ViewBag.TITLE = title;
                int cateID = 0;
                if (cate == null)
                {
                    var objCate = MyContext.CMS_TypeOfPhoto.FirstOrDefault();
                    cateID = objCate.ID;
                    ViewBag.CATENAME = objCate.Name;
                }
                else
                {
                    cateID = Convert.ToInt32(cate);
                    var objCate = MyContext.CMS_TypeOfPhoto.Find(cateID);
                    ViewBag.CATENAME = objCate.Name;
                }
                var listData = MyContext.CMS_Photos.Where(s => s.Publish == true && s.TypeOfPhotoID == cateID).OrderBy(s => s.CreateDate).ToList();

                //album
                TempData["ALBUM"] = MyContext.CMS_TypeOfPhoto.ToList();
                TempData.Keep("ALBUM");
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }
        public PartialViewResult ThuVienHinhAnh(string title, string category)
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                ViewBag.TITLE = title;
                var listData = MyContext.CMS_Photos.Where(s=>s.Publish==true).OrderByDescending(s=>s.CreateDate).Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

    }
}
