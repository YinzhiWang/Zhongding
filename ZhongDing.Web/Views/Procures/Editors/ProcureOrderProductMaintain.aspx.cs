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
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Procures.Editors
{
    public partial class ProcureOrderProductMaintain : BasePage
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
                string sOwnerEntityID = Request.QueryString["OwnerEntityID"];

                int iOwnerEntityID;

                if (int.TryParse(sOwnerEntityID, out iOwnerEntityID))
                    return iOwnerEntityID;
                else
                    return null;
            }
        }

        #endregion

        #region Members

        private IProcureOrderAppDetailRepository _PageProcureOrderAppDetailRepository;
        private IProcureOrderAppDetailRepository PageProcureOrderAppDetailRepository
        {
            get
            {
                if (_PageProcureOrderAppDetailRepository == null)
                    _PageProcureOrderAppDetailRepository = new ProcureOrderAppDetailRepository();

                return _PageProcureOrderAppDetailRepository;
            }
        }

        private IProcureOrderApplicationRepository _PageProcureOrderApplicationRepository;
        private IProcureOrderApplicationRepository PageProcureOrderApplicationRepository
        {
            get
            {
                if (_PageProcureOrderApplicationRepository == null)
                    _PageProcureOrderApplicationRepository = new ProcureOrderApplicationRepository();

                return _PageProcureOrderApplicationRepository;
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

        private ProcureOrderApplication _CurrentOwnerEntity;
        private ProcureOrderApplication CurrentOwnerEntity
        {
            get
            {
                if (_CurrentOwnerEntity == null)
                    if (this.OwnerEntityID.HasValue && this.OwnerEntityID > 0)
                        _CurrentOwnerEntity = PageProcureOrderApplicationRepository.GetByID(this.OwnerEntityID);

                return _CurrentOwnerEntity;
            }
        }

        #endregion


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

                BindProducts();

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

        private void BindProducts()
        {
            var products = PageProductRepository.GetDropdownItems(new UISearchDropdownItem()
            {
                Extension = new UISearchExtension
                {
                    CompanyID = CurrentUser.CompanyID,
                    SupplierID = CurrentOwnerEntity.SupplierID,
                }
            });

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProductSpecifications(int productID)
        {
            if (productID > 0)
            {
                ddlProductSpecification.ClearSelection();
                ddlProductSpecification.Items.Clear();

                var productSpecifications = PageProductSpecificationRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension { ProductID = productID }
                });

                ddlProductSpecification.DataSource = productSpecifications;
                ddlProductSpecification.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                ddlProductSpecification.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                ddlProductSpecification.DataBind();

                if (!string.IsNullOrEmpty(ddlProductSpecification.SelectedValue)
                    && !string.IsNullOrEmpty(rcbxWarehouse.SelectedValue))
                {
                    int warehouseID = Convert.ToInt32(rcbxWarehouse.SelectedValue);
                    int productSpecificationID = Convert.ToInt32(ddlProductSpecification.SelectedValue);

                    txtProcurePrice.DbValue = PageProcureOrderApplicationRepository.GetPrefillProcurePrice(warehouseID, productID, productSpecificationID);
                }
            }
        }

        private void LoadCurrentEntity()
        {
            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var procureOrderApp = PageProcureOrderApplicationRepository.GetByID(this.OwnerEntityID);

                if (procureOrderApp != null)
                {
                    var procureOrderAppDetail = procureOrderApp.ProcureOrderAppDetail
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (procureOrderAppDetail != null)
                    {
                        rcbxWarehouse.SelectedValue = procureOrderAppDetail.WarehouseID.ToString();
                        rcbxProduct.SelectedValue = procureOrderAppDetail.ProductID.ToString();

                        BindProductSpecifications(procureOrderAppDetail.ProductID);

                        ddlProductSpecification.SelectedValue = procureOrderAppDetail.ProductSpecificationID.ToString();

                        lblUnitOfMeasurement.Text = procureOrderAppDetail.ProductSpecification.UnitOfMeasurement == null
                            ? string.Empty : procureOrderAppDetail.ProductSpecification.UnitOfMeasurement.UnitName;

                        txtProcurePrice.DbValue = procureOrderAppDetail.ProcurePrice;
                        txtProcureCount.DbValue = procureOrderAppDetail.ProcureCount;
                        txtTotalAmount.DbValue = procureOrderAppDetail.TotalAmount;
                        txtTaxAmount.DbValue = procureOrderAppDetail.TaxAmount;
                    }
                }
            }
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var procureOrderApp = PageProcureOrderApplicationRepository.GetByID(this.OwnerEntityID);

                if (procureOrderApp != null)
                {
                    var procureOrderAppDetail = procureOrderApp.ProcureOrderAppDetail
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (procureOrderAppDetail == null)
                    {
                        procureOrderAppDetail = new ProcureOrderAppDetail();

                        procureOrderApp.ProcureOrderAppDetail.Add(procureOrderAppDetail);
                    }

                    procureOrderAppDetail.WarehouseID = Convert.ToInt32(rcbxWarehouse.SelectedValue);
                    procureOrderAppDetail.ProductID = Convert.ToInt32(rcbxProduct.SelectedValue);
                    procureOrderAppDetail.ProductSpecificationID = Convert.ToInt32(ddlProductSpecification.SelectedValue);
                    procureOrderAppDetail.ProcurePrice = (decimal)txtProcurePrice.Value;
                    procureOrderAppDetail.ProcureCount = (int)txtProcureCount.Value;
                    procureOrderAppDetail.TotalAmount = procureOrderAppDetail.ProcurePrice * procureOrderAppDetail.ProcureCount;
                    procureOrderAppDetail.TaxAmount = (decimal?)txtTaxAmount.Value;

                    PageProcureOrderApplicationRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
                }
            }

        }

        protected void rcbxProduct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int productID;

                if (int.TryParse(e.Value, out productID))
                    BindProductSpecifications(productID);
            }
        }

        protected void ddlProductSpecification_ItemDataBound(object sender, DropDownListItemEventArgs e)
        {
            UIDropdownItem dataItem = (UIDropdownItem)e.Item.DataItem;
            System.Reflection.PropertyInfo unitNamePropertyInfo = dataItem.Extension.GetType().GetProperty("UnitName");

            if (unitNamePropertyInfo != null)
            {
                e.Item.Attributes["UnitName"] = unitNamePropertyInfo.GetValue(dataItem.Extension).ToString();
            }
        }

        protected void cvTaxAmount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var totalAmount = (txtProcurePrice.Value.HasValue ? txtProcurePrice.Value : 0)
                * (txtProcureCount.Value.HasValue ? txtProcureCount.Value : 0);

            if (txtTaxAmount.Value.HasValue
                && txtTaxAmount.Value > totalAmount)
            {
                args.IsValid = false;
            }
        }

        protected void rcbxWarehouse_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value)
                && !string.IsNullOrEmpty(rcbxProduct.SelectedValue)
                && !string.IsNullOrEmpty(ddlProductSpecification.SelectedValue))
            {
                int warehouseID = Convert.ToInt32(e.Value);
                int productID = Convert.ToInt32(rcbxProduct.SelectedValue);
                int productSpecificationID = Convert.ToInt32(ddlProductSpecification.SelectedValue);

                txtProcurePrice.DbValue = PageProcureOrderApplicationRepository.GetPrefillProcurePrice(warehouseID, productID, productSpecificationID);
            }
        }

        protected void ddlProductSpecification_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value)
                && !string.IsNullOrEmpty(rcbxProduct.SelectedValue)
                && !string.IsNullOrEmpty(rcbxWarehouse.SelectedValue))
            {
                int warehouseID = Convert.ToInt32(rcbxWarehouse.SelectedValue);
                int productID = Convert.ToInt32(rcbxProduct.SelectedValue);
                int productSpecificationID = Convert.ToInt32(e.Value);

                txtProcurePrice.DbValue = PageProcureOrderApplicationRepository.GetPrefillProcurePrice(warehouseID, productID, productSpecificationID);
            }
        }
    }
}