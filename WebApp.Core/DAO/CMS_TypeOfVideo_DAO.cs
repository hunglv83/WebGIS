using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
  public  class CMS_TypeOfVideo_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_TypeOfVideo_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CMS_TypeOfVideo_LayTatCa_Result> Search(string keyWord)
        {
            try
            {
                List<CMS_TypeOfVideo_LayTatCa_Result> lData = MyContext.CMS_TypeOfVideo_LayTatCa(keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CMS_TypeOfVideo entity)
        {
            try
            {
                MyContext.CMS_TypeOfVideo.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }


        public bool Update(CMS_TypeOfVideo entity)
        {
            try
            {
                MyContext.Entry(entity).State = EntityState.Modified;
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
                var obj = MyContext.CMS_TypeOfVideo.Find(id);
                MyContext.CMS_TypeOfVideo.Remove(obj);
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
