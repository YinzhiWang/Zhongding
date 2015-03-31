using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Common.Enums;

using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace ZhongDing.Common
{

    /// <summary>
    /// Class ExcelHelper
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 读取Excel并转化为DataSet
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="excelType">Type of the excel.</param>
        /// <returns>DataSet.</returns>
        public static DataSet ConvertExcelToDataSet(Stream stream, EExcelType excelType)
        {
            DataSet dsExcelData = new DataSet();

            switch (excelType)
            {
                case EExcelType.Excel2003:

                    #region Import Data for Excel 2003
                    //创建Workbook，即整个excel文档
                    HSSFWorkbook excel2003Workbook = new HSSFWorkbook(stream);

                    if (excel2003Workbook != null && excel2003Workbook.NumberOfSheets > 0)
                    {
                        for (int i = 0; i < excel2003Workbook.NumberOfSheets; i++)
                        {
                            HSSFSheet sheet = (HSSFSheet)excel2003Workbook.GetSheetAt(i);

                            if (sheet != null)
                            {
                                HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);

                                //如果header row 不存在则该sheet是空的，不处理
                                if (headerRow == null) continue;

                                DataTable table = new DataTable();
                                table.TableName = sheet.SheetName;

                                //一行最后一个方格的编号 即总的列数
                                int cellCount = headerRow.LastCellNum;

                                for (int j = headerRow.FirstCellNum; j < cellCount; j++)
                                {
                                    DataColumn column = new DataColumn(headerRow.GetCell(j).StringCellValue);
                                    table.Columns.Add(column);
                                }

                                //最后一列的标号  即总的行数
                                int rowCount = sheet.LastRowNum;

                                for (int k = (sheet.FirstRowNum + 1); k <= sheet.LastRowNum; k++)
                                {
                                    HSSFRow row = (HSSFRow)sheet.GetRow(k);

                                    if (row == null) continue;

                                    if (row.FirstCellNum < 0) break;

                                    DataRow dataRow = table.NewRow();

                                    int emptyCellCount = 0;

                                    for (int l = row.FirstCellNum; l < cellCount; l++)
                                    {
                                        var cell = row.GetCell(l);
                                        if (cell != null)
                                        {
                                            if (cell.CellType == NPOI.SS.UserModel.CellType.Numeric)
                                            {
                                                if (HSSFDateUtil.IsCellDateFormatted(cell))
                                                {
                                                    try
                                                    {
                                                        dataRow[l] = cell.DateCellValue;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        dataRow[l] = cell.NumericCellValue;
                                                    }
                                                }
                                                else
                                                    dataRow[l] = cell.NumericCellValue;
                                            }
                                            else
                                                dataRow[l] = cell.ToString();

                                            if (string.IsNullOrEmpty(Utility.GetValueFromObject(dataRow[l])))
                                                emptyCellCount++;
                                        }
                                        else
                                            emptyCellCount++;
                                    }

                                    if (emptyCellCount != cellCount)
                                        table.Rows.Add(dataRow);
                                }

                                dsExcelData.Tables.Add(table);

                            }
                        }
                    }

                    #endregion

                    break;

                case EExcelType.Excel2007Plus:

                    #region Import Data for Excel 2007+

                    XSSFWorkbook excel2007PlusWorkbook = new XSSFWorkbook(stream);

                    if (excel2007PlusWorkbook != null && excel2007PlusWorkbook.NumberOfSheets > 0)
                    {
                        for (int i = 0; i < excel2007PlusWorkbook.NumberOfSheets; i++)
                        {
                            XSSFSheet sheet = (XSSFSheet)excel2007PlusWorkbook.GetSheetAt(i);

                            if (sheet != null)
                            {
                                XSSFRow headerRow = (XSSFRow)sheet.GetRow(0);

                                //如果header row 不存在则该sheet是空的，不处理
                                if (headerRow == null) continue;

                                DataTable table = new DataTable();
                                table.TableName = sheet.SheetName;

                                //一行最后一个方格的编号 即总的列数
                                int cellCount = headerRow.LastCellNum;

                                for (int j = headerRow.FirstCellNum; j < cellCount; j++)
                                {
                                    DataColumn column = new DataColumn(headerRow.GetCell(j).StringCellValue);
                                    table.Columns.Add(column);
                                }

                                //最后一列的标号  即总的行数
                                int rowCount = sheet.LastRowNum;

                                for (int k = (sheet.FirstRowNum + 1); k <= sheet.LastRowNum; k++)
                                {
                                    XSSFRow row = (XSSFRow)sheet.GetRow(k);

                                    if (row == null) continue;

                                    if (row.FirstCellNum < 0) break;

                                    DataRow dataRow = table.NewRow();

                                    int emptyCellCount = 0;

                                    for (int l = row.FirstCellNum; l < cellCount; l++)
                                    {
                                        var cell = row.GetCell(l);
                                        if (cell != null)
                                        {
                                            if (cell.CellType == NPOI.SS.UserModel.CellType.Numeric)
                                            {
                                                if (HSSFDateUtil.IsCellDateFormatted(cell))
                                                {
                                                    try
                                                    {
                                                        dataRow[l] = cell.DateCellValue;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        dataRow[l] = cell.NumericCellValue;
                                                    }
                                                }
                                                else
                                                    dataRow[l] = cell.NumericCellValue;
                                            }
                                            else
                                                dataRow[l] = cell.ToString();

                                            if (string.IsNullOrEmpty(Utility.GetValueFromObject(dataRow[l])))
                                                emptyCellCount++;
                                        }
                                        else
                                            emptyCellCount++;
                                    }

                                    if (emptyCellCount != cellCount)
                                        table.Rows.Add(dataRow);
                                }

                                dsExcelData.Tables.Add(table);
                            }
                        }
                    }
                    #endregion

                    break;
            }

            return dsExcelData;
        }


        /// <summary>
        /// 读取Excel并转化为DataSet
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径.</param>
        /// <returns>DataSet.</returns>
        public static DataSet ConvertExcelToDataSet(string excelFilePath)
        {
            if (!File.Exists(excelFilePath))
                return null;

            Stream stream;

            EExcelType excelType = EExcelType.NonExcel;

            try
            {
                FileInfo fileInfo = new FileInfo(excelFilePath);

                string fileExtension = Path.GetExtension(fileInfo.Name);
                if (fileExtension.ToLower().Equals(".xls"))
                    excelType = EExcelType.Excel2003;
                else if (fileExtension.ToLower().Equals(".xlsx"))
                    excelType = EExcelType.Excel2007Plus;

                stream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read);
            }
            catch (IOException ioExp)
            {
                Utility.WriteExceptionLog(ioExp);

                return null;
            }
            catch (UnauthorizedAccessException uaExp)
            {
                Utility.WriteExceptionLog(uaExp);

                return null;
            }

            if (excelType == EExcelType.NonExcel)
                return null;

            return ConvertExcelToDataSet(stream, excelType);
        }

        /// <summary>
        /// Model 转 Excel
        /// </summary>
        /// <typeparam name="T">泛型List</typeparam>
        /// <param name="items">Model集合</param>
        /// <param name="headerSimpleName">Excel的列名(Model属性与自定义列名的映射集合，这里的集合顺序就是将来显示出来的Excel列的顺序，调整这里即可调整Excel列的顺序)</param>
        /// <param name="fileName">导出的文件存放名字</param>
        /// <returns></returns>
        public static bool RenderToExcel<T>(IList<T> items, IList<ExcelHeader> headerSimpleName, string fileName)
        {
            MemoryStream ms = new MemoryStream();

            IWorkbook workbook = new HSSFWorkbook();
            {
                ISheet sheet = workbook.CreateSheet();
                {
                    IRow headerRow = sheet.CreateRow(0);

                    // handling header.
                    foreach (var column in headerSimpleName)
                        headerRow.CreateCell(headerSimpleName.IndexOf(column)).SetCellValue(column.Name);//If Caption not set, returns the ColumnName value

                    // handling value.
                    int rowIndex = 1;

                    foreach (var row in items)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);

                        var properties = row.GetType().GetProperties();
                        int columnIndex = 0;
                        foreach (var column in headerSimpleName)
                        {
                            var cellValue = properties.First(x => x.Name == column.Key).GetValue(row, null);
                            var cellValueText = cellValue == null ? string.Empty : cellValue.ToString();
                            dataRow.CreateCell(columnIndex).SetCellValue(cellValueText);
                            columnIndex++;
                        }

                        rowIndex++;
                    }

                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
            }
            SaveToFile(ms, fileName);
            return true;
        }

        static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
            }
        }
    }
    public class ExcelHeader
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
