using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace WebApp.Common
{
    public static class QNIExtensions
    {
        public static System.Web.Mvc.SelectList ToSelectList<TEnum>(this TEnum obj)
            where TEnum : struct, IComparable, IFormattable, IConvertible // correct one
        {
            return new SelectList(Enum.GetValues(typeof(TEnum)).OfType<Enum>()
                .Select(x =>
                    new SelectListItem
                    {
                        Text = x.GetType().GetField(x.ToString()).GetCustomAttribute<DescriptionAttribute>().Description,
                        Value = (Convert.ToInt32(x)).ToString()
                    }), "Value", "Text");
        }
    }

    public enum LoaiHoSoEnum
    {
        [Description("Hồ sơ thăm dò")] THAMDO = 1,
        [Description("Hồ sơ khai thác")] KHAITHAC = 2,
        [Description("Hồ sơ tận thu")] TANTHU = 3,
        [Description("Hồ sơ phê duyệt trữ lượng")] PDTRULUONG = 4,
        [Description("Hồ sơ thu hồi giấy phép hoạt động khoáng sản")] THUHOIGPHDKS = 5,
        [Description("Hồ sơ đóng cửa mỏ")] DONGCUAMO = 6,
        [Description("Hồ sơ thanh tra")] THANHTRA = 7,
        [Description("Hồ sơ đấu giá quyền khai thác")] DAUGIAQKT = 8,
        [Description("Khu vực cấm hoạt động khoáng sản")] KVCAMHDKS = 9,
        [Description("Khu vực dự trữ khoáng sản quốc gia")] KVDUTRUKSQG = 10
    }

    public enum CapTruLuongEnum
    {
        [Description("111")] CAP111 = 1,
        [Description("112")] CAP112 = 2,
        [Description("121")] CAP121 = 3,
        [Description("122")] CAP122 = 4,
        [Description("212")] CAP212 = 5,
        [Description("222")] CAP222 = 6,
        [Description("321")] CAP321 = 7,
        [Description("322")] CAP322 = 8,
        [Description("333")] CAP333 = 9,
    }

    public enum TrangThaiHieuLucEnum
    {
        [Description("Có hiệu lực")]
        COHIEULUC = 1,

        [Description("Hết hiệu lực")]
        HETHIEULUC = 2
    }

    public enum LoaiGiayPhepEnum
    {
        [Description("Thăm dò")]
        THAMDO = 1,

        [Description("Khai thác")]
        KHAITHAC = 2
    }
}