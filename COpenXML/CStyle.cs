using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace Ciren.COpenXML
{
    public class CStyle
    {
        public string numberFormatCode  { get; set; }
        public double fontSize { get; set; }
        public string fontName { get; set; }
        public string fontColor { get; set; }
        public bool fontBold { get; set; }
        public bool fontItalic { get; set; }
        public PatternValues fillPattern { get; set; }
        public string fillForeGroundColor { get; set; }
        public string borderLeftColor { get; set; }
        public BorderStyleValues borderLeftStyleValue { get; set; }
        public string borderBottomColor { get; set; }
        public BorderStyleValues borderBottomStyleValue { get; set; }
        public string borderRightColor { get; set; }
        public BorderStyleValues borderRightStyleValue { get; set; }
        public string borderTopColor { get; set; }
        public BorderStyleValues borderTopStyleValue { get; set; }
        public HorizontalAlignmentValues alignmentHorizontal { get; set; }
        public VerticalAlignmentValues alignmentVertical { get; set; }
        public bool wrapText { get; set; }
        public uint textRotate { get; set; }

        /// <summary>
        /// ket qua style cua cell sau khi khoi bao doi tuong
        /// </summary>
        public Fill _oFill { get; set; }
        public Border _oBorder { get; set; }
        public Font _oFont { get; set; }
        public Alignment _oAlignment { get; set; }
        public NumberingFormat _oNumberingFormat { get; set; }

        public CStyle() 
        {
            numberFormatCode = null;
            fontSize = 11;
            fontName = "TimeNewRoman";
            fontColor = "000";
            fontBold = false;
            fontItalic = false;
            fillPattern = PatternValues.None;
            fillForeGroundColor = null;
            borderLeftColor = null;
            borderLeftStyleValue = BorderStyleValues.None;
            borderBottomColor = null;
            borderBottomStyleValue = BorderStyleValues.None;
            borderRightColor = null;
            borderRightStyleValue = BorderStyleValues.None;
            borderTopColor = null;
            borderTopStyleValue = BorderStyleValues.None;
            alignmentHorizontal = HorizontalAlignmentValues.Left;
            alignmentVertical = VerticalAlignmentValues.Center;
            wrapText = true;
            textRotate = (uint)0;
        }

        public CStyle(string numberFormatCode, 
            double fontSize, string fontName, string fontColor
            , bool fontBold, bool fontItalic, PatternValues fillPattern, string fillForeGroundColor, string borderLeftColor
            , BorderStyleValues borderLeftStyleValue, string borderBottomColor, BorderStyleValues borderBottomStyleValue, string borderRightColor
            , BorderStyleValues borderRightStyleValue, string borderTopColor, BorderStyleValues borderTopStyleValue, HorizontalAlignmentValues alignmentHorizontal, VerticalAlignmentValues alignmentVertical
            )
        {
            LCreateStyle(numberFormatCode,
            fontSize, fontName, fontColor
            , fontBold, fontItalic, fillPattern, fillForeGroundColor, borderLeftColor
            , borderLeftStyleValue, borderBottomColor, borderBottomStyleValue, borderRightColor
            , borderRightStyleValue, borderTopColor, borderTopStyleValue, alignmentHorizontal, alignmentVertical,textRotate);
        }
        public CStyle(string numberFormatCode,
            double fontSize, string fontName, System.Drawing.Color fontColor
            , bool fontBold, bool fontItalic, PatternValues fillPattern, System.Drawing.Color fillForeGroundColor, System.Drawing.Color borderLeftColor
            , BorderStyleValues borderLeftStyleValue, System.Drawing.Color borderBottomColor, BorderStyleValues borderBottomStyleValue, System.Drawing.Color borderRightColor
            , BorderStyleValues borderRightStyleValue, System.Drawing.Color borderTopColor, BorderStyleValues borderTopStyleValue, HorizontalAlignmentValues alignmentHorizontal, VerticalAlignmentValues alignmentVertical
            )
        {
            LCreateStyle(numberFormatCode,
            fontSize, fontName, fontColor
            , fontBold, fontItalic, fillPattern, fillForeGroundColor, borderLeftColor
            , borderLeftStyleValue, borderBottomColor, borderBottomStyleValue, borderRightColor
            , borderRightStyleValue, borderTopColor, borderTopStyleValue, alignmentHorizontal, alignmentVertical);
        } 
       
        /// <summary>
        /// Thêm mới 1 style và danh sách
        /// </summary>
        public void CreateStyle()
        {
            LCreateStyle(numberFormatCode,
            fontSize, fontName, fontColor
            , fontBold, fontItalic, fillPattern, fillForeGroundColor, borderLeftColor
            , borderLeftStyleValue, borderBottomColor, borderBottomStyleValue, borderRightColor
            , borderRightStyleValue, borderTopColor, borderTopStyleValue, alignmentHorizontal, alignmentVertical,textRotate);
        }

        public void CreateStyle(string numberFormatCode, 
            double fontSize, string fontName, string fontColor
            , bool fontBold, bool fontItalic, PatternValues fillPattern, string fillForeGroundColor, string borderLeftColor
            , BorderStyleValues borderLeftStyleValue, string borderBottomColor, BorderStyleValues borderBottomStyleValue, string borderRightColor
            , BorderStyleValues borderRightStyleValue, string borderTopColor, BorderStyleValues borderTopStyleValue, HorizontalAlignmentValues alignmentHorizontal, VerticalAlignmentValues alignmentVertical)
        {
            LCreateStyle(numberFormatCode, 
            fontSize, fontName, fontColor
            , fontBold, fontItalic, fillPattern, fillForeGroundColor, borderLeftColor
            , borderLeftStyleValue, borderBottomColor, borderBottomStyleValue, borderRightColor
            , borderRightStyleValue, borderTopColor, borderTopStyleValue, alignmentHorizontal, alignmentVertical, textRotate);
        }

        /// <summary>
        /// Gọi mặc định khi create style
        /// </summary>
        /// <param name="numberFormatCode"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontName"></param>
        /// <param name="fontColor"></param>
        /// <param name="fontBold"></param>
        /// <param name="fontItalic"></param>
        /// <param name="fillPattern"></param>
        /// <param name="fillForeGroundColor"></param>
        /// <param name="borderLeftColor"></param>
        /// <param name="borderLeftStyleValue"></param>
        /// <param name="borderBottomColor"></param>
        /// <param name="borderBottomStyleValue"></param>
        /// <param name="borderRightColor"></param>
        /// <param name="borderRightStyleValue"></param>
        /// <param name="borderTopColor"></param>
        /// <param name="borderTopStyleValue"></param>
        /// <param name="alignmentHorizontal"></param>
        /// <param name="alignmentVertical"></param>
        /// <param name="textRotate"></param>
        private void LCreateStyle(string numberFormatCode,
            double fontSize, string fontName, string fontColor
            , bool fontBold, bool fontItalic, PatternValues fillPattern, string fillForeGroundColor, string borderLeftColor
            , BorderStyleValues borderLeftStyleValue, string borderBottomColor, BorderStyleValues borderBottomStyleValue, string borderRightColor
            , BorderStyleValues borderRightStyleValue, string borderTopColor, BorderStyleValues borderTopStyleValue, HorizontalAlignmentValues alignmentHorizontal, VerticalAlignmentValues alignmentVertical,
            uint textRotate)
        {
            if (numberFormatCode != null)
            {
                NumberingFormat numberingFormat = new NumberingFormat();
                numberingFormat.FormatCode = numberFormatCode;
                _oNumberingFormat  = numberingFormat;
            }
            //font
            Font font = new Font();
            FontSize _fontSize = new FontSize() { Val = DoubleValue.FromDouble(fontSize) };
            font.Append(_fontSize);
            //text rotate
            //if (textRotate!=null)
            //{
            //    Alignment align = new Alignment();
            //    align.TextRotation.Value = textRotate;
            //    CellFormat format =new CellFormat(){Alignment= align};
            //}
           
          

            if (fontName != null)
            {
                FontName _fontName = new FontName() { Val = fontName };
                font.Append(_fontName);
            }
            if (fontColor != null)
            {
                //string color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Black);
                Color _fontColor = new Color() { Rgb = new HexBinaryValue() { Value = fontColor } };
                font.Append(_fontColor);
            }
            if (fontBold)
            {
                Bold _fontBold = new Bold();
                font.Append(_fontBold);
            }
            if (fontItalic)
            {
                Italic _fontItalic = new Italic();
                font.Append(_fontItalic);
            }
            _oFont = font;

            //fill
            Fill _fontFill = null;

            if (fillForeGroundColor == null)
            {
                _fontFill = new Fill(new PatternFill() { PatternType = fillPattern });
            }
            else
            {
                _fontFill = new Fill(new PatternFill(
                    new ForegroundColor() { Rgb = new HexBinaryValue() { Value = fillForeGroundColor } }
                    ) { PatternType = PatternValues.Solid });
            }

            _oFill = _fontFill;

            //border
            Border _border = new Border();
            LeftBorder leftBorder = new LeftBorder() { Style = borderLeftStyleValue };

            Color colorLeftBorder = null;
            if (borderLeftColor != null)
            {
                colorLeftBorder = new Color() { Rgb = new HexBinaryValue { Value = borderLeftColor } };
            }
            else { colorLeftBorder = new Color(); }
            leftBorder.Append(colorLeftBorder);
            _border.Append(leftBorder);

            RightBorder rightBorder = new RightBorder() { Style = borderRightStyleValue };

            Color colorRightBorder = null;
            if (borderRightColor != null)
            {
                colorRightBorder = new Color() { Rgb = new HexBinaryValue { Value = borderRightColor } };
            }
            else { colorRightBorder = new Color(); }
            rightBorder.Append(colorRightBorder);
            _border.Append(rightBorder);

            TopBorder topBorder = new TopBorder() { Style = borderTopStyleValue };

            Color colorTopBorder = null;
            if (borderTopColor != null)
            {
                colorTopBorder = new Color() { Rgb = new HexBinaryValue { Value = borderTopColor } };
            }
            else { colorTopBorder = new Color(); }
            topBorder.Append(colorTopBorder);
            _border.Append(topBorder);

            BottomBorder bottomBorder = new BottomBorder() { Style = borderBottomStyleValue };

            Color colorBottomBorder = null;
            if (borderBottomColor != null)
            {
                colorBottomBorder = new Color() { Rgb = new HexBinaryValue { Value = borderBottomColor } };
            }
            else { colorBottomBorder = new Color(); }
            bottomBorder.Append(colorBottomBorder);
            _border.Append(bottomBorder);
            _oBorder = _border;

            Alignment _alignment = new Alignment();
            _alignment.Horizontal = alignmentHorizontal;
            _alignment.Vertical = alignmentVertical;
            
            _alignment.WrapText = wrapText;
            
            //if (textRotate != 0)
            //{
            //    _oAlignment.TextRotation =  (UInt32Value)90U;
            //}
            _oAlignment = _alignment;
            _oAlignment.TextRotation = textRotate;
        }

        private void LCreateStyle(string numberFormatCode,
            double fontSize, string fontName, System.Drawing.Color fontColor
            , bool fontBold, bool fontItalic, PatternValues fillPattern, System.Drawing.Color fillForeGroundColor, System.Drawing.Color borderLeftColor
            , BorderStyleValues borderLeftStyleValue, System.Drawing.Color borderBottomColor, BorderStyleValues borderBottomStyleValue, System.Drawing.Color borderRightColor
            , BorderStyleValues borderRightStyleValue, System.Drawing.Color borderTopColor, BorderStyleValues borderTopStyleValue, HorizontalAlignmentValues alignmentHorizontal, VerticalAlignmentValues alignmentVertical)
        {
            if (numberFormatCode != null)
            {
                NumberingFormat numberingFormat = new NumberingFormat();
                numberingFormat.FormatCode = numberFormatCode;
                _oNumberingFormat = numberingFormat;
            }
            //font
            Font font = new Font();
            FontSize _fontSize = new FontSize() { Val = DoubleValue.FromDouble(fontSize) };
            font.Append(_fontSize);

            if (fontName != null)
            {
                FontName _fontName = new FontName() { Val = fontName };
                font.Append(_fontName);
            }
            if (fontColor != null)
            {
                Color _fontColor = new Color() { Rgb = new HexBinaryValue() { Value = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(fontColor.R, fontColor.G, fontColor.B)).Replace("#", "") } };
                font.Append(_fontColor);
            }
            if (fontBold)
            {
                Bold _fontBold = new Bold();
                font.Append(_fontBold);
            }
            if (fontItalic)
            {
                Italic _fontItalic = new Italic();
                font.Append(_fontItalic);
            }
            _oFont = font;

            //fill
            Fill _fontFill = null;

            if (fillForeGroundColor == null)
            {
                _fontFill = new Fill(new PatternFill() { PatternType = fillPattern });
            }
            else
            {
                _fontFill = new Fill(new PatternFill(
                    new ForegroundColor() { Rgb = new HexBinaryValue() { Value = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(fillForeGroundColor.R, fillForeGroundColor.G, fillForeGroundColor.B)).Replace("#", "") } }
                    ) { PatternType = PatternValues.Solid });
            }

            _oFill = _fontFill;

            //border
            Border _border = new Border();
            LeftBorder leftBorder = new LeftBorder() { Style = borderLeftStyleValue };

            Color colorLeftBorder = null;
            if (borderLeftColor != null)
            {
                colorLeftBorder = new Color() { Rgb = new HexBinaryValue { Value = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(borderLeftColor.R, borderLeftColor.G, borderLeftColor.B)).Replace("#","") } };
            }
            else { colorLeftBorder = new Color(); }
            leftBorder.Append(colorLeftBorder);
            _border.Append(leftBorder);

            RightBorder rightBorder = new RightBorder() { Style = borderRightStyleValue };

            Color colorRightBorder = null;
            if (borderRightColor != null)
            {
                colorRightBorder = new Color() { Rgb = new HexBinaryValue { Value = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(borderRightColor.R, borderRightColor.G, borderRightColor.B)).Replace("#", "") } };
            }
            else { colorRightBorder = new Color(); }
            rightBorder.Append(colorRightBorder);
            _border.Append(rightBorder);

            TopBorder topBorder = new TopBorder() { Style = borderTopStyleValue };

            Color colorTopBorder = null;
            if (borderTopColor != null)
            {
                colorTopBorder = new Color() { Rgb = new HexBinaryValue { Value = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(borderTopColor.R, borderTopColor.G, borderTopColor.B)).Replace("#", "") } };
            }
            else { colorTopBorder = new Color(); }
            topBorder.Append(colorTopBorder);
            _border.Append(topBorder);

            BottomBorder bottomBorder = new BottomBorder() { Style = borderBottomStyleValue };

            Color colorBottomBorder = null;
            if (borderBottomColor != null)
            {
                colorBottomBorder = new Color() { Rgb = new HexBinaryValue { Value = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(borderBottomColor.R, borderBottomColor.G, borderBottomColor.B)).Replace("#", "") } };
            }
            else { colorBottomBorder = new Color(); }
            bottomBorder.Append(colorBottomBorder);
            _border.Append(bottomBorder);
            _oBorder = _border;

            Alignment _alignment = new Alignment();
            _alignment.Horizontal = alignmentHorizontal;
            _alignment.Vertical = alignmentVertical;
            _oAlignment = _alignment;
            _oAlignment.WrapText = wrapText;
            _oAlignment.TextRotation.Value = textRotate;
        }

        /// <summary>
        /// Khi thêm mới 1 workbook, luôn khởi tạo 01 style mặc định
        /// </summary>
        /// <returns></returns>
        public static Stylesheet GenerateStyleSheet()
        {
            Stylesheet stylesheet = new Stylesheet();
            //default numberingFormat
            stylesheet.NumberingFormats = new NumberingFormats();
            stylesheet.NumberingFormats.Count = 1;
            stylesheet.NumberingFormats.AppendChild(new NumberingFormat { NumberFormatId = 0, FormatCode = StringValue.FromString("#,##0") });

            //blank fonts
            stylesheet.Fonts = new Fonts();
            stylesheet.Fonts.Count = 1;
            stylesheet.Fonts.AppendChild(new Font());

            //blank fills
            stylesheet.Fills = new Fills();
            stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.None } }); // required, reserved by Excel
            stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.Gray125 } }); // required, reserved by Excel
            stylesheet.Fills.Count = 2;

            // blank border list
            stylesheet.Borders = new Borders();
            stylesheet.Borders.Count = 1;
            stylesheet.Borders.AppendChild(new Border());

            // blank cell format list
            stylesheet.CellStyleFormats = new CellStyleFormats();
            stylesheet.CellStyleFormats.Count = 1;
            stylesheet.CellStyleFormats.AppendChild(new CellFormat());

            // cell format list
            stylesheet.CellFormats = new CellFormats();
            // empty one for index 0, seems to be required
            stylesheet.CellFormats.AppendChild(new CellFormat());
            stylesheet.CellFormats.Count = 1;

            return stylesheet;
        }

        public CellFormat GetCellFormat(uint styleIndex, WorkbookPart workbookPart)
        {
            return workbookPart.WorkbookStylesPart.Stylesheet.Elements<CellFormats>().First().Elements<CellFormat>().ElementAt((int)styleIndex);
        }

        
        /// <summary>
        /// Xây dựng nội dung format của cell
        /// </summary>
        /// <param name="cellFormat"></param>
        /// <param name="workbookPart"></param>
        public void AppendCellFormat(CellFormat cellFormat, WorkbookPart workbookPart)
        {
            cellFormat.FillId = InsertFill(_oFill, workbookPart);
            cellFormat.BorderId = InsertBorder(_oBorder, workbookPart);
            cellFormat.FontId = InsertFont(_oFont, workbookPart);
            cellFormat.Alignment = _oAlignment;

            if (_oNumberingFormat != null)
            {
                
                cellFormat.NumberFormatId = InsertNumberingFormat(_oNumberingFormat, workbookPart);
            }
        }

        #region xây dựng các thành phần style của cellFormat
        public void SetBorderStyle(BorderStyleValues BorderStyle)
        {
            borderLeftStyleValue = BorderStyle;
            borderBottomStyleValue = BorderStyle;
            borderRightStyleValue = BorderStyle;
            borderTopStyleValue = BorderStyle;
        }
        public void SetBorderStyle(BorderStyleValues borderLeftStyleValue, BorderStyleValues borderBottomStyleValue, BorderStyleValues borderRightStyleValue, BorderStyleValues borderTopStyleValue)
        {
            this.borderLeftStyleValue = borderLeftStyleValue;
            this.borderBottomStyleValue = borderBottomStyleValue;
            this.borderRightStyleValue = borderRightStyleValue;
            this.borderTopStyleValue = borderTopStyleValue;
        }        
        public void SetBorderColor(System.Drawing.Color BorderColor)
        {
            borderLeftColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(BorderColor.R, BorderColor.G, BorderColor.B)).Replace("#", "");
            borderBottomColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(BorderColor.R, BorderColor.G, BorderColor.B)).Replace("#", "");
            borderRightColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(BorderColor.R, BorderColor.G, BorderColor.B)).Replace("#", "");
            borderTopColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(BorderColor.R, BorderColor.G, BorderColor.B)).Replace("#", "");
        }
        public void SetBorderColor(System.Drawing.Color borderLeftColor, System.Drawing.Color borderBottomColor, System.Drawing.Color borderRightColor, System.Drawing.Color borderTopColor)
        {
            this.borderLeftColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(borderLeftColor.R, borderLeftColor.G, borderLeftColor.B)).Replace("#", "");
            this.borderBottomColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(borderBottomColor.R, borderBottomColor.G, borderBottomColor.B)).Replace("#", "");
            this.borderRightColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(borderRightColor.R, borderRightColor.G, borderRightColor.B)).Replace("#", "");
            this.borderTopColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(borderTopColor.R, borderTopColor.G, borderTopColor.B)).Replace("#", "");
        }
        public void SetBorderColor(string BorderColor)
        {
            borderLeftColor = BorderColor;
            borderBottomColor = BorderColor;
            borderRightColor = BorderColor;
            borderTopColor = BorderColor;
        }
        public void SetBorderColor(string borderLeftColor, string borderBottomColor, string borderRightColor, string borderTopColor)
        {
            this.borderLeftColor = borderLeftColor;
            this.borderBottomColor = borderBottomColor;
            this.borderRightColor = borderRightColor;
            this.borderTopColor = borderTopColor;
        }
        public void SetFont(double fontSize, string fontName, string fontColor, bool fontBold, bool fontItalic)
        {
            this.fontSize = fontSize;
            this.fontName = fontName;
            this.fontColor = fontColor;
            this.fontBold = fontBold;
            this.fontItalic = fontItalic;
        }
        public void SetFont(double fontSize, string fontName, System.Drawing.Color fontColor, bool fontBold, bool fontItalic)
        {
            this.fontSize = fontSize;
            this.fontName = fontName;
            this.fontColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(fontColor.R, fontColor.G, fontColor.B)).Replace("#", "");
            this.fontBold = fontBold;
            this.fontItalic = fontItalic;
        }
        public void SetFontColor(System.Drawing.Color fontColor)
        {
            this.fontColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(fontColor.R, fontColor.G, fontColor.B)).Replace("#", "");
        }
        public void SetFillForeGroundColor(System.Drawing.Color fillForeGroundColor)
        {
            this.fillForeGroundColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(fillForeGroundColor.R, fillForeGroundColor.G, fillForeGroundColor.B)).Replace("#", "");
        }

        public uint InsertBorder(Border border, WorkbookPart workbookPart)
        {
            Borders borders = workbookPart.WorkbookStylesPart.Stylesheet.Elements<Borders>().First();
            borders.Append(border);
            return (uint)borders.Count++;
        }

        public uint InsertFill(Fill fill, WorkbookPart workbookPart)
        {
            Fills fills = workbookPart.WorkbookStylesPart.Stylesheet.Elements<Fills>().First();
            fills.Append(fill);
            return (uint)fills.Count++;
        }

        public uint InsertFont(Font font, WorkbookPart workbookPart)
        {
            Fonts fonts = workbookPart.WorkbookStylesPart.Stylesheet.Elements<Fonts>().First();
            fonts.Append(font);
            return (uint)fonts.Count++;
        }

        /// <summary>
        /// thêm mới format dạng số
        /// </summary>
        /// <param name="numberingFormat"></param>
        /// <param name="workbookPart"></param>
        /// <returns></returns>
        public uint InsertNumberingFormat(NumberingFormat numberingFormat, WorkbookPart workbookPart)
        {
            NumberingFormats numberingFormats = workbookPart.WorkbookStylesPart.Stylesheet.Elements<NumberingFormats>().First();
            //cập nhật id cho numberingFormat
            numberingFormat.NumberFormatId = numberingFormats.Count;
            numberingFormats.Append(numberingFormat);
            return (uint)numberingFormats.Count++;
        }

        public uint InsertCellFormat(CellFormat cellFormat, WorkbookPart workbookPart)
        {
            CellFormats cellFormats = workbookPart.WorkbookStylesPart.Stylesheet.Elements<CellFormats>().First();
            cellFormats.Append(cellFormat);
            return (uint)cellFormats.Count++;
        }


        #endregion


        public uint GetStyleIndex(WorkbookPart _workbookPart, Cell cell)
        {
            CellFormat cellFormat = cell.StyleIndex != null ? this.GetCellFormat(cell.StyleIndex, _workbookPart).CloneNode(true) as CellFormat : new CellFormat();
            this.AppendCellFormat(cellFormat, _workbookPart);
            uint styleIndex = this.InsertCellFormat(cellFormat, _workbookPart);
            return styleIndex;
        }

        /// <summary>
        /// Thêm mới style cho cell vào danh sách style. 
        /// </summary>
        /// <param name="_workbookPart"></param>
        /// <returns>styleIndex vừa thêm mới</returns>
        public uint GetStyleIndex(WorkbookPart _workbookPart)
        {
            CellFormat cellFormat = new CellFormat();
            this.AppendCellFormat(cellFormat, _workbookPart);
            uint styleIndex = this.InsertCellFormat(cellFormat, _workbookPart);
            return styleIndex;
        }
    }
}
