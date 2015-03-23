using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.Repositories.Imports
{
    public class ImportDataRepository : IImportDataRepository
    {
        public void ImportData()
        {
            IImportFileLogRepository importFileLogRepository = new ImportFileLogRepository();

            var toBeImportFileLogs = importFileLogRepository.GetList(x =>
                //x.IsDeleted == false && 
                !string.IsNullOrEmpty(x.FilePath)
                && (x.ImportStatusID == (int)EImportStatus.ToBeImport
                    || x.ImportStatusID == (int)EImportStatus.ImportError)).ToList();

            foreach (var fileLog in toBeImportFileLogs)
            {
                string filePath = fileLog.FilePath;

                //网页中
                if (HttpContext.Current != null)
                {
                    if (!filePath.StartsWith("~"))
                        filePath = "~" + filePath;

                    filePath = HttpContext.Current.Server.MapPath(filePath);
                }
                else//Win service中
                {
                    if (filePath.StartsWith("~"))
                        filePath = filePath.Substring(1, filePath.Length - 1);

                    filePath = WebConfig.WebsiteAbsoluteRootPath + filePath;
                }

                DataSet dsData = ExcelHelper.ConvertExcelToDataSet(filePath);

                if (dsData != null && dsData.Tables.Count > 0)
                {
                    var importDataType = (EImportDataType)fileLog.ImportDataTypeID;

                    switch (importDataType)
                    {
                        case EImportDataType.DCFlowData:
                            SaveDCFlowData(fileLog, dsData);
                            break;

                        case EImportDataType.DCInventoryData:
                            break;
                        case EImportDataType.ProcureOrderData:
                            break;
                        case EImportDataType.StockInData:
                            break;
                    }
                }

                importFileLogRepository.Save();
            }
        }

        #region Private Methods

        /// <summary>
        /// 保存导入的配送公司流向数据
        /// </summary>
        /// <param name="ds">The ds.</param>
        private void SaveDCFlowData(ImportFileLog fileLog, DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0
                && ds.Tables[0].Rows.Count > 0)
            {
                //using (IUnitOfWork unitOfWork = new UnitOfWork())
                //{
                //    var db = unitOfWork.GetDbModel();
                //}

                fileLog.ImportBeginDate = DateTime.Now;

                foreach (var row in ds.Tables[0].Rows)
                {

                }

                fileLog.ImportEndDate = DateTime.Now;
            }
        }

        #endregion
    }
}
