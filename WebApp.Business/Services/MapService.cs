using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Core.EF;

namespace WebApp.Business.Services
{
    public class MapService
    {
        private DT_WebGISEntities db = new DT_WebGISEntities();

        public List<CMS_Maps> SelectAll()
        {
            var objs = db.CMS_Maps.ToList();
            return objs;
        }

        public List<CMS_Maps> SelectAll(string strSearch)
        {
            var objs = db.CMS_Maps.ToList();
            return objs;
        }

        public List<CMS_Maps> Top10Newer()
        {
            var objs = db.CMS_Maps.OrderByDescending(m => m.ID).Take(10).ToList();
            return objs;
        }

        public CMS_Maps SelectByID(int id)
        {
            var obj = db.CMS_Maps.SingleOrDefault(k => k.ID == id);
            return obj;
        }

        public List<CMS_Maps> GetByMapTypeId(int mapTypeId)
        {
            var result = new List<CMS_Maps>();
            var maps = db.CMS_Maps.Where(m => m.TypeOfMapID == mapTypeId).ToList();

            if (maps != null && maps.Count > 0)
            {
                result = maps;
            }

            return result;
        }

        public List<CMS_TypeOfMap> GetAllMapTypes()
        {
            var result = new List<CMS_TypeOfMap>();
            result = db.CMS_TypeOfMap.ToList();
            return result;
        }

        public List<CMS_Maps> GetDataByMapType(int mapTypeId)
        {
            var result = new List<CMS_Maps>();
            var maps = db.CMS_Maps.Where(m => m.TypeOfMapID == mapTypeId).ToList();

            if (maps != null && maps.Count > 0)
            {
                result = maps;
            }

            return result;
        }

        public List<CMS_Maps> GetTop10MapDesc()
        {
            var result = new List<CMS_Maps>();
            var obj = db.CMS_Maps.Take(10).ToList();
            if (obj != null)
                return obj;

            return result;
        }
    }
}