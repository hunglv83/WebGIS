using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebApp.Common;
namespace WebApp.Core.DAO
{
  public  class CMS_Documents_DAO
    {
        DT_WebGISEntities MyContext = null;
        public CMS_Documents_DAO()
        {
            MyContext = new DT_WebGISEntities();
        }

        public List<CMS_Documents_LayTatCa_Result> Search(string keyWord, int type, int area, int org)
        {
            try
            {
                List<CMS_Documents_LayTatCa_Result> lData = MyContext.CMS_Documents_LayTatCa(type, keyWord, area, org).ToList();
                return lData;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public int Insert(CMS_Documents entity)
        {
            try
            {
                entity.CreateDate = System.DateTime.Now;
                MyContext.CMS_Documents.Add(entity);
                MyContext.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                throw;
            }
        }

        public bool CheckExist_DocumentNumber(CMS_Documents entity)
        {
            try
            {
                var obj = MyContext.CMS_Documents.Where(x => x.DocumentNumber.Trim().Contains(entity.DocumentNumber.Trim())).FirstOrDefault();
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
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        public bool Update(CMS_Documents entity)
        {
            try
            {
                CMS_Documents obj = MyContext.CMS_Documents.Find(entity.ID);
                obj.DocumentNumber = entity.DocumentNumber;
                obj.TypeOfDocumentID = entity.TypeOfDocumentID;
                obj.AreaOfDocument = entity.AreaOfDocument;
                obj.Abstract = entity.Abstract;
                obj.Contents = entity.Contents;
                obj.OrganizationID = entity.OrganizationID;
                obj.IssuedDate = entity.IssuedDate;
                obj.EffectiveDate = entity.EffectiveDate;
                obj.Signer = entity.Signer;
                obj.FileName = entity.FileName;
                obj.Publish = entity.Publish == null ? false : entity.Publish;
                obj.Effective = entity.Effective == null ? false : entity.Effective;
                obj.Description = entity.Description;
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
                var obj = MyContext.CMS_Documents.Find(id);
                MyContext.CMS_Documents.Remove(obj);
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
