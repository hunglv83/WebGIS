using WebApp.Common;
using WebApp.Core.DAO;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApp.App_Start;
using Ciren.COpenXML;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Text.RegularExpressions;

namespace WebApp.Areas.Admin.Controllers.ThongKe
{
    public class CMS_ThongKeController : BaseController
    {
        //
        // GET: /Admin/CMS_ThongKe/
        #region VANBAN
        [CheckPermission]
        public ActionResult ThongKeVanBan(string keyWord, string type, string area, string org, int? page)
        {
            try
            {
                DT_WebGISEntities MVCContext = new DT_WebGISEntities();
                TempData["TypeOfDocumentID"] = MVCContext.CMS_TypeOfDocument.ToList();
                TempData.Keep("TypeOfDocumentID");
                TempData["AreaOfDocument"] = MVCContext.CMS_AreaOfDocument.ToList();
                TempData.Keep("AreaOfDocument");
                TempData["OrganizationID"] = MVCContext.CMS_Organization.ToList();
                TempData.Keep("OrganizationID");
                int typeID = Convert.ToInt32(type);
                int areaID = Convert.ToInt32(area);
                int orgID = Convert.ToInt32(org);
                keyWord = keyWord != null ? keyWord : "";
                CMS_Documents_DAO objDAO = new CMS_Documents_DAO();
                var data = objDAO.Search(keyWord, typeID, areaID, orgID).Where(x => x.Publish == true);
                ViewBag.SearchString = keyWord;
                ViewBag.TYPE = type;
                ViewBag.AREA = area;
                ViewBag.ORG = org;
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(data.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return View();
            }
        }

        [HttpPost]
        public ActionResult export_excel(FormCollection fc)
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                CMS_Documents_DAO objDAO = new CMS_Documents_DAO();
                int typeID = fc["type_excel"].ToString() != "" ? Convert.ToInt32(fc["type_excel"].ToString()) : 0;
                int areaID = fc["area_excel"].ToString() != "" ? Convert.ToInt32(fc["area_excel"].ToString()) : 0;
                int orgID = fc["org_excel"].ToString() != "" ? Convert.ToInt32(fc["org_excel"].ToString()) : 0;
                string keyWord = fc["key_excel"].ToString();
                var data = objDAO.Search(keyWord, typeID, areaID, orgID).Where(x => x.Publish == true).ToList();
                MemoryStream stream = new MemoryStream();

                using (SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    CWorkbook workbook = new CWorkbook(spreadsheetDoc);
                    string sheetName = "ThongKe";

                    CSheet sheetLH = createSheet(spreadsheetDoc, workbook, sheetName, data);

                    sheetLH._worksheetPart.Worksheet.Save();
                }
                string fileName = "ThongKeVanBan.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Clear();
                Response.ContentType = contentType;
                string contentDisposition = "attachment; filename*=UTF-8''" + Uri.EscapeDataString(fileName);
                Response.AddHeader("content-disposition", contentDisposition);
                Response.AddHeader("Content-Length", stream.Length.ToString());
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                Response.SuppressContent = true;
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                return View();
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return View();
            }
        }

        private CSheet createSheet(SpreadsheetDocument spreadsheetDoc, CWorkbook workbook, string sheetName, List<CMS_Documents_LayTatCa_Result> data)
        {
            CSheet sheet = new CSheet(spreadsheetDoc, sheetName, workbook._sheets);
            List<uint> styleIndex = new List<uint>();
            CStyle cStyle = new CStyle();
            cStyle.fontBold = true;
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None);
            cStyle.fontName = "Times New Roman";
            cStyle.fontSize = 12;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));

            uint rowIndex = 1;
            uint colindex = 1;

            #region HEADER
            string header = "BỘ TÀI NGUYÊN VÀ MÔI TRƯỜNG";
            uint rowHeader = rowIndex + 1;
            uint colHeader = colindex + 1;
            int textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            Cell cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 1) + (rowHeader).ToString());

            cStyle = new CStyle();
            cStyle.fontBold = true;
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None);
            cStyle.fontName = "Times New Roman";
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));
            header = "Cục công nghệ thông tin và DLTNMT";
            rowHeader = rowIndex + 2;
            colHeader = colindex + 1;
            textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 1) + (rowHeader).ToString());

            cStyle.fontSize = 17;
            header = "Thống kê văn bản";
            rowHeader = rowIndex + 4;
            colHeader = colindex + 1;
            textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 6) + (rowHeader).ToString());

            //cStyle = new CStyle();
            //cStyle.fontBold = true;
            //cStyle.fontItalic = true;
            //cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            //cStyle.SetBorderStyle(BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None);
            //cStyle.fontName = "Times New Roman";
            //cStyle.fontSize = 13;
            //cStyle.CreateStyle();
            //styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));
            //header = "(Từ ngày " + listLH[0].ngay + " đến ngày " + listLH[listLH.Count - 1].ngay + ")";
            //rowHeader = rowIndex + 5;
            //colHeader = colindex + 1;
            //textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            //cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            //cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            //ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 6) + (rowHeader).ToString());
            #endregion



            #region headerTable
            rowIndex = rowIndex + 6;
            cStyle = new CStyle();
            cStyle.fontBold = true;
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
            cStyle.fontName = "Times New Roman";
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));

            string title = "STT";
            uint rowindexHeader = rowIndex + 1;
            uint colindexHeader = colindex + 1;
            int textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellSTT = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellSTT.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Số hiệu";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 2;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellLanhDaoVu = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellLanhDaoVu.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Trích yếu";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 3;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellChuyenVien = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellChuyenVien.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Loại văn bản";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 4;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellLanhDaoBo = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellLanhDaoBo.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Lĩnh vực";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 5;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellGio = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellGio.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Ngày ban hành";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 6;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellNoiDung = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellNoiDung.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Ngày hiệu lực";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 7;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellDiaDiem = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellDiaDiem.StyleIndex = styleIndex[styleIndex.Count - 1];

            #endregion
            rowIndex++;
            cStyle = new CStyle();
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
            cStyle.fontName = "Times New Roman";
            cStyle.fontBold = false;
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));

            cStyle = new CStyle();
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Left;
            cStyle.SetBorderStyle(BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
            cStyle.fontName = "Times New Roman";
            cStyle.fontBold = false;
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));
            uint index = 0;
            foreach (var item in data)
            {

                string titleBody = (index + 1).ToString();
                uint rowindexBody = rowIndex + index + 1;
                uint colIndexBody = colindex + 1;
                int textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellSTT = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellSTT.StyleIndex = styleIndex[styleIndex.Count - 2];
                //ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colIndexBody) + rowindexBody.ToString(), ExcelUtils.GetColumnNameByIndex(colIndexBody) + (rowindexBody + item.data.Count() - 1).ToString());

                titleBody = item.DocumentNumber;
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 2;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellLanhDaoVu = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellLanhDaoVu.StyleIndex = styleIndex[styleIndex.Count - 1];

                titleBody = item.Abstract;
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 3;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellChuyenVien = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellChuyenVien.StyleIndex = styleIndex[styleIndex.Count - 1];

                titleBody = item.LoaiVanBan;
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 4;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellLanhDaoBo = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellLanhDaoBo.StyleIndex = styleIndex[styleIndex.Count - 1];

                titleBody = item.LinhVuc;
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 5;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellGio = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellGio.StyleIndex = styleIndex[styleIndex.Count - 1];

                string content = Convert.ToDateTime(item.IssuedDate).ToString("dd/MM/yyyy");
                string html = HttpUtility.HtmlDecode(content);
                titleBody = Regex.Replace(html, "<.*?>", String.Empty);
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 6;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellNoiDung = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellNoiDung.StyleIndex = styleIndex[styleIndex.Count - 2];

                titleBody = Convert.ToDateTime(item.EffectiveDate).ToString("dd/MM/yyyy");
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 7;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellDiaDiem = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellDiaDiem.StyleIndex = styleIndex[styleIndex.Count - 2];
                index++;

            }
            #region ChinhDoRongChoTungCotSheetThietBi
            //Chỉnh độ rộng cho từng cột
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 1, 5);
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 2, 10);//STT
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 3, 30);//Số hiệu
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 4, 35);//Trích yêu
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 5, 20);//Loại văn bản
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 6, 15);//Lĩnh vực
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 7, 25);//Ngày ban hành
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 8, 25);//Ngày hiệu lực
            #endregion
            //Set page setup cho sheet
            //ExcelUtils.PageSetupUpdate(sheet._worksheetPart, OrientationValues.Landscape, 0.590, 0.157, 0.66, 0.49, 0.2, 0.2, true, 0, 1, (UInt32Value)(int)PaperSize.A4, "", "", "", "", "");
            return sheet;
        }

        #endregion

        #region CAUHOI
        [CheckPermission]
        public ActionResult ThongKeCauHoi(string keyWord, string type, int? page)
        {
            try
            {
                DT_WebGISEntities MVCContext = new DT_WebGISEntities();
                TempData["TypeOfQuestionID"] = MVCContext.CMS_TypeOfQuestion.ToList();
                TempData.Keep("TypeOfQuestionID");


                int typeID = type != null ? Convert.ToInt32(type) : 0;
                keyWord = keyWord != null ? keyWord : "";
                CMS_Questions_DAO objDAO = new CMS_Questions_DAO();
                var data = objDAO.Search(keyWord, typeID, true);
                ViewBag.SearchString = keyWord;
                ViewBag.TYPE = type;
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(data.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return View();
            }
        }

        [HttpPost]
        public ActionResult export_excel_cauhoi(FormCollection fc)
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                CMS_Questions_DAO objDAO = new CMS_Questions_DAO();
                int typeID = fc["type_excel"].ToString() != "" ? Convert.ToInt32(fc["type_excel"].ToString()) : 0;
                string keyWord = fc["key_excel"].ToString();
                var data = objDAO.Search(keyWord, typeID, true);
                MemoryStream stream = new MemoryStream();

                using (SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    CWorkbook workbook = new CWorkbook(spreadsheetDoc);
                    string sheetName = "ThongKe";

                    CSheet sheetLH = createSheet_cauhoi(spreadsheetDoc, workbook, sheetName, data);

                    sheetLH._worksheetPart.Worksheet.Save();
                }
                string fileName = "ThongKeCauHoi.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Clear();
                Response.ContentType = contentType;
                string contentDisposition = "attachment; filename*=UTF-8''" + Uri.EscapeDataString(fileName);
                Response.AddHeader("content-disposition", contentDisposition);
                Response.AddHeader("Content-Length", stream.Length.ToString());
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                Response.SuppressContent = true;
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                return View();
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return View();
            }
        }

        private CSheet createSheet_cauhoi(SpreadsheetDocument spreadsheetDoc, CWorkbook workbook, string sheetName, List<CMS_Questions_LayTatCa_Result> data)
        {
            CSheet sheet = new CSheet(spreadsheetDoc, sheetName, workbook._sheets);
            List<uint> styleIndex = new List<uint>();
            CStyle cStyle = new CStyle();
            cStyle.fontBold = true;
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None);
            cStyle.fontName = "Times New Roman";
            cStyle.fontSize = 12;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));

            uint rowIndex = 1;
            uint colindex = 1;

            #region HEADER
            string header = "BỘ TÀI NGUYÊN VÀ MÔI TRƯỜNG";
            uint rowHeader = rowIndex + 1;
            uint colHeader = colindex + 1;
            int textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            Cell cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 1) + (rowHeader).ToString());

            cStyle = new CStyle();
            cStyle.fontBold = true;
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None);
            cStyle.fontName = "Times New Roman";
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));
            header = "Cục công nghệ thông tin và DLTNMT";
            rowHeader = rowIndex + 2;
            colHeader = colindex + 1;
            textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 1) + (rowHeader).ToString());

            cStyle.fontSize = 17;
            header = "Thống kê câu hỏi";
            rowHeader = rowIndex + 4;
            colHeader = colindex + 1;
            textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 6) + (rowHeader).ToString());

            //cStyle = new CStyle();
            //cStyle.fontBold = true;
            //cStyle.fontItalic = true;
            //cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            //cStyle.SetBorderStyle(BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None);
            //cStyle.fontName = "Times New Roman";
            //cStyle.fontSize = 13;
            //cStyle.CreateStyle();
            //styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));
            //header = "(Từ ngày " + listLH[0].ngay + " đến ngày " + listLH[listLH.Count - 1].ngay + ")";
            //rowHeader = rowIndex + 5;
            //colHeader = colindex + 1;
            //textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            //cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            //cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            //ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 6) + (rowHeader).ToString());
            #endregion



            #region headerTable
            rowIndex = rowIndex + 6;
            cStyle = new CStyle();
            cStyle.fontBold = true;
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
            cStyle.fontName = "Times New Roman";
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));

            string title = "STT";
            uint rowindexHeader = rowIndex + 1;
            uint colindexHeader = colindex + 1;
            int textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellSTT = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellSTT.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Tiêu đề";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 2;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellLanhDaoVu = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellLanhDaoVu.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Thể loại";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 3;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellChuyenVien = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellChuyenVien.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Ngày tạo";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 4;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellLanhDaoBo = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellLanhDaoBo.StyleIndex = styleIndex[styleIndex.Count - 1];

            #endregion
            rowIndex++;
            cStyle = new CStyle();
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
            cStyle.fontName = "Times New Roman";
            cStyle.fontBold = false;
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));

            cStyle = new CStyle();
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Left;
            cStyle.SetBorderStyle(BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
            cStyle.fontName = "Times New Roman";
            cStyle.fontBold = false;
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));
            uint index = 0;
            foreach (var item in data)
            {

                string titleBody = (index + 1).ToString();
                uint rowindexBody = rowIndex + index + 1;
                uint colIndexBody = colindex + 1;
                int textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellSTT = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellSTT.StyleIndex = styleIndex[styleIndex.Count - 2];
                //ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colIndexBody) + rowindexBody.ToString(), ExcelUtils.GetColumnNameByIndex(colIndexBody) + (rowindexBody + item.data.Count() - 1).ToString());

                titleBody = item.Title;
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 2;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellLanhDaoVu = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellLanhDaoVu.StyleIndex = styleIndex[styleIndex.Count - 1];

                titleBody = item.LoaiCauHoi;
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 3;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellChuyenVien = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellChuyenVien.StyleIndex = styleIndex[styleIndex.Count - 1];

                titleBody = Convert.ToDateTime(item.CreateDate).ToString("dd/MM/yyyy");
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 4;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellLanhDaoBo = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellLanhDaoBo.StyleIndex = styleIndex[styleIndex.Count - 2];
                index++;

            }
            #region ChinhDoRongChoTungCotSheetThietBi
            //Chỉnh độ rộng cho từng cột
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 1, 5);
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 2, 10);//STT
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 3, 45);//Tiêu đề
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 4, 25);//Thể loại
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 5, 20);//Ngày tạo
            #endregion
            //Set page setup cho sheet
            //ExcelUtils.PageSetupUpdate(sheet._worksheetPart, OrientationValues.Landscape, 0.590, 0.157, 0.66, 0.49, 0.2, 0.2, true, 0, 1, (UInt32Value)(int)PaperSize.A4, "", "", "", "", "");
            return sheet;
        }

        #endregion

        #region BANDO
        [CheckPermission]
        public ActionResult ThongKeBanDo(string keyWord, string type, int? page)
        {
            try
            {
                DT_WebGISEntities MVCContext = new DT_WebGISEntities();
                TempData["TypeOfMapID"] = MVCContext.CMS_TypeOfMap.ToList();
                TempData.Keep("TypeOfMapID");


                int TypeOfMapID = type == null ? 0 : Convert.ToInt32(type);
                string searchString = keyWord != null ? keyWord : "";
                CMS_Maps_DAO objDAO = new CMS_Maps_DAO();
                var data = objDAO.Search(searchString, TypeOfMapID);
                ViewBag.SearchString = searchString;
                ViewBag.TYPE = type;

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(data.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return View();
            }
        }

        [HttpPost]
        public ActionResult export_excel_bando(FormCollection fc)
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                CMS_Maps_DAO objDAO = new CMS_Maps_DAO();
                int typeID = fc["type_excel"].ToString() != "" ? Convert.ToInt32(fc["type_excel"].ToString()) : 0;
                string keyWord = fc["key_excel"].ToString();
                var data = objDAO.Search(keyWord, typeID);
                MemoryStream stream = new MemoryStream();

                using (SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    CWorkbook workbook = new CWorkbook(spreadsheetDoc);
                    string sheetName = "ThongKe";

                    CSheet sheetLH = createSheet_bando(spreadsheetDoc, workbook, sheetName, data);

                    sheetLH._worksheetPart.Worksheet.Save();
                }
                string fileName = "ThongKeBanDo.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Clear();
                Response.ContentType = contentType;
                string contentDisposition = "attachment; filename*=UTF-8''" + Uri.EscapeDataString(fileName);
                Response.AddHeader("content-disposition", contentDisposition);
                Response.AddHeader("Content-Length", stream.Length.ToString());
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                Response.SuppressContent = true;
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                return View();
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi" + ex.Message.ToString(), "error");
                Logs.WriteLog(ex);
                return View();
            }
        }

        private CSheet createSheet_bando(SpreadsheetDocument spreadsheetDoc, CWorkbook workbook, string sheetName, List<CMS_Maps_LayTatCa_Result> data)
        {
            CSheet sheet = new CSheet(spreadsheetDoc, sheetName, workbook._sheets);
            List<uint> styleIndex = new List<uint>();
            CStyle cStyle = new CStyle();
            cStyle.fontBold = true;
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None);
            cStyle.fontName = "Times New Roman";
            cStyle.fontSize = 12;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));

            uint rowIndex = 1;
            uint colindex = 1;

            #region HEADER
            string header = "BỘ TÀI NGUYÊN VÀ MÔI TRƯỜNG";
            uint rowHeader = rowIndex + 1;
            uint colHeader = colindex + 1;
            int textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            Cell cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 1) + (rowHeader).ToString());

            cStyle = new CStyle();
            cStyle.fontBold = true;
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None);
            cStyle.fontName = "Times New Roman";
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));
            header = "Cục công nghệ thông tin và DLTNMT";
            rowHeader = rowIndex + 2;
            colHeader = colindex + 1;
            textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 1) + (rowHeader).ToString());

            cStyle.fontSize = 17;
            header = "Thống kê bản đồ";
            rowHeader = rowIndex + 4;
            colHeader = colindex + 1;
            textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 6) + (rowHeader).ToString());

            //cStyle = new CStyle();
            //cStyle.fontBold = true;
            //cStyle.fontItalic = true;
            //cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            //cStyle.SetBorderStyle(BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None, BorderStyleValues.None);
            //cStyle.fontName = "Times New Roman";
            //cStyle.fontSize = 13;
            //cStyle.CreateStyle();
            //styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));
            //header = "(Từ ngày " + listLH[0].ngay + " đến ngày " + listLH[listLH.Count - 1].ngay + ")";
            //rowHeader = rowIndex + 5;
            //colHeader = colindex + 1;
            //textHeader = sheet.InsertSharedStringItem(header, workbook._sharedStringTablePart);
            //cellHeader = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colHeader), rowHeader, 1, textHeader, null);
            //cellHeader.StyleIndex = styleIndex[styleIndex.Count - 1];
            //ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colHeader) + rowHeader.ToString(), ExcelUtils.GetColumnNameByIndex(colHeader + 6) + (rowHeader).ToString());
            #endregion



            #region headerTable
            rowIndex = rowIndex + 6;
            cStyle = new CStyle();
            cStyle.fontBold = true;
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
            cStyle.fontName = "Times New Roman";
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));

            string title = "STT";
            uint rowindexHeader = rowIndex + 1;
            uint colindexHeader = colindex + 1;
            int textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellSTT = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellSTT.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Tên";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 2;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellLanhDaoVu = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellLanhDaoVu.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Loại bản đồ";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 3;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellChuyenVien = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellChuyenVien.StyleIndex = styleIndex[styleIndex.Count - 1];

            title = "Mô tả";
            rowindexHeader = rowIndex + 1;
            colindexHeader = colindex + 4;
            textIndexHeader = sheet.InsertSharedStringItem(title, workbook._sharedStringTablePart);
            Cell cellLanhDaoBo = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colindexHeader), rowindexHeader, 1, textIndexHeader, null);
            cellLanhDaoBo.StyleIndex = styleIndex[styleIndex.Count - 1];

            #endregion
            rowIndex++;
            cStyle = new CStyle();
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Center;
            cStyle.SetBorderStyle(BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
            cStyle.fontName = "Times New Roman";
            cStyle.fontBold = false;
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));

            cStyle = new CStyle();
            cStyle.alignmentHorizontal = HorizontalAlignmentValues.Left;
            cStyle.SetBorderStyle(BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
            cStyle.fontName = "Times New Roman";
            cStyle.fontBold = false;
            cStyle.fontSize = 13;
            cStyle.CreateStyle();
            styleIndex.Add(cStyle.GetStyleIndex(workbook._workbookPart));
            uint index = 0;
            foreach (var item in data)
            {

                string titleBody = (index + 1).ToString();
                uint rowindexBody = rowIndex + index + 1;
                uint colIndexBody = colindex + 1;
                int textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellSTT = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellSTT.StyleIndex = styleIndex[styleIndex.Count - 2];
                //ExcelUtils.MergeTwoCells(sheet._worksheetPart.Worksheet, sheetName, ExcelUtils.GetColumnNameByIndex(colIndexBody) + rowindexBody.ToString(), ExcelUtils.GetColumnNameByIndex(colIndexBody) + (rowindexBody + item.data.Count() - 1).ToString());

                titleBody = item.Name;
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 2;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellLanhDaoVu = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellLanhDaoVu.StyleIndex = styleIndex[styleIndex.Count - 1];

                titleBody = item.LoaiBanDo;
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 3;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellChuyenVien = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellChuyenVien.StyleIndex = styleIndex[styleIndex.Count - 1];

                titleBody = item.Description;
                rowindexBody = rowIndex + index + 1;
                colIndexBody = colindex + 4;
                textIndexBody = sheet.InsertSharedStringItem(titleBody, workbook._sharedStringTablePart);
                cellLanhDaoBo = sheet.InsertCellInWorksheet(ExcelUtils.GetColumnNameByIndex(colIndexBody), rowindexBody, 1, textIndexBody, titleBody);
                cellLanhDaoBo.StyleIndex = styleIndex[styleIndex.Count - 1];
                index++;

            }
            #region ChinhDoRongChoTungCotSheetThietBi
            //Chỉnh độ rộng cho từng cột
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 1, 5);
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 2, 10);//STT
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 3, 30);//Tiêu đề
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 4, 20);//Thể loại
            ExcelUtils.UpdateColumnWidth(sheet._worksheetPart, 5, 20);//Mô tả
            #endregion
            //Set page setup cho sheet
            //ExcelUtils.PageSetupUpdate(sheet._worksheetPart, OrientationValues.Landscape, 0.590, 0.157, 0.66, 0.49, 0.2, 0.2, true, 0, 1, (UInt32Value)(int)PaperSize.A4, "", "", "", "", "");
            return sheet;
        }
        #endregion
    }
}
