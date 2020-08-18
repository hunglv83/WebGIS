using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
  public  class CMS_TypeOfQuestion_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_TypeOfQuestion_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CMS_TypeOfQuestion_LayTatCa_Result> Search(string keyWord)
        {
            try
            {
                List<CMS_TypeOfQuestion_LayTatCa_Result> lData = MyContext.CMS_TypeOfQuestion_LayTatCa(keyWord).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CMS_TypeOfQuestion entity)
        {
            try
            {
                MyContext.CMS_TypeOfQuestion.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }


        public bool Update(CMS_TypeOfQuestion entity)
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
        public bool CheckDelete(int id)
        {
            try
            {
                var obj = MyContext.CMS_Questions.Where(x => x.TypeOfQuestionID == id).FirstOrDefault();
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
        public bool Delete(int id)
        {
            try
            {
                var obj = MyContext.CMS_TypeOfQuestion.Find(id);
                MyContext.CMS_TypeOfQuestion.Remove(obj);
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
