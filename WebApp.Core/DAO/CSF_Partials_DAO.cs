using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Core.EF;
using System.Data.Entity;
using WebApp.Common;

namespace WebApp.Core.DAO
{
    public class CSF_Partials_DAO
    {
        private DT_WebGISEntities MyContext = null;
        public CSF_Partials_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CSF_Partials_LayTatCa_Result> GetAll()
        {
            var objects = MyContext.CSF_Partials_LayTatCa().ToList();
            return objects;
        }
        public List<CSF_Partials_LayTatCa_Result> Search(string strSearch)
        {
            try
            {
                List<CSF_Partials_LayTatCa_Result> objs = MyContext.CSF_Partials_LayTatCa().ToList();
                if (!String.IsNullOrEmpty(strSearch))
                {
                    objs = objs.Where(f => f.Name.ToLower().Contains(strSearch.ToLower())).ToList();
                }
                return objs;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public bool CheckExist_PartialsKey(CSF_Partials entity)
        {
            try
            {
                var obj = MyContext.CSF_Partials.Where(x => x.Key.Trim().ToLower() == entity.Key.Trim().ToLower()).FirstOrDefault();
                if (obj != null)
                {
                    if (entity.ID != obj.ID)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public bool Update(CSF_Partials entity)
        {
            try
            {
                CSF_Partials obj = MyContext.CSF_Partials.Find(entity.ID);
                obj.Name = entity.Name;
                obj.Key = entity.Key;
                obj.ModuleID = entity.ModuleID;
                obj.Controller = entity.Controller;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CSF_Partials entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CSF_Partials.Add(entity);
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
                var obj = MyContext.CSF_Partials.Find(id);
                MyContext.CSF_Partials.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CSF_Partials GetPartialsByID(int id)
        {
            try
            {
                return MyContext.CSF_Partials.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
    }
}
