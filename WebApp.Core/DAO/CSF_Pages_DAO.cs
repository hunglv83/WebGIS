using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace WebApp.Core.DAO
{
    public class CSF_Pages_DAO
    {
        private DT_WebGISEntities MyContext = null;

        public CSF_Pages_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CSF_Pages_LayTatCa_Result> GetAll()
        {
            var lData = MyContext.CSF_Pages_LayTatCa().ToList();
            return lData;
        }

        public List<CSF_Pages_LayTatCa_Result> Search(string strSearch, string isadmin)
        {
            try
            {
                strSearch = strSearch != null ? strSearch : "";
                bool IsAdmin = false;
                if (isadmin != null && isadmin == "true")
                {
                    IsAdmin = true;
                }
                List<CSF_Pages_LayTatCa_Result> objs = MyContext.CSF_Pages_LayTatCa().ToList();
                objs = objs.Where(f => f.Name.ToLower().Contains(strSearch.ToLower()) && Convert.ToBoolean(f.IsAdmin) == IsAdmin).ToList();
                return objs;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CSF_Pages entity)
        {
            try
            {
                entity.CreateDate = DateTime.Now;
                entity.IsActive = entity.IsActive == null ? false : entity.IsActive;
                entity.IsAdmin = entity.IsAdmin == null ? false : entity.IsAdmin;
                entity.IsHost = entity.IsHost == null ? false : entity.IsHost;
                entity.IsBlank = entity.IsBlank == null ? false : entity.IsBlank;
                MyContext.CSF_Pages.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool CheckExist_PageKey(CSF_Pages entity)
        {
            try
            {
                if (entity.Key != "~")
                {
                    var obj = MyContext.CSF_Pages.Where(x => x.Key.Trim().ToLower() == entity.Key.Trim().ToLower()).FirstOrDefault();
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
                return false;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public bool Update(CSF_Pages entity)
        {
            try
            {
                CSF_Pages obj = MyContext.CSF_Pages.Find(entity.ID);
                obj.IsActive = entity.IsActive == null ? false : entity.IsActive;
                obj.IsAdmin = entity.IsAdmin == null ? false : entity.IsAdmin;
                obj.IsHost = entity.IsHost == null ? false : entity.IsHost;
                obj.IsBlank = entity.IsBlank == null ? false : entity.IsBlank;
                obj.Name = entity.Name;
                obj.Order = entity.Order;
                obj.Icon = entity.Icon;
                obj.ParentID = entity.ParentID == null ? 0 : entity.ParentID;
                obj.Key = entity.Key;
                obj.ModuleID = entity.ModuleID;
                MyContext.Entry(obj).State = EntityState.Modified;
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
                var obj = MyContext.CSF_Pages.Find(id);
                MyContext.CSF_Pages.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public CSF_Pages GetPageByID(int id)
        {
            try
            {
                return MyContext.CSF_Pages.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public List<CSF_Pages_GetPartial_Result> GetPartialByPageID(int id, Int16 active)
        {
            try
            {
                string keyPage = MyContext.CSF_Pages.Find(id).Key;
                if (keyPage != null)
                {
                    return MyContext.CSF_Pages_GetPartial(keyPage, active).ToList();
                }
                return new List<CSF_Pages_GetPartial_Result>();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int PagePartialSave(CSF_PagePartial entity)
        {
            try
            {
                if (entity.ID == 0)
                {
                    entity.CreateDate = DateTime.Now;
                    MyContext.CSF_PagePartial.Add(entity);
                }
                else
                {
                    MyContext.Entry(entity).State = EntityState.Modified;
                }
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
    }
}
