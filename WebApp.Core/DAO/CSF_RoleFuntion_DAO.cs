using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Core.EF;
using System.Data.Entity;
using WebApp.Common;

namespace WebApp.Core.DAO
{
    public class CSF_RoleFunction_DAO
    {
        private DT_WebGISEntities MyContext = null;
        public CSF_RoleFunction_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public IQueryable<CSF_RoleFunction> GetAll()
        {
            var objects = from s in MyContext.CSF_RoleFunction select s;
            return objects;
        }

        public List<CSF_RoleFunction_LayTatCa_Result> Search(string strSearch)
        {
            try
            {

                List<CSF_RoleFunction_LayTatCa_Result> objs = MyContext.CSF_RoleFunction_LayTatCa().ToList();
                if (!String.IsNullOrEmpty(strSearch))
                {
                    objs = objs.Where(f => f.TenChucNang.ToLower().Contains(strSearch.ToLower())).ToList();
                }
                return objs;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CSF_RoleFunction entity)
        {
            try
            {
                MyContext.CSF_RoleFunction.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int AddPermission(int RoleID, int ModuleID, string FunctionIDs)
        {
            try
            {
                string sql = "";
                if (FunctionIDs != null && FunctionIDs != "")
                {
                    string[] FuncIDs = FunctionIDs.Split(',');
                    foreach (var item in FuncIDs)
                    {
                        sql += "(" + RoleID + "," + item + "),";
                    }
                    sql = sql.Substring(0, sql.Length - 1);
                }
                int result = MyContext.CSF_Role_Function_Add(RoleID, ModuleID, sql);
                return result;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool CheckExist_RF(CSF_RoleFunction entity)
        {
            try
            {
                var rf = MyContext.CSF_RoleFunction.Where(x => x.RoleID == entity.RoleID && x.FunctionID == entity.FunctionID).First();
                if (rf != null)
                {
                    if (entity.ID != rf.ID)
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

        public bool Update(CSF_RoleFunction entity)
        {
            try
            {
                MyContext.Entry(entity).State = EntityState.Modified;
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
                var func = MyContext.CSF_RoleFunction.Find(id);
                MyContext.CSF_RoleFunction.Remove(func);
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
