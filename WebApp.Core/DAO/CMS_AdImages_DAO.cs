using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Core.DAO
{
    public class CMS_AdImages_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_AdImages_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }





        public List<CMS_AdImages_LayTatCa_Result> Search(string keyWord)
        {
            try
            {
                List<CMS_AdImages_LayTatCa_Result> lData = MyContext.CMS_AdImages_LayTatCa(keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool Update(CMS_AdImages entity)
        {
            try
            {
                CMS_AdImages obj = MyContext.CMS_AdImages.Find(entity.ID);
                obj.Name = entity.Name;
                obj.Description = entity.Description;
                obj.Publish = entity.Publish;
                obj.Orders = entity.Orders;
                obj.FileName = entity.FileName;
                obj.Location = entity.Location;
                obj.Url = entity.Url;
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public int Insert(CMS_AdImages entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_AdImages.Add(entity);
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
                var obj = MyContext.CMS_AdImages.Find(id);
                MyContext.CMS_AdImages.Remove(obj);
                MyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }
        public CMS_AdImages GetAdImagesID(int id)
        {
            try
            {
                return MyContext.CMS_AdImages.Find(id);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }




    }
}
