using WebApp.Business.Services;
using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TimKiemController : BaseController
    {
        private static DT_WebGISEntities db = new DT_WebGISEntities();
        public PartialViewResult Index()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public class DatatableParams
        {
            public int Draw { get; set; } = 1;
            public int Start { get; set; } = 1;
            public int Length { get; set; } = 5;
            public string Search { get; set; }
            public int SortColumn { get; set; } = -1;
            public string SortDirection { get; set; } = "asc";
        }

        public class DataItem
        {
            public string STT { get; set; }
            public string TTChung { get; set; }
            public string TTChiTiet { get; set; }
            public string ThaoTac { get; set; }
        }

        public class DataSearch
        {
            public string STT { get; set; }
            public string NoiDung { get; set; } = string.Empty;
            public string ToChucCaNhan { get; set; } = string.Empty;
            public string LoaiKhoangSan { get; set; } = string.Empty;
            public string LoaiHoSo { get; set; } = string.Empty;
            public string DiemMo { get; set; } = string.Empty;
            public string DVHC { get; set; } = string.Empty;
            public string ThaoTac { get; set; }
        }

        public class DataTableData
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            public List<DataItem> datanew { get; set; }
            public List<DataSearch> data { get; set; }
        }
    }
}