using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Core.EF;
using System.Data.Entity;
using WebApp.Common;

namespace WebApp.Core.DAO
{
    public class CSF_Roles_DAO
    {
        private DT_WebGISEntities MyContext = null;
        public CSF_Roles_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public IQueryable<CSF_Roles> Search(string strSearchString)
        {
            try
            {
                var roles = from s in MyContext.CSF_Roles
                            select s;
                if (!String.IsNullOrEmpty(strSearchString))
                {
                    roles = roles.Where(s => s.Name.Contains(strSearchString));
                }
                return roles;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public IQueryable<CSF_Roles> GetAll()
        {
            var roles = from s in MyContext.CSF_Roles select s;
            return roles;
        }

        public int Insert(CSF_Roles entity)
        {
            try
            {
                MyContext.CSF_Roles.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool Update(CSF_Roles entity)
        {
            try
            {
                var role = MyContext.CSF_Roles.Find(entity.ID);
                role.Name = entity.Name;
                role.Description = entity.Description;
                role.IsAdmin = entity.IsAdmin;
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
                var role = MyContext.CSF_Roles.Find(id);
                MyContext.CSF_Roles.Remove(role);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public CSF_Roles GetRoleByID(int id)
        {
            try
            {
                return MyContext.CSF_Roles.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
    }
}
