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

        private IDCInventoryDataRepository _dcInventoryDataRepository;
        private IDCInventoryDataRepository DCInventoryDataRepository
        {
            get
            {
                if (_dcInventoryDataRepository == null)
                    _dcInventoryDataRepository = new DCInventoryDataRepository();

                return _dcInventoryDataRepository;
            }
        }

        private IClientFlowDataRepository _clientFlowDataRepository;
        private IClientFlowDataRepository ClientFlowDataRepository
        {
            get
            {
                if (_clientFlowDataRepository == null)
                    _clientFlowDataRepository = new ClientFlowDataRepository();

                return _clientFlowDataRepository;
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

        private IDeptMarketRepository _deptMarketRepository;
        private IDeptMarketRepository DeptMarketRepository
        {
            get
            {
                if (_deptMarketRepository == null)
                    _deptMarketRepository = new DeptMarketRepository();

                return _deptMarketRepository;
            }
        }

        private IStockOutDetailRepository _stockOutDetailRepository;
        private IStockOutDetailRepository StockOutDetailRepository
        {
            get
            {
                if (_stockOutDetailRepository == null)
                    _stockOutDetailRepository = new StockOutDetailRepository();

                return _stockOutDetailRepository;
            }
        }

        #endregion

        public void ImportData()
        {
            var toBeImportFileLogs = ImportFileLogRepository.GetList(x =>
                x.IsDeleted == false && !string.IsNullOrEmpty(x.FilePath)
                && x.ImportStatusID == (int)EImportStatus.ToBeImport).ToList();

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
                            SaveDCInventoryData(fileLog, dsData);
                            break;

                        case EImportDataType.ClientFlowData:
                            SaveClientFlowData(fileLog, dsData);
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
        private void SaveDCFlowData(ImportFileLog fileLog, DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0
                && ds.Tables[0].Rows.Count > 0)
            {
                int totalCount = ds.Tables[0].Rows.Count;

                fileLog.ImportBeginDate = DateTime.Now;
                fileLog.ImportStatusID = (int)EImportStatus.Importing;

                ImportFileLogRepository.Save();

                DCImportFileLog dCImportFileLog = fileLog.DCImportFileLog;
                int distributionCompanyID = dCImportFileLog.DistributionCompanyID;
                IDistributionCompanyProductMappingRepository distributionCompanyProductMappingRepository = new DistributionCompanyProductMappingRepository();
                string errorMsg;
                int errorRowCount = 0;
                int rowIndex = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    errorMsg = string.Empty;

                    string productCode = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.PRODUCT_CODE]);
                    string productName = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.PRODUCT_NAME]);
                    string specification = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.PRODUCT_SPECIFICATION]);
                    string factoryName = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.FACTORY_NAME]);
                    string tempSaleDate = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.SALE_DATE]);
                    string tempSaleQty = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.SALE_QTY]);
                    string flowTo = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.FLOW_TO]);

                    try
                    {
                        rowIndex++;

                        var distributionCompanyProductMapping = distributionCompanyProductMappingRepository.GetList(x => x.DistributionCompanyID == distributionCompanyID
                        && x.DistributionCompanyProductCode.ToLower() == productCode.ToLower()
                        && x.DistributionCompanyProductSpecification.ToLower() == specification.ToLower()).FirstOrDefault();
                        if (distributionCompanyProductMapping != null
                            && distributionCompanyProductMapping.ProductID.HasValue
                            && distributionCompanyProductMapping.ProductSpecificationID.HasValue)
                        {


                            DateTime saleDate;
                            if (DateTime.TryParse(tempSaleDate, out saleDate))
                            {
                                int saleQty;
                                if (int.TryParse(tempSaleQty, out saleQty))
                                {

                                    var product = ProductRepository.GetByID(distributionCompanyProductMapping.ProductID);

                                    if (product != null)
                                    {
                                        var productSpecification = ProductSpecificationRepository.GetByID(distributionCompanyProductMapping.ProductSpecificationID);

                                        if (productSpecification != null && productSpecification.ProductID == distributionCompanyProductMapping.ProductID)
                                        {
                                            //设置真实的 productCode 和 productName 和 specification
                                            productCode = distributionCompanyProductMapping.ProductCode;
                                            productName = distributionCompanyProductMapping.ProductName;
                                            specification = distributionCompanyProductMapping.ProductSpecificationName;

                                            var hospital = HospitalRepository.GetList(x =>
                                                x.HospitalName.ToLower().Equals(flowTo.ToLower())).FirstOrDefault();

                                            DBContract dBContract = null;
                                            bool isMatchedContract = false;

                                            if (hospital != null)
                                            {
                                                dBContract = DBContractRepository.GetList(x =>
                                                    x.ProductID == product.ID && x.ProductSpecificationID == productSpecification.ID
                                                    && x.DBContractHospital.Any(y => y.HospitalCodeID == hospital.HospitalCodeID)).FirstOrDefault();

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
                                                        SaleDate = saleDate,
                                                        SaleQty = saleQty
                                                    };

                                                    dCFlowData.DCFlowDataDetail.Add(dCFlowDataDetail);
                                                }
                                                else
                                                    dCFlowData.IsCorrectlyFlow = false;
                                            }
                                            else
                                                errorMsg += "该条数据在数据库中已存在；";
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
                        else
                            errorMsg += "配送公司货品映射文件格式错误；";
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

                fileLog.TotalCount = totalCount;
                fileLog.FailedCount = errorRowCount;
                fileLog.SucceedCount = totalCount - errorRowCount;

                ImportFileLogRepository.Save();
            }
        }

        /// <summary>
        /// 保存导入的客户流向数据
        /// </summary>
        private void SaveClientFlowData(ImportFileLog fileLog, DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0
                && ds.Tables[0].Rows.Count > 0)
            {
                int totalCount = ds.Tables[0].Rows.Count;

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
                    string factoryName = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.FACTORY_NAME]);
                    string tempSaleDate = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.SALE_DATE]);
                    string tempSaleQty = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.SALE_QTY]);
                    string hospitalName = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.HOSPITAL_NAME]);
                    string hospitalType = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.HOSPITAL_TYPE]);
                    string marketName = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.MARKET_NAME]);

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
                                            x.HospitalName.ToLower().Equals(hospitalName.ToLower())).FirstOrDefault();

                                        if (hospital != null)
                                        {
                                            int hospitalTypeID = 0;
                                            if (hospitalType.Equals(GlobalConst.HospitalTypes.BASE_MEDICINE))
                                                hospitalTypeID = (int)EHospitalType.BaseMedicine;
                                            else if (hospitalType.Equals(GlobalConst.HospitalTypes.BUSINESS_MEDICINE))
                                                hospitalTypeID = (int)EHospitalType.BusinessMedicine;

                                            if (hospitalTypeID > 0)
                                            {
                                                var clientFlowData = ClientFlowDataRepository.GetList(x =>
                                                    x.ClientUserID == fileLog.ClientImportFileLog.ClientUserID
                                                    && x.ClientCompanyID == fileLog.ClientImportFileLog.ClientCompanyID
                                                    && x.ProductID == product.ID && x.ProductSpecificationID == productSpecification.ID
                                                    && x.SaleDate == saleDate && x.SettlementDate == fileLog.ClientImportFileLog.SettlementDate
                                                    && x.FlowTo == hospitalName).FirstOrDefault();

                                                //限制重复导入，如果存在就跳过该条记录
                                                if (clientFlowData == null)
                                                {
                                                    var deptMarket = DeptMarketRepository
                                                    .GetList(x => x.MarketName.ToLower().Equals(marketName.ToLower()))
                                                    .FirstOrDefault();

                                                    clientFlowData = new ClientFlowData()
                                                    {
                                                        ClientUserID = fileLog.ClientImportFileLog.ClientUserID,
                                                        ClientCompanyID = fileLog.ClientImportFileLog.ClientCompanyID,
                                                        ImportFileLogID = fileLog.ID,
                                                        ProductID = product.ID,
                                                        ProductName = productName,
                                                        ProductCode = productCode,
                                                        ProductSpecificationID = productSpecification.ID,
                                                        ProductSpecification = specification,
                                                        SaleDate = saleDate,
                                                        SaleQty = saleQty,
                                                        SettlementDate = fileLog.ClientImportFileLog.SettlementDate,
                                                        FlowTo = hospitalName,
                                                        FactoryName = factoryName,
                                                        HospitalID = hospital.ID,
                                                        HospitalTypeID = hospitalTypeID,
                                                        MarketName = marketName
                                                    };

                                                    if (deptMarket != null)
                                                        clientFlowData.DeptMarketID = deptMarket.ID;

                                                    fileLog.ClientFlowData.Add(clientFlowData);
                                                }
                                                else
                                                    errorMsg += "该条数据在数据库中已存在；";
                                            }
                                            else
                                                errorMsg += GlobalConst.ImportDataColumns.HOSPITAL_TYPE + "不存在；";
                                        }
                                        else
                                            errorMsg += GlobalConst.ImportDataColumns.HOSPITAL_NAME + "不存在；";
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
                            + GlobalConst.ImportDataColumns.FACTORY_NAME + ":" + factoryName + ", "
                            + GlobalConst.ImportDataColumns.SALE_DATE + ":" + tempSaleDate + ", "
                            + GlobalConst.ImportDataColumns.SALE_QTY + ":" + tempSaleQty + ", "
                            + GlobalConst.ImportDataColumns.HOSPITAL_NAME + ":" + hospitalName + ", "
                            + GlobalConst.ImportDataColumns.HOSPITAL_TYPE + ":" + hospitalType + ", "
                            + GlobalConst.ImportDataColumns.MARKET_NAME + ":" + marketName
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

                fileLog.TotalCount = totalCount;
                fileLog.FailedCount = errorRowCount;
                fileLog.SucceedCount = totalCount - errorRowCount;

                ImportFileLogRepository.Save();
            }
        }


        /// <summary>
        /// 保存配送公司库存数据
        /// </summary>
        private void SaveDCInventoryData(ImportFileLog fileLog, DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0
                && ds.Tables[0].Rows.Count > 0)
            {
                int totalCount = ds.Tables[0].Rows.Count;

                fileLog.ImportBeginDate = DateTime.Now;
                fileLog.ImportStatusID = (int)EImportStatus.Importing;

                ImportFileLogRepository.Save();

                DateTime settlementDate = fileLog.DCImportFileLog.SettlementDate;
                int distributionCompanyID = fileLog.DCImportFileLog.DistributionCompanyID;

                IDistributionCompanyProductMappingRepository distributionCompanyProductMappingRepository = new DistributionCompanyProductMappingRepository();
                string errorMsg;
                int errorRowCount = 0;
                int rowIndex = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    errorMsg = string.Empty;

                    string productCode = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.PRODUCT_CODE]);
                    string productName = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.PRODUCT_NAME]);
                    string specification = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.PRODUCT_SPECIFICATION]);
                    string tempBalanceQty = Utility.GetValueFromObject(row[GlobalConst.ImportDataColumns.INVENTORY_BALANCE_QTY]);

                    try
                    {
                        rowIndex++;

                        var distributionCompanyProductMapping = distributionCompanyProductMappingRepository.GetList(x => x.DistributionCompanyID == distributionCompanyID
                      && x.DistributionCompanyProductCode.ToLower() == productCode.ToLower()
                      && x.DistributionCompanyProductSpecification.ToLower() == specification.ToLower()).FirstOrDefault();
                        if (distributionCompanyProductMapping != null
                            && distributionCompanyProductMapping.ProductID.HasValue
                            && distributionCompanyProductMapping.ProductSpecificationID.HasValue)
                        {

                            int dcBalanceQty;
                            if (int.TryParse(tempBalanceQty, out dcBalanceQty))
                            {
                                var product = ProductRepository.GetByID(distributionCompanyProductMapping.ProductID);

                                if (product != null)
                                {
                                    var productSpecification = ProductSpecificationRepository.GetByID(distributionCompanyProductMapping.ProductSpecificationID);

                                    if (productSpecification != null && productSpecification.ProductID == distributionCompanyProductMapping.ProductID)
                                    {
                                        //设置真实的 productCode 和 productName 和 specification
                                        productCode = distributionCompanyProductMapping.ProductCode;
                                        productName = distributionCompanyProductMapping.ProductName;
                                        specification = distributionCompanyProductMapping.ProductSpecificationName;

                                        var inventoryData = DCInventoryDataRepository.GetList(x =>
                                                     x.DistributionCompanyID == distributionCompanyID
                                                     && x.ProductID == product.ID && x.ProductSpecificationID == productSpecification.ID
                                                     && x.SettlementDate == settlementDate).FirstOrDefault();

                                        //限制重复导入，如果存在就跳过该条记录
                                        if (inventoryData == null)
                                        {
                                            int bookBalanceQty = 0;

                                            var queryStockOutDetails = StockOutDetailRepository.GetList(x => x.IsDeleted == false
                                                && x.StockOut.ReceiverTypeID == (int)EReceiverType.DistributionCompany
                                                && x.StockOut.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse
                                                && x.StockOut.DistributionCompanyID == distributionCompanyID
                                                && x.ProductID == product.ID && x.ProductSpecificationID == productSpecification.ID
                                                && x.StockOut.OutDate <= settlementDate);

                                            int stockOutQty = queryStockOutDetails.Count() > 0 ?
                                                queryStockOutDetails.Sum(x => x.OutQty) : 0;

                                            var queryDCFlowData = DCFlowDataRepository.GetList(x => x.IsCorrectlyFlow == true
                                                && x.ProductID == product.ID && x.ProductSpecificationID == productSpecification.ID
                                                && x.DistributionCompanyID == distributionCompanyID
                                                && x.SaleDate <= settlementDate);

                                            int flowDataQty = queryDCFlowData.Count() > 0 ? queryDCFlowData.Sum(x => x.SaleQty) : 0;

                                            bookBalanceQty = stockOutQty - flowDataQty;

                                            inventoryData = new DCInventoryData()
                                            {
                                                DistributionCompanyID = distributionCompanyID,
                                                SettlementDate = settlementDate,
                                                ImportFileLogID = fileLog.ID,
                                                ProductID = product.ID,
                                                ProductName = productName,
                                                ProductCode = productCode,
                                                ProductSpecificationID = productSpecification.ID,
                                                ProductSpecification = specification,
                                                DCBalanceQty = dcBalanceQty,
                                                BookBalanceQty = bookBalanceQty,
                                            };

                                            fileLog.DCInventoryData.Add(inventoryData);
                                        }
                                        else
                                            errorMsg += "该条数据在数据库中已存在；";
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
                            errorMsg += "配送公司货品映射文件格式错误；";
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
                            + GlobalConst.ImportDataColumns.INVENTORY_BALANCE_QTY + ":" + tempBalanceQty
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

                fileLog.TotalCount = totalCount;
                fileLog.FailedCount = errorRowCount;
                fileLog.SucceedCount = totalCount - errorRowCount;

                ImportFileLogRepository.Save();
            }
        }

        #endregion
    }
}
