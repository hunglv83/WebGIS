using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Core.DAO
{
    public class CMS_Schedules_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Schedules_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }



        public List<CMS_Schedules_LayTatCa_Result> GetAll()
        {
            var objects = MyContext.CMS_Schedules_LayTatCa().ToList();
            return objects;
        }

        public List<CMS_Schedules_LayTatCa_Result> Search(string strSearch)
        {
            try
            {
                List<CMS_Schedules_LayTatCa_Result> objs = MyContext.CMS_Schedules_LayTatCa().ToList();
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

        public bool Update(CMS_Schedules entity)
        {
            try
            {
                CMS_Schedules obj = MyContext.CMS_Schedules.Find(entity.ID);
                obj.Title = entity.Title;
                obj.Contents = entity.Contents;
                obj.StartDate = entity.StartDate;
                obj.EndDate = entity.EndDate;
                obj.Place = entity.Place;
                obj.Participants = entity.Participants;
                obj.UserPrepare = entity.UserPrepare;
                obj.Ministry_leaders = entity.Ministry_leaders;
                obj.Leaders = entity.Leaders;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CMS_Schedules entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_Schedules.Add(entity);
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
                var obj = MyContext.CMS_Schedules.Find(id);
                MyContext.CMS_Schedules.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_Schedules GetSchedulesByID(int id)
        {
            try
            {
                var obj = MyContext.CMS_Schedules.Find(id);
               
                return MyContext.CMS_Schedules.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }




    }
}
