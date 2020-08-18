using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public partial class TuLieuViewModel
    {
        public int STT { get; set; }
        public string Ten { get; set; }
        public string KyHieu { get; set; }
        public List<TuLieuLoaiModel> LoaiTuLieuList { get; set; }
        public List<TuLieuCapHangModel> CapHangList { get; set; }
    }
    public class TuLieuCapHangModel
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
    }
    public class TuLieuLoaiModel
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
    }
}