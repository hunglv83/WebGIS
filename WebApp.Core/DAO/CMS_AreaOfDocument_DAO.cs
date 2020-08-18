using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
  public  class CMS_AreaOfDocument_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_AreaOfDocument_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CMS_AreaOfDocument_LayTatCa_Result> Search(string keyWord)
        {
            try
            {
                List<CMS_AreaOfDocument_LayTatCa_Result> lData = MyContext.CMS_AreaOfDocument_LayTatCa(keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CMS_AreaOfDocument entity)
        {
            try
            {
                MyContext.CMS_AreaOfDocument.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }


        public bool Update(CMS_AreaOfDocument entity)
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
                var obj = MyContext.CMS_AreaOfDocument.Find(id);
                MyContext.CMS_AreaOfDocument.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public CMS_AreaOfDocument GetAreaOfDocumentByID(int id)
        {
            try
            {
                return MyContext.CMS_AreaOfDocument.Find(id);
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
                var obj = MyContext.CMS_Documents.Where(x => x.AreaOfDocument == id).FirstOrDefault();
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
