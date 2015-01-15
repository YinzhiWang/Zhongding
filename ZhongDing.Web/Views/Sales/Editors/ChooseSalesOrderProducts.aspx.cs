using System;
using System.Collections;
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
    public partial class ChooseSalesOrderProducts : BasePage
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

        /// <summary>
        /// 配送公司ID
        /// </summary>
        private int? DistributionCompanyID
        {
            get
            {
                return WebUtility.GetValueFromQueryString("DistributionCompanyID");
            }
        }

        #endregion

        #region Members

        private ISalesOrderAppDetailRepository _PageSalesOrderAppDetailRepository;
        private ISalesOrderAppDetailRepository PageSalesOrderAppDetailRepository
        {
            get
            {
                if (_PageSalesOrderAppDetailRepository == null)
                    _PageSalesOrderAppDetailRepository = new SalesOrderAppDetailRepository();

                return _PageSalesOrderAppDetailRepository;
            }
        }

        private IStockOutRepository _PageStockOutRepository;
        private IStockOutRepository PageStockOutRepository
        {
            get
            {
                if (_PageStockOutRepository == null)
                    _PageStockOutRepository = new StockOutRepository();

                return _PageStockOutRepository;
            }
        }

        private IStockOutDetailRepository _PageStockOutDetailRepository;
        private IStockOutDetailRepository PageStockOutDetailRepository
        {
            get
            {
                if (_PageStockOutDetailRepository == null)
                    _PageStockOutDetailRepository = new StockOutDetailRepository();

                return _PageStockOutDetailRepository;
            }
        }

        private IStockInDetailRepository _PageStockInDetailRepository;
        private IStockInDetailRepository PageStockInDetailRepository
        {
            get
            {
                if (_PageStockInDetailRepository == null)
                    _PageStockInDetailRepository = new StockInDetailRepository();

                return _PageStockInDetailRepository;
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

        private StockOut _CurrentOwnerEntity;
        private StockOut CurrentOwnerEntity
        {
            get
            {
                if (_CurrentOwnerEntity == null)
                    if (this.OwnerEntityID.HasValue && this.OwnerEntityID > 0)
                        _CurrentOwnerEntity = PageStockOutRepository.GetByID(this.OwnerEntityID);

                return _CurrentOwnerEntity;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentOwnerEntity != null)
            {
                if (!IsPostBack)
                {
                    hdnGridClientID.Value = base.GridClientID;

                    BindWarehouses();
                }
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }
        }

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

            if (warehouses.Count > 0)
                rcbxWarehouse.SelectedIndex = 0;
        }

        private void BindSalesOrderAppDetails(bool isNeedRebind)
        {
            if (this.CurrentOwnerEntity != null)
            {
                var uiSearchObj = new UISearchToBeOutSalesOrderDetail();

                var saleOrderTypeIDs = new List<int>();

                if (this.CurrentOwnerEntity.ReceiverTypeID == (int)EReceiverType.DistributionCompany)
                {
                    uiSearchObj.DistributionCompanyID = this.DistributionCompanyID.HasValue
                    ? this.DistributionCompanyID.Value : GlobalConst.INVALID_INT;

                    saleOrderTypeIDs.Add((int)ESaleOrderType.DaBaoMode);
                }
                else if (this.CurrentOwnerEntity.ReceiverTypeID == (int)EReceiverType.ClientUser)
                {
                    saleOrderTypeIDs.Add((int)ESaleOrderType.AttractBusinessMode);
                    saleOrderTypeIDs.Add((int)ESaleOrderType.AttachedMode);
                }

                var salesOrderAppDetailIDs = CurrentOwnerEntity.StockOutDetail
                    .Where(x => x.IsDeleted == false).Select(x => x.SalesOrderAppDetailID).Distinct();

                var excludeIDs = new List<int>();

                foreach (var salesOrderAppDetailID in salesOrderAppDetailIDs)
                {
                    var salesOrderAppDetail = PageSalesOrderAppDetailRepository.GetByID(salesOrderAppDetailID);

                    if (salesOrderAppDetail != null)
                    {
                        int dbOutQty = PageStockOutDetailRepository.GetList(x => x.SalesOrderAppDetailID == salesOrderAppDetailID)
                            .Sum(x => x.OutQty);

                        int needOutQty = salesOrderAppDetail.Count - (dbOutQty > 0 ? dbOutQty : 0);

                        if (needOutQty <= 0)
                            excludeIDs.Add(salesOrderAppDetailID);
                    }
                }

                uiSearchObj.ExcludeIDs = excludeIDs;

                int totalRecords;

                var orderProducts = PageSalesOrderAppDetailRepository.GetToBeOutUIList(uiSearchObj,
                    rgSalesOrderAppDetails.CurrentPageIndex, rgSalesOrderAppDetails.PageSize, out totalRecords);

                rgSalesOrderAppDetails.DataSource = orderProducts;

                rgSalesOrderAppDetails.VirtualItemCount = totalRecords;

                if (isNeedRebind)
                    rgSalesOrderAppDetails.Rebind();
            }
        }

        protected void rgSalesOrderAppDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindSalesOrderAppDetails(false);
        }

        protected void rgSalesOrderAppDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIToBeOutSalesOrderDetail)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    var ddlWarehouse = (RadDropDownList)gridDataItem.FindControl("ddlWarehouse");

                    if (ddlWarehouse != null)
                    {
                        ddlWarehouse.DataSource = uiEntity.WarehouseData;
                        ddlWarehouse.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                        ddlWarehouse.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                        ddlWarehouse.DataBind();

                        if (uiEntity.WarehouseData.Count > 0)
                            ddlWarehouse.SelectedIndex = 0;
                    }

                    var txtCurrentOutQty = (RadNumericTextBox)gridDataItem.FindControl("txtCurrentOutQty");

                    if (txtCurrentOutQty != null)
                    {
                        txtCurrentOutQty.MaxValue = uiEntity.ToBeOutQty;

                        if (ddlWarehouse != null
                            && !string.IsNullOrEmpty(ddlWarehouse.SelectedValue))
                        {
                            var selectedWarehouseData = uiEntity.WarehouseData
                                .FirstOrDefault(x => x.ItemValue.ToString() == ddlWarehouse.SelectedValue);

                            if (selectedWarehouseData != null)
                            {
                                System.Reflection.PropertyInfo balanceQtyPropertyInfo = selectedWarehouseData.Extension.GetType().GetProperty("BalanceQty");

                                if (balanceQtyPropertyInfo != null)
                                {
                                    string sBalanceQty = balanceQtyPropertyInfo.GetValue(selectedWarehouseData.Extension).ToString();

                                    int iBalanceQty;
                                    if (int.TryParse(sBalanceQty, out iBalanceQty))
                                    {
                                        if (iBalanceQty > uiEntity.ToBeOutQty)
                                            txtCurrentOutQty.DbValue = uiEntity.ToBeOutQty;
                                        else
                                            txtCurrentOutQty.DbValue = iBalanceQty;

                                        var lblBalanceQty = (Label)gridDataItem.FindControl("lblBalanceQty");

                                        if (lblBalanceQty != null)
                                            lblBalanceQty.Text = sBalanceQty;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var selectedItems = rgSalesOrderAppDetails.SelectedItems;

            if (selectedItems.Count == 0)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show("请选择要加入出库单的订单");

                return;
            }

            if (this.CurrentOwnerEntity != null)
            {
                foreach (var item in selectedItems)
                {
                    var editableItem = ((GridEditableItem)item);
                    int salesOrderAppDetailID = Convert.ToInt32(editableItem.GetDataKeyValue("ID").ToString());
                    int salesOrderApplicationID = Convert.ToInt32(editableItem.GetDataKeyValue("SalesOrderApplicationID").ToString());
                    int productID = Convert.ToInt32(editableItem.GetDataKeyValue("ProductID").ToString());
                    int productSpecificationID = Convert.ToInt32(editableItem.GetDataKeyValue("ProductSpecificationID").ToString());

                    var ddlWarehouse = (RadDropDownList)editableItem.FindControl("ddlWarehouse");
                    int warehouseID = Convert.ToInt32(ddlWarehouse.SelectedValue);

                    if (int.TryParse(ddlWarehouse.SelectedValue, out warehouseID))
                    {
                        var txtCurrentOutQty = (RadNumericTextBox)editableItem.FindControl("txtCurrentOutQty");

                        if (txtCurrentOutQty.Value.HasValue)
                        {
                            int needOutQty = Convert.ToInt32(txtCurrentOutQty.Value.Value);

                            var uiSearchObj = new UISearchStockInDetail
                            {
                                WarehouseID = warehouseID,
                                ProductID = productID,
                                ProductSpecificationID = productSpecificationID
                            };

                            var canOutStockInDetails = PageStockInDetailRepository.GetInventory(uiSearchObj)
                                .Where(x => x.BalanceQty > 0).ToList();

                            if (canOutStockInDetails.Count > 0)
                            {
                                var salesOrderAppDetail = PageSalesOrderAppDetailRepository.GetByID(salesOrderAppDetailID);

                                int tempOutQty = needOutQty;

                                foreach (var canOutStockInDetail in canOutStockInDetails.OrderBy(x => x.ExpirationDate))
                                {
                                    var tempStockOutDetail = CurrentOwnerEntity.StockOutDetail.FirstOrDefault(x => x.IsDeleted == false
                                        && x.WarehouseID == warehouseID && x.ProductID == productID && x.ProductSpecificationID == productSpecificationID
                                        && x.BatchNumber == canOutStockInDetail.BatchNumber && x.LicenseNumber == canOutStockInDetail.LicenseNumber);

                                    int curOutQty = 0;

                                    if (canOutStockInDetail.BalanceQty >= tempOutQty)
                                    {
                                        curOutQty = tempOutQty;
                                        tempOutQty = 0;
                                    }
                                    else
                                    {
                                        tempOutQty -= canOutStockInDetail.BalanceQty;
                                        curOutQty = canOutStockInDetail.BalanceQty;
                                    }

                                    if (tempStockOutDetail != null)
                                    {
                                        tempStockOutDetail.OutQty += curOutQty;

                                        tempStockOutDetail.TotalSalesAmount = tempStockOutDetail.SalesPrice * tempStockOutDetail.OutQty;
                                    }
                                    else
                                    {
                                        var stockOutDetail = new StockOutDetail
                                        {
                                            SalesOrderAppDetailID = salesOrderAppDetailID,
                                            SalesOrderApplicationID = salesOrderApplicationID,
                                            ProductID = canOutStockInDetail.ProductID,
                                            ProductSpecificationID = canOutStockInDetail.ProductSpecificationID,
                                            WarehouseID = canOutStockInDetail.WarehouseID,
                                            SalesPrice = salesOrderAppDetail.SalesPrice,
                                            OutQty = curOutQty,
                                            BatchNumber = canOutStockInDetail.BatchNumber,
                                            ExpirationDate = canOutStockInDetail.ExpirationDate,
                                            LicenseNumber = canOutStockInDetail.LicenseNumber,
                                            TotalSalesAmount = salesOrderAppDetail.SalesPrice * curOutQty
                                        };

                                        CurrentOwnerEntity.StockOutDetail.Add(stockOutDetail);
                                    }

                                    if (tempOutQty == 0)
                                        break;
                                }
                            }
                        }
                    }

                }

                PageStockOutRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_CLOSE_WIN);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSalesOrderAppDetails(true);
        }

        protected void ddlWarehouse_ItemDataBound(object sender, DropDownListItemEventArgs e)
        {
            UIDropdownItem dataItem = (UIDropdownItem)e.Item.DataItem;

            if (dataItem != null)
                e.Item.Attributes["Extension"] = Utility.JsonSeralize(dataItem.Extension);
        }
    }
}