using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Core.DAO
{
    public class CMS_Maps_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Maps_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }


        public List<CMS_Maps> GetAll()
        {
            try
            {
                List<CMS_Maps> lData = MyContext.CMS_Maps.ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }


        public List<CMS_Maps_LayTatCa_Result> Search(string keyWord, int TypeOfMapID)
        {
            try
            {
                List<CMS_Maps_LayTatCa_Result> lData = MyContext.CMS_Maps_LayTatCa(TypeOfMapID, keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool Update(CMS_Maps entity)
        {
            try
            {
                CMS_Maps obj = MyContext.CMS_Maps.Find(entity.ID);
                obj.Name = entity.Name;
                obj.Description = entity.Description;
                obj.TypeOfMapID = entity.TypeOfMapID;
                obj.UserCreate = entity.UserCreate;
                //obj.Thumbnail = string.IsNullOrEmpty(obj.Thumbnail) ? "thumbnail_DEFAULT.jpg" : entity.Thumbnail;

                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CMS_Maps entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_Maps.Add(entity);
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
                var obj = MyContext.CMS_Maps.Find(id);
                MyContext.CMS_Maps.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_Maps GetMapByID(int id)
        {
            try
            {
                return MyContext.CMS_Maps.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }




    }
}
