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
    public partial class ProcureOrderImportData : BasePage
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


        private IProcureOrderApplicationImportFileLogRepository _PageProcureOrderApplicationImportFileLogRepository;
        private IProcureOrderApplicationImportFileLogRepository PageProcureOrderApplicationImportFileLogRepository
        {
            get
            {
                if (_PageProcureOrderApplicationImportFileLogRepository == null)
                    _PageProcureOrderApplicationImportFileLogRepository = new ProcureOrderApplicationImportFileLogRepository();

                return _PageProcureOrderApplicationImportFileLogRepository;
            }
        }


        #endregion

        //protected override int GetCurrentWorkFlowID()
        //{
        //    return (int)EWorkflow.ProcureOrder;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ProcureOrderData;
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
            var uiSearchObj = new UISearchDCImportFileLog()
            {
                ImportDataTypeID = (int)EImportDataType.ProcureOrderData,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate
            };



            int totalRecords;

            var entities = PageProcureOrderApplicationImportFileLogRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

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

            int importDataTypeID = (int)EImportDataType.ProcureOrderData;
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
            ProcureOrderApplicationImportFileLog procureOrderApplicationImportFileLog = new ProcureOrderApplicationImportFileLog()
            {

            };
            var importFileLog = new ImportFileLog()
            {
                ImportDataTypeID = (int)EImportDataType.ProcureOrderData,
                ImportStatusID = (int)EImportStatus.ToBeImport,
                FileName = txtFileName.Text.Trim() + uploadFile.GetExtension(),
                FilePath = uploadFilePath + fileName
            };
            procureOrderApplicationImportFileLog.ImportFileLog = importFileLog;
            PageProcureOrderApplicationImportFileLogRepository.Add(procureOrderApplicationImportFileLog);
            PageProcureOrderApplicationImportFileLogRepository.Save();


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

                PageProcureOrderApplicationImportFileLogRepository.Save();

                using (TransactionScope transaction = new TransactionScope())
                {
                    using (IUnitOfWork unitOfWork = new UnitOfWork())
                    {
                        DbModelContainer db = unitOfWork.GetDbModel();

                        ISupplierRepository supplierRepository = new SupplierRepository();
                        IWarehouseRepository warehouseRepository = new WarehouseRepository();
                        IProductRepository productRepository = new ProductRepository();
                        IProductSpecificationRepository productSpecificationRepository = new ProductSpecificationRepository();
                        IProcureOrderApplicationRepository procureOrderApplicationRepository = new ProcureOrderApplicationRepository();
                        IProcureOrderApplicationImportDataRepository procureOrderApplicationImportDataRepository = new ProcureOrderApplicationImportDataRepository();
                        IImportErrorLogRepository importErrorLogRepository = new ImportErrorLogRepository();
                        supplierRepository.SetDbModel(db);
                        warehouseRepository.SetDbModel(db);
                        productRepository.SetDbModel(db);
                        procureOrderApplicationRepository.SetDbModel(db);
                        procureOrderApplicationImportDataRepository.SetDbModel(db);
                        importErrorLogRepository.SetDbModel(db);

                        string errorMsg;
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
                                string orderCode = row[GlobalConst.ImportDataColumns.ORDER_CODE].ToStringOrNull();
                                if (orderCode.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.ORDER_CODE + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.ORDER_CODE + ":" + orderCode + "；");
                                DateTime? orderDate = row[GlobalConst.ImportDataColumns.ORDER_DATE].ToDateTimeOrNull();
                                if (!orderDate.HasValue)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.ORDER_DATE + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.ORDER_DATE + ":" + orderDate + "；");
                                string supplierName = row[GlobalConst.ImportDataColumns.SUPPLIER_NAME].ToStringOrNull();
                                int? supplierID = null;
                                if (supplierName.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.SUPPLIER_NAME + "不能为空；");
                                }
                                else
                                {
                                    var supplier = supplierRepository.GetBySupplierName(supplierName);
                                    if (supplier != null)
                                        supplierID = supplier.ID;
                                    else
                                        errorList.Add(GlobalConst.ImportDataColumns.SUPPLIER_NAME + "(" + supplierName + ")" + "不存在；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.SUPPLIER_NAME + ":" + supplierName + "；");
                                DateTime? estDeliveryDate = row[GlobalConst.ImportDataColumns.ESTDELIVERY_DATE].ToDateTimeOrNull();
                                int? warehouseID = null;
                                if (!estDeliveryDate.HasValue)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.ESTDELIVERY_DATE + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.ESTDELIVERY_DATE + ":" + estDeliveryDate + "；");
                                string warehouseName = row[GlobalConst.ImportDataColumns.STOCKIN_WAREHOUSE_NAME].ToStringOrNull();
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
                                int? procureCount = row[GlobalConst.ImportDataColumns.PROCURE_COUNT].ToIntOrNull();
                                if (procureCount.GetValueOrDefault(0) == 0)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PROCURE_COUNT + "必须大于0；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PROCURE_COUNT + ":" + procureCount + "；");
                                decimal? procurePrice = row[GlobalConst.ImportDataColumns.PROCURE_PRICE].ToDecimalOrNull();
                                if (procurePrice.GetValueOrDefault(0) == 0)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PROCURE_PRICE + "必须大于0；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PROCURE_PRICE + ":" + procurePrice + "；");
                                decimal? procureTotalAmount = row[GlobalConst.ImportDataColumns.PROCURE_PRICE].ToDecimalOrNull();
                                if (procureTotalAmount.GetValueOrDefault(0) == 0)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PROCURE_TOTAL_AMOUNT + "必须大于0；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PROCURE_TOTAL_AMOUNT + ":" + procureTotalAmount + "；");
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
                                    ProcureOrderApplication currentEntity = procureOrderApplicationRepository.GetList(x => x.OrderCode.ToLower() == orderCode.ToLower()).FirstOrDefault();
                                    ProcureOrderApplicationImportData procureOrderApplicationImportData = procureOrderApplicationImportDataRepository.GetList(x => x.OrderCode.ToLower() == orderCode.ToLower()).FirstOrDefault();
                                    if (currentEntity == null)
                                    {
                                        currentEntity = new ProcureOrderApplication();
                                        currentEntity.OrderCode = orderCode;
                                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
                                        currentEntity.SupplierID = supplierID.Value;
                                        currentEntity.CreatedBy = CurrentUser.UserID;
                                        currentEntity.EstDeliveryDate = estDeliveryDate.Value;
                                        currentEntity.OrderDate = orderDate.Value;
                                        currentEntity.IsImport = true;
                                        procureOrderApplicationRepository.Add(currentEntity);
                                        if (!orderCodeList.Contains(orderCode))
                                            orderCodeList.Add(orderCode);
                                        unitOfWork.SaveChanges();
                                        procureOrderApplicationImportData = new ProcureOrderApplicationImportData()
                                        {
                                            OrderCode = orderCode,
                                            SupplierName = supplierName,
                                            EstDeliveryDate = estDeliveryDate.Value,
                                            OrderDate = orderDate.Value,
                                            ProcureOrderApplicationImportFileLogID = procureOrderApplicationImportFileLog.ID,
                                            ProcureOrderApplicationID = currentEntity.ID,
                                        };
                                        procureOrderApplicationImportDataRepository.Add(procureOrderApplicationImportData);
                                    }
                                    if (orderCodeList.Contains(orderCode))
                                    {

                                        ProcureOrderAppDetail procureOrderAppDetail = new ProcureOrderAppDetail();
                                        procureOrderAppDetail.WarehouseID = warehouseID.Value;
                                        procureOrderAppDetail.ProductID = productID.Value;
                                        procureOrderAppDetail.ProductSpecificationID = productSpecificationID.Value;
                                        procureOrderAppDetail.ProcurePrice = procurePrice.Value;
                                        procureOrderAppDetail.ProcureCount = procureCount.Value;
                                        procureOrderAppDetail.TotalAmount = procureTotalAmount.Value;
                                        //procureOrderAppDetail.TaxAmount = (decimal?)txtTaxAmount.Value;
                                        currentEntity.ProcureOrderAppDetail.Add(procureOrderAppDetail);
                                        unitOfWork.SaveChanges();

                                        ProcureOrderAppDetailImportData procureOrderAppDetailImportData = new ProcureOrderAppDetailImportData()
                                        {
                                            ProcureCount = procureCount.Value,
                                            ProcureOrderAppDetailID = procureOrderAppDetail.ID,
                                            ProcurePrice = procurePrice.Value,
                                            ProductName = productName,
                                            Specification = specification,
                                            TotalAmount = procureTotalAmount.Value,
                                            WarehouseName = warehouseName,
                                            UnitOfMeasurement = unitName
                                        };
                                        procureOrderApplicationImportData.ProcureOrderAppDetailImportData.Add(procureOrderAppDetailImportData);

                                        unitOfWork.SaveChanges();
                                        succeedCount++;
                                    }

                                    savedData = true;
                                }
                            }
                            catch
                            { }
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

                            PageProcureOrderApplicationImportFileLogRepository.Save();
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
                            PageProcureOrderApplicationImportFileLogRepository.Save();

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
                PageProcureOrderApplicationImportFileLogRepository.Save();
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