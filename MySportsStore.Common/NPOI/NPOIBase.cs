using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;


namespace MySportsStore.Common
{
    public class NPOIBase : ActionResult
    {
        public IWorkbook _workbook { get; set; }
        public ISheet _sheet { get; set; }
        public ICellStyle _titleStyle { get; set; }
        public ICellStyle _leftStyle { get; set; }
        public ICellStyle _centerStyle { get; set; }
        public ICellStyle _rightStyle { get; set; }
        public ICellStyle _headStyle { get; set; }
        public ICellStyle _leftborderStyle { get; set; }
        public ICellStyle _rightborderStyle { get; set; }
        public ICellStyle _noneRightBorderStyle { get; set; }
        public ICellStyle _noneLeftBorderStyle { get; set; }
        public ICellStyle _noneLeftAndRightBorderStyle { get; set; }
        public ICellStyle _borderStyle { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
        }
        public void IniNPOI(int postfix=0,bool isHeadBorder = false, string sheetName = "")
        {
            if (postfix == 0)
            {
                _workbook = new HSSFWorkbook();//这里能不能调整为XSSF
            }
            else if (postfix == 1)
            {
                _workbook = new XSSFWorkbook();
            }

            _sheet = string.IsNullOrWhiteSpace(sheetName) ? _workbook.CreateSheet() : _workbook.CreateSheet(sheetName);
            IniStyle(isHeadBorder);
        }
        public void IniStyle(bool isHeadBorder = false)
        {
            IFont font12 = _workbook.CreateFont();
            font12.FontHeightInPoints = 12;
            font12.Boldweight = 700;

            _titleStyle = _workbook.CreateCellStyle();
            _titleStyle.Alignment = HorizontalAlignment.Center;
            _titleStyle.VerticalAlignment = VerticalAlignment.Top;
            _titleStyle.SetFont(font12);

            _leftStyle = _workbook.CreateCellStyle();
            _leftStyle.Alignment = HorizontalAlignment.Left;
            _leftStyle.VerticalAlignment = VerticalAlignment.Top;

            _centerStyle = _workbook.CreateCellStyle();
            _centerStyle.Alignment = HorizontalAlignment.Center;
            _centerStyle.VerticalAlignment = VerticalAlignment.Top;

            _rightStyle = _workbook.CreateCellStyle();
            _rightStyle.Alignment = HorizontalAlignment.Right;
            _rightStyle.VerticalAlignment = VerticalAlignment.Top;

            _headStyle = _workbook.CreateCellStyle();
            _headStyle.Alignment = HorizontalAlignment.Center;
            _headStyle.VerticalAlignment = VerticalAlignment.Top;
            if (isHeadBorder)
            {
                _headStyle.BorderBottom = BorderStyle.Thin;
                _headStyle.BorderLeft = BorderStyle.Thin;
                _headStyle.BorderRight = BorderStyle.Thin;
                _headStyle.BorderTop = BorderStyle.Thin;
            }
            _leftborderStyle = _workbook.CreateCellStyle();
            _leftborderStyle.Alignment = HorizontalAlignment.Left;
            _leftborderStyle.VerticalAlignment = VerticalAlignment.Top;
            _leftborderStyle.BorderBottom = BorderStyle.Thin;
            _leftborderStyle.BorderLeft = BorderStyle.Thin;
            _leftborderStyle.BorderRight = BorderStyle.Thin;
            _leftborderStyle.BorderTop = BorderStyle.Thin;

            _rightborderStyle = _workbook.CreateCellStyle();
            _rightborderStyle.Alignment = HorizontalAlignment.Right;
            _rightborderStyle.VerticalAlignment = VerticalAlignment.Top;
            _rightborderStyle.BorderBottom = BorderStyle.Thin;
            _rightborderStyle.BorderLeft = BorderStyle.Thin;
            _rightborderStyle.BorderRight = BorderStyle.Thin;
            _rightborderStyle.BorderTop = BorderStyle.Thin;

            _noneRightBorderStyle = _workbook.CreateCellStyle();
            _noneRightBorderStyle.Alignment = HorizontalAlignment.Left;
            _noneRightBorderStyle.VerticalAlignment = VerticalAlignment.Top;
            _noneRightBorderStyle.BorderBottom = BorderStyle.Thin;
            _noneRightBorderStyle.BorderLeft = BorderStyle.Thin;
            _noneRightBorderStyle.BorderTop = BorderStyle.Thin;

            _noneLeftBorderStyle = _workbook.CreateCellStyle();
            _noneLeftBorderStyle.Alignment = HorizontalAlignment.Right;
            _noneLeftBorderStyle.VerticalAlignment = VerticalAlignment.Top;
            _noneLeftBorderStyle.BorderBottom = BorderStyle.Thin;
            _noneLeftBorderStyle.BorderRight = BorderStyle.Thin;
            _noneLeftBorderStyle.BorderTop = BorderStyle.Thin;

            _noneLeftAndRightBorderStyle = _workbook.CreateCellStyle();
            _noneLeftAndRightBorderStyle.Alignment = HorizontalAlignment.Center;
            _noneLeftAndRightBorderStyle.VerticalAlignment = VerticalAlignment.Top;
            _noneLeftAndRightBorderStyle.BorderBottom = BorderStyle.Thin;
            _noneLeftAndRightBorderStyle.BorderTop = BorderStyle.Thin;

            _borderStyle = _workbook.CreateCellStyle();
            _borderStyle.Alignment = HorizontalAlignment.Center;
            _borderStyle.VerticalAlignment = VerticalAlignment.Top;
            _borderStyle.BorderBottom = BorderStyle.Thin;
            _borderStyle.BorderLeft = BorderStyle.Thin;
            _borderStyle.BorderRight = BorderStyle.Thin;
            _borderStyle.BorderTop = BorderStyle.Thin;

            IFont font = _workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            _headStyle.SetFont(font);
        }
        public void FillHeadCell(IRow row, int colIndex, string value, ICellStyle cellStyle = null, NPOI.SS.Util.CellRangeAddress mergedCellRangeAddress = null)
        {
            if (_sheet == null || row == null) return;
            if (cellStyle == null) cellStyle = _headStyle;
            FillCell(row, colIndex, value, cellStyle, mergedCellRangeAddress);
            _sheet.SetColumnWidth(colIndex, (Encoding.Default.GetBytes(value.Trim()).Length + 4) * 256);
        }
        public void FillCell(IRow row, int colIndex, string value, ICellStyle cellStyle = null, NPOI.SS.Util.CellRangeAddress mergedCellRangeAddress = null)
        {
            if (_sheet == null || row == null) return;
            ICell titleSum = row.CreateCell(colIndex);
            titleSum.SetCellValue(value);
            if (cellStyle != null) titleSum.CellStyle = cellStyle;
            else if (_centerStyle != null) titleSum.CellStyle = _centerStyle;
            if (mergedCellRangeAddress != null) _sheet.AddMergedRegion(mergedCellRangeAddress);
        }
        public void FillCell(IRow row, int colIndex, double value, ICellStyle cellStyle = null, NPOI.SS.Util.CellRangeAddress mergedCellRangeAddress = null)
        {
            if (_sheet == null || row == null) return;
            ICell titleSum = row.CreateCell(colIndex);
            titleSum.SetCellValue(value);
            if (cellStyle != null) titleSum.CellStyle = cellStyle;
            else if (_centerStyle != null) titleSum.CellStyle = _centerStyle;
            if (mergedCellRangeAddress != null) _sheet.AddMergedRegion(mergedCellRangeAddress);
        }
        public void ResponseOutPutExcelStream(string fildName, int postfix=0)
        {
            if (postfix == 0)
            {
                if (string.IsNullOrWhiteSpace(fildName)) fildName = DateTime.Now.ToString("yyyyMMddHHmmss.xls");
                //后缀没有.xls 或者
                if (fildName.ToLower().IndexOf(".xls") == -1 || fildName.ToLower().IndexOf(".xls") != fildName.Length-4) fildName += ".xls";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fildName));
                HttpContext.Current.Response.Clear();

                MemoryStream file = new MemoryStream();
                _workbook.Write(file);
                file.WriteTo(HttpContext.Current.Response.OutputStream);

                HttpContext.Current.Response.End();
            }
            else if (postfix == 1)
            {
                if (string.IsNullOrWhiteSpace(fildName)) fildName = DateTime.Now.ToString("yyyyMMddHHmmss.xlsx");
                if (fildName.ToLower().IndexOf(".xlsx") == -1) fildName += ".xlsx";
                //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fildName));
                HttpContext.Current.Response.Clear();

                MemoryStream file = new MemoryStream();
                _workbook.Write(file);
                //file.WriteTo(HttpContext.Current.Response.OutputStream);

                HttpContext.Current.Response.BinaryWrite(file.ToArray());

                HttpContext.Current.Response.Flush();

                HttpContext.Current.Response.End();
                
            }
        }
        public void SetPrint(bool isLandscape = false, bool isFitToPage = false, double topMargin = 0, double rightMargin = 0, double bottomMargin = 0.5, double leftMargin = 0, short scale = 100)
        {
            _sheet.PrintSetup.Landscape = isLandscape;
            _sheet.SetMargin(MarginType.TopMargin, topMargin);
            _sheet.SetMargin(MarginType.RightMargin, rightMargin);
            _sheet.SetMargin(MarginType.LeftMargin, leftMargin);
            _sheet.SetMargin(MarginType.BottomMargin, bottomMargin);
            _sheet.PrintSetup.PaperSize = 9;
            _sheet.PrintSetup.Scale = scale;
            _sheet.FitToPage = isFitToPage;
            if (isFitToPage)
            {
                _sheet.PrintSetup.FitWidth = 1;
                _sheet.PrintSetup.FitHeight = 0;
            }
        }
    }


}
