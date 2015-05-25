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
    public partial class ClientSaleApplicationImportData : BasePage
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


        private IClientSaleApplicationImportFileLogRepository _PageClientSaleApplicationImportFileLogRepository;
        private IClientSaleApplicationImportFileLogRepository PageClientSaleApplicationImportFileLogRepository
        {
            get
            {
                if (_PageClientSaleApplicationImportFileLogRepository == null)
                    _PageClientSaleApplicationImportFileLogRepository = new ClientSaleApplicationImportFileLogRepository();

                return _PageClientSaleApplicationImportFileLogRepository;
            }
        }


        private ISalesOrderApplicationImportDataRepository _PageSalesOrderApplicationImportDataRepository;
        private ISalesOrderApplicationImportDataRepository PageSalesOrderApplicationImportDataRepository
        {
            get
            {
                if (_PageSalesOrderApplicationImportDataRepository == null)
                    _PageSalesOrderApplicationImportDataRepository = new SalesOrderApplicationImportDataRepository();

                return _PageSalesOrderApplicationImportDataRepository;
            }
        }

        #endregion

        //protected override int GetCurrentWorkFlowID()
        //{
        //    return (int)EWorkflow.ProcureOrder;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientSaleApplicationData;
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
            var uiSearchObj = new UISearchClientSaleApplicationImportFileLog()
            {
                ImportDataTypeID = (int)EImportDataType.ProcureOrderData,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate
            };



            int totalRecords;

            var entities = PageClientSaleApplicationImportFileLogRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

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
            if (!rcbxSaleOrderType.SelectedValue.ToIntOrNull().HasValue)
                cvSaleOrderType.IsValid = false;

            if (!IsValid)
                return;
            if (radAsyncUpload.UploadedFiles.Count == 0)
            {
                //GlobalConst.NotificationSettings
                this.Master.BaseNotification.Show("请选择文件");
                return;
            }
            UploadedFile uploadFile = radAsyncUpload.UploadedFiles[0];


            string uploadFilePath = WebConfig.UploadFilePathClientSaleApplicationData;
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
            ClientSaleApplicationImportFileLog clientSaleApplicationImportFileLog = new ClientSaleApplicationImportFileLog()
            {

            };
            var importFileLog = new ImportFileLog()
            {
                ImportDataTypeID = (int)EImportDataType.ClientOrderData,
                ImportStatusID = (int)EImportStatus.ToBeImport,
                FileName = txtFileName.Text.Trim() + uploadFile.GetExtension(),
                FilePath = uploadFilePath + fileName
            };
            clientSaleApplicationImportFileLog.ImportFileLog = importFileLog;
            PageClientSaleApplicationImportFileLogRepository.Add(clientSaleApplicationImportFileLog);
            PageClientSaleApplicationImportFileLogRepository.Save();


            DataSet dsData = null;
            try
            {
                dsData = ExcelHelper.ConvertExcelToDataSet(fileNameFullPath);
            }
            catch (Exception ex)
            {
                divInfo.Visible = true;
                lblError.Text = "文件格式错误，请检查文件格式";
                ShowErrorMessage("导入失败，请查看错误信息");
                return;
            }

            if (dsData == null || dsData.Tables.Count == 0 || dsData.Tables[0].Rows.Count == 0)
            {
                divInfo.Visible = true;
                lblError.Text = "文件内容不能为空";
                ShowErrorMessage("导入失败，请查看错误信息");
                return;
            }
            try
            {
                int totalCount = dsData.Tables[0].Rows.Count;

                importFileLog.ImportBeginDate = DateTime.Now;
                importFileLog.ImportStatusID = (int)EImportStatus.Importing;

                PageClientSaleApplicationImportFileLogRepository.Save();

                using (TransactionScope transaction = new TransactionScope())
                {
                    using (IUnitOfWork unitOfWork = new UnitOfWork())
                    {
                        DbModelContainer db = unitOfWork.GetDbModel();

                        ISupplierRepository supplierRepository = new SupplierRepository();
                        IWarehouseRepository warehouseRepository = new WarehouseRepository();
                        IProductRepository productRepository = new ProductRepository();
                        IProductSpecificationRepository productSpecificationRepository = new ProductSpecificationRepository();

                        ISalesOrderApplicationRepository salesOrderApplicationRepository = new SalesOrderApplicationRepository();
                        ISalesOrderApplicationImportDataRepository salesOrderApplicationImportDataRepository = new SalesOrderApplicationImportDataRepository();
                        IClientSaleApplicationRepository clientSaleApplicationRepository = new ClientSaleApplicationRepository();
                        IImportErrorLogRepository importErrorLogRepository = new ImportErrorLogRepository();
                        IClientUserRepository clientUserRepository = new ClientUserRepository();
                        IClientCompanyRepository clientCompanyRepository = new ClientCompanyRepository();
                        ISalesOrderAppDetailImportDataRepository salesOrderAppDetailImportDataRepository = new SalesOrderAppDetailImportDataRepository();
                        IClientSaleApplicationImportDataRepository clientSaleApplicationImportDataRepository = new ClientSaleApplicationImportDataRepository();
                        supplierRepository.SetDbModel(db);
                        warehouseRepository.SetDbModel(db);
                        productRepository.SetDbModel(db);

                        importErrorLogRepository.SetDbModel(db);
                        salesOrderApplicationRepository.SetDbModel(db);
                        salesOrderApplicationImportDataRepository.SetDbModel(db);
                        clientSaleApplicationRepository.SetDbModel(db);
                        clientUserRepository.SetDbModel(db);
                        clientCompanyRepository.SetDbModel(db);
                        salesOrderAppDetailImportDataRepository.SetDbModel(db);
                        salesOrderAppDetailImportDataRepository.SetDbModel(db);
                        clientSaleApplicationImportDataRepository.SetDbModel(db);
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
                                //------------------------------------------------------------------------------------------
                                DateTime? orderDate = row[GlobalConst.ImportDataColumns.ORDER_DATE].ToDateTimeOrNull();
                                if (!orderDate.HasValue)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.ORDER_DATE + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.ORDER_DATE + ":" + orderDate + "；");
                                //---------------------------------------------------------------------------------------
                                string orderCode = row[GlobalConst.ImportDataColumns.CLIENT_ORDER_CODE].ToStringOrNull();
                                if (orderCode.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.CLIENT_ORDER_CODE + "不能为空；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.CLIENT_ORDER_CODE + ":" + orderCode + "；");

                                //---------------------------------------------------------------------------------------
                                string clientUserName = row[GlobalConst.ImportDataColumns.CLIENT_USER_NAME].ToStringOrNull();
                                int? clientUserID = null;
                                ClientUser clientUser = null;
                                if (clientUserName.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.CLIENT_USER_NAME + "不能为空；");
                                }
                                else
                                {
                                    clientUser = clientUserRepository.GetClientUserByClientName(clientUserName);
                                    if (clientUser != null)
                                        clientUserID = clientUser.ID;
                                    else
                                        errorList.Add(GlobalConst.ImportDataColumns.CLIENT_USER_NAME + "(" + clientUserName + ")" + "不存在；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.CLIENT_USER_NAME + ":" + clientUserName + "；");
                                //---------------------------------------------------------------------------------------
                                string clientCompanyName = row[GlobalConst.ImportDataColumns.CLIENT_COMPANY_NAME].ToStringOrNull();
                                int? clientCompanyID = null;
                                ClientCompany clientCompany = null;
                                if (clientCompanyName.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.CLIENT_COMPANY_NAME + "不能为空；");
                                }
                                else
                                {
                                    if (clientUserID.HasValue)
                                    {
                                        clientCompany = clientCompanyRepository.GetClientCompanyByClientCompanyNameAndClientUserID(clientCompanyName, clientUserID.Value);
                                        if (clientCompany != null)
                                            clientCompanyID = clientCompany.ID;
                                        else
                                            errorList.Add(GlobalConst.ImportDataColumns.CLIENT_COMPANY_NAME + "(" + clientCompanyName + ")" + "和(" + clientUserName + ")不匹配；");
                                    }
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.CLIENT_COMPANY_NAME + ":" + clientCompanyName + "；");
                                //---------------------------------------------------------------------------------------
                                string warehouseName = row[GlobalConst.ImportDataColumns.STOCKOUT_WAREHOUSE_NAME].ToStringOrNull();
                                int? warehouseID = null;
                                if (warehouseName.IsNullOrWhiteSpace())
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.STOCKOUT_WAREHOUSE_NAME + "不能为空；");
                                }
                                else
                                {
                                    var warehouse = warehouseRepository.GetByWarehouseName(warehouseName);
                                    if (warehouse != null)
                                        warehouseID = warehouse.ID;
                                    else
                                        errorList.Add(GlobalConst.ImportDataColumns.STOCKOUT_WAREHOUSE_NAME + "(" + warehouseName + ")" + "不存在；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.STOCKOUT_WAREHOUSE_NAME + ":" + warehouseName + "；");
                                //---------------------------------------------------------------------------------------
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
                                //---------------------------------------------------------------------------------------
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
                                            productCode + ")不匹配；");
                                    }
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PRODUCT_NAME + ":" + productName + "；");
                                //---------------------------------------------------------------------------------------
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
                                //---------------------------------------------------------------------------------------
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
                                //---------------------------------------------------------------------------------------
                                int? procureCount = row[GlobalConst.ImportDataColumns.PROCURE_COUNT].ToIntOrNull();
                                if (procureCount.GetValueOrDefault(0) == 0)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PROCURE_COUNT + "必须大于0；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PROCURE_COUNT + ":" + procureCount + "；");
                                //---------------------------------------------------------------------------------------
                                decimal? procurePrice = row[GlobalConst.ImportDataColumns.PRICE].ToDecimalOrNull();
                                if (procurePrice.GetValueOrDefault(0) == 0)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.PRICE + "必须大于0；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.PRICE + ":" + procurePrice + "；");
                                //---------------------------------------------------------------------------------------
                                decimal? procureTotalAmount = row[GlobalConst.ImportDataColumns.AMOUNT].ToDecimalOrNull();
                                if (procureTotalAmount.GetValueOrDefault(0) == 0)
                                {
                                    errorList.Add(GlobalConst.ImportDataColumns.AMOUNT + "必须大于0；");
                                }
                                rowDataList.Add(GlobalConst.ImportDataColumns.AMOUNT + ":" + procureTotalAmount + "；");

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

                                    SalesOrderApplication currentEntity = salesOrderApplicationRepository.GetList(x => x.OrderCode.ToLower() == orderCode.ToLower()).FirstOrDefault();
                                    SalesOrderApplicationImportData salesOrderApplicationImportData = salesOrderApplicationImportDataRepository.GetList(x => x.OrderCode.ToLower() == orderCode.ToLower()).FirstOrDefault();
                                    if (currentEntity == null)
                                    {
                                        currentEntity = new SalesOrderApplication()
                                        {
                                            OrderCode = orderCode,
                                            OrderDate = orderDate.Value,
                                            IsImport = true,
                                            IsStop = false,
                                            SaleOrderTypeID = rcbxSaleOrderType.SelectedValue.ToInt(),


                                        };
                                        ClientSaleApplication clientSaleApplication = new ClientSaleApplication()
                                        {
                                            CompanyID = CurrentUser.CompanyID,
                                            WorkflowStatusID = (int)EWorkflowStatus.TemporarySave,
                                            ClientCompanyID = clientCompanyID.Value,
                                            ClientContactID = null,
                                            ClientUserID = clientUserID.Value,
                                            SalesOrderApplication = currentEntity,
                                            DeliveryModeID = (int)EDeliveryMode.ReceiptedDelivery,
                                        };

                                        var eSaleOrderType = (ESaleOrderType)rcbxSaleOrderType.SelectedValue.ToInt();

                                        switch (eSaleOrderType)
                                        {
                                            case ESaleOrderType.AttractBusinessMode:
                                                clientSaleApplication.DeliveryModeID = rcbxSaleOrderType.SelectedValue.ToInt();
                                                clientSaleApplication.IsGuaranteed = false;
                                                clientSaleApplication.Guaranteeby = null;
                                                clientSaleApplication.GuaranteeExpirationDate = null;
                                                break;
                                            case ESaleOrderType.AttachedMode:
                                                clientSaleApplication.DeliveryModeID = null;
                                                clientSaleApplication.IsGuaranteed = false;
                                                clientSaleApplication.Guaranteeby = null;
                                                clientSaleApplication.GuaranteeExpirationDate = null;
                                                break;
                                        }

                                        clientSaleApplicationRepository.Add(clientSaleApplication);


                                        if (!orderCodeList.Contains(orderCode))
                                            orderCodeList.Add(orderCode);
                                        unitOfWork.SaveChanges();

                                        salesOrderApplicationImportData = new SalesOrderApplicationImportData()
                                        {
                                            OrderCode = orderCode,

                                            OrderDate = orderDate.Value,
                                            SaleOrderType = rcbxSaleOrderType.SelectedItem.Text,
                                            SalesOrderApplicationID = currentEntity.ID,
                                            SaleApplicationImportFileLogID = clientSaleApplicationImportFileLog.ID

                                        };
                                        salesOrderApplicationImportDataRepository.Add(salesOrderApplicationImportData);

                                        ZhongDing.Domain.Models.ClientSaleApplicationImportData clientSaleApplicationImportData = new ZhongDing.Domain.Models.ClientSaleApplicationImportData()
                                        {
                                            ClientCompanyName = clientCompanyName,
                                            ClientUserName = clientUserName,
                                            SalesOrderApplicationImportData = salesOrderApplicationImportData
                                        };

                                        clientSaleApplicationImportDataRepository.Add(clientSaleApplicationImportData);
                                    }
                                    if (orderCodeList.Contains(orderCode))
                                    {

                                        SalesOrderAppDetail salesOrderAppDetail = new SalesOrderAppDetail();
                                        salesOrderAppDetail.WarehouseID = warehouseID.Value;
                                        salesOrderAppDetail.ProductID = productID.Value;
                                        salesOrderAppDetail.ProductSpecificationID = productSpecificationID.Value;
                                        salesOrderAppDetail.SalesPrice = procurePrice.Value;
                                        salesOrderAppDetail.Count = procureCount.Value;
                                        salesOrderAppDetail.TotalSalesAmount = procureTotalAmount.Value;
                                        //procureOrderAppDetail.TaxAmount = (decimal?)txtTaxAmount.Value;
                                        currentEntity.SalesOrderAppDetail.Add(salesOrderAppDetail);
                                        unitOfWork.SaveChanges();

                                        SalesOrderAppDetailImportData salesOrderAppDetailImportData = new SalesOrderAppDetailImportData()
                                        {
                                            Count = procureCount.Value,
                                            SalesOrderAppDetailID = salesOrderAppDetail.ID,
                                            SalesPrice = procurePrice.Value,
                                            ProductName = productName,
                                            ProductSpecification = specification,
                                            TotalSalesAmount = procureTotalAmount.Value,
                                            WarehouseName = warehouseName,
                                            UnitOfMeasurement = unitName,
                                            SalesOrderApplicationImportDataID = salesOrderApplicationImportData.ID
                                        };
                                        salesOrderAppDetailImportDataRepository.Add(salesOrderAppDetailImportData);

                                        unitOfWork.SaveChanges();
                                        succeedCount++;
                                    }

                                    savedData = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                divInfo.Visible = true;
                                lblError.Text = ex.ToString();
                                BindEntities(true);
                                ShowErrorMessage("导入失败，请查看错误信息");
                                return;
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

                            PageClientSaleApplicationImportFileLogRepository.Save();
                            unitOfWork.SaveChanges();
                            BindEntities(true);
                            ShowErrorMessage("导入失败，请查看错误信息");
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
                            PageClientSaleApplicationImportFileLogRepository.Save();

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
                PageClientSaleApplicationImportFileLogRepository.Save();
            }
            txtFileName.Text = string.Empty;
            BindEntities(true);

        }


    }
}