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
    public partial class SupplierInvoiceMaintenance : BasePage
    {

        #region Members

        private ISupplierInvoiceRepository _PageSupplierInvoiceRepository;
        private ISupplierInvoiceRepository PageSupplierInvoiceRepository
        {
            get
            {
                if (_PageSupplierInvoiceRepository == null)
                    _PageSupplierInvoiceRepository = new SupplierInvoiceRepository();
                return _PageSupplierInvoiceRepository;
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
        private ISupplierInvoiceDetailRepository _PageSupplierInvoiceDetailRepository;
        private ISupplierInvoiceDetailRepository PageSupplierInvoiceDetailRepository
        {
            get
            {
                if (_PageSupplierInvoiceDetailRepository == null)
                    _PageSupplierInvoiceDetailRepository = new SupplierInvoiceDetailRepository();

                return _PageSupplierInvoiceDetailRepository;
            }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierInvoiceManage;
            if (!IsPostBack)
            {
                BindSuppliers();
                LoadEntityData();
            }

        }
        private void LoadEntityData()
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                var currentEntity = PageSupplierInvoiceRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    hdnCurrentEntityID.Value = currentEntity.ID.ToString();


                    //BindTransportCompanys(currentEntity.TransportCompanyID);

                    //txtTransportCompanyNumber.Text = currentEntity.TransportCompanyNumber;

                    //txtDriver.Text = currentEntity.Driver;
                    //txtDriverTelephone.Text = currentEntity.DriverTelephone;
                    //txtStartPlace.Text = currentEntity.StartPlace;
                    //txtStartTelephone.Text = currentEntity.StartPlaceTelephone;
                    //txtEndPlace.Text = currentEntity.EndPlace;
                    //txtEndPlaceTelephone.Text = currentEntity.EndPlaceTelephone;
                    //txtFee.Value = currentEntity.Fee.ToDouble();
                    //txtSendDate.SelectedDate = currentEntity.SendDate;

                    //txtRemark.Text = currentEntity.Remark;

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
            if (rcbxSupplier.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                BindProcureOrderAppDetails(true);
            }
            else
            {
                cvSupplier.IsValid = false;
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!rcbxSupplier.SelectedValue.ToIntOrNull().BiggerThanZero())
            {
                cvSupplier.IsValid = false;
            }

            if (rgProcureOrderAppDetails.SelectedItems.Count == 0)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show("请选择订单");

                return;
            }

            if (!IsValid) return;

            SupplierInvoice currentEntity = null;

            if (this.CurrentEntityID.BiggerThanZero())
                currentEntity = PageSupplierInvoiceRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new SupplierInvoice();

                PageSupplierInvoiceRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.CompanyID = SiteUser.GetCurrentSiteUser().CompanyID;
                currentEntity.Amount = txtAmount.Value.ToDecimal();
                currentEntity.InvoiceDate = txtInvoiceDate.SelectedDate.Value;
                currentEntity.InvoiceNumber = txtInvoiceNumber.Text.Trim();
                currentEntity.SupplierID = rcbxSupplier.SelectedValue.ToInt();
                PageSupplierInvoiceRepository.Save();
                var selectedItems = rgProcureOrderAppDetails.SelectedItems;

                foreach (var item in selectedItems)
                {
                    var editableItem = ((GridEditableItem)item);
                    int procureOrderAppDetailID = Convert.ToInt32(editableItem.GetDataKeyValue("ID").ToString());
                    var txtSupplierInvoiceDetailAmount = (RadNumericTextBox)editableItem.FindControl("txtSupplierInvoiceDetailAmount");

                    SupplierInvoiceDetail supplierInvoiceDetail = new SupplierInvoiceDetail()
                    {
                        Amount = txtSupplierInvoiceDetailAmount.Value.ToDecimal(),
                        ProcureOrderAppDetailID = procureOrderAppDetailID,
                        SupplierInvoiceID = currentEntity.ID,
                    };
                    PageSupplierInvoiceDetailRepository.Add(supplierInvoiceDetail);

                }
                PageSupplierInvoiceDetailRepository.Save();
                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                PageSupplierInvoiceRepository.DeleteByID(this.CurrentEntityID);
                PageSupplierInvoiceRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }


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

        protected void rcbxSupplier_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }
        private void BindProcureOrderAppDetails(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchProcureOrderAppDetail()
            {
                SupplierID = rcbxSupplier.SelectedValue.ToIntOrNull().GetValueOrDefault(0)
            };

            int totalRecords;
            
            var procureOrderAppDetails = PageProcureOrderAppDetailRepository.GetSupplierInvoiceChooseProcureOrderAppDetailUIList(uiSearchObj,
                rgProcureOrderAppDetails.CurrentPageIndex, rgProcureOrderAppDetails.PageSize, out totalRecords);


            rgProcureOrderAppDetails.VirtualItemCount = totalRecords;
            rgProcureOrderAppDetails.DataSource = procureOrderAppDetails;

            if (isNeedRebind)
                rgProcureOrderAppDetails.Rebind();

        }
        protected void rgProcureOrderAppDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (rcbxSupplier.SelectedValue.ToIntOrNull().BiggerThanZero())
                BindProcureOrderAppDetails(false);
        }

        protected void rgProcureOrderAppDetails_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            //if (this.CurrentOwnerEntity != null
            //    && this.CurrentOwnerEntity.ReceiverTypeID == (int)EReceiverType.ClientUser)
            //    e.OwnerTableView.Columns.FindByUniqueName("CurrentTaxQty").Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName("CurrentTaxQty").Visible = false;
        }

        protected void rgProcureOrderAppDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;


            }
        }

    }
}