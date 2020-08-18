using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
  public  class CMS_Videos_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Videos_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CMS_Videos_LayTatCa_Result> Search(string keyWord, int type)
        {
            try
            {
                List<CMS_Videos_LayTatCa_Result> lData = MyContext.CMS_Videos_LayTatCa(type, keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CMS_Videos entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_Videos.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }


        public bool Update(CMS_Videos entity)
        {
            try
            {
                CMS_Videos obj = MyContext.CMS_Videos.Find(entity.ID);
                obj.Name = entity.Name;
                obj.TypeOfVideoID = entity.TypeOfVideoID;
                obj.Description = entity.Description;
                obj.FileName = entity.FileName;
                obj.Publish = entity.Publish;
                obj.AvatarPicture = entity.AvatarPicture;
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
                var obj = MyContext.CMS_Videos.Find(id);
                MyContext.CMS_Videos.Remove(obj);
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
