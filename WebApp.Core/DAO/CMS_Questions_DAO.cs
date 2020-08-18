using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
    public class CMS_Questions_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Questions_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CMS_Questions_LayTatCa_Result> Search(string keyWord, int type, bool? publish)
        {
            try
            {
                List<CMS_Questions_LayTatCa_Result> lData = MyContext.CMS_Questions_LayTatCa(type, keyWord, publish).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CMS_Questions entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_Questions.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool Update(CMS_Questions entity)
        {
            try
            {
                CMS_Questions obj = MyContext.CMS_Questions.Find(entity.ID);
                obj.Title = entity.Title;
                obj.Contents = entity.Contents;
                obj.Answer = entity.Answer;
                obj.TypeOfQuestionID = entity.TypeOfQuestionID;
                obj.Publish = entity.Publish;
                obj.FileName = entity.FileName;
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
                var obj = MyContext.CMS_Questions.Find(id);
                MyContext.CMS_Questions.Remove(obj);
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
