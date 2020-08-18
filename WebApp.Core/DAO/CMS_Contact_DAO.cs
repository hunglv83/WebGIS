using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Core.EF;
using System.Data.Entity;
using WebApp.Common;

namespace WebApp.Core.DAO
{
    public class CMS_Contact_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Contact_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }


        public List<CMS_Contact_LayTatCa_Result> Search(string keyWord)
        {
            try
            {
                List<CMS_Contact_LayTatCa_Result> lData = MyContext.CMS_Contact_LayTatCa(keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
        public bool Update(CMS_Contact entity)
        {
            try
            {
                CMS_Contact obj = MyContext.CMS_Contact.Find(entity.ID);
                obj.Name = entity.Name;
                obj.Address = entity.Address;
                obj.Title = entity.Title;
                obj.Mobile = entity.Mobile;
                obj.Email = entity.Email;
                obj.Contents = entity.Contents;
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
        public int Insert(CMS_Contact entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_Contact.Add(entity);
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
                var obj = MyContext.CMS_Contact.Find(id);
                MyContext.CMS_Contact.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_Contact GetContactByID(int id)
        {
            try
            {
                return MyContext.CMS_Contact.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }
      
    }
}
