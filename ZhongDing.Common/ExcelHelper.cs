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
    }
}
