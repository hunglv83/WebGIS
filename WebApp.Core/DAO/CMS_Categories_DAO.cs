using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Core.EF;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
    public class CMS_Categories_DAO
    {

        DT_WebGISEntities MyContext = null;
        public CMS_Categories_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CMS_Categories_LayTatCa_Result> Search(string strSearch)
        {
            try
            {
                List<CMS_Categories_LayTatCa_Result> objs = MyContext.CMS_Categories_LayTatCa().ToList();
                if (!String.IsNullOrEmpty(strSearch))
                {
                    objs = objs.Where(f => f.NAME.ToLower().Contains(strSearch.ToLower())).ToList();
                }
                return objs;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool CheckExist_CategoriesKey(CMS_Categories entity)
        {
            try
            {
                var obj = MyContext.CMS_Categories.Where(x => x.KEY.Trim()==entity.KEY.Trim()).FirstOrDefault();
                if (obj != null)
                {
                    if (entity.ID != obj.ID)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public bool Update(CMS_Categories entity)
        {
            try
            {
                CMS_Categories obj = MyContext.CMS_Categories.Find(entity.ID);
                obj.NAME = entity.NAME;
                obj.DESCRIPTION = entity.DESCRIPTION;
                obj.PICTURE = entity.PICTURE;
                obj.PUBLISH = entity.PUBLISH;
                obj.KEY = entity.KEY;
                obj.ORDERS = entity.ORDERS;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CMS_Categories entity)
        {
            try
            {

                MyContext.CMS_Categories.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var obj = MyContext.CMS_Categories.Find(id);
                MyContext.CMS_Categories.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_Categories GetCategoriesByID(int id)
        {
            try
            {
                return MyContext.CMS_Categories.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public bool CheckDelete(int id)
        {
            try
            {
                var obj = MyContext.CMS_News.Where(x => x.ID_CATEGORIES == id).FirstOrDefault();
                if (obj != null)
                {
                    return false;
                }
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
