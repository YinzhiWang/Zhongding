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
        #region Members

        private IDCFlowDataRepository _dCFlowDataRepository;
        private IDCFlowDataRepository DCFlowDataRepository
        {
            get
            {
                if (_dCFlowDataRepository == null)
                    _dCFlowDataRepository = new DCFlowDataRepository();

                return _dCFlowDataRepository;
            }
        }

        private IImportFileLogRepository _importFileLogRepository;
        private IImportFileLogRepository ImportFileLogRepository
        {
            get
            {
                if (_importFileLogRepository == null)
                    _importFileLogRepository = new ImportFileLogRepository();

                return _importFileLogRepository;
            }
        }

        private IProductRepository _productRepository;
        private IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository();

                return _productRepository;
            }
        }

        private IProductSpecificationRepository _productSpecificationRepository;
        private IProductSpecificationRepository ProductSpecificationRepository
        {
            get
            {
                if (_productSpecificationRepository == null)
                    _productSpecificationRepository = new ProductSpecificationRepository();

                return _productSpecificationRepository;
            }
        }

        private IHospitalRepository _hospitalRepository;
        private IHospitalRepository HospitalRepository
        {
            get
            {
                if (_hospitalRepository == null)
                    _hospitalRepository = new HospitalRepository();

                return _hospitalRepository;
            }
        }

        private IDBContractRepository _dBContractRepository;
        private IDBContractRepository DBContractRepository
        {
            get
            {
                if (_dBContractRepository == null)
                    _dBContractRepository = new DBContractRepository();

                return _dBContractRepository;
            }
        }

        #endregion

        public void ImportData()
        {
            var toBeImportFileLogs = ImportFileLogRepository.GetList(x =>
                x.IsDeleted == false && !string.IsNullOrEmpty(x.FilePath)
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

                ImportFileLogRepository.Save();
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
                fileLog.ImportBeginDate = DateTime.Now;
                fileLog.ImportStatusID = (int)EImportStatus.Importing;

                ImportFileLogRepository.Save();

                string errorMsg;
                int errorRowCount = 0;
                int rowIndex = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    errorMsg = string.Empty;

                    string productCode = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.PRODUCT_CODE]);
                    string productName = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.PRODUCT_NAME]);
                    string specification = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.PRODUCT_SPECIFICATION]);
                    string unitName = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.UNIT_NAME]);
                    string factoryName = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.FACTORY_NAME]);
                    string tempSaleDate = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.SALE_DATE]);
                    string tempSaleQty = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.SALE_QTY]);
                    string flowTo = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.FLOW_TO]);

                    try
                    {
                        rowIndex++;

                        DateTime saleDate;
                        if (DateTime.TryParse(tempSaleDate, out saleDate))
                        {
                            int saleQty;
                            if (int.TryParse(tempSaleQty, out saleQty))
                            {
                                var product = ProductRepository.GetList(x =>
                                    x.ProductCode.ToLower().Equals(productCode.ToLower())).FirstOrDefault();

                                if (product != null)
                                {
                                    var productSpecification = ProductSpecificationRepository
                                        .GetList(x => x.Specification.ToLower().Equals(specification.ToLower()))
                                        .FirstOrDefault();

                                    if (productSpecification != null)
                                    {
                                        var hospital = HospitalRepository.GetList(x =>
                                            x.HospitalName.ToLower().Equals(flowTo.ToLower())).FirstOrDefault();

                                        DBContract dBContract = null;
                                        bool isMatchedContract = false;

                                        if (hospital != null)
                                        {
                                            dBContract = DBContractRepository.GetList(x =>
                                                x.ProductID == product.ID && x.ProductSpecificationID == productSpecification.ID
                                                && x.DBContractHospital.Any(y => y.HospitalID == hospital.ID)).FirstOrDefault();

                                            if (dBContract != null)
                                                isMatchedContract = true;
                                        }

                                        var dCFlowData = DCFlowDataRepository.GetList(x =>
                                            x.DistributionCompanyID == fileLog.DCImportFileLog.DistributionCompanyID
                                            && x.ProductID == product.ID && x.ProductSpecificationID == productSpecification.ID
                                            && x.SaleDate == saleDate && x.SettlementDate == fileLog.DCImportFileLog.SettlementDate
                                            && x.FlowTo == flowTo).FirstOrDefault();

                                        //限制重复导入，如果存在就跳过该条记录
                                        if (dCFlowData == null)
                                        {
                                            dCFlowData = new DCFlowData()
                                            {
                                                DistributionCompanyID = fileLog.DCImportFileLog.DistributionCompanyID,
                                                ImportFileLogID = fileLog.ID,
                                                ProductID = product.ID,
                                                ProductName = productName,
                                                ProductCode = productCode,
                                                ProductSpecificationID = productSpecification.ID,
                                                ProductSpecification = specification,
                                                SaleDate = saleDate,
                                                SaleQty = saleQty,
                                                SettlementDate = fileLog.DCImportFileLog.SettlementDate,
                                                FlowTo = flowTo,
                                                FactoryName = factoryName,
                                                IsOverwritten = false
                                            };

                                            fileLog.DCFlowData.Add(dCFlowData);

                                            //找到匹配的协议
                                            if (isMatchedContract)
                                            {
                                                dCFlowData.HospitalID = hospital.ID;
                                                dCFlowData.IsCorrectlyFlow = true;

                                                var dCFlowDataDetail = new DCFlowDataDetail()
                                                {
                                                    DBContractID = dBContract.ID,
                                                    ContractCode = dBContract.ContractCode,
                                                    IsTempContract = dBContract.IsTempContract,
                                                    HospitalID = hospital.ID,
                                                    HospitalName = hospital.HospitalName,
                                                    ClientUserID = dBContract.ClientUserID.HasValue
                                                        ? dBContract.ClientUserID.Value : GlobalConst.INVALID_INT,
                                                    ClientUserName = dBContract.ClientUser == null
                                                        ? string.Empty : dBContract.ClientUser.ClientName,
                                                    InChargeUserID = dBContract.InChargeUserID,
                                                    InChargeUserFullName = dBContract.Users == null
                                                        ? string.Empty : dBContract.Users.FullName,
                                                    UnitOfMeasurementID = dBContract.ProductSpecification != null
                                                        ? dBContract.ProductSpecification.UnitOfMeasurementID : null,
                                                    UnitName = (dBContract.ProductSpecification != null && dBContract.ProductSpecification.UnitOfMeasurement != null)
                                                        ? dBContract.ProductSpecification.UnitOfMeasurement.UnitName : string.Empty,
                                                    SaleQty = saleQty
                                                };

                                                dCFlowData.DCFlowDataDetail.Add(dCFlowDataDetail);
                                            }
                                            else
                                                dCFlowData.IsCorrectlyFlow = false;
                                        }
                                        else
                                            continue;
                                    }
                                    else
                                        errorMsg += GlobalConst.ImportDataColumns.PRODUCT_SPECIFICATION + "不存在；";
                                }
                                else
                                    errorMsg += GlobalConst.ImportDataColumns.PRODUCT_NAME + "不存在；";
                            }
                            else
                                errorMsg += GlobalConst.ImportDataColumns.SALE_QTY + "格式错误；";
                        }
                        else
                            errorMsg += GlobalConst.ImportDataColumns.SALE_DATE + "格式错误；";
                    }
                    catch (Exception exp)
                    {
                        Utility.WriteExceptionLog(exp);

                        errorMsg += exp.Message;
                    }

                    if (errorMsg != string.Empty)
                    {
                        errorRowCount++;

                        fileLog.ImportErrorLog.Add(new ImportErrorLog
                        {
                            ErrorRowIndex = rowIndex,
                            ErrorMsg = errorMsg,
                            ErrorRowData = GlobalConst.ImportDataColumns.PRODUCT_CODE + ":" + productCode + ", "
                            + GlobalConst.ImportDataColumns.PRODUCT_NAME + ":" + productName + ", "
                            + GlobalConst.ImportDataColumns.PRODUCT_SPECIFICATION + ":" + specification + ", "
                            + GlobalConst.ImportDataColumns.UNIT_NAME + ":" + unitName + ", "
                            + GlobalConst.ImportDataColumns.FACTORY_NAME + ":" + factoryName + ", "
                            + GlobalConst.ImportDataColumns.SALE_DATE + ":" + tempSaleDate + ", "
                            + GlobalConst.ImportDataColumns.SALE_QTY + ":" + tempSaleQty + ", "
                            + GlobalConst.ImportDataColumns.FLOW_TO + ":" + flowTo
                        });
                    }
                }

                if (errorRowCount > 0)
                {
                    fileLog.ImportStatusID = (int)EImportStatus.ImportError;
                }
                else
                {
                    fileLog.ImportStatusID = (int)EImportStatus.Completed;
                    fileLog.ImportEndDate = DateTime.Now;
                }

                ImportFileLogRepository.Save();
            }
        }

        #endregion
    }
}
