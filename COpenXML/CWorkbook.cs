using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Ciren.COpenXML
{
    public class CWorkbook
    {
        //variables
        public SpreadsheetDocument _spreadsheetDocument;
        public WorkbookPart _workbookPart;
        public Workbook _workbook;
        public SharedStringTablePart _sharedStringTablePart;
        public Sheets _sheets;

        /// <summary>
        /// Khoi tao file excel khong chua bat ky sheet nao
        /// </summary>
        /// <param name="spreadsheetDocument"></param>
        public CWorkbook(SpreadsheetDocument spreadsheetDocument)
        {
            _spreadsheetDocument = spreadsheetDocument;
            //create workbook
            _workbookPart = _spreadsheetDocument.AddWorkbookPart();
            _workbook = _spreadsheetDocument.WorkbookPart.Workbook = new Workbook();

            _sharedStringTablePart = SharedStringTPGet(_workbookPart);
            _sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            
            //tao init style: cac the dau tien phuc vu add style. Ca file excel chi co mot tap hop the style
            WorkbookStylesPart stylesPart = _workbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = CStyle.GenerateStyleSheet();
            stylesPart.Stylesheet.Save();
        }

        #region private function
        private SharedStringTablePart SharedStringTPGet(WorkbookPart workbookPart)
        {
            SharedStringTablePart shareStringPart;
            if (workbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = workbookPart.AddNewPart<SharedStringTablePart>();
            }
            return shareStringPart;
        }
        #endregion
    }
}
