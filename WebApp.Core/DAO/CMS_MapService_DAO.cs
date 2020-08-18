using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Core.DAO
{
    public class CMS_MapService_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_MapService_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }







        public bool Update(CMS_MapService entity)
        {
            try
            {
                //CMS_MapService obj = MyContext.CMS_MapService.Find(entity.ID);
                //obj.Name = entity.Name;
                //obj.Description = entity.Description;
                //obj.TypeOfMapID = entity.TypeOfMapID;
                //obj.UserCreate = entity.UserCreate;
                //entity.CreateDate = System.DateTime.Now;
           
                //MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CMS_MapService entity)
        {
            try
            {
               
                MyContext.CMS_MapService.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public bool DeleteByIDMap(int idMap)
        {
            try
            {
                return MyContext.CMS_MapService_DelByMapID(idMap) > 0;
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
                //var obj = MyContext.CMS_MapService.Find(id);
                //MyContext.CMS_MapService.Remove(obj);
                //MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_MapService GetMapByID(int id)
        {
            try
            {
                return MyContext.CMS_MapService.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public List<CMS_MapService_ByMapID_Result> ByMapID( int MapID)
        {
            try
            {
                List<CMS_MapService_ByMapID_Result> lData = MyContext.CMS_MapService_ByMapID(MapID).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }



    }
}
