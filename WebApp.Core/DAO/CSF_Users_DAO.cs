using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using WebApp.Core.EF;
using System.Data.Entity;
using WebApp.Common;

namespace WebApp.Core.DAO
{
    public class CSF_Users_DAO
    {
        private DT_WebGISEntities MyContext = null;
        public CSF_Users_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public IQueryable<CSF_Users> Search(string strSearchString)
        {
            try
            {
                var users = from s in MyContext.CSF_Users
                            where s.UserName.ToLower().Trim() != "host"
                            select s;
                if (!String.IsNullOrEmpty(strSearchString))
                {
                    users = users.Where(s => s.UserName.Contains(strSearchString) || s.FullName.Contains(strSearchString));
                }

                return users;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CSF_Users entity)
        {
            try
            {
                entity.RegisterDate = System.DateTime.Now;
                entity.Password = Encryptor.MD5Hash(entity.Password);
                entity.Status = 1;
                MyContext.CSF_Users.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool Update(CSF_Users entity)
        {
            try
            {
                var user = MyContext.CSF_Users.Find(entity.ID);
                user.FullName = entity.FullName;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = Encryptor.MD5Hash(entity.Password);
                }
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.Mobile = entity.Mobile;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public CSF_Users GetByUserName(string userName)
        {
            try
            {
                return MyContext.CSF_Users.SingleOrDefault(x => x.UserName == userName);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public int GetUserIDByUserName(string userName)
        {
            try
            {
                CSF_Users user = MyContext.CSF_Users.SingleOrDefault(x => x.UserName == userName);
                if (user != null)
                {
                    return user.ID;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public List<int> GetRoleIDByUserName(string userName,int intIDGuestGroup)
        {
            try
            {
                CSF_Users user = MyContext.CSF_Users.SingleOrDefault(x => x.UserName == userName);
                List<int> lRoleID = (from a in MyContext.CSF_Users
                                     join b in MyContext.CSF_UserRole on a.ID equals b.UserID
                                     where a.UserName == userName
                                     select b).Select(x => x.RoleID).ToList();
                if (lRoleID != null && lRoleID.Count() > 0)
                {
                    return lRoleID;
                }
                return new List<int> { intIDGuestGroup };//id nhom guest
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }


        public CSF_Users GetUserByID(int id)
        {
            try
            {
                return MyContext.CSF_Users.Where(x => x.UserName != "host" && x.ID == id).SingleOrDefault();
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
                var user = MyContext.CSF_Users.Where(x => x.UserName != "host" && x.ID == id).SingleOrDefault();
                MyContext.CSF_Users.Remove(user);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public bool CheckUserName(string userName)
        {
            try
            {
                return MyContext.CSF_Users.Count(x => x.UserName == userName) > 0;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool CheckEmail(string email)
        {
            try
            {
                return MyContext.CSF_Users.Count(x => x.Email == email) > 0;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Login(string userName, string passWord)
        {
            try
            {
                var result = MyContext.CSF_Users.Where(x => x.UserName == userName).FirstOrDefault();
                if (result == null)
                {
                    return 0;//Tên đăng nhập hoặc mật khẩu không đúng
                }
                else
                {
                    if (result.Status == 0)
                    {
                        return -1;//Chưa click hoạt
                    }
                    else
                    {
                        if (result.Status == 1)//Đã click hoạt
                        {
                            if (result.Password == passWord) return 1;//Đăng nhập thành công
                            else return -2;//Mật khẩu không đúng
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public List<CSF_Users_GetUsersNotInGroup_Result> GetUsersNotInGroup(string strFullName, int RoleID)
        {
            try
            {
                var users = MyContext.CSF_Users_GetUsersNotInGroup(RoleID).ToList();
                if (strFullName != null && strFullName != "")
                {
                    users = users.Where(u => u.FullName.ToLower().Contains(strFullName.ToLower())).ToList();
                }
                return users;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public List<CSF_Users_GetUsersInGroup_Result> GetUsersInGroup(int RoleID)
        {
            try
            {
                var users = MyContext.CSF_Users_GetUsersInGroup(RoleID).ToList();
                //if (strFullName != null && strFullName != "")
                //{
                //    users = users.Where(u => u.FullName.ToLower().Contains(strFullName.ToLower())).ToList();
                //}
                return users;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
    }
}
