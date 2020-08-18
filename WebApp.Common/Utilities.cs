using System;
using System.Collections.Generic;

namespace WebApp.Common
{
    public static class Utilities
    {
        public static List<string> arrColors = new List<string>(new string[] { "#FA8072", "#E9967A", "#F08080", "#CD5C5C", "#DC143C", "#B22222", "#FF0000", "#8B0000", "#FF7F50", "#FF6347", "#FF4500", "#FFD700", "#FFA500", "#FF8C00", "#FFFFE0", "#FFFACD", "#FAFAD2", "#FFEFD5", "#FFE4B5", "#FFDAB9", "#EEE8AA", "#F0E68C", "#BDB76B", "#FFFF00", "#7CFC00", "#7FFF00", "#32CD32", "#00FF00", "#228B22", "#008000", "#006400", "#ADFF2F", "#9ACD32", "#00FF7F", "#00FA9A", "#90EE90", "#98FB98", "#8FBC8F", "#3CB371", "#2E8B57", "#808000", "#556B2F", "#6B8E23", "#E0FFFF", "#00FFFF", "#00FFFF", "#7FFFD4", "#66CDAA", "#AFEEEE", "#40E0D0", "#48D1CC", "#00CED1", "#20B2AA", "#5F9EA0", "#008B8B", "#008080", "#B0E0E6", "#ADD8E6", "#87CEFA", "#87CEEB", "#00BFFF", "#B0C4DE", "#1E90FF", "#6495ED", "#4682B4", "#4169E1", "#0000FF", "#0000CD", "#00008B", "#000080", "#191970", "#7B68EE", "#6A5ACD", "#483D8B", "#E6E6FA", "#D8BFD8", "#DDA0DD", "#EE82EE", "#DA70D6", "#FF00FF", "#FF00FF", "#BA55D3", "#9370DB", "#8A2BE2", "#9400D3", "#9932CC", "#8B008B", "#800080", "#4B0082", "#FFC0CB", "#FFB6C1", "#FF69B4", "#FF1493", "#DB7093", "#C71585", "#FFFFFF", "#FFFAFA", "#F0FFF0", "#F5FFFA", "#F0FFFF", "#F0F8FF", "#F8F8FF", "#F5F5F5", "#FFF5EE", "#F5F5DC", "#FDF5E6", "#FFFAF0", "#FFFFF0", "#FAEBD7", "#FAF0E6", "#FFF0F5", "#FFE4E1", "#DCDCDC", "#D3D3D3", "#C0C0C0", "#A9A9A9", "#808080", "#696969", "#778899", "#708090", "#2F4F4F", "#000000", "#FFF8DC", "#FFEBCD", "#FFE4C4", "#FFDEAD", "#F5DEB3", "#DEB887", "#D2B48C", "#BC8F8F", "#F4A460", "#DAA520", "#CD853F", "#D2691E", "#8B4513", "#A0522D", "#A52A2A", "#800000" });

        public static string sqlDateTimeToString(DateTime myDateTime)
        {
            string sqlFormattedDate = myDateTime.Date.ToString("dd/MM/yyyy");
            return sqlFormattedDate;
        }

        public static string[,] sArray = new string[14, 18];

        //Khởi tạo mảng
        public static void InitArrayToneMarks()
        {
            byte i, j;
            string sString = "aAeEoOuUiIdDyY";
            string LC_a = "áàạảãâấầậẩẫăắằặẳẵ";
            string UC_A = "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ";
            string LC_e = "éèẹẻẽêếềệểễeeeeee";
            string UC_E = "ÉÈẸẺẼÊẾỀỆỂỄEEEEEE";
            string LC_o = "óòọỏõôốồộổỗơớờợởỡ";
            string UC_O = "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ";
            string LC_u = "úùụủũưứừựửữuuuuuu";
            string UC_U = "ÚÙỤỦŨƯỨỪỰỬỮUUUUUU";
            string LC_i = "íìịỉĩiiiiiiiiiiii";
            string UC_I = "ÍÌỊỈĨIIIIIIIIIIII";
            string LC_d = "đdddddddddddddddd";
            string UC_D = "ĐDDDDDDDDDDDDDDDD";
            string LC_y = "ýỳỵỷỹyyyyyyyyyyyy";
            string UC_Y = "ÝỲỴỶỸYYYYYYYYYYYY";
            for (i = 0; i < 14; i++)
                sArray[i, 0] = sString.Substring(i, 1);
            for (j = 1; j < 18; j++)
                for (i = 1; i < 18; i++)
                {
                    sArray[0, i] = LC_a.Substring(i - 1, 1);
                    sArray[1, i] = UC_A.Substring(i - 1, 1);
                    sArray[2, i] = LC_e.Substring(i - 1, 1);
                    sArray[3, i] = UC_E.Substring(i - 1, 1);
                    sArray[4, i] = LC_o.Substring(i - 1, 1);
                    sArray[5, i] = UC_O.Substring(i - 1, 1);
                    sArray[6, i] = LC_u.Substring(i - 1, 1);
                    sArray[7, i] = UC_U.Substring(i - 1, 1);
                    sArray[8, i] = LC_i.Substring(i - 1, 1);
                    sArray[9, i] = UC_I.Substring(i - 1, 1);
                    sArray[10, i] = LC_d.Substring(i - 1, 1);
                    sArray[11, i] = UC_D.Substring(i - 1, 1);
                    sArray[12, i] = LC_y.Substring(i - 1, 1);
                    sArray[13, i] = UC_Y.Substring(i - 1, 1);
                }
        }

        //Hàm loại bỏ dấu
        public static string RemoveToneMarks(string sSource)
        {
            InitArrayToneMarks();
            if (sSource == "") return "";
            byte i, j;
            for (j = 0; j < 14; j++)
            {
                for (i = 1; i < 18; i++)
                {
                    sSource = sSource.Replace(sArray[j, i], sArray[j, 0]);
                }
            }
            return sSource;
        }

        //Ham loai bo dau va ky tu dac biet
        public static string RemoveUrlMarks(string strUrlName)
        {
            try
            {
                string strOldUrl = strUrlName;
                strOldUrl = RemoveToneMarks(strOldUrl);
                strOldUrl = strOldUrl.Replace(" ", "-").Replace("\"", "").Replace(":", "").Replace("<", "-").Replace(">", "-").Replace(";", "-").Replace(",", "-").
                        Replace(".", "-").Replace("/", "-").Replace("'", "").Replace("&", "-").Replace("%", "-").Replace("$", "-").Replace("|", "-").Replace("\\", "-").
                        Replace("#", "-").Replace("@", "-").Replace("!", "-").Replace("`", "-").Replace("~", "-").Replace("+", "-").Replace("*", "-").Replace("?", "-").Replace("--", "-");
                if (strOldUrl.Trim().Length > 100)
                {
                    return strOldUrl.Substring(0, 100).ToLower();
                }
                return strOldUrl.ToLower();
            }
            catch
            {
                return strUrlName;
            }
        }

        public static string SiteURL()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString();
        }

        public static string SiteURL_Resources()
        {
            return System.Configuration.ConfigurationManager.AppSettings["UrlImage"].ToString();
        }

        public static string ReturnSubString(int iSize, string strName)
        {
            string strReturn = strName;
            int intIndex = 0;
            if (strName.Trim().Length > iSize)
            {
                intIndex = strName.IndexOf(' ', iSize);
                if (intIndex > 0)
                    strReturn = strName.Substring(0, intIndex) + "...";
            }
            return strReturn;
        }

        public static string UrlContent(string strCategoryKey, string strNewsTitle, string strPageType, string strNewsID)
        {
            if (strNewsTitle.Trim().Length > 0)
            {
                strNewsTitle = Utilities.ReturnSubString(100, strNewsTitle);
                return SiteURL() + "/" + strCategoryKey + "/" + RemoveUrlMarks(strNewsTitle) + "/" + strPageType + "-" + strNewsID;
            }
            else
                return SiteURL() + "/" + strCategoryKey;
        }
    }
}