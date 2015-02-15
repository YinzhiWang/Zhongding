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
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Sales.Editors
{
    public partial class ClientOrderProductMaintain : WorkflowBasePage
    {
        #region Fields

        /// <summary>
        /// 所属的实体ID
        /// </summary>
        /// <value>The owner entity ID.</value>
        private int? OwnerEntityID
        {
            get
            {
                return WebUtility.GetValueFromQueryString("OwnerEntityID");
            }
        }

        #endregion

        #region Members

        private IClientSaleApplicationRepository _PageClientSaleAppRepository;
        private IClientSaleApplicationRepository PageClientSaleAppRepository
        {
            get
            {
                if (_PageClientSaleAppRepository == null)
                    _PageClientSaleAppRepository = new ClientSaleApplicationRepository();

                return _PageClientSaleAppRepository;
            }
        }

        private IWarehouseRepository _PageWarehouseRepository;
        private IWarehouseRepository PageWarehouseRepository
        {
            get
            {
                if (_PageWarehouseRepository == null)
                    _PageWarehouseRepository = new WarehouseRepository();

                return _PageWarehouseRepository;
            }
        }

        private IProductRepository _PageProductRepository;
        private IProductRepository PageProductRepository
        {
            get
            {
                if (_PageProductRepository == null)
                    _PageProductRepository = new ProductRepository();

                return _PageProductRepository;
            }
        }

        private IProductSpecificationRepository _PageProductSpecificationRepository;
        private IProductSpecificationRepository PageProductSpecificationRepository
        {
            get
            {
                if (_PageProductSpecificationRepository == null)
                    _PageProductSpecificationRepository = new ProductSpecificationRepository();

                return _PageProductSpecificationRepository;
            }
        }

        private ClientSaleApplication _CurrentOwnerEntity;
        private ClientSaleApplication CurrentOwnerEntity
        {
            get
            {
                if (_CurrentOwnerEntity == null)
                    if (this.OwnerEntityID.HasValue && this.OwnerEntityID > 0)
                        _CurrentOwnerEntity = PageClientSaleAppRepository.GetByID(this.OwnerEntityID);

                return _CurrentOwnerEntity;
            }
        }

        private IClientInfoRepository _PageClientInfoRepository;
        private IClientInfoRepository PageClientInfoRepository
        {
            get
            {
                if (_PageClientInfoRepository == null)
                    _PageClientInfoRepository = new ClientInfoRepository();

                return _PageClientInfoRepository;
            }
        }

        private IList<int> _CanAddUserIDs;
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientOrder);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientOrder);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanAuditUserIDs;
        private IList<int> CanAuditUserIDs
        {
            get
            {
                if (_CanAuditUserIDs == null)
                    _CanAuditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditClientOrder);

                return _CanAuditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientOrder;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentOwnerEntity == null)
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                BindWarehouses();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindWarehouses()
        {
            var uiSearchObj = new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    CompanyID = CurrentUser.CompanyID
                }
            };

            var warehouses = PageWarehouseRepository.GetDropdownItems(uiSearchObj);

            rcbxWarehouse.DataSource = warehouses;
            rcbxWarehouse.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxWarehouse.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxWarehouse.DataBind();

            rcbxWarehouse.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts(IList<int> excludeItemValues = null)
        {
            rcbxProduct.ClearSelection();
            rcbxProduct.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem()
            {
                Extension = new UISearchExtension { CompanyID = CurrentUser.CompanyID }
            };

            if (excludeItemValues != null)
                uiSearchObj.ExcludeItemValues = excludeItemValues;

            var products = PageProductRepository.GetDropdownItems(uiSearchObj);

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProductSpecifications()
        {
            ddlProductSpecification.Items.Clear();

            if (!string.IsNullOrEmpty(rcbxProduct.SelectedValue))
            {
                int productID;
                if (int.TryParse(rcbxProduct.SelectedValue, out productID))
                {
                    var productSpecifications = PageProductSpecificationRepository.GetDropdownItems(new UISearchDropdownItem()
                    {
                        Extension = new UISearchExtension { ProductID = productID }
                    });

                    ddlProductSpecification.DataSource = productSpecifications;
                    ddlProductSpecification.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    ddlProductSpecification.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    ddlProductSpecification.DataBind();
                }
            }
        }

        private decimal? GetSalesPrice()
        {
            decimal? salesPrice = null;

            int productID;
            int productSpecificationID;
            int warehouseID;

            if (int.TryParse(rcbxProduct.SelectedValue, out productID)
                && int.TryParse(ddlProductSpecification.SelectedValue, out productSpecificationID)
                && int.TryParse(rcbxWarehouse.SelectedValue, out warehouseID))
            {
                int clientUserID = this.CurrentOwnerEntity.ClientUserID;
                int clientCompanyID = this.CurrentOwnerEntity.ClientCompanyID;

                var clientInfo = PageClientInfoRepository.GetByConditions(clientUserID, clientCompanyID);

                if (clientInfo != null)
                {
                    bool isHighPrice = false;

                    var warehouse = PageWarehouseRepository.GetByID(warehouseID);

                    if (warehouse != null && warehouse.SaleTypeID == (int)ESaleType.HighPrice)
                        isHighPrice = true;

                    var clientProductSetting = clientInfo.ClientInfoProductSetting
                        .FirstOrDefault(x => x.IsDeleted == false && x.ProductID == productID
                            && x.ProductSpecificationID == productSpecificationID);

                    //从ClientInfoProductSetting取值
                    if (clientProductSetting != null)
                    {
                        if (isHighPrice)
                            salesPrice = clientProductSetting.HighPrice;
                        else
                            salesPrice = clientProductSetting.BasicPrice;
                    }
                    else//从货品定价配置取值
                    {
                        if (isHighPrice)
                        {
                            IProductHighPriceRepository highPriceRepository = new ProductHighPriceRepository();

                            var productPrice = highPriceRepository.GetList(x => x.ProductID == productID
                                  && x.ProductSpecificationID == productSpecificationID).FirstOrDefault();

                            if (productPrice != null)
                                salesPrice = productPrice.HighPrice;
                        }
                        else
                        {
                            IProductBasicPriceRepository basicPriceRepository = new ProductBasicPriceRepository();

                            var productPrice = basicPriceRepository.GetList(x => x.ProductID == productID
                                  && x.ProductSpecificationID == productSpecificationID).FirstOrDefault();

                            if (productPrice != null)
                                salesPrice = productPrice.SalePrice;
                        }
                    }
                }
            }

            return salesPrice;
        }

        private void LoadCurrentEntity()
        {
            if (CurrentOwnerEntity == null) return;

            var eWorkflowStatus = (EWorkflowStatus)CurrentOwnerEntity.WorkflowStatusID;

            switch (eWorkflowStatus)
            {
                case EWorkflowStatus.TemporarySave:
                case EWorkflowStatus.ReturnBasicInfo:
                    if (CurrentOwnerEntity.CreatedBy == CurrentUser.UserID
                        || CanEditUserIDs.Contains(CurrentUser.UserID))
                        btnSave.Enabled = true;
                    else
                        DisabledBasicInfoControls();

                    break;
                case EWorkflowStatus.Submit:
                    if (CanAuditUserIDs.Contains(CurrentUser.UserID))
                    {
                        DisabledBasicInfoControls();

                        txtSalesPrice.Enabled = true;
                        btnSave.Enabled = true;
                    }

                    break;

                default:
                    DisabledBasicInfoControls();
                    break;
            }

            if (CurrentOwnerEntity.SalesOrderApplication != null)
            {
                var salesOrderAppDetails = CurrentOwnerEntity.SalesOrderApplication.SalesOrderAppDetail
                    .Where(x => x.IsDeleted == false);

                var salesOrderAppDetail = salesOrderAppDetails.Where(x => x.ID == this.CurrentEntityID).FirstOrDefault();

                if (salesOrderAppDetail != null)
                {
                    rcbxWarehouse.SelectedValue = salesOrderAppDetail.WarehouseID.ToString();

                    BindProducts();

                    rcbxProduct.SelectedValue = salesOrderAppDetail.ProductID.ToString();

                    BindProductSpecifications();

                    ddlProductSpecification.SelectedValue = salesOrderAppDetail.ProductSpecificationID.ToString();

                    if (salesOrderAppDetail.ProductSpecification != null)
                    {
                        if (salesOrderAppDetail.ProductSpecification.UnitOfMeasurement != null)
                            lblUnitOfMeasurement.Text = salesOrderAppDetail.ProductSpecification.UnitOfMeasurement.UnitName;

                        int numberInLargePackage = salesOrderAppDetail.ProductSpecification.NumberInLargePackage.HasValue
                            ? salesOrderAppDetail.ProductSpecification.NumberInLargePackage.Value : 1;

                        lblNumberOfPackages.Text = (salesOrderAppDetail.Count / numberInLargePackage).ToString();
                    }

                    txtSalesPrice.DbValue = salesOrderAppDetail.SalesPrice;
                    txtCount.DbValue = salesOrderAppDetail.Count;
                    txtGiftCount.DbValue = salesOrderAppDetail.GiftCount;
                    txtTotalSalesAmount.DbValue = salesOrderAppDetail.TotalSalesAmount;
                }
                else
                {
                    BindProducts(salesOrderAppDetails.Select(x => x.ProductID).ToList());
                }
            }
            else
                BindProducts();
        }

        /// <summary>
        /// 禁用相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rcbxWarehouse.Enabled = false;
            rcbxProduct.Enabled = false;
            ddlProductSpecification.Enabled = false;
            txtCount.Enabled = false;
            txtGiftCount.Enabled = false;

            btnSave.Enabled = false;
        }

        #endregion

        protected void rcbxProduct_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindProductSpecifications();
        }

        protected void ddlProductSpecification_ItemDataBound(object sender, DropDownListItemEventArgs e)
        {
            var dataItem = (UIDropdownItem)e.Item.DataItem;

            if (dataItem != null)
                e.Item.Attributes["Extension"] = Utility.JsonSeralize(dataItem.Extension);
        }

        protected void ddlProductSpecification_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            decimal? salesPrice = GetSalesPrice();

            txtSalesPrice.DbValue = salesPrice;

            if (txtSalesPrice.Value.HasValue
                && txtCount.Value.HasValue)
                txtTotalSalesAmount.DbValue = txtSalesPrice.Value * txtCount.Value;
            else
                txtTotalSalesAmount.DbValue = null;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!txtSalesPrice.Value.HasValue)
            {
                if (CanAuditUserIDs.Contains(CurrentUser.UserID)
                    && CurrentOwnerEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit)
                    cvSalesPrice.ErrorMessage = "销售单价必填";

                cvSalesPrice.IsValid = false;
            }

            if (!IsValid) return;

            if (this.CurrentOwnerEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientSaleApplicationRepository clientSaleAppRepository = new ClientSaleApplicationRepository();
                    IClientInfoRepository clientInfoRepository = new ClientInfoRepository();

                    clientSaleAppRepository.SetDbModel(db);
                    clientInfoRepository.SetDbModel(db);

                    var ownerEntity = clientSaleAppRepository.GetByID(CurrentOwnerEntity.ID);

                    var salesOrderAppDetail = ownerEntity.SalesOrderApplication.SalesOrderAppDetail
                            .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (salesOrderAppDetail == null)
                    {
                        salesOrderAppDetail = new SalesOrderAppDetail();

                        ownerEntity.SalesOrderApplication.SalesOrderAppDetail.Add(salesOrderAppDetail);
                    }

                    decimal? salesPrice = GetSalesPrice();

                    salesOrderAppDetail.WarehouseID = Convert.ToInt32(rcbxWarehouse.SelectedValue);
                    salesOrderAppDetail.ProductID = Convert.ToInt32(rcbxProduct.SelectedValue);
                    salesOrderAppDetail.ProductSpecificationID = Convert.ToInt32(ddlProductSpecification.SelectedValue);
                    salesOrderAppDetail.Count = (int)txtCount.Value;
                    salesOrderAppDetail.GiftCount = (int?)txtGiftCount.Value;

                    //修改单价
                    if (CanAuditUserIDs.Contains(CurrentUser.UserID)
                        && ownerEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit
                        && (decimal?)txtSalesPrice.Value != salesPrice)
                    {
                        salesOrderAppDetail.SalesPrice = (decimal)txtSalesPrice.Value;

                        int clientUserID = ownerEntity.ClientUserID;
                        int clientCompanyID = ownerEntity.ClientCompanyID;

                        var clientInfo = clientInfoRepository.GetByConditions(clientUserID, clientCompanyID);

                        if (clientInfo != null)
                        {
                            bool isHighPrice = false;

                            var warehouse = PageWarehouseRepository.GetByID(salesOrderAppDetail.WarehouseID);

                            if (warehouse != null && warehouse.SaleTypeID == (int)ESaleType.HighPrice)
                                isHighPrice = true;

                            var clientProductSetting = clientInfo.ClientInfoProductSetting
                                .FirstOrDefault(x => x.IsDeleted == false && x.ProductID == salesOrderAppDetail.ProductID
                                    && x.ProductSpecificationID == salesOrderAppDetail.ProductSpecificationID);

                            if (clientProductSetting == null)
                            {
                                clientProductSetting = new ClientInfoProductSetting
                                {
                                    ClientInfoID = clientInfo.ID,
                                    ProductID = salesOrderAppDetail.ProductID,
                                    ProductSpecificationID = salesOrderAppDetail.ProductSpecificationID,
                                    UseFlowData = true,
                                };

                                clientInfo.ClientInfoProductSetting.Add(clientProductSetting);
                            }

                            if (isHighPrice)
                                clientProductSetting.HighPrice = salesOrderAppDetail.SalesPrice;
                            else
                                clientProductSetting.BasicPrice = salesOrderAppDetail.SalesPrice;

                        }
                    }
                    else
                        salesOrderAppDetail.SalesPrice = salesPrice.HasValue ? salesPrice.Value : 0;

                    salesOrderAppDetail.TotalSalesAmount = salesOrderAppDetail.SalesPrice * salesOrderAppDetail.Count;

                    unitOfWork.SaveChanges();

                }

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);
            }
        }
    }
}