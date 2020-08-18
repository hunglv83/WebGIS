using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Core.EF;
using System.Data.Entity;
using WebApp.Common;

namespace WebApp.Core.DAO
{
    public class CSF_Functions_DAO
    {
        private DT_WebGISEntities MyContext = null;
        public CSF_Functions_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public IQueryable<CSF_Functions> GetAll()
        {
            var objects = from s in MyContext.CSF_Functions.OrderBy(m => m.ID) select s;
            return objects;
        }

        public List<CSF_Functions_LayTatCa_Result> Search(string strSearch)
        {
            try
            {

                List<CSF_Functions_LayTatCa_Result> objs = MyContext.CSF_Functions_LayTatCa().ToList();
                if (!String.IsNullOrEmpty(strSearch))
                {
                    objs = objs.Where(f => f.Name.ToLower().Contains(strSearch.ToLower())).OrderBy(m => m.ID).ToList();
                }
                return objs;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CSF_Functions entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                entity.ParentID = entity.ParentID != null ? entity.ParentID : 0;
                MyContext.CSF_Functions.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool Update(CSF_Functions entity)
        {
            try
            {
                var obj = MyContext.CSF_Functions.Find(entity.ID);
                obj.Name = entity.Name;
                obj.Description = entity.Description;
                obj.ModuleID = entity.ModuleID;
                obj.ParentID = entity.ParentID;
                obj.Controller_Action = entity.Controller_Action;
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
                var func = MyContext.CSF_Functions.Find(id);
                MyContext.CSF_Functions.Remove(func);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public CSF_Functions GetFunctionByID(int id)
        {
            try
            {
                return MyContext.CSF_Functions.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool CheckControllerAction(int id, string controllerAction)
        {
            try
            {
                if (id != 0)
                {
                    return MyContext.CSF_Functions.Count(x => (x.Controller_Action == controllerAction && x.ID != id )) > 0;
                }
                else
                    return MyContext.CSF_Functions.Count(x => x.Controller_Action == controllerAction) > 0;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool CheckParentFunction(int id)
        {
            try
            {
                return MyContext.CSF_Functions.Count(x => x.ParentID == id) > 0;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
    }
}
