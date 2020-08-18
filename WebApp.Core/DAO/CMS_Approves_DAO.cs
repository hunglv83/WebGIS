using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Core.DAO
{
  public  class CMS_Approves_DAO
    {
         DT_WebGISEntities MyContext = null;
         public CMS_Approves_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

         public List<CMS_Approves_byIDNews_Result> GetByIDNew(int idNew)
         {
             try
             {
                 List<CMS_Approves_byIDNews_Result> lData = MyContext.CMS_Approves_byIDNews(idNew).ToList();
                 return lData;
             }
             catch (Exception ex)
             {
                 Logs.WriteLog(ex);
                 throw;
             }
         }

         public int Insert(CMS_Approves entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;

                MyContext.CMS_Approves.Add(entity);
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
