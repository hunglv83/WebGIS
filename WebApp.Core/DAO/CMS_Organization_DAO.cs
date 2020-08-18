using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Core.EF;
using System.Data.Entity;
using WebApp.Common;

namespace WebApp.Core.DAO
{
    public class CMS_Organization_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Organization_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }


        public List<CMS_Organization_LayTatCa_Result> Search(string keyWord)
        {
            try
            {
                List<CMS_Organization_LayTatCa_Result> lData = MyContext.CMS_Organization_LayTatCa(keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public bool Update(CMS_Organization entity)
        {
            try
            {
                CMS_Organization obj = MyContext.CMS_Organization.Find(entity.ID);
                obj.Name = entity.Name;
                obj.ShortName = entity.ShortName;
                obj.ParentID = entity.ParentID;
                obj.Mobile = entity.Mobile;
                obj.Email = entity.Email;
                obj.Website = entity.Website;
                obj.Address = entity.Address;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CMS_Organization entity)
        {
            try
            {
              
                MyContext.CMS_Organization.Add(entity);
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
                var obj = MyContext.CMS_Organization.Find(id);
                MyContext.CMS_Organization.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_Organization GetOrganizationByID(int id)
        {
            try
            {
                return MyContext.CMS_Organization.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public bool CheckDelete(int id)
        {
            try
            {
                var obj = MyContext.CMS_Documents.Where(x => x.OrganizationID == id).FirstOrDefault();
                if (obj != null)
                {
                    return false;
                }
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
