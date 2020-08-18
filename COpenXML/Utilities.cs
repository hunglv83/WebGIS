using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Text.RegularExpressions;

namespace Ciren.COpenXML
{
    public enum PaperSize : int
    {
        A5 = 11,
        A4 = 9,
        A3 = 512
    } 
    public class ExcelUtils
    {
        #region Print Titles
        /// <summary>
        /// http://stackoverflow.com/questions/8405025/set-print-area-openxml-with-excel
        /// https://msdn.microsoft.com/en-us/library/documentformat.openxml.spreadsheet.definedname(v=office.14).aspx
        /// </summary>
        /// <param name="spreadsheetDoc"></param>
        /// <param name="sheetName"></param>
        /// <param name="columnStart">Ex: "$A"</param>
        /// <param name="columnEnd">EX: "$B"</param>
        /// null if just not set print areas column or row
        public static void SetPrintTitles(WorkbookPart workbookPart, string sheetName, string columnStart, string columnEnd, string rowStart, string rowEnd)
        {
            String definedName = sheetName;
            var definedNames = workbookPart.Workbook.Descendants<DefinedNames>().FirstOrDefault();

            DefinedName name = null;
            UInt32Value locSheetId;
            if (definedNames == null)
            {
                definedNames = new DefinedNames();
                workbookPart.Workbook.Append(definedNames);
                workbookPart.Workbook.Save();
                locSheetId = UInt32Value.FromUInt32(0);
            }
            else
            {
                int defineNameCount = definedNames.Descendants<DefinedName>().Count();
                locSheetId = UInt32Value.FromUInt32((UInt32)defineNameCount);
            }
            //_xlnm.Print_Area la tham so de set up cho khoang khong in
            if (rowStart == null && rowEnd == null && columnEnd != null && columnStart != null)
            {
                name = new DefinedName()
                {
                    Name = "_xlnm.Print_Titles",
                    LocalSheetId = locSheetId
                    ,
                    Text = String.Format("{0}!${1}:${2}", sheetName, columnStart.Replace("$", ""), columnEnd.Replace("$", ""))
                };
            }
            else if (rowStart != null && rowEnd != null && columnEnd == null && columnStart == null)
            {
                name = new DefinedName()
                {
                    Name = "_xlnm.Print_Titles",
                    LocalSheetId = locSheetId
                    ,
                    Text = String.Format("{0}!${1}:${2}", sheetName, rowStart.Replace("$", ""), rowEnd.Replace("$", ""))
                };
            }
            else if (rowStart != null && rowEnd != null && columnEnd != null && columnStart != null)
            {
                name = new DefinedName() { Name = "_xlnm.Print_Titles", LocalSheetId = locSheetId
                    ,
                                           Text = String.Format("{0}!${1}:${2},{0}!${3}:${4}", sheetName, columnStart.Replace("$", ""), columnEnd.Replace("$", ""), rowStart.Replace("$", ""), rowEnd.Replace("$", ""))
                };
            }
            definedNames.Append(name);
            workbookPart.Workbook.Save();
        }
        #endregion
        #region Print Areas
        /// <summary>
        /// http://stackoverflow.com/questions/8405025/set-print-area-openxml-with-excel
        /// https://msdn.microsoft.com/en-us/library/documentformat.openxml.spreadsheet.definedname(v=office.14).aspx
        /// </summary>
        /// <param name="spreadsheetDoc"></param>
        /// <param name="sheetName"></param>
        /// <param name="columnStart">Ex: "$A"</param>
        /// <param name="columnEnd">EX: "$B"</param>
        /// null if just not set print areas column or row
        public static void SetPrintArea(WorkbookPart workbookPart, string sheetName, string columnStart, string columnEnd, string rowStart, string rowEnd)
        {
            String definedName = sheetName;
            var definedNames = workbookPart.Workbook.Descendants<DefinedNames>().FirstOrDefault();

            DefinedName name = null;
            UInt32Value locSheetId;
            if (definedNames == null)
            {
                definedNames = new DefinedNames();
                workbookPart.Workbook.Append(definedNames);
                workbookPart.Workbook.Save();
                locSheetId = UInt32Value.FromUInt32(0);
            }
            else
            {
                int defineNameCount = definedNames.Descendants<DefinedName>().Count();
                locSheetId = UInt32Value.FromUInt32((UInt32)defineNameCount);
            }
            //_xlnm.Print_Area la tham so de set up cho khoang khong in            
            if (rowStart != null && rowEnd != null && columnEnd != null && columnStart != null)
            {
                name = new DefinedName()
                {
                    Name = "_xlnm.Print_Area",
                    LocalSheetId = locSheetId
                    ,
                    Text = String.Format("{0}!${1}${2}:${3}${4}", sheetName, columnStart.Replace("$", ""), rowStart.Replace("$", ""), columnEnd.Replace("$", ""), rowEnd.Replace("$", ""))
                };
            }
            definedNames.Append(name);
            workbookPart.Workbook.Save();
        }
        #endregion
        
        #region Column width
        public static void UpdateColumnWidth(WorksheetPart worksheetPart, uint colIndex, int characterNum)
        {
            if (worksheetPart != null)
            {
                SetColumnWidth(worksheetPart.Worksheet, colIndex, GetColumnWidth(characterNum));
                // Save the worksheet.
                //worksheetPart.Worksheet.Save();
            }
        }
        private static void SetColumnWidth(Worksheet worksheet, uint Index, DoubleValue dwidth)
        {

            DocumentFormat.OpenXml.Spreadsheet.Columns cs = worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Columns>();
            if (cs != null)
            {
                IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Column> ic = cs.Elements<DocumentFormat.OpenXml.Spreadsheet.Column>().Where(r => r.Min == Index).Where(r => r.Max == Index);
                if (ic.Count() > 0)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Column c = ic.First();
                    c.Width = dwidth;
                }
                else
                {
                    DocumentFormat.OpenXml.Spreadsheet.Column c = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = Index, Max = Index, Width = dwidth, CustomWidth = true };
                    cs.Append(c);
                }
            }
            else
            {
                cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                DocumentFormat.OpenXml.Spreadsheet.Column c = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = Index, Max = Index, Width = dwidth, CustomWidth = true };
                cs.Append(c);
                worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetFormatProperties>());
            }
        }
        private const double digitWidth = 20;
        private static DoubleValue GetColumnWidth(int characterNum)
        {
            DoubleValue a = new DoubleValue();
            a.Value = Math.Truncate((characterNum * (digitWidth + 5.0)) / digitWidth * 200) / 256;
            return a;
        }
        #endregion

        #region pagesetup
        public static void PageSetupUpdate(WorksheetPart worksheetPart, OrientationValues landscapeOrPortrait,
            DoubleValue marginLeft, DoubleValue marginRight, DoubleValue marginTop, DoubleValue marginBottom, DoubleValue marginHeader, DoubleValue marginFooter,
            Boolean isFitToPage, UInt32Value pageSize)
        {

            Worksheet ws = worksheetPart.Worksheet;
            //page setup them moi pagesetup properties
            SheetProperties sp = new SheetProperties(new PageSetupProperties());
            ws.SheetProperties = sp;

            PrintOptions printOp = new PrintOptions();
            printOp.HorizontalCentered = true;
            ws.AppendChild(printOp);

            PageMargins pageMargins = new PageMargins();
            pageMargins.Left = marginLeft;
            pageMargins.Right = marginRight;
            pageMargins.Top = marginTop;
            pageMargins.Bottom = marginBottom;
            pageMargins.Header = marginHeader;
            pageMargins.Footer = marginFooter;
            ws.AppendChild(pageMargins);

            // Set the FitToPage property to true
            ws.SheetProperties.PageSetupProperties.FitToPage = BooleanValue.FromBoolean(isFitToPage);

            DocumentFormat.OpenXml.Spreadsheet.PageSetup pgOr = new DocumentFormat.OpenXml.Spreadsheet.PageSetup();
            pgOr.Orientation = landscapeOrPortrait;
            pgOr.PaperSize = pageSize;
            pgOr.FitToHeight = 0;
            pgOr.FitToWidth = 1;
            ws.AppendChild(pgOr);           

            //save worksheet properties
            //worksheetPart.Worksheet.Save();

        }
        public static void PageSetupUpdate(WorksheetPart worksheetPart, OrientationValues landscapeOrPortrait,
           DoubleValue marginLeft, DoubleValue marginRight, DoubleValue marginTop, DoubleValue marginBottom, DoubleValue marginHeader, DoubleValue marginFooter,
           Boolean isFitToPage, UInt32Value FitToHeight, UInt32Value FitToWidth, UInt32Value pageSize, string headerLeft, string headerCenter, string headerRight, string footerLeft, string footerRight)
        {

            Worksheet ws = worksheetPart.Worksheet;
            //page setup them moi pagesetup properties
            SheetProperties sp = new SheetProperties(new PageSetupProperties());
            ws.SheetProperties = sp;

            PrintOptions printOp = new PrintOptions();
            printOp.HorizontalCentered = true;
            ws.AppendChild(printOp);

            PageMargins pageMargins = new PageMargins();
            pageMargins.Left = marginLeft;
            pageMargins.Right = marginRight;
            pageMargins.Top = marginTop;
            pageMargins.Bottom = marginBottom;
            pageMargins.Header = marginHeader;
            pageMargins.Footer = marginFooter;
            ws.AppendChild(pageMargins);

            // Set the FitToPage property to true
            ws.SheetProperties.PageSetupProperties.FitToPage = BooleanValue.FromBoolean(isFitToPage);

            DocumentFormat.OpenXml.Spreadsheet.PageSetup pgOr = new DocumentFormat.OpenXml.Spreadsheet.PageSetup();
            pgOr.Orientation = landscapeOrPortrait;
            pgOr.PaperSize = pageSize;
            pgOr.FitToHeight = FitToHeight;
            pgOr.FitToWidth = FitToWidth;
            ws.AppendChild(pgOr);

            HeaderFooter headerFooter1 = new HeaderFooter();
            OddHeader oddHeader1 = new OddHeader();
            oddHeader1.Text = "&L&\"Times New Roman,Regular\"" + headerLeft + "&C&\"Times New Roman,Regular\"" + headerCenter + "&R&\"Times New Roman,Regular\"" + headerRight;
            OddFooter oddFooter1 = new OddFooter();
            oddFooter1.Text = "&L&\"Times New Roman,Regular\"" + footerLeft + "&C&P&R&\"Times New Roman,Regular\"" + footerRight;
            headerFooter1.Append(oddHeader1);
            headerFooter1.Append(oddFooter1);
            ws.AppendChild(headerFooter1);

            //save worksheet properties
            //worksheetPart.Worksheet.Save();

        }   
        #endregion

        #region merge 2 cell
        // Given a document name, a worksheet name, and the names of two adjacent cells, merges the two cells.
        // When two cells are merged, only the content from one cell is preserved:
        // the upper-left cell for left-to-right languages or the upper-right cell for right-to-left languages.
        public static void MergeTwoCells(Worksheet worksheet, string sheetName, string cell1Name, string cell2Name)
        {
            if (worksheet == null || string.IsNullOrEmpty(cell1Name) || string.IsNullOrEmpty(cell2Name))
            {
                return;
            }
            uint columnIndex1 = GetColumnIndex(cell1Name);
            uint rowIndex1 = GetRowIndexUint(cell1Name);
            uint columnIndex2 = GetColumnIndex(cell2Name);
            uint rowIndex2 = GetRowIndexUint(cell2Name);

            if (rowIndex1 == rowIndex2)
            {
                Cell cell = CreateSpreadsheetCellIfNotExist(worksheet, cell1Name);
                for (uint i = columnIndex1 + 1; i <= columnIndex2; i++)
                {
                    if(cell.StyleIndex != null)
                        CreateSpreadsheetCellIfNotExist(worksheet, GetColumnNameByIndex(i) + rowIndex1.ToString(), cell.StyleIndex.Value);
                    else
                        CreateSpreadsheetCellIfNotExist(worksheet, GetColumnNameByIndex(i) + rowIndex1.ToString());
                }
            }
            else
            {
                if (columnIndex1 == columnIndex2)
                {
                    Cell cell = CreateSpreadsheetCellIfNotExist(worksheet, cell1Name);
                    for (uint i = rowIndex1 + 1; i <= rowIndex2; i++)
                    {
                        if (cell.StyleIndex != null)
                            CreateSpreadsheetCellIfNotExist(worksheet, GetColumnNameByIndex(columnIndex1) + i.ToString(), cell.StyleIndex.Value);
                        else
                            CreateSpreadsheetCellIfNotExist(worksheet, GetColumnNameByIndex(columnIndex1) + i.ToString());
                    }
                }
                else
                {
                    Cell cell = CreateSpreadsheetCellIfNotExist(worksheet, cell1Name);
                    for (uint i = rowIndex1; i <= rowIndex2; i++)
                    {
                        for (uint j = columnIndex1; j <= columnIndex2; j++)
                        {
                            if (i == rowIndex1 && j == columnIndex1)
                                continue;
                            else
                            {
                                if (cell.StyleIndex != null)
                                    CreateSpreadsheetCellIfNotExist(worksheet, GetColumnNameByIndex(j) + i.ToString(), cell.StyleIndex.Value);
                                else
                                    CreateSpreadsheetCellIfNotExist(worksheet, GetColumnNameByIndex(j) + i.ToString());
                            }
                        }
                    }
                }
            }
           
            MergeCells mergeCells;
            if (worksheet.Elements<MergeCells>().Count() > 0)
            {
                mergeCells = worksheet.Elements<MergeCells>().First();
            }
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.
                if (worksheet.Elements<CustomSheetView>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<CustomSheetView>().First());
                }
                else if (worksheet.Elements<DataConsolidate>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<DataConsolidate>().First());
                }
                else if (worksheet.Elements<SortState>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SortState>().First());
                }
                else if (worksheet.Elements<AutoFilter>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<AutoFilter>().First());
                }
                else if (worksheet.Elements<Scenarios>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<Scenarios>().First());
                }
                else if (worksheet.Elements<ProtectedRanges>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<ProtectedRanges>().First());
                }
                else if (worksheet.Elements<SheetProtection>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetProtection>().First());
                }
                else if (worksheet.Elements<SheetCalculationProperties>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetCalculationProperties>().First());
                }
                else
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
                }
            }

            // Create the merged cell and append it to the MergeCells collection.
            MergeCell mergeCell = new MergeCell() { Reference = new StringValue(cell1Name + ":" + cell2Name) };
            mergeCells.Append(mergeCell);
            //worksheet.Save();
        }

        // Given a Worksheet and a cell name, verifies that the specified cell exists.
        // If it does not exist, creates a new cell. 
        private static Cell CreateSpreadsheetCellIfNotExist(Worksheet worksheet, string cellName)
        {
            string columnName = GetColumnName(cellName);
            uint rowIndex = GetRowIndexUint(cellName);

            IEnumerable<Row> rows = worksheet.Descendants<Row>().Where(r => r.RowIndex.Value == rowIndex);

            // If the Worksheet does not contain the specified row, create the specified row.
            // Create the specified cell in that row, and insert the row into the Worksheet.
            if (rows.Count() == 0)
            {
                Row row = new Row() { RowIndex = new UInt32Value(rowIndex) };
                Cell cell = new Cell() { CellReference = new StringValue(cellName) };
                row.Append(cell);
                worksheet.Descendants<SheetData>().First().Append(row);
                //worksheet.Save();
                return cell;
            }
            else
            {
                Row row = rows.First();

                IEnumerable<Cell> cells = row.Elements<Cell>().Where(c => c.CellReference.Value == cellName);

                // If the row does not contain the specified cell, create the specified cell.
                if (cells.Count() == 0)
                {
                    Cell cell = new Cell() { CellReference = new StringValue(cellName) };
                    row.Append(cell);
                    //worksheet.Save();
                    return cell;
                }
                else
                {
                    Cell cell = cells.First<Cell>();
                    return cell;
                }
            }
            //return cell;
        }
        private static Cell CreateSpreadsheetCellIfNotExist(Worksheet worksheet, string cellName, uint StyleIndex)
        {
            string columnName = GetColumnName(cellName);
            uint rowIndex = GetRowIndexUint(cellName);

            IEnumerable<Row> rows = worksheet.Descendants<Row>().Where(r => r.RowIndex.Value == rowIndex);

            // If the Worksheet does not contain the specified row, create the specified row.
            // Create the specified cell in that row, and insert the row into the Worksheet.
            if (rows.Count() == 0)
            {
                Row row = new Row() { RowIndex = new UInt32Value(rowIndex) };
                Cell cell = new Cell() { CellReference = new StringValue(cellName) };
                cell.StyleIndex = StyleIndex;
                row.Append(cell);
                worksheet.Descendants<SheetData>().First().Append(row);
                //worksheet.Save();
                return cell;
            }
            else
            {
                Row row = rows.First();

                IEnumerable<Cell> cells = row.Elements<Cell>().Where(c => c.CellReference.Value == cellName);

                // If the row does not contain the specified cell, create the specified cell.
                if (cells.Count() == 0)
                {
                    Cell cell = new Cell() { CellReference = new StringValue(cellName) };
                    cell.StyleIndex = StyleIndex;
                    row.Append(cell);
                    //worksheet.Save();
                    return cell;
                }
                else
                {
                    Cell cell = cells.First<Cell>();
                    return cell;
                }
            }
        }
        #endregion

        #region get row column
        public static string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }
        public static uint GetColumnIndex(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            string name = match.Value.ToUpper();
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return (uint)number;
        }
        public static string GetColumnNameByIndex(uint columnNumber)
        {
            uint dividend = columnNumber;
            string columnName = String.Empty;
            uint modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (uint)((dividend - modulo) / 26);
            }

            return columnName;
        }
        // Given a cell name, parses the specified cell to get the row index.
        private static uint GetRowIndexUint(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }
        #endregion
    }
}
