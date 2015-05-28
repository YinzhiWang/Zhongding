using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;
using System.IO;
using ZhongDing.Domain.Models;
using System.Data;
using ZhongDing.Common.Extension;
using System.Transactions;
using System.Text;

namespace ZhongDing.Web.Views.Imports
{
    public partial class StockInImportData : BasePage
    {
        #region Members

        private IProcureOrderApplicationRepository _PageProcureOrderAppRepository;
        private IProcureOrderApplicationRepository PageProcureOrderAppRepository
        {
            get
            {
                if (_PageProcureOrderAppRepository == null)
                    _PageProcureOrderAppRepository = new ProcureOrderApplicationRepository();

                return _PageProcureOrderAppRepository;
            }
        }

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                    _PageSupplierRepository = new SupplierRepository();

                return _PageSupplierRepository;
            }
        }


        private IStockInImportFileLogRepository _PageStockInImportFileLogRepository;
        private IStockInImportFileLogRepository PageStockInImportFileLogRepository
        {
            get
            {
                if (_PageStockInImportFileLogRepository == null)
                    _PageStockInImportFileLogRepository = new StockInImportFileLogRepository();

                return _PageStockInImportFileLogRepository;
            }
        }


        #endregion

        //protected override int GetCurrentWorkFlowID()
        //{
        //    return (int)EWorkflow.ProcureOrder;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.StockInData;
            divInfo.Visible = false;
            if (!IsPostBack)
            {
                BindSuppliers();
            }
        }

        #region Private Methods

        private void BindSuppliers()
        {
            var suppliers = PageSupplierRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    CompanyID = CurrentUser.CompanyID
                }
            });

            rcbxSupplier.DataSource = suppliers;
            rcbxSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxSupplier.DataBind();

            rcbxSupplier.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchStockInImportFileLog()
            {
                ImportDataTypeID = (int)EImportDataType.StockInData,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate
            };



            int totalRecords;

            var entities = PageStockInImportFileLogRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion


        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_DeleteCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void rgEntities_ItemCreated(object sender, GridItemEventArgs e)
        {

        }

        protected void rgEntities_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {

        }

        protected void rgEntities_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void rgEntities_ItemCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            rcbxSupplier.ClearSelection();

            BindEntities(true);
        }

        protected void btnImportData_Click(object sender, EventArgs e)
        {

            if (radAsyncUpload.UploadedFiles.Count == 0)
                cvRadAsyncUpload.IsValid = false;

            if (!IsValid)
                return;
            if (radAsyncUpload.UploadedFiles.Count == 0)
            {
                //GlobalConst.NotificationSettings
                this.Master.BaseNotification.Show("请选择文件");
                return;
            }
            UploadedFile uploadFile = radAsyncUpload.UploadedFiles[0];

            int importDataTypeID = (int)EImportDataType.StockInData;
            string uploadFilePath = WebConfig.UploadFilePathProcureOrderData;
            string uploadFileServerFolderPath = Server.MapPath(uploadFilePath);
            if (!Directory.Exists(uploadFileServerFolderPath))
                Directory.CreateDirectory(uploadFileServerFolderPath);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + uploadFile.GetExtension();
            string fileNameFullPath = uploadFileServerFolderPath + fileName;
            try
            {
                uploadFile.SaveAs(fileNameFullPath);
            }
            catch
            {
                cvRadAsyncUpload.IsValid = false;
                return;
            }

            ////////////////////////////////////////
            StockInImportFileLog stockInImportFileLog = new StockInImportFileLog()
            {

            };
            var importFileLog = new ImportFileLog()
            {
                ImportDataTypeID = (int)EImportDataType.ProcureOrderData,
                ImportStatusID = (int)EImportStatus.ToBeImport,
                FileName = txtFileName.Text.Trim() + uploadFile.GetExtension(),
                FilePath = uploadFilePath + fileName
            };
            stockInImportFileLog.ImportFileLog = importFileLog;
            PageStockInImportFileLogRepository.Add(stockInImportFileLog);
            PageStockInImportFileLogRepository.Save();


            DataSet dsData = null;
            try
            {
                dsData = ExcelHelper.ConvertExcelToDataSet(fileNameFullPath);
            }
            catch (Exception ex)
            {
                divInfo.Visible = true;
                lblError.Text = "文件格式错误，请检查文件格式";
                return;
            }

            if (dsData == null || dsData.Tables.Count == 0 || dsData.Tables[0].Rows.Count == 0)
            {
                divInfo.Visible = true;
                lblError.Text = "文件内容不能为空";
                return;
            }
            try
            {
                int totalCount = dsData.Tables[0].Rows.Count;

                importFileLog.ImportBeginDate = DateTime.Now;
                importFileLog.ImportStatusID = (int)EImportStatus.Importing;

                PageStockInImportFileLogRepository.Save();

                using (TransactionScope transaction = new TransactionScope())
                {
                    using (IUnitOfWork unitOfWork = new UnitOfWork())
                    {
                        DbModelContainer db = unitOfWork.GetDbModel();

                        ISupplierRepository supplierRepository = new SupplierRepository();
                        IWarehouseRepository warehouseRepository = new WarehouseRepository();
                        IProductRepository productRepository = new ProductRepository();
                        IProductSpecificationRepository productSpecificationRepository = new ProductSpecificationRepository();
                        IStockInRepository stockInRepository = new StockInRepository();
                        IStockInImportDataRepository stockInImportDataRepository = new StockInImportDataRepository();
                        IImportErrorLogRepository importErrorLogRepository = new ImportErrorLogRepository();
                        IProcureOrderApplicationRepository procureOrderApplicationRepository = new ProcureOrderApplicationRepository();

                        supplierRepository.SetDbModel(db);
                        warehouseRepository.SetDbModel(db);
                        productRepository.SetDbModel(db);
                        stockInRepository.SetDbModel(db);
                        stockInImportDataRepository.SetDbModel(db);
                        importErrorLogRepository.SetDbModel(db);

                        int succeedCount = 0;
                        int errorCount = 0;
                        int errorRowCount = 0;
                        int rowIndex = 0;

                        bool checkFinished = false;
                        bool savedData = false;
                    checkOrWrite: List<string> errorList = new List<string>();
                        List<string> orderCodeList = new List<string>();
                        List<string> rowDataList = new List<string>();

                        foreach (DataRow row in dsData.Tables[0].Rows)
                        {
                            try
                            {
                                rowIndex++;
                                rowDataList.Clear();
                                errorCount = errorList.Count;
                                //--------------------------------------------------------------------------------------
                                string orderCode = row[GlobalConst.ImportDataColumns.STOCKIN_ORDER_CODE].ToStringOrNull();
                                if (orderCode.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.STOCKIN_ORDER_CODE + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.STOCKIN_ORDER_CODE + ":" + orderCode + "；");
                                //--------------------------------------------------------------------------------------
                                string supplierName = row[GlobalConst.ImportDataColumns.SUPPLIER_NAME].ToStringOrNull();
                                int? supplierID = null;
                                Supplier supplier = null;
                                if (supplierName.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.SUPPLIER_NAME + "不能为空；");
                                }
                                else
                                {
                                    supplier = supplierRepository.GetBySupplierName(supplierName);
                                    if (supplier != null)
                                        supplierID = supplier.ID;
                                    else
                                        errorList.Add(GlobalConst.ImportDataColumns.SUPPLIER_NAME + "(" + supplierName + ")" + "不存在；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.SUPPLIER_NAME + ":" + supplierName + "；");
                                //--------------------------------------------------------------------------------------

                                DateTime? orderDate = row[GlobalConst.ImportDataColumns.STOCKIN_ORDER_DATE].ToDateTimeOrNull();
                                if (!orderDate.HasValue)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.STOCKIN_ORDER_DATE + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.STOCKIN_ORDER_DATE + ":" + orderDate + "；");
                                //--------------------------------------------------------------------------------------
                                string procureOrderApplicationOrderCode = row[GlobalConst.ImportDataColumns.PROCURE_ORDER_CODE].ToStringOrNull();
                                int? procureOrderApplicationID = null;
                                if (procureOrderApplicationOrderCode.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PROCURE_ORDER_CODE + "不能为空；");
                                }
                                else
                                {
                                    var procureOrderApplication = procureOrderApplicationRepository.GetList(x => x.OrderCode.ToLower() == procureOrderApplicationOrderCode.ToLower()).FirstOrDefault();
                                    if (procureOrderApplication != null)
                                        procureOrderApplicationID = procureOrderApplication.ID;
                                    else
                                        errorList.Add(GlobalConst.ImportDataColumns.PROCURE_ORDER_CODE + "(" + procureOrderApplicationOrderCode + ")" + "不存在；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PROCURE_ORDER_CODE + ":" + procureOrderApplicationOrderCode + "；");
                                //--------------------------------------------------------------------------------------
                                string warehouseName = row[GlobalConst.ImportDataColumns.STOCKIN_WAREHOUSE_NAME].ToStringOrNull();
                                int? warehouseID = null;
                                if (warehouseName.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.STOCKIN_WAREHOUSE_NAME + "不能为空；");
                                }
                                else
                                {
                                    var warehouse = warehouseRepository.GetByWarehouseName(warehouseName);
                                    if (warehouse != null)
                                        warehouseID = warehouse.ID;
                                    else
                                        errorList.Add(GlobalConst.ImportDataColumns.STOCKIN_WAREHOUSE_NAME + "(" + warehouseName + ")" + "不存在；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.STOCKIN_WAREHOUSE_NAME + ":" + warehouseName + "；");
                                //--------------------------------------------------------------------------------------
                                string productCode = row[GlobalConst.ImportDataColumns.PRODUCT_CODE].ToStringOrNull();
                                Product product = null;
                                int? productID = null;
                                if (productCode.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PRODUCT_CODE + "不能为空；");
                                }
                                else
                                {
                                    product = productRepository.GetByProductCode(productCode);
                                    if (product != null)
                                        productID = product.ID;
                                    else
                                        errorList.Add(GlobalConst.ImportDataColumns.PRODUCT_CODE + "(" + productCode + ")" + "不存在；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PRODUCT_CODE + ":" + productCode + "；");
                                //--------------------------------------------------------------------------------------
                                string productName = row[GlobalConst.ImportDataColumns.PRODUCT_NAME].ToStringOrNull();
                                if (productName.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PRODUCT_NAME + "不能为空；");
                                }
                                else
                                {

                                    if (product != null && product.ProductName.ToLower() != productName.ToLower())
                                    {
                                        errorList.Add(GlobalConst.ImportDataColumns.PRODUCT_NAME + "(" + productName + ")" + "和" + GlobalConst.ImportDataColumns.PRODUCT_CODE + "(" +
                                            productCode + "不匹配)；");
                                    }
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PRODUCT_NAME + ":" + productName + "；");
                                //--------------------------------------------------------------------------------------
                                string specification = row[GlobalConst.ImportDataColumns.PRODUCT_SPECIFICATION].ToStringOrNull();
                                int? productSpecificationID = null;
                                ProductSpecification productSpecification = null;
                                if (specification.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PRODUCT_SPECIFICATION + "不能为空；");
                                }
                                else
                                {
                                    productSpecification = productSpecificationRepository
                                        .GetList(x => x.Specification.ToLower().Equals(specification.ToLower()))
                                        .FirstOrDefault();
                                    if (productSpecification != null)
                                        productSpecificationID = productSpecification.ID;
                                    else
                                        errorList.Add(GlobalConst.ImportDataColumns.PRODUCT_SPECIFICATION + "(" + specification + ")" + "不存在；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PRODUCT_SPECIFICATION + ":" + specification + "；");
                                //--------------------------------------------------------------------------------------
                                string factoryName = row[GlobalConst.ImportDataColumns.FACTORY_NAME].ToStringOrNull();

                                if (factoryName.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.UNIT_NAME + "不能为空；");
                                }
                                else
                                {
                                    //var supplier = supplierRepository.GetList(x => x.FactoryName.ToLower() == factoryName.ToLower());
                                    if (supplier != null && supplier.FactoryName.ToLower() != factoryName.ToLower())
                                    {
                                        errorList.Add(GlobalConst.ImportDataColumns.FACTORY_NAME + "(" + factoryName + ")" + "和供应商不匹配；");
                                    }
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.UNIT_NAME + ":" + factoryName + "；");
                                //--------------------------------------------------------------------------------------
                                string unitName = row[GlobalConst.ImportDataColumns.UNIT_NAME].ToStringOrNull();
                                int? unitOfMeasurementID = null;
                                if (unitName.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.UNIT_NAME + "不能为空；");
                                }
                                else
                                {
                                    if (productSpecification != null && productSpecification.UnitOfMeasurement.UnitName.ToLower() != unitName.ToLower())
                                    {
                                        errorList.Add(GlobalConst.ImportDataColumns.UNIT_NAME + "(" + unitName + ")" + "不存在；");
                                    }
                                    else if (productSpecification != null)
                                    {
                                        unitOfMeasurementID = productSpecification.UnitOfMeasurementID;
                                    }
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.UNIT_NAME + ":" + unitName + "；");
                                //--------------------------------------------------------------------------------------
                                int? stockInCount = row[GlobalConst.ImportDataColumns.STOCKIN_COUNT].ToIntOrNull();
                                if (stockInCount.GetValueOrDefault(0) == 0)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.STOCKIN_COUNT + "必须大于0；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.STOCKIN_COUNT + ":" + stockInCount + "；");
                                //--------------------------------------------------------------------------------------
                                decimal? procurePrice = row[GlobalConst.ImportDataColumns.PROCURE_PRICE].ToDecimalOrNull();
                                if (procurePrice.GetValueOrDefault(0) == 0)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PROCURE_PRICE + "必须大于0；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PROCURE_PRICE + ":" + procurePrice + "；");
                                //--------------------------------------------------------------------------------------
                                string batchNumber = row[GlobalConst.ImportDataColumns.BATCH_NUMBER].ToStringOrNull();
                                if (batchNumber.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.BATCH_NUMBER + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.BATCH_NUMBER + ":" + batchNumber + "；");
                                //--------------------------------------------------------------------------------------
                                DateTime? expirationDate = row[GlobalConst.ImportDataColumns.EXPIRATION_DATE].ToDateTimeOrNull();
                                if (!expirationDate.HasValue)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.EXPIRATION_DATE + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.EXPIRATION_DATE + ":" + expirationDate + "；");
                                //--------------------------------------------------------------------------------------
                                string licenseNumber = row[GlobalConst.ImportDataColumns.LICENSE_NUMBER].ToStringOrNull();
                                if (licenseNumber.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.LICENSE_NUMBER + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.LICENSE_NUMBER + ":" + licenseNumber + "；");
                                //--------------------------------------------------------------------------------------
                                string mortgagedProduct = row[GlobalConst.ImportDataColumns.MORTGAGED_PRODUCT].ToStringOrNull();
                                bool? isMortgagedProduct = null;
                                if (mortgagedProduct.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.MORTGAGED_PRODUCT + "不能为空；");
                                }
                                else
                                {
                                    isMortgagedProduct = (mortgagedProduct == "是") ? true : false;
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.MORTGAGED_PRODUCT + ":" + licenseNumber + "；");
                                //--------------------------------------------------------------------------------------

                                if (errorCount != errorList.Count)
                                {
                                    StringBuilder sbError = new StringBuilder();
                                    for (int i = errorCount; i < errorList.Count; i++)
                                    {
                                        sbError.Append(errorList[i]);
                                    }
                                    StringBuilder sbData = new StringBuilder();
                                    rowDataList.ForEach(x =>
                                    {
                                        sbData.Append(x);
                                    });
                                    ImportErrorLog importErrorLog = new ImportErrorLog()
                                    {
                                        ErrorRowIndex = rowIndex,
                                        ImportFileLogID = importFileLog.ID,
                                        ErrorMsg = sbError.ToString(),
                                        ErrorRowData = sbData.ToString(),
                                    };
                                    importErrorLogRepository.Add(importErrorLog);
                                    errorList.Insert(errorCount, "第" + rowIndex + "行");
                                    errorRowCount++;
                                }


                                if (checkFinished)
                                {
                                    //bool currentEntityExist = PageProcureOrderAppRepository.GetList(x => x.OrderCode == orderCode).Any();
                                    StockIn currentEntity = stockInRepository.GetList(x => x.Code.ToLower() == orderCode.ToLower()).FirstOrDefault();
                                    ZhongDing.Domain.Models.StockInImportData stockInImportData = stockInImportDataRepository.GetList(x => x.Code.ToLower() == orderCode.ToLower()).FirstOrDefault();
                                    if (currentEntity == null)
                                    {
                                        currentEntity = new StockIn();
                                        currentEntity.Code = orderCode;
                                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
                                        currentEntity.SupplierID = supplierID.Value;
                                        currentEntity.CreatedBy = CurrentUser.UserID;
                                        currentEntity.EntryDate = orderDate.Value;
                                        currentEntity.IsImport = true;

                                        stockInRepository.Add(currentEntity);
                                        if (!orderCodeList.Contains(orderCode))
                                            orderCodeList.Add(orderCode);
                                        unitOfWork.SaveChanges();
                                        stockInImportData = new ZhongDing.Domain.Models.StockInImportData()
                                        {
                                            Code = orderCode,
                                            EntryDate = orderDate,
                                            SupplierName = supplierName,
                                            StockInImportFileLogID = stockInImportFileLog.ID,
                                            StockInID = currentEntity.ID,

                                        };
                                        stockInImportDataRepository.Add(stockInImportData);
                                    }
                                    if (orderCodeList.Contains(orderCode))
                                    {

                                        StockInDetail stockInDetail = new StockInDetail()
                                        {
                                            BatchNumber = batchNumber,
                                            ExpirationDate = expirationDate,
                                            InQty = stockInCount.Value,
                                            IsMortgagedProduct = isMortgagedProduct,
                                            LicenseNumber = licenseNumber,
                                            ProcureOrderAppDetailID = 0,//这个ID的确认是个难点，可能要放弃这个导入方案
                                            ProcureOrderAppID = procureOrderApplicationID.Value,
                                            ProcurePrice = procurePrice.Value,
                                            ProductID = productID.Value,
                                            ProductSpecificationID = productSpecificationID.Value,
                                            WarehouseID = warehouseID.Value,
                                        };
                                        stockInDetail.WarehouseID = warehouseID.Value;
                                        stockInDetail.ProductID = productID.Value;
                                        stockInDetail.ProductSpecificationID = productSpecificationID.Value;
                                        stockInDetail.InQty = stockInCount.Value;

                                        currentEntity.StockInDetail.Add(stockInDetail);
                                        unitOfWork.SaveChanges();

                                        StockInDetailImportData stockInDetailImportData = new StockInDetailImportData()
                                        {
                                            InQty = stockInCount.Value,
                                            ProcureOrderAppDetailID = stockInDetail.ID,
                                            ProcureOrderAppID = procureOrderApplicationID.Value,
                                            BatchNumber = batchNumber,
                                            ExpirationDate = expirationDate,
                                            IsMortgagedProduct = isMortgagedProduct,
                                            LicenseNumber = licenseNumber,
                                            ProcurePrice = procurePrice.Value,
                                            ProductName = productName,
                                            ProductSpecification = specification,

                                            WarehouseName = warehouseName,
                                            UnitOfMeasurement = unitName
                                        };
                                        stockInImportData.StockInDetailImportData.Add(stockInDetailImportData);

                                        unitOfWork.SaveChanges();
                                        succeedCount++;
                                    }

                                    savedData = true;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        if (errorList.Count > 0)
                        {
                            checkFinished = false;
                            StringBuilder sb = new StringBuilder();
                            errorList.ForEach(x =>
                            {
                                sb.AppendLine(x + "<br/>");
                            });
                            divInfo.Visible = true;
                            lblError.Text = sb.ToString();

                            importFileLog.ImportEndDate = DateTime.Now;
                            importFileLog.ImportStatusID = (int)EImportStatus.ImportError;
                            importFileLog.FailedCount = errorRowCount;
                            importFileLog.SucceedCount = 0;
                            importFileLog.TotalCount = dsData.Tables[0].Rows.Count;

                            PageStockInImportFileLogRepository.Save();
                            unitOfWork.SaveChanges();
                            BindEntities(true);

                        }
                        else
                        {
                            if (savedData == false)
                            {
                                checkFinished = true;
                                goto checkOrWrite;
                            }

                            unitOfWork.SaveChanges();

                            importFileLog.ImportEndDate = DateTime.Now;
                            importFileLog.ImportStatusID = (int)EImportStatus.Completed;
                            importFileLog.FailedCount = 0;
                            importFileLog.SucceedCount = succeedCount;
                            importFileLog.TotalCount = dsData.Tables[0].Rows.Count;
                            PageStockInImportFileLogRepository.Save();

                            this.Master.BaseNotification.Show("导入完成");
                        }
                    }


                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                importFileLog.ImportEndDate = DateTime.Now;
                importFileLog.ImportStatusID = (int)EImportStatus.ImportError;
                //importFileLog.FailedCount = errorList.Count;
                importFileLog.SucceedCount = 0;
                importFileLog.TotalCount = dsData.Tables[0].Rows.Count;
                PageStockInImportFileLogRepository.Save();
            }
            txtFileName.Text = string.Empty;
            BindEntities(true);

        }


        protected override EPermission PagePermissionID()
        {
            return EPermission.DataImport;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Create;
        }

    }
}