using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
    public class CMS_News_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_News_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }
        public List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result> LayTinBaiCongKhaiTheoCateKey(string strCateKey)
        {
            try
            {
                return MyContext.CMS_News_LayTinBaiCongKhaiTheoCateKey(strCateKey).ToList();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public List<CMS_News_LayTatCa_Result> Search(string keyWord, int category, int newStatus)
        {
            try
            {
                List<CMS_News_LayTatCa_Result> lData = MyContext.CMS_News_LayTatCa(category, keyWord, newStatus).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public List<CMS_News_LayCongBoK0CongBo_Result> LayDanhSachDeCongBo(string keyWord, int category)
        {
            try
            {
                List<CMS_News_LayCongBoK0CongBo_Result> lData = MyContext.CMS_News_LayCongBoK0CongBo(category, keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public int Insert(CMS_News entity)
        {
            try
            {
                entity.CREATEDATE = System.DateTime.Now;
                entity.ID_NEWS_STATUS = 1;
                entity.NUMBEROFVIEW = 0;
                entity.ISFOCUS = 0;
                MyContext.CMS_News.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }


        public int InsertVaDuyet(CMS_News entity)
        {
            try
            {
                entity.CREATEDATE = System.DateTime.Now;
                entity.ID_NEWS_STATUS = 2;
                entity.NUMBEROFVIEW = 0;
                entity.ISFOCUS = 0;
                MyContext.CMS_News.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool UpdateTrangThai(int iIDNews, Int16 iTrangThai, Int16 iTieuDiem)
        {
            try
            {
                CMS_News obj = MyContext.CMS_News.Find(iIDNews);
                obj.ID_NEWS_STATUS = iTrangThai;
                obj.ISFOCUS = iTieuDiem;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }


        public bool Update(CMS_News entity)
        {
            try
            {
                CMS_News obj = MyContext.CMS_News.Find(entity.ID);
                obj.TITLE = entity.TITLE;
                obj.CONTENTS = entity.CONTENTS;
                obj.EXCERPT = entity.EXCERPT;
                obj.ID_CATEGORIES = entity.ID_CATEGORIES;
                obj.PICTURE = entity.PICTURE;
                obj.EDITDATE = System.DateTime.Now;
                obj.ID_USERMODIFY = entity.ID_USERMODIFY;
                MyContext.Entry(obj).State = EntityState.Modified;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public bool UpdateVaDuyet(CMS_News entity)
        {
            try
            {
                CMS_News obj = MyContext.CMS_News.Find(entity.ID);
                obj.TITLE = entity.TITLE;
                obj.CONTENTS = entity.CONTENTS;
                obj.EXCERPT = entity.EXCERPT;
                obj.ID_CATEGORIES = entity.ID_CATEGORIES;
                obj.PICTURE = entity.PICTURE;
                obj.EDITDATE = System.DateTime.Now;
                obj.ID_USERMODIFY = entity.ID_USERMODIFY;
                if (obj.ID_NEWS_STATUS == 1 || obj.ID_NEWS_STATUS == 3 || obj.ID_NEWS_STATUS == 6)
                {
                    obj.ID_NEWS_STATUS = 2;
                }
                MyContext.Entry(obj).State = EntityState.Modified;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var ykien = MyContext.CMS_Approves.Where(x => x.ID_News == id);
                if (ykien != null && ykien.Count() > 0)
                {
                    foreach (var item in ykien)
                    {
                        MyContext.CMS_Approves.Remove(item);
                    }
                }
                var obj = MyContext.CMS_News.Find(id);
                MyContext.CMS_News.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

    }

}
