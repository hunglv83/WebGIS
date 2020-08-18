using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
  public  class CMS_Links_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Links_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CMS_Links_LayTatCa_Result> Search(string keyWord)
        {
            try
            {
                List<CMS_Links_LayTatCa_Result> lData = MyContext.CMS_Links_LayTatCa(keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CMS_Links entity)
        {
            try
            {
                MyContext.CMS_Links.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }


        public bool Update(CMS_Links entity)
        {
            try
            {
                CMS_Links obj = MyContext.CMS_Links.Find(entity.ID);
                obj.Name = entity.Name;
                obj.Url = entity.Url;
                obj.Picture = entity.Picture;
                obj.Shows = entity.Shows;
                obj.Order = entity.Order;
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
                var obj = MyContext.CMS_Links.Find(id);
                MyContext.CMS_Links.Remove(obj);
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
