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
    public partial class DBClientInfoInvoiceMaintenance : BasePage
    {

        #region Members

        private IDBClientInvoiceRepository _PageDBClientInvoiceRepository;
        private IDBClientInvoiceRepository PageDBClientInvoiceRepository
        {
            get
            {
                if (_PageDBClientInvoiceRepository == null)
                    _PageDBClientInvoiceRepository = new DBClientInvoiceRepository();
                return _PageDBClientInvoiceRepository;
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
        private IDBClientInvoiceDetailRepository _PageDBClientInvoiceDetailRepository;
        private IDBClientInvoiceDetailRepository PageDBClientInvoiceDetailRepository
        {
            get
            {
                if (_PageDBClientInvoiceDetailRepository == null)
                    _PageDBClientInvoiceDetailRepository = new DBClientInvoiceDetailRepository();

                return _PageDBClientInvoiceDetailRepository;
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
        private IDistributionCompanyRepository _PageDistributionCompanyRepository;
        private IDistributionCompanyRepository PageDistributionCompanyRepository
        {
            get
            {
                if (_PageDistributionCompanyRepository == null)
                    _PageDistributionCompanyRepository = new DistributionCompanyRepository();

                return _PageDistributionCompanyRepository;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBClientInvoiceManage;
            if (!IsPostBack)
            {
                BindDistributionCompanies();
                LoadEntityData();
                base.PermissionOptionCheckButtonDelete(btnDelete);
            }

        }
        private void LoadEntityData()
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                var currentEntity = PageDBClientInvoiceRepository.GetByID(this.CurrentEntityID);

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
            if (rcbxDistributionCompany.SelectedValue.ToIntOrNull().BiggerThanZero())
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
            if (!rcbxDistributionCompany.SelectedValue.ToIntOrNull().BiggerThanZero())
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

            DBClientInvoice currentEntity = null;

            if (this.CurrentEntityID.BiggerThanZero())
                currentEntity = PageDBClientInvoiceRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new DBClientInvoice();

                PageDBClientInvoiceRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.CompanyID = SiteUser.GetCurrentSiteUser().CompanyID;
                currentEntity.Amount = txtAmount.Value.ToDecimal();
                currentEntity.InvoiceDate = txtInvoiceDate.SelectedDate.Value;
                currentEntity.InvoiceNumber = txtInvoiceNumber.Text.Trim();
                currentEntity.TransportCompany = rcbxTransportCompany.SelectedItem.Text;
                currentEntity.TransportNumber = txtTransportNumber.Text.Trim();
                //currentEntity.ClientCompanyID = rcbxDistributionCompany.SelectedValue.ToInt();
                currentEntity.SaleOrderTypeID = hdnSaleOrderTypeID.Value.ToInt();
                currentEntity.DistributionCompanyID = rcbxDistributionCompany.SelectedValue.ToInt();
                PageDBClientInvoiceRepository.Save();
                var selectedItems = rgStockOutDetails.SelectedItems;

                foreach (var item in selectedItems)
                {
                    var editableItem = ((GridEditableItem)item);
                    int stockOutDetailID = Convert.ToInt32(editableItem.GetDataKeyValue("ID").ToString());
                    var txtDBClientInvoiceDetailAmount = (RadNumericTextBox)editableItem.FindControl("txtDBClientInvoiceDetailAmount");

                    DBClientInvoiceDetail DBClientInvoiceDetail = new DBClientInvoiceDetail()
                    {
                        Amount = txtDBClientInvoiceDetailAmount.Value.ToDecimal(),
                        StockOutDetailID = stockOutDetailID,
                        DBClientInvoiceID = currentEntity.ID,

                    };
                    PageDBClientInvoiceDetailRepository.Add(DBClientInvoiceDetail);

                }
                PageDBClientInvoiceDetailRepository.Save();
                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                PageDBClientInvoiceRepository.DeleteByID(this.CurrentEntityID);
                PageDBClientInvoiceRepository.Save();

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

        private void BindDistributionCompanies()
        {
            var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems();

            rcbxDistributionCompany.DataSource = distributionCompanies;
            rcbxDistributionCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDistributionCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDistributionCompany.DataBind();

            rcbxDistributionCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        protected void rcbxDistributionCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }
        private void BindStockOutDetails(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchStockOutDetail()
            {
                ReceiverTypeID = (int)EReceiverType.DistributionCompany,
                DistributionCompanyID = rcbxDistributionCompany.SelectedValue.ToIntOrNull().GetValueOrDefault(0)

            };

            int totalRecords;

            var procureOrderAppDetails = PageStockOutDetailRepository.GetDBClientInvoiceChooseStockOutDetailUIList(uiSearchObj,
                rgStockOutDetails.CurrentPageIndex, rgStockOutDetails.PageSize, out totalRecords);


            rgStockOutDetails.VirtualItemCount = totalRecords;
            rgStockOutDetails.DataSource = procureOrderAppDetails;

            if (isNeedRebind)
                rgStockOutDetails.Rebind();

        }
        protected void rgStockOutDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (rcbxDistributionCompany.SelectedValue.ToIntOrNull().BiggerThanZero())
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