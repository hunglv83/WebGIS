using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
  public  class CMS_Photos_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Photos_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CMS_Photos_LayTatCa_Result> Search(string keyWord, int type)
        {
            try
            {
                List<CMS_Photos_LayTatCa_Result> lData = MyContext.CMS_Photos_LayTatCa(type, keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CMS_Photos entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_Photos.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }


        public bool Update(CMS_Photos entity)
        {
            try
            {
                CMS_Photos obj = MyContext.CMS_Photos.Find(entity.ID);
                obj.Name = entity.Name;
                obj.TypeOfPhotoID = entity.TypeOfPhotoID;
                obj.Description = entity.Description;
                obj.FileName = entity.FileName;
                obj.Publish = entity.Publish;
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
                var obj = MyContext.CMS_Photos.Find(id);
                MyContext.CMS_Photos.Remove(obj);
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
