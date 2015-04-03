namespace ZhongDing.Common.NPOIHelper.Excel
{
    using MyNPOI.Helpers;
    using NPOI.HPSF;
    using NPOI.HSSF.Record;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public class NPOIHelper
    {
        private bool cellColorBug;
        private HSSFCellStyle cellStyle;
        private bool IsOnlyTableHead;
        private string outFileName;
        private List<IRow> rowList;
        private ISheet sheet;
        private HSSFWorkbook workbook;
        private HSSFPalette XlPalette;

        public NPOIHelper()
        {
            this.IsOnlyTableHead = false;
            this.outFileName = string.Empty;
            this.XlPalette = null;
            this.workbook = null;
            this.sheet = null;
            this.cellStyle = null;
            this.rowList = new List<IRow>();
            this.cellColorBug = true;
        }

        public NPOIHelper(string outfilename, bool isHeadOnly)
        {
            this.IsOnlyTableHead = false;
            this.outFileName = string.Empty;
            this.XlPalette = null;
            this.workbook = null;
            this.sheet = null;
            this.cellStyle = null;
            this.rowList = new List<IRow>();
            this.cellColorBug = true;
            if (outfilename != null)
            {
                this.outFileName = outfilename;
            }
            this.IsOnlyTableHead = isHeadOnly;
        }

        public NPOIHelper(string outfilename, string sheetName)
        {
            this.IsOnlyTableHead = false;
            this.outFileName = string.Empty;
            this.XlPalette = null;
            this.workbook = null;
            this.sheet = null;
            this.cellStyle = null;
            this.rowList = new List<IRow>();
            this.cellColorBug = true;
            this.outFileName = outfilename;
            this.sheet = this.Workbook.CreateSheet(sheetName);
        }

        public MemoryStream Export(DataTable dtSource)
        {
            HSSFCellStyle style = (HSSFCellStyle)this.Workbook.CreateCellStyle();
            this.cellStyle = this.SetContentFormat(600, 10);
            style.DataFormat = ((HSSFDataFormat)this.Workbook.CreateDataFormat()).GetFormat("yyyy-mm-dd");
            int[] numArray = new int[dtSource.Columns.Count];
            foreach (DataColumn column in dtSource.Columns)
            {
                numArray[column.Ordinal] = Encoding.GetEncoding(0x3a8).GetBytes(column.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int length = Encoding.GetEncoding(0x3a8).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (length > numArray[j])
                    {
                        numArray[j] = length;
                    }
                }
            }
            int rownum = this.rowList.Count<IRow>();
            dtSource.Rows.RemoveAt(0);
            foreach (DataRow row in dtSource.Rows)
            {
                IRow row2 = this.sheet.CreateRow(rownum);
                foreach (DataColumn column2 in dtSource.Columns)
                {
                    ICell cell = row2.CreateCell(column2.Ordinal);
                    cell.CellStyle = this.cellStyle;
                    string str = row[column2].ToString();
                    switch (column2.DataType.ToString())
                    {
                        case "System.String":
                            cell.SetCellValue(str);
                            break;

                        case "System.DateTime":
                            DateTime time;
                            DateTime.TryParse(str, out time);
                            cell.SetCellValue(time);
                            cell.CellStyle = style;
                            break;

                        case "System.Boolean":
                            {
                                bool result = false;
                                bool.TryParse(str, out result);
                                cell.SetCellValue(result);
                                break;
                            }
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            {
                                int num5 = 0;
                                int.TryParse(str, out num5);
                                cell.SetCellValue((double)num5);
                                break;
                            }
                        case "System.Decimal":
                        case "System.Double":
                            {
                                double num6 = 0.0;
                                double.TryParse(str, out num6);
                                cell.SetCellValue(num6);
                                break;
                            }
                        case "System.DBNull":
                            cell.SetCellValue("");
                            break;

                        default:
                            cell.SetCellValue("");
                            break;
                    }
                }
                rownum++;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                this.Workbook.Write(stream);
                stream.Flush();
                stream.Position = 0L;
                return stream;
            }
        }

        public void ExportToExcel<T>(List<T> list, string fileName, string[] titles, string headJson, params Func<T, string>[] fieldFuncs)
        {
            this.outFileName = fileName;
            this.SetHead(headJson);
            DataTable dtSource = list.ToDataTable<T>(titles, fieldFuncs);
            this.MemExport(dtSource);
        }
        public void ExportToExcel<T>(List<T> list, string fileName, string[] titles, Root head, params Func<T, string>[] fieldFuncs)
        {
            this.outFileName = fileName;
            this.SetHead(head);
            DataTable dtSource = list.ToDataTable<T>(titles, fieldFuncs);
            this.MemExport(dtSource);
        }
        public short GetColorIndex(HSSFWorkbook workbook, string color)
        {
            Color systemColour = ColorTranslator.FromHtml(color);
            return this.GetXLColour(workbook, systemColour);
        }

        private short GetXLColour(HSSFWorkbook workbook, Color SystemColour)
        {
            short num = 0;
            HSSFColor color = this.XlPalette.FindColor(SystemColour.R, SystemColour.G, SystemColour.B);

            //if (color == null)
            //{
            //    if (PaletteRecord.STANDARD_PALETTE_SIZE >= 0xff)
            //    {
            //        return num;
            //    }
            //    if (PaletteRecord.STANDARD_PALETTE_SIZE < 0x40)
            //    {
            //        PaletteRecord.STANDARD_PALETTE_SIZE = 0x40;
            //        PaletteRecord.STANDARD_PALETTE_SIZE = (byte) (PaletteRecord.STANDARD_PALETTE_SIZE + 1);
            //        color = this.XlPalette.AddColor(SystemColour.R, SystemColour.G, SystemColour.B);
            //    }
            //    else
            //    {
            //        color = this.XlPalette.FindSimilarColor(SystemColour.R, SystemColour.G, SystemColour.B);
            //    }
            //    return color.GetIndex();
            //}
            //return color.GetIndex();
            return color.Indexed;
        }

        public void MemExport(DataTable dtSource)
        {
            using (MemoryStream stream = this.Export(dtSource))
            {
                using (FileStream stream2 = new FileStream(this.outFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = stream.ToArray();
                    stream2.Write(buffer, 0, buffer.Length);
                    stream2.Flush();
                }
            }
        }

        private HSSFCellStyle SetCellStyle(AttributeList al)
        {
            HSSFCellStyle style = (HSSFCellStyle)this.Workbook.CreateCellStyle();
            this.XlPalette = this.Workbook.GetCustomPalette();
            style.Alignment = string.IsNullOrEmpty(al.align) ? HorizontalAlignment.Center : al.align.ToHorAlign();
            style.VerticalAlignment = string.IsNullOrEmpty(al.valign) ? VerticalAlignment.Center : al.valign.ToVerAlign();
            style.FillPattern = FillPattern.SolidForeground;// FillPatternType.SOLID_FOREGROUND;
            if (string.IsNullOrEmpty(al.bgcolor))
            {
                style.FillForegroundColor = HSSFColor.Grey25Percent.Index;// HSSFColor.GREY_25_PERCENT.index;
            }
            else
            {
                style.FillForegroundColor = this.GetColorIndex(this.Workbook, al.bgcolor);
                if (this.cellColorBug)
                {
                    Color color = ColorTranslator.FromHtml(al.bgcolor);
                    this.XlPalette.SetColorAtIndex(HSSFColor.Pink.Index, color.R, color.G, color.B);
                    style.FillForegroundColor = HSSFColor.Pink.Index;
                    this.cellColorBug = false;
                }
            }
            HSSFFont font = (HSSFFont)this.Workbook.CreateFont();
            font.Color = string.IsNullOrEmpty(al.fontcolor) ? ((short)8) : this.GetColorIndex(this.Workbook, al.fontcolor);
            short? fontsize = al.fontsize;
            font.FontHeightInPoints = fontsize.HasValue ? fontsize.GetValueOrDefault() : ((short)12);
            font.Boldweight = 400;
            font.FontName = string.IsNullOrWhiteSpace(al.fontName) ? "黑体" : al.fontName;
            font.IsItalic = al.IsItalic.HasValue && al.IsItalic.Value;
            font.IsStrikeout = al.IsStrikeout.HasValue && al.IsStrikeout.Value;
            font.Underline = (FontUnderlineType)((al.Underline.HasValue && al.Underline.Value) ? ((byte)1) : ((byte)0));
            style.SetFont(font);
            return style;
        }

        public HSSFCellStyle SetContentFormat(short fontweight = 600, short fontsize = 10)
        {
            this.cellStyle = (HSSFCellStyle)this.Workbook.CreateCellStyle();
            this.cellStyle.Alignment = HorizontalAlignment.Center;
            IFont font = this.Workbook.CreateFont();
            font.FontHeightInPoints = fontsize;
            font.Boldweight = fontweight;
            this.cellStyle.SetFont(font);
            return this.cellStyle;
        }

        public void SetHead(string json)
        {
            Root headRoot = NPOIUtility.JsonUtility.DecodeObject<Root>(json);
            SetHead(headRoot);
        }
        public void SetHead(Root headRoot)
        {
            Root root = headRoot;

            int num2;
            AttributeList list;
            int[] numArray;
            if (this.sheet == null)
            {
                this.sheet = this.Workbook.CreateSheet(root.root.sheetname);
                this.sheet.DisplayGridlines = true;
                if (string.IsNullOrEmpty(this.outFileName))
                {
                    this.outFileName = root.root.sheetname;
                }
                if (root.root.defaultwidth.HasValue)
                {
                    this.sheet.DefaultColumnWidth = root.root.defaultwidth.Value;
                }
                if (root.root.defaultheight.HasValue)
                {
                    this.sheet.DefaultRowHeight = (short)root.root.defaultheight.Value;
                }
            }
            int num = Convert.ToInt32(root.root.rowspan);
            for (num2 = 0; num2 < num; num2++)
            {
                IRow item = this.sheet.CreateRow(num2);
                this.rowList.Add(item);
            }
            for (num2 = 0; num2 < root.root.head.Count; num2++)
            {
                list = root.root.head[num2];
                numArray = list.cellregion.Split(new char[] { ',' }).ToIntArray();
                if ((numArray[0] < numArray[1]) || (numArray[2] < numArray[3]))
                {
                    this.sheet.AddMergedRegion(new CellRangeAddress(numArray[0], numArray[1], numArray[2], numArray[3]));
                }
            }
            for (num2 = 0; num2 < root.root.head.Count; num2++)
            {
                ICell cell;
                list = root.root.head[num2];
                numArray = list.cellregion.Split(new char[] { ',' }).ToIntArray();
                int column = -1;
                int num4 = -1;
                if (((numArray[0] == numArray[1]) && (numArray[2] == numArray[3])) || ((numArray[0] == numArray[1]) && (numArray[2] < numArray[3])))
                {
                    column = numArray[2];
                    num4 = numArray[0];
                    cell = this.rowList[num4].CreateCell(column);
                    if (list.height.HasValue)
                    {
                        this.rowList[num4].Height = (short)(list.height.Value * 20);
                    }
                    cell.SetCellValue(list.title);
                    cell.CellStyle = this.SetCellStyle(list);
                }
                if ((numArray[0] < numArray[1]) && (numArray[2] == numArray[3]))
                {
                    column = numArray[2];
                    num4 = numArray[0];
                    cell = this.rowList[num4].CreateCell(column);
                    if (list.height.HasValue)
                    {
                        this.rowList[num4].Height = (short)(list.height.Value * 20);
                    }
                    cell.SetCellValue(list.title);
                    cell.CellStyle = this.SetCellStyle(list);
                }
                if ((numArray[0] < numArray[1]) && (numArray[2] < numArray[3]))
                {
                    column = numArray[2];
                    num4 = numArray[0];
                    cell = this.rowList[num4].CreateCell(column);
                    if (list.height.HasValue)
                    {
                        this.rowList[num4].Height = (short)(list.height.Value * 20);
                    }
                    cell.SetCellValue(list.title);
                    cell.CellStyle = this.SetCellStyle(list);
                }
                if (!(root.root.defaultwidth.HasValue || !list.width.HasValue))
                {
                    this.sheet.SetColumnWidth(num2, list.width.Value * 0x100);
                }
            }
            if (this.IsOnlyTableHead)
            {
                this.WriteToFileTest(this.Workbook);
            }
        }
        public void SetWorkbook(string company = "慧择网", string author = "慧择", string ApplicationName = "", string LastAuthor = "", string Comments = "", string title = "", string Subject = "")
        {
            DocumentSummaryInformation information = PropertySetFactory.CreateDocumentSummaryInformation();
            information.Company = company;
            this.Workbook.DocumentSummaryInformation = information;
            SummaryInformation information2 = PropertySetFactory.CreateSummaryInformation();
            information2.Author = author;
            information2.ApplicationName = ApplicationName;
            information2.LastAuthor = LastAuthor;
            information2.Comments = Comments;
            information2.Title = title;
            information2.Subject = Subject;
            information2.CreateDateTime = new DateTime?(DateTime.Now);
            this.Workbook.SummaryInformation = information2;
        }

        private void WriteToFile(HSSFWorkbook hssfworkbook, string fileName)
        {
            FileStream stream = new FileStream(fileName + ".xls", FileMode.Create);
            hssfworkbook.Write(stream);
            stream.Close();
        }

        private void WriteToFileTest(HSSFWorkbook hssfworkbook)
        {
            FileStream stream = new FileStream(@"c:\test" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls", FileMode.Create);
            hssfworkbook.Write(stream);
            stream.Close();
        }

        public HSSFWorkbook Workbook
        {
            get
            {
                if (this.workbook == null)
                {
                    this.workbook = new HSSFWorkbook();
                }
                return this.workbook;
            }
            set
            {
            }
        }
    }
}

