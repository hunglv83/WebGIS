using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;

namespace Ciren.COpenXML
{
    public class CSheet
    {
        public WorksheetPart _worksheetPart;
        public SheetData _sheetData;

        /// <summary>
        /// Khoi tao: add 01 sheet vao workbook. Day sheetData vao thuoc tinh cua lop
        /// </summary>
        /// <param name="spreadsheetDocument"></param>
        /// <param name="sheetName"></param>
        /// <param name="sheets"></param>
        public CSheet(SpreadsheetDocument spreadsheetDocument, string sheetName, Sheets sheets)
        {
            _worksheetPart = spreadsheetDocument.WorkbookPart.AddNewPart<WorksheetPart>();

            // Get a unique ID for the new worksheet.
            uint sheetId = 1;

            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(_worksheetPart),
                SheetId = sheetId,
                Name = sheetName
            };
            sheets.Append(sheet);
            _sheetData = new SheetData();
            _worksheetPart.Worksheet = new Worksheet(_sheetData);
        }

        /// <summary>
        /// Insert tung cell vao worksheet
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellDataType">1:SharedString, 2:Number, 3: Date</param>
        /// <param name="index">neu cellDataType != 1 thi lay gia tri tu index</param>
        /// <param name="cellValue">neu cellDataType != 1 thi lay gia tri tu cellValue</param>
        /// <returns></returns>
        public Cell InsertCellInWorksheet( 
            string columnName, 
            uint rowIndex, 
            int cellDataType, 
            int index, 
            string cellValue)
        {
            Worksheet worksheet = _worksheetPart.Worksheet;
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (_sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = _sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                _sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {               
                Cell newCell = new Cell() { CellReference = cellReference };
                if (cellDataType == 1)
                {
                    newCell.DataType = CellValues.SharedString;
                    //newCell.CellValue = new CellValue(index.ToString());
                    CellValue cellVal = new CellValue();
                    cellVal.Text = index.ToString();
                    newCell.Append(cellVal);
                }
                else if (cellDataType == 2)
                {
                    newCell.DataType = CellValues.Number;
                    newCell.CellValue = new CellValue(cellValue);
                }
                else if (cellDataType == 3)
                {
                    newCell.DataType = CellValues.Date;
                    newCell.CellValue = new CellValue(cellValue);
                }
                //row.InsertBefore(newCell, refCell);
                row.Append(newCell);
                //worksheet.Save();
                return newCell;
            }
        }

        /// <summary>
        /// Thêm mới cell
        /// </summary>
        /// <param name="row">Row đã tồn tại trong file excel </param>
        /// <param name="columnName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellDataType"></param>
        /// <param name="index"></param>
        /// <param name="cellValue"></param>
        /// <returns></returns>
        public Cell InsertCellInWorksheet(
            Row row, 
            string columnName, 
            uint rowIndex, 
            int cellDataType, 
            int index, 
            string cellValue)
        {
            string cellReference = columnName + rowIndex;
            Cell newCell = new Cell() { CellReference = cellReference };
            if (cellDataType == 1)
            {
                newCell.DataType = CellValues.SharedString;
                
                CellValue cellVal = new CellValue();
                cellVal.Text = index.ToString();
                newCell.Append(cellVal);
            }
            else if (cellDataType == 2)
            {
                newCell.DataType = CellValues.Number;
                newCell.CellValue = new CellValue(cellValue);
            }
            else if (cellDataType == 3)
            {
                newCell.DataType = CellValues.Date;
                newCell.CellValue = new CellValue(cellValue);
            }
            else if (cellDataType == 4) //Cell formula
            {
                CellFormula cf = new CellFormula();
                cf.Text = index.ToString();
                newCell.Append(cf);
            }
            row.Append(newCell);
            return newCell;
        }

        #region ThemMoiCellFormula
        /// <summary>
        /// Hungpn: Bổ sung thêm phương thức insert cell formula
        /// Thêm mới cell Formula
        /// </summary>
        /// <param name="row">Row đã tồn tại trong file excel </param>
        /// <param name="columnName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellDataType"></param>
        /// <param name="index"></param>
        /// <param name="cellValue"></param>
        /// <returns>CellFormula</returns>
        public Cell InsertCellInWorksheet(Row row, string columnName, uint rowIndex, string strFormula)
        {
            string cellReference = columnName + rowIndex;
            Cell newCell = new Cell() { CellReference = cellReference };
            CellFormula cf = new CellFormula();
            cf.Text = strFormula;
            newCell.Append(cf);
            row.Append(newCell);
            return newCell;
        }
        #endregion

        public int InsertSharedStringItem(string text, SharedStringTablePart sharedStringTablePart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (sharedStringTablePart.SharedStringTable == null)
            {
                sharedStringTablePart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in sharedStringTablePart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            sharedStringTablePart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            sharedStringTablePart.SharedStringTable.Save();

            return i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">text in format: "truongpm <br /> <b><i>bold italic</i></b> <b>bold</b> <i>italic</i>" </param>
        /// <param name="shareStringPart"></param>
        /// <returns></returns>
        public static int InsertSharedStringItem_Format(string text, SharedStringTablePart shareStringPart)
        {
            text = text.Replace("<br />", "<br/>").Replace("<br/>", "\n");
            List<TextFormat> tfs = new List<TextFormat>();
            TextFormat tf = new TextFormat();
            //string html = "truongpm<b><i>bold italic</i></b><b>bold</b><i>italic</i>";
            SharedStringItem sharedStringItem = new SharedStringItem();
            string tagRegex = @"[^<>]+|<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>";
           
            MatchCollection matchesImgSrc = Regex.Matches(text, tagRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            int i = 0;
            if (matchesImgSrc.Count > 1)
            {
                foreach (Match m in matchesImgSrc)
                {
                    string[] values1 = m.Value.Split(new string[] { "<b><i>", "</i></b>" }, StringSplitOptions.None);
                    if (values1.Length > 2)
                    {
                        //la the <b><i>
                        tf = new TextFormat();
                        tf.Ftype = 1;
                        tf.Str = values1[1];
                        tfs.Add(tf);
                    }
                    else
                    {
                        values1 = m.Value.Split(new string[] { "<b>", "</b>" }, StringSplitOptions.None);
                        if (values1.Length > 2)
                        {
                            //la the <b>
                            tf = new TextFormat();
                            tf.Ftype = 2;
                            tf.Str = values1[1];
                            tfs.Add(tf);
                        }
                        else
                        {
                            values1 = m.Value.Split(new string[] { "<i>", "</i>" }, StringSplitOptions.None);
                            if (values1.Length > 2)
                            {
                                tf = new TextFormat();
                                tf.Ftype = 3;
                                tf.Str = values1[1];
                                tfs.Add(tf);
                            }
                            else
                            {
                                tf = new TextFormat();
                                tf.Ftype = 0;
                                tf.Str = values1[0];
                                tfs.Add(tf);

                            }
                        }
                    }

                }
                // If the part does not contain a SharedStringTable, create one.
                if (shareStringPart.SharedStringTable == null)
                {
                    shareStringPart.SharedStringTable = new SharedStringTable();
                }

                // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
                foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
                {
                    if (item.InnerText == text)
                    {
                        return i;
                    }
                    i++;
                }

                for (int j = 0; j < tfs.Count; j++)
                {
                    switch (tfs[j]._ftype)
                    {
                        case 0:
                            GenerateCellContentStyle(sharedStringItem, "Time New Roman", false, false, tfs[j].Str);
                            break;
                        case 1: //bold; italic

                            GenerateCellContentStyle(sharedStringItem, "Time New Roman", true, true, tfs[j].Str);
                            break;
                        case 2: //bold

                            GenerateCellContentStyle(sharedStringItem, "Time New Roman", true, false, tfs[j].Str);
                            break;
                        case 3: //italic

                            GenerateCellContentStyle(sharedStringItem, "Time New Roman", false, true, tfs[j].Str);
                            break;
                        default:
                            break;
                    }
                }
                shareStringPart.SharedStringTable.AppendChild(sharedStringItem);
            }
            else
            {
                // If the part does not contain a SharedStringTable, create one.
                if (shareStringPart.SharedStringTable == null)
                {
                    shareStringPart.SharedStringTable = new SharedStringTable();
                }



                // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
                foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
                {
                    if (item.InnerText == text)
                    {
                        return i;
                    }
                    i++;
                }
                shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            }

            shareStringPart.SharedStringTable.Save();
            return i;

        }

        private static void GenerateCellContentStyle(SharedStringItem shareStringItem, string font, bool isBold, bool isItalic, string str)
        {
            Run run = new Run();
            RunProperties runProperties = new RunProperties();
            Bold bold = new Bold();
            Italic italic = new Italic();
            DocumentFormat.OpenXml.Spreadsheet.FontSize fontSize = new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11D };
            Color color = new Color() { Theme = (UInt32Value)1U };
            RunFont runFont = new RunFont() { Val = font };
            FontFamily fontFamily = new FontFamily() { Val = 2 };
            FontScheme fontScheme = new FontScheme() { Val = FontSchemeValues.Minor };

            if (isBold)
            {
                runProperties.Append(bold);
            }
            if (isItalic)
            {
                runProperties.Append(italic);
            }

            runProperties.Append(fontSize);
            runProperties.Append(color);
            runProperties.Append(runFont);
            runProperties.Append(fontFamily);
            runProperties.Append(fontScheme);
            Text text1 = new Text() { Space = SpaceProcessingModeValues.Preserve };
            text1.Text = str;
            run.Append(runProperties);
            run.Append(text1);
            shareStringItem.Append(run);
        }

        /// <summary>
        /// Xuat mot so row lua chon
        /// </summary>
        /// <param name="spreadsheetDoc"></param>
        /// <param name="sheetName"></param>
        /// <param name="table"></param>
        /// <param name="sheetData"></param>
        /// <param name="chs"></param>
        public void DatatableExport(WorkbookPart workbookPart, 
            DataTable table, 
            SheetData sheetData, 
            List<ColunmHelper> chs, 
            SharedStringTablePart sharedStringTablePart
            )
        {
            foreach (DataRow dr in table.Rows)
            {
                Row row = RowAddNew(sheetData);
                foreach (DataColumn column in table.Columns)
                {
                    foreach (ColunmHelper ch in chs)
                    {
                        if (column.ColumnName == ch.CValue)
                        {
                            Cell cell = new Cell();
                            int index;
                            string numberFormat = null;
                            if (ch.CType == 1)
                            {
                                index = InsertSharedStringItem_Format(dr[column].ToString(), sharedStringTablePart);
                                cell.CellValue = new CellValue(index.ToString());
                                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                            }
                            else if (ch.CType == 2)
                            {
                                cell.DataType = CellValues.Number;
                                cell.CellValue = new CellValue(dr[column].ToString());
                                numberFormat = "#,##0";
                            }
                            else if (ch.CType == 3)
                            {
                                cell.DataType = CellValues.Date;
                                cell.CellValue = new CellValue(dr[column].ToString());
                            }

                            //style for cell
                            CStyle cStyle = new CStyle();
                            if (ch._cAlignment == 1) //left
                            {
                                cStyle.CreateStyle(numberFormat,12, "TimeNewRoman", "#000", true, true, PatternValues.None, null, "#000", BorderStyleValues.Thin,
                                    "#000", BorderStyleValues.Dotted,
                                    "#000", BorderStyleValues.DashDot,
                                    "#000", BorderStyleValues.Hair, HorizontalAlignmentValues.Right, VerticalAlignmentValues.Center);
                            }
                            else if (ch._cAlignment == 2)//center
                            {
                                cStyle.CreateStyle(numberFormat, 12, "TimeNewRoman", "#000", true, true, PatternValues.None, null, "#000", BorderStyleValues.Thin,
                                    "#000", BorderStyleValues.Dotted,
                                    "#000", BorderStyleValues.DashDot,
                                    "#000", BorderStyleValues.Hair, HorizontalAlignmentValues.Right, VerticalAlignmentValues.Center);
                            }
                            else if (ch._cAlignment == 3) //right
                            {
                                cStyle.CreateStyle(numberFormat, 12, "TimeNewRoman", "#000", true, true, PatternValues.None, null, "#000", BorderStyleValues.Thin,
                                    "#000", BorderStyleValues.Dotted,
                                    "#000", BorderStyleValues.DashDot,
                                    "#000", BorderStyleValues.Hair, HorizontalAlignmentValues.Right, VerticalAlignmentValues.Center);
                            }

                            CellFormat cellFormat = cell.StyleIndex != null ? cStyle.GetCellFormat(cell.StyleIndex, workbookPart).CloneNode(true) as CellFormat : new CellFormat();
                            cStyle.AppendCellFormat(cellFormat, workbookPart);
                            uint styleIndex = cStyle.InsertCellFormat(cellFormat, workbookPart);
                            cell.StyleIndex = styleIndex;

                            row.AppendChild(cell);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add row moi vao sheetdata
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="workbookPart"></param>
        /// <returns></returns>
        public Row RowAddNew(SheetData sheetData)
        {
            Row lastRow = sheetData.Elements<Row>().LastOrDefault();
            Row newRow;
            if (lastRow != null)
            {
                newRow = new Row() { RowIndex = (lastRow.RowIndex + 1) };
                sheetData.InsertAfter(newRow, lastRow);
            }
            else
            {
                newRow = new Row() { RowIndex = 0 };
                sheetData.InsertAt(newRow, 0);
            }
            return newRow; 
        }
    }
}
