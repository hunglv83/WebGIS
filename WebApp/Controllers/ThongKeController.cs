using WebApp.Business.Services;
using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ThongKeController : BaseController
    {
        //[HttpPost]
        //public ActionResult XemChiTiet(int hosoID)
        //{
        //    string maHoSo = hoSoService.SelectByHoSoId(hosoID).MaHoSo;
        //    var tailieu = new List<TQG_TaiLieuDinhKem>();
        //    tailieu = hoSoService.GetTaiLieuByMaHoSo(maHoSo);
        //    return PartialView("_TaiLieuDinhKem", tailieu);
        //}

        //[HttpPost]
        //public ActionResult XemChiTietDiemMo(string maMo)
        //{
        //    var lstTaiLieu = new List<TQG_TaiLieuDinhKem>();
        //    lstTaiLieu = hoSoService.SelectAllTaiLieuByMaMo(maMo);
        //    return PartialView("_TaiLieuDinhKemChiTiet", lstTaiLieu);
        //}

        //[HttpPost]
        //public ActionResult ViewPDF(string guiId)
        //{
        //    var fullPath = hoSoService.GetUrlByGuiId(guiId);

        //    string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"500px\" height=\"300px\">";
        //    embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
        //    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
        //    embed += "</object>";
        //    TempData["Embed"] = string.Format(embed, VirtualPathUtility.ToAbsolute(fullPath));

        //    return PartialView("_ViewPDF");
        //}

        //public FileResult ShowFile(string guiId)
        //{
        //    var fullPath = hoSoService.GetUrlByGuiId(guiId);
        //    string ReportURL = fullPath;
        //    string extension = Path.GetExtension(ReportURL).ToLower();
        //    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);

        //    if (extension == ".pdf")
        //        return File(FileBytes, "application/pdf");
        //    if (extension == ".jpg" || extension == ".jpeg")
        //        return File(FileBytes, "image/jpeg");
        //    if (extension == ".png")
        //        return File(FileBytes, "image/png");
        //    if (extension == ".gif")
        //        return File(FileBytes, "image/gif");
        //    if (extension == ".doc")
        //        return File(FileBytes, "application/msword");
        //    if (extension == ".docx")
        //        return File(FileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        //    if (extension == ".xls")
        //        return File(FileBytes, "application/vnd.ms-excel");
        //    if (extension == ".xlsx")
        //        return File(FileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //    if (extension == ".ppt")
        //        return File(FileBytes, "application/vnd.ms-powerpoint");
        //    if (extension == ".pptx")
        //        return File(FileBytes, "application/vnd.openxmlformats-officedocument.presentationml.presentation");
        //    if (extension == ".mp3")
        //        return File(FileBytes, "audio/mpeg");
        //    if (extension == ".js")
        //        return File(FileBytes, "text/javascript");
        //    if (extension == ".json")
        //        return File(FileBytes, "application/json");
        //    if (extension == ".doc")
        //        return File(FileBytes, "application/msword");

        //    return File(new byte[10], "application/pdf");
        //}

        //public FileResult Download(string mahoso, string file, string display)
        //{
        //    string urlUploadFile = System.Configuration.ConfigurationManager.AppSettings["UploadFileUrl"];
        //    return File(Path.Combine(urlUploadFile + mahoso, file), System.Net.Mime.MediaTypeNames.Application.Octet, display);
        //}

        //[HttpPost]
        //public ActionResult LoadDataDVHC(string LoaiHoSo, string QuanHuyen)
        //{
        //    var lstData = new List<TQG_HoSo>();
        //    if (QuanHuyen == "0")
        //    {
        //        lstData = hoSoService.GetHoSoByLoaiHS(LoaiHoSo);
        //    }
        //    else
        //    {
        //        lstData = new List<TQG_HoSo>();//hoSoService.GetHoSoByLoaiHSQuanHuyen(LoaiHoSo, QuanHuyen);
        //    }

        //    ThongKeHoSoReport rpt = new ThongKeHoSoReport(lstData);
        //    string strFileName = "Thống kê hồ sơ";
        //    var stream = new MemoryStream();
        //    rpt.CreateDocument();
        //    rpt.ExportToPdf(stream);
        //    var cd = new System.Net.Mime.ContentDisposition
        //    {
        //        FileName = strFileName,
        //        Inline = false,
        //    };
        //    Response.AppendHeader("Content-Disposition", cd.ToString());
        //    return File(stream.GetBuffer(), "application/pdf");
        //}
    }
}