using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Common
{
    public class Constants
    {
        public const int PAGESIZE = 10;
        public const string HS_CHUTRUONG_HDKS = "HS_CHUTRUONG_HDKS";
        public const string HS_DONGCUA = "HS_DONGCUA";
        public const string HS_KHAITHAC = "HS_KHAITHAC";
        public const string HS_QUYHOACH = "HS_QUYHOACH";
        public const string HS_THAMDO_KS = "HS_THAMDO_KS";
        public const string HS_THANHTRA = "HS_THANHTRA";
        public const string HS_TINHTIEN_CQKTKS = "HS_TINHTIEN_CQKTKS";
        public const string HS_KHAC = "HS_KHAC";
    }

    public enum TanSuatEnum
    {
        NAM, SAUTHANGDAU, SAUTHANGCUOI, QUY1, QUY2, QUY3, QUY4
    }

    public class MetaConst
    {
        public const string DINHDANH_ATTRIBUTE = "DinhDanh.Attribute.";
        public const string DINHDANH_VALUE = "DinhDanh.Value.";
        public const string KHONGGIAN_ATTRIBUTE = "KhongGian.Attribute.";
        public const string KHONGGIAN_VALUE = "KhongGian.Value.";
        public const string PHAMVI_ATTRIBUTE = "PhamVi.Attribute.";
        public const string PHAMVI_VALUE = "PhamVi.Value.";
        public const string HEQUYCHIEU_ATTRIBUTE = "HeQuyChieu.Attribute.";
        public const string HEQUYCHIEU_VALUE = "HeQuyChieu.Value.";
        public const string CHATLUONGDL_ATTRIBUTE = "ChatLuongDL.Attribute.";
        public const string CHATLUONGDL_VALUE = "ChatLuongDL.Value.";
        public const string METADATA_ATTRIBUTE = "Metadata.Attribute.";
        public const string METADATA_VALUE = "Metadata.Value.";
        public const string DONVILIENQUAN_ATTRIBUTE = "DonViLienQuan.Attribute.";
        public const string DONVILIENQUAN_VALUE = "DonViLienQuan.Value.";
        public const string DONVIXAYDUNG_ATTRIBUTE = "DonViXayDung.Attribute.";
        public const string DONVIXAYDUNG_VALUE = "DonViXayDung.Value.";


        public const string DONVILIENQUAN_TENNGUOIDAIDIEN = "DonViLienQuan.Attribute.TenNguoiDaiDien";
        public const string DONVIXAYDUNG_TENNGUOIDAIDIEN = "DonViXayDung.Attribute.TenNguoiDaiDien";
    }
}