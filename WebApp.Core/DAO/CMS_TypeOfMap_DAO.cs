using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Core.DAO
{
    public class CMS_TypeOfMap_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_TypeOfMap_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }
        public List<CMS_TypeOfMap_LayTatCa_Result> Search(string keyWord)
        {
            try
            {
                List<CMS_TypeOfMap_LayTatCa_Result> lData = MyContext.CMS_TypeOfMap_LayTatCa(keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool Update(CMS_TypeOfMap entity)
        {
            try
            {
                CMS_TypeOfMap obj = MyContext.CMS_TypeOfMap.Find(entity.ID);
                obj.Name = entity.Name;
                obj.Description = entity.Description;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CMS_TypeOfMap entity)
        {
            try
            {
                MyContext.CMS_TypeOfMap.Add(entity);
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
                var obj = MyContext.CMS_TypeOfMap.Find(id);
                MyContext.CMS_TypeOfMap.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_TypeOfMap GetTypeOfMapByID(int id)
        {
            try
            {
                return MyContext.CMS_TypeOfMap.Find(id);
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
                var obj = MyContext.CMS_Maps.Where(x => x.TypeOfMapID == id).FirstOrDefault();
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
