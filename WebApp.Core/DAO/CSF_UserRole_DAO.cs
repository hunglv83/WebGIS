using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Core.EF;
using System.Data.Entity;
using WebApp.Common;

namespace WebApp.Core.DAO
{
    public class CSF_UserRole_DAO
    {
        private DT_WebGISEntities MyContext = null;
        public CSF_UserRole_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public int Insert(int UserID, int RoleID)
        {
            try
            {
                CSF_UserRole entity = new CSF_UserRole();
                entity.UserID = UserID;
                entity.RoleID = RoleID;
                MyContext.CSF_UserRole.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public List<CSF_UserRole_GetByUser_Result> GetListRoleByUserID(int UserID)
        {
            try
            {
                var role = MyContext.CSF_UserRole_GetByUser(UserID).ToList();
                return role;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool DelRoleByUserID(int UserID)
        {
            try
            {
                return MyContext.CSF_UserRole_DelByUser(UserID) > 0;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool RemoveUsersOutGroup(int RoleID, string listUserID)
        {
            try
            {
                int result = MyContext.CSF_UserRole_RemoveUser(RoleID, listUserID);
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
