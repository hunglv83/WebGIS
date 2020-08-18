using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Core.DAO
{
    public class CMS_Notifications_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Notifications_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }



        public List<CMS_Notifications_LayTatCa_Result> GetAll()
        {
            var objects = MyContext.CMS_Notifications_LayTatCa().ToList();
            return objects;
        }

        public List<CMS_Notifications_LayTatCa_Result> Search(string strSearch)
        {
            try
            {
                List<CMS_Notifications_LayTatCa_Result> objs = MyContext.CMS_Notifications_LayTatCa().ToList();
                if (!String.IsNullOrEmpty(strSearch))
                {
                    objs = objs.Where(f => f.Title.ToLower().Contains(strSearch.ToLower())).ToList();
                }
                return objs;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool Update(CMS_Notifications entity)
        {
            try
            {
                CMS_Notifications obj = MyContext.CMS_Notifications.Find(entity.ID);
                obj.Title = entity.Title;
                obj.Contents = entity.Contents;
                obj.Publish = entity.Publish;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CMS_Notifications entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_Notifications.Add(entity);
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
                var obj = MyContext.CMS_Notifications.Find(id);
                MyContext.CMS_Notifications.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_Notifications GetNotificationsByID(int id)
        {
            try
            {
                return MyContext.CMS_Notifications.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }




    }
}
