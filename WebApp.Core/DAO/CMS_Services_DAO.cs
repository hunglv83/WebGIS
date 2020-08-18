using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Core.DAO
{
    public class CMS_Services_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Services_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }
        public List<CMS_Services_LayTatCa_Result> Search(string keyWord)
        {
            try
            {
                List<CMS_Services_LayTatCa_Result> lData = MyContext.CMS_Services_LayTatCa(keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool Update(CMS_Services entity)
        {
            try
            {
                CMS_Services obj = MyContext.CMS_Services.Find(entity.ID);
                obj.Name = entity.Name;
                obj.Description = entity.Description;
                obj.URL = entity.URL;
                obj.Publish = entity.Publish;
                obj.Source = entity.Source;
                obj.XMax = entity.XMax;
                obj.XMin = entity.XMin;
                obj.YMax = entity.YMax;
                obj.YMin = entity.YMin;
                obj.TypeOfMapID = entity.TypeOfMapID;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CMS_Services entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_Services.Add(entity);
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
                var obj = MyContext.CMS_Services.Find(id);
                MyContext.CMS_Services.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_Services GetMapByID(int id)
        {
            try
            {
                return MyContext.CMS_Services.Find(id);
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
                var obj = MyContext.CMS_MapService.Where(x => x.ServiceID == id).FirstOrDefault();
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
