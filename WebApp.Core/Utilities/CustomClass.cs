using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Core.Utilities
{
    public class ThongTinYeuCau
    {
        public string TT { get; set; }
        public string LoaiThongTinDL { get; set; }
        public string KhuVuc { get; set; }
        public string DonVi { get; set; }
        public string SoLuong { get; set; }
        public string MucDichSD { get; set; }
    }

    public class QNI_DiemGoc
    {
        public string STT { get; set; }
        public string KhuVuc { get; set; }
        public string TenDiemGoc { get; set; }
        public string ToaDoXY { get; set; }
    }

    public class MetadataInfo
    {
        public List<MetadataVM> MetadataVM { get; set; }
        public List<List<DonViVM>> DonViLienQuanVM { get; set; }
        public List<List<DonViVM>> DonViXayDungVM { get; set; }
    }

    public class MetadataVM
    {
        public int OrderNumber { get; set; }
        public string AttributeCode { get; set; }
        public string Attribute { get; set; }
        public string ValueCode { get; set; }
        public string Value { get; set; }
    }

    public class DonViVM
    {
        public int OrderNumber { get; set; }
        public string AttributeCode { get; set; }
        public string Attribute { get; set; }
        public string ValueCode { get; set; }
        public string Value { get; set; }
    }

    public class TpHoSoBoSung
    {
        public string STT { get; set; }
        public string TpHoSoBoSungName { get; set; }
        public string TpHoSoBoSungFile { get; set; }
        public string TpHoSoBoSungValue { get; set; }
        public string TpHoSoBoSungFileName { get; set; }
    }

    public class PageVM
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageKey { get; set; }
        public int ParentID { get; set; }
        public List<PageVM> SubPage { get; set; }
    }

    public class DiemGoc
    {
        public string STT { get; set; }
        public string NameKhuVuc { get; set; }
        public string KhuVuc { get; set; }
        public string NameTenDiemGoc { get; set; }
        public string NameToaDoXY { get; set; }
        public string TenDiemGoc { get; set; }
        public string ToaDoXY { get; set; }
    }

    public class ToaDoKhepGoc
    {
        public string STT { get; set; }
        public string NameKhuVuc { get; set; }
        public string KhuVuc { get; set; }
        public string NameTenDiemGoc { get; set; }
        public string NameToaDoXY { get; set; }
        public string TenDiemGoc { get; set; }
        public string ToaDoXY { get; set; }
    }

    public class RanhGioiDiemMo
    {
        public string STT { get; set; }
        public string NameKhuVuc { get; set; }
        public string KhuVuc { get; set; }
        public string NameTenDiemGoc { get; set; }
        public string NameToaDoXY { get; set; }
        public string TenDiemGoc { get; set; }
        public string ToaDoXY { get; set; }
    }

    public class TruLuongDCCT
    {
        public string STT { get; set; }
        public string TenLoaiDC_Att { get; set; }
        public string TenLoaiDC_Val { get; set; }
        public string TruLuong_Att { get; set; }
        public string TruLuong_Val { get; set; }
        public string DonVi_Att { get; set; }
        public string DonVi_Val { get; set; }
    }

    public class TruLuongKTCT
    {
        public string STT { get; set; }
        public string TenLoaiKS_Att { get; set; }
        public string TenLoaiKS_Val { get; set; }
        public string CapTruLuong_Att { get; set; }
        public string CapTruLuong_Val { get; set; }
        public string TruLuong_Att { get; set; }
        public string TruLuong_Val { get; set; }
        public string DonVi_Att { get; set; }
        public string DonVi_Val { get; set; }
    }

    public class Lop
    {
        public string TenLop { get; set; }
        public int SoLuongSV { get; set; }
        public int XepHang { get; set; }
    }

    public class Chart
    {
        public string[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
    }

    public class ChartDecimal
    {
        public string[] labels { get; set; }
        public List<DatasetsDecimal> datasets { get; set; }
    }

    public class Datasets
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public string borderWidth { get; set; }
        public int[] data { get; set; }
    }

    public class DatasetsDecimal
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public string borderWidth { get; set; }
        public decimal[] data { get; set; }
    }
}
