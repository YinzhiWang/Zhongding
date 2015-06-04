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
using ZhongDing.Common.Extension;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;


namespace ZhongDing.Web.Views.Invoices
{
    public partial class ClientInfoInvoiceMaintenance : BasePage
    {

        #region Members

        private IClientInvoiceRepository _PageClientInvoiceRepository;
        private IClientInvoiceRepository PageClientInvoiceRepository
        {
            get
            {
                if (_PageClientInvoiceRepository == null)
                    _PageClientInvoiceRepository = new ClientInvoiceRepository();
                return _PageClientInvoiceRepository;
            }
        }
        private IClientCompanyRepository _PageClientCompanyRepository;
        private IClientCompanyRepository PageClientCompanyRepository
        {
            get
            {
                if (_PageClientCompanyRepository == null)
                    _PageClientCompanyRepository = new ClientCompanyRepository();

                return _PageClientCompanyRepository;
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
        private IClientInvoiceDetailRepository _PageClientInvoiceDetailRepository;
        private IClientInvoiceDetailRepository PageClientInvoiceDetailRepository
        {
            get
            {
                if (_PageClientInvoiceDetailRepository == null)
                    _PageClientInvoiceDetailRepository = new ClientInvoiceDetailRepository();

                return _PageClientInvoiceDetailRepository;
            }
        }

        private ITransportCompanyRepository _PageTransportCompanyRepository;
        private ITransportCompanyRepository PageTransportCompanyRepository
        {
            get
            {
                if (_PageTransportCompanyRepository == null)
                    _PageTransportCompanyRepository = new TransportCompanyRepository();
                return _PageTransportCompanyRepository;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientInvoiceManage;
            if (!IsPostBack)
            {
                BindClientCompanys();
                LoadEntityData();
                base.PermissionOptionCheckButtonDelete(btnDelete);
            }

        }
        private void LoadEntityData()
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                var currentEntity = PageClientInvoiceRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    hdnCurrentEntityID.Value = currentEntity.ID.ToString();
                }
            }
            else
            {
                btnDelete.Visible = false;
                BindTransportCompanys();
            }


        }
        private void BindTransportCompanys(int? ransportCompanyID = null)
        {
            //var companys = PageTransportCompanyRepository.GetDropdownItems();
            //rcbxTransportCompany.DataSource = companys;
            //rcbxTransportCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            //rcbxTransportCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            //rcbxTransportCompany.DataBind();
            //rcbxTransportCompany.Items.Insert(0, new RadComboBoxItem("", ""));
            //if (ransportCompanyID.HasValue)
            //    rcbxTransportCompany.SelectedValue = ransportCompanyID.Value.ToString();
        }


        protected void cvCompanyName_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void btnSearchOrder_Click(object sender, EventArgs e)
        {
            if (rcbxClientCompany.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                BindStockOutDetails(true);
            }
            else
            {
                cvSupplier.IsValid = false;
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!rcbxClientCompany.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                cvSupplier.IsValid = false;
            }
            if (!rcbxTransportCompany.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                cvrTransportCompany.IsValid = false;
            }
            if (rgStockOutDetails.SelectedItems.Count == 0)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show("请选择订单");

                return;
            }

            if (!IsValid) return;

            ClientInvoice currentEntity = null;

            if (this.CurrentEntityID.BiggerThanZero())
                currentEntity = PageClientInvoiceRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new ClientInvoice();

                PageClientInvoiceRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.CompanyID = SiteUser.GetCurrentSiteUser().CompanyID;
                currentEntity.Amount = txtAmount.Value.ToDecimal();
                currentEntity.InvoiceDate = txtInvoiceDate.SelectedDate.Value;
                currentEntity.InvoiceNumber = txtInvoiceNumber.Text.Trim();
                currentEntity.TransportCompany = rcbxTransportCompany.SelectedItem.Text;
                currentEntity.TransportNumber = txtTransportNumber.Text.Trim();
                currentEntity.ClientCompanyID = rcbxClientCompany.SelectedValue.ToInt();
                currentEntity.SaleOrderTypeID = hdnSaleOrderTypeID.Value.ToInt();

                PageClientInvoiceRepository.Save();
                var selectedItems = rgStockOutDetails.SelectedItems;

                foreach (var item in selectedItems)
                {
                    var editableItem = ((GridEditableItem)item);
                    int stockOutDetailID = Convert.ToInt32(editableItem.GetDataKeyValue("ID").ToString());
                    var txtClientInvoiceDetailAmount = (RadNumericTextBox)editableItem.FindControl("txtClientInvoiceDetailAmount");
                    var txtClientInvoiceDetailQty = (RadNumericTextBox)editableItem.FindControl("txtClientInvoiceDetailQty");
                    var rblInvoiceType = (RadioButtonList)editableItem.FindControl("rblInvoiceType");
                    ClientInvoiceDetail ClientInvoiceDetail = new ClientInvoiceDetail()
                    {
                        Amount = txtClientInvoiceDetailAmount.Value.ToDecimal(),
                        StockOutDetailID = stockOutDetailID,
                        ClientInvoiceID = currentEntity.ID,
                        InvoiceTypeID = rblInvoiceType.SelectedValue.ToInt(),
                        Qty = txtClientInvoiceDetailQty.Value.ToInt()

                    };
                    PageClientInvoiceDetailRepository.Add(ClientInvoiceDetail);

                }
                PageClientInvoiceDetailRepository.Save();
                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                PageClientInvoiceRepository.DeleteByID(this.CurrentEntityID);
                PageClientInvoiceRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }

        private void BindTransportCompanys()
        {
            var companys = PageTransportCompanyRepository.GetDropdownItems();
            rcbxTransportCompany.DataSource = companys;
            rcbxTransportCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxTransportCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxTransportCompany.DataBind();
            rcbxTransportCompany.Items.Insert(0, new RadComboBoxItem("", ""));

        }

        private void BindClientCompanys()
        {
            var uiSearchObj = new UISearchDropdownItem();

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        protected void rcbxClientCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }
        private void BindStockOutDetails(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchStockOutDetail()
            {
                ClientCompanyID = rcbxClientCompany.SelectedValue.ToIntOrNull(),
            };

            int totalRecords;

            var procureOrderAppDetails = PageStockOutDetailRepository.GetClientInvoiceChooseStockOutDetailUIList(uiSearchObj,
                rgStockOutDetails.CurrentPageIndex, rgStockOutDetails.PageSize, out totalRecords);


            rgStockOutDetails.VirtualItemCount = totalRecords;
            rgStockOutDetails.DataSource = procureOrderAppDetails;

            if (isNeedRebind)
                rgStockOutDetails.Rebind();

        }
        protected void rgStockOutDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (rcbxClientCompany.SelectedValue.ToIntOrNull().BiggerThanZero())
                BindStockOutDetails(false);
        }

        protected void rgStockOutDetails_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            //if (this.CurrentOwnerEntity != null
            //    && this.CurrentOwnerEntity.ReceiverTypeID == (int)EReceiverType.ClientUser)
            //    e.OwnerTableView.Columns.FindByUniqueName("CurrentTaxQty").Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName("CurrentTaxQty").Visible = false;
        }

        protected void rgStockOutDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;


            }
        }


        protected override EPermission PagePermissionID()
        {
            return EPermission.InvoiceManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}