namespace WebApp.Common
{
    public static class CommonConstants
    {
        public static string USER_SESSION = "USER_SESSION";
        public static int USER_SIGNIN = 12;

        public struct AttachmentType
        {
            public const string GiayPhep = "GiayPhep";
            public const string YeuCauCapPhatTuLieu = "YeuCauCapPhatTuLieu";
            public const string YeuCauCapPhatDiemDoDac = "YeuCauCapPhatDiemDoDac";
            public const string SoHoaMoc = "SoHoaMoc";
            public const string LuoiToaDo = "LuoiToaDo";
            public const string ThanhQuaTracDia = "ThanhQuaTracDia";
            public const string SoDoGhiChuDiem = "SoDoGhiChuDiem";
            public const string Thumbnail = "Thumbnail";
        }
    }

    public static class LoaiCapPhep
    {
        public const string CAPMOI = "CAPMOI";
        public const string GIAHAN = "GIAHAN";
        public const string CHUYENNHUONG = "CHUYENNHUONG";
        public const string MORONG = "MORONG";
        public const string UPDATE = "UPDATE";
    }

    public static class LoaiHoSo
    {
        public const string DIEMMO = "DIEMMO";
        public const string THAMDO = "HS_CPTHAMDO";
        public const string KHAITHAC = "HS_CPKHAITHAC";
        public const string TANTHU = "HS_CPTANTHU";
        public const string PDTRULUONG = "HS_PDTRULUONG";
        public const string THUHOIGPHDKS = "HS_THUHOIGPHDKS";
        public const string DONGCUAMO = "HS_DONGCUAMO";
        public const string THANHTRA = "HS_THANHTRA";
        public const string THUEDAT = "HS_THUEDAT";
        public const string DAUGIAQKT = "HS_DAUGIAQKT";
        public const string KVCAMHDKS = "HS_KVCAMHDKS";
        public const string KVDUTRUKSQG = "HS_KVDUTRUKSQG";
    }

    public static class AlertType
    {
        public static string Success = "success";
        public static string Warning = "warning";
        public static string Error = "error";
    }

    public static class LoaiDonViTinh
    {
        public static int DODAI = 1831;
        public static int THOIHAN = 2751;
        public static int DIENTICH = 6158;
        public static int TRULUONG = 7753;
        public static int TIENTE = 9457;
    }

    public static class AttachmentType
    {
        public static string GiayPhep = "GiayPhep";
        public static string YeuCauCapPhatTuLieu = "YeuCauCapPhatTuLieu";
        public static string YeuCauCapPhatBanDoSo = "YeuCauCapPhatBanDoSo";
        public static string SoHoaMoc = "SoHoaMoc";
        public static string LuoiToaDo = "LuoiToaDo";
        public static string ThanhQuaTracDia = "ThanhQuaTracDia";
        public static string Thumbnail = "Thumbnail";

        public static string PTiepNhan_Rep_CapPhat = "PTiepNhan_Rep_CapPhat";
        public static string PCapPhat_Rep_CapPhat = "PCapPhat_Rep_CapPhat";
        public static string PKeToan_Rep_CapPhat = "PKeToan_Rep_CapPhat";
        public static string PTraKQ_Rep_CapPhat = "PTraKQ_Rep_CapPhat";
    }
}