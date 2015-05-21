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
using ZhongDing.Domain.UIObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.CautionMoneys
{
    public partial class ClientCautionMoneyMaintenance : WorkflowBasePage
    {
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientCautionMoney;
        }
        #region Members

        private IApplicationPaymentRepository _PageAppPaymentRepository;
        private IApplicationPaymentRepository PageAppPaymentRepository
        {
            get
            {
                if (_PageAppPaymentRepository == null)
                    _PageAppPaymentRepository = new ApplicationPaymentRepository();

                return _PageAppPaymentRepository;
            }
        }

        private IClientCautionMoneyRepository _PageClientCautionMoneyRepository;
        private IClientCautionMoneyRepository PageClientCautionMoneyRepository
        {
            get
            {
                if (_PageClientCautionMoneyRepository == null)
                    _PageClientCautionMoneyRepository = new ClientCautionMoneyRepository();
                return _PageClientCautionMoneyRepository;
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
        private ICautionMoneyTypeRepository _PageCautionMoneyTypeRepository;
        private ICautionMoneyTypeRepository PageCautionMoneyTypeRepository
        {
            get
            {
                if (_PageCautionMoneyTypeRepository == null)
                    _PageCautionMoneyTypeRepository = new CautionMoneyTypeRepository();

                return _PageCautionMoneyTypeRepository;
            }
        }

        private IBankAccountRepository _PageBankAccountRepository;
        private IBankAccountRepository PageBankAccountRepository
        {
            get
            {
                if (_PageBankAccountRepository == null)
                    _PageBankAccountRepository = new BankAccountRepository();

                return _PageBankAccountRepository;
            }
        }
        private ISupplierBankAccountRepository _PageSupplierBankAccountRepository;
        private ISupplierBankAccountRepository PageSupplierBankAccountRepository
        {
            get
            {
                if (_PageSupplierBankAccountRepository == null)
                    _PageSupplierBankAccountRepository = new SupplierBankAccountRepository();

                return _PageSupplierBankAccountRepository;
            }
        }

        private IDepartmentRepository _PageDepartmentRepository;
        private IDepartmentRepository PageDepartmentRepository
        {
            get
            {
                if (_PageDepartmentRepository == null)
                    _PageDepartmentRepository = new DepartmentRepository();

                return _PageDepartmentRepository;
            }
        }
        private IClientUserRepository _PageClientUserRepository;
        private IClientUserRepository PageClientUserRepository
        {
            get
            {
                if (_PageClientUserRepository == null)
                    _PageClientUserRepository = new ClientUserRepository();

                return _PageClientUserRepository;
            }
        }

        private IClientCautionMoneyReturnApplicationRepository _PageClientCautionMoneyReturnApplicationRepository;
        private IClientCautionMoneyReturnApplicationRepository PageClientCautionMoneyReturnApplicationRepository
        {
            get
            {
                if (_PageClientCautionMoneyReturnApplicationRepository == null)
                    _PageClientCautionMoneyReturnApplicationRepository = new ClientCautionMoneyReturnApplicationRepository();

                return _PageClientCautionMoneyReturnApplicationRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientCautionMoneyManage;
            if (!IsPostBack)
            {
                LoadCurrentEntity();
            }
        }

        private ClientCautionMoney _CurrentEntity;
        private ClientCautionMoney CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageClientCautionMoneyRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }



        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                rdpEndDate.SelectedDate = this.CurrentEntity.EndDate;
                BindProducts();
                rcbxProduct.SelectedValue = CurrentEntity.ProductID.ToString();
                BindCautionMoneyTypes();
                rcbxCautionMoneyType.SelectedValue = CurrentEntity.CautionMoneyTypeID.ToString();
                BindProductSpecifications(CurrentEntity.ProductID);
                rcbxProductSpecification.SelectedValue = CurrentEntity.ProductSpecificationID.ToString();
                BindDepartments();
                rcbxDepartment.SelectedValue = CurrentEntity.DepartmentID.ToString();
                BindClientUsers();
                rcbxClientUser.SelectedValue = CurrentEntity.ClientUserID.ToString();
                BindPaymentSummary();
                txtRemark.Text = CurrentEntity.Remark;
                txtPayer.Text = CurrentEntity.Payer;

                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;
            }
            else
            {
                InitDefaultData();
                BindProducts();
                BindCautionMoneyTypes();
                BindDepartments();
                BindClientUsers();
            }
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        private void InitDefaultData()
        {

            divAppPayments.Visible = false;
            divClientCautionMoneyReturnApplications.Visible = false;
        }

        private void BindProducts()
        {
            var products = PageProductRepository.GetDropdownItems(new UISearchDropdownItem()
            {
                Extension = new UISearchExtension { CompanyID = CurrentUser.CompanyID }
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
                rcbxProductSpecification.Items.Clear();

                var productSpecifications = PageProductSpecificationRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension { ProductID = productID }
                });

                rcbxProductSpecification.DataSource = productSpecifications;
                rcbxProductSpecification.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxProductSpecification.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxProductSpecification.DataBind();
            }
        }
        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();

            rcbxDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void BindCautionMoneyTypes()
        {
            var items = PageCautionMoneyTypeRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    CautionMoneyTypeCategory = (int)ECautionMoneyTypeCategory.Client
                }
            });

            rcbxCautionMoneyType.DataSource = items;
            rcbxCautionMoneyType.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxCautionMoneyType.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxCautionMoneyType.DataBind();

            rcbxCautionMoneyType.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems();
            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        protected void rcbxProduct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int productID;
                if (int.TryParse(e.Value, out productID))
                {
                    BindProductSpecifications(productID);
                }
            }
        }
        protected void cvCompanyName_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (rcbxProduct.SelectedValue.IsNullOrEmpty())
                cvProductName.IsValid = false;
            if (rcbxProductSpecification.SelectedValue.IsNullOrEmpty())
                cvProductSpecification.IsValid = false;

            if (rcbxDepartment.SelectedValue.IsNullOrEmpty())
                cvSupplier.IsValid = false;
            if (rcbxCautionMoneyType.SelectedValue.IsNullOrEmpty())
                cvCautionMoneyType.IsValid = false;

            if (rcbxClientUser.SelectedValue.IsNullOrEmpty())
                cvSupplier.IsValid = false;


            if (!IsValid) return;
            //ClientSaleApplication currentEntity = this.CurrentEntity;

            ClientCautionMoney currentEntity = this.CurrentEntity;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageClientCautionMoneyRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new ClientCautionMoney()
                {
                    CompanyID = SiteUser.GetCurrentSiteUser().CompanyID
                };

                PageClientCautionMoneyRepository.Add(currentEntity);
            }
            //currentEntity.ApplyDate = rdpApplyDate.SelectedDate.Value;
            currentEntity.CautionMoneyTypeID = rcbxCautionMoneyType.SelectedValue.ToInt();
            currentEntity.EndDate = rdpEndDate.SelectedDate.Value;
            currentEntity.IsStop = false;
            //currentEntity.PaymentCautionMoney = txtPaymentCautionMoney.Value.ToDecimal();
            currentEntity.ProductID = rcbxProduct.SelectedValue.ToInt();
            currentEntity.ProductSpecificationID = rcbxProductSpecification.SelectedValue.ToInt();
            currentEntity.Remark = txtRemark.Text.Trim();
            currentEntity.DepartmentID = rcbxDepartment.SelectedValue.ToInt();
            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
            currentEntity.ClientUserID = rcbxClientUser.SelectedValue.ToInt();
            currentEntity.Remark = txtRemark.Text;
            currentEntity.Payer = txtPayer.Text.Trim();

            PageClientCautionMoneyRepository.Save();


            hdnCurrentEntityID.Value = currentEntity.ID.ToString();
            if (this.CurrentEntity != null)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REFRESH);
            }
        }

        private void SaveClientCautionMoneyBasicData(ClientCautionMoney ClientCautionMoney)
        {
            PageClientCautionMoneyRepository.Save();
        }
        private void ValidControls()
        {

        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageClientCautionMoneyRepository.DeleteByID(this.CurrentEntityID);
                PageClientCautionMoneyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }

        #region rgAppPayments

        protected void rgAppPayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

            var uiSearchObj = new UISearchApplicationPayment
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                PaymentTypeID = (int)EPaymentType.Income
            };

            int totalRecords;

            var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;
        }
        protected void rgAppPayments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UIApplicationPayment uiEntity = null;

                if (e.Item.ItemIndex >= 0)
                    uiEntity = (UIApplicationPayment)gridDataItem.DataItem;

                var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                if (rdpPayDate != null && uiEntity != null)
                    rdpPayDate.SelectedDate = uiEntity.PayDate;
                else
                    rdpPayDate.SelectedDate = DateTime.Now;

                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                if (rcbxToAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Company,
                            CompanyID = CurrentEntity.CompanyID
                        }
                    };

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
                    rcbxToAccount.DataSource = bankAccounts;
                    rcbxToAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxToAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxToAccount.DataBind();

                    if (uiEntity != null)
                        rcbxToAccount.SelectedValue = uiEntity.ToBankAccountID.ToString();
                }

                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                if (txtAmount != null && uiEntity != null)
                    txtAmount.DbValue = uiEntity.Amount;

                var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                if (txtFee != null && uiEntity != null)
                    txtFee.DbValue = uiEntity.Fee;
            }
        }

        protected void rgAppPayments_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                ApplicationPayment appPayment = new ApplicationPayment();

                var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                appPayment.PayDate = rdpPayDate.SelectedDate;


                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");
                if (rcbxToAccount.SelectedItem != null)
                {
                    appPayment.ToBankAccountID = rcbxToAccount.SelectedValue.ToIntOrNull();
                    appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                }
                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                appPayment.Amount = (decimal?)txtAmount.Value;

                var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                appPayment.Fee = (decimal?)txtFee.Value;

                appPayment.ApplicationID = this.CurrentEntityID.Value;
                appPayment.WorkflowID = this.CurrentWorkFlowID;
                appPayment.PaymentStatusID = (int)EPaymentStatus.Paid;
                appPayment.PaymentTypeID = (int)EPaymentType.Income;

                PageAppPaymentRepository.Add(appPayment);

                PageAppPaymentRepository.Save();
            }

            rgAppPayments.Rebind();

            BindPaymentSummary();
        }
        private void BindPaymentSummary()
        {
            var appPaymentAmounts = PageAppPaymentRepository
                .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == CurrentEntity.ID && x.IsDeleted == false)
                .Select(x => x.Amount).ToList();

            if (appPaymentAmounts.Count > 0)
            {
                decimal totalPaymentAmount = appPaymentAmounts.Sum(x => x ?? 0);

                lblTotalPaymentAmount.Text = totalPaymentAmount.ToString("C2");
                lblCapitalTotalPaymentAmount.Text = totalPaymentAmount.ToString().ConvertToChineseMoney();
            }
        }
        protected void rgAppPayments_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    var appPayment = PageAppPaymentRepository.GetByID(id);

                    var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                    appPayment.PayDate = rdpPayDate.SelectedDate;


                    var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");
                    if (rcbxToAccount.SelectedItem != null)
                    {
                        appPayment.ToBankAccountID = rcbxToAccount.SelectedValue.ToIntOrNull();
                        appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                    }
                    var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    appPayment.Amount = (decimal?)txtAmount.Value;

                    var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                    appPayment.Fee = (decimal?)txtFee.Value;
                    PageAppPaymentRepository.Save();

                    rgAppPayments.Rebind();
                }
            }

            BindPaymentSummary();
        }

        protected void rgAppPayments_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                //if (plAddCommand != null)
                //{
                //    if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                //            || (this.CanAuditByTreasurersUserIDs.Contains(CurrentUser.UserID)
                //    && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByTreasurers)))
                //        plAddCommand.Visible = true;
                //    else
                //        plAddCommand.Visible = false;
                //}
            }
        }

        protected void rgAppPayments_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            //if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
            //    || (this.CanAuditByTreasurersUserIDs.Contains(CurrentUser.UserID)
            //        && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByTreasurers)))
            //{
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            //}
            //else
            //{
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
            //}
        }

        protected void rgAppPayments_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (id.BiggerThanZero())
            {

                PageAppPaymentRepository.DeleteByID(id);
                PageAppPaymentRepository.Save();

                rgAppPayments.Rebind();
                BindPaymentSummary();
            }

        }

        #endregion




        #region 退款申请

        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null || _CanAccessUserIDs.Count == 0)
                {
                    _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewSupplierCautionMoneyApply);
                }

                return _CanAccessUserIDs;
            }
        }

        private void BindClientCautionMoneyReturnApplication(bool isNeedRebind)
        {

            IList<int> includeWorkflowStatusIDs = PageWorkflowStatusRepository
         .GetCanAccessIDsByUserID(this.CurrentWorkFlowID, CurrentUser.UserID);

            if (includeWorkflowStatusIDs == null)
            {
                includeWorkflowStatusIDs = new List<int>();
                includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Completed);
            }
            else
            {
                if (this.CanAccessUserIDs.Contains(CurrentUser.UserID) || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.TemporarySave))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.TemporarySave);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Submit))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Submit);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ReturnBasicInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ReturnBasicInfo);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByDeptManagers))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByDeptManagers);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByTreasurers))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByTreasurers);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Completed))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Completed);
                }
            }
            UISearchClientCautionMoneyReturnApplication uiSearchObj = new UISearchClientCautionMoneyReturnApplication()
            {
                //BeginDate = rdpBeginDate.SelectedDate,
                //EndDate = rdpEndDate.SelectedDate,
                //WorkflowStatusIDs = new int[] { (int)EWorkflowStatus.TemporarySave, (int)EWorkflowStatus.ReturnBasicInfo,
                //(int)EWorkflowStatus.Submit,(int)EWorkflowStatus.ApprovedByDeptManagers,
                //(int)EWorkflowStatus.ApprovedByTreasurers},
                //SupplierName = txtSupplierName.Text.Trim(),
                //ProductName = txtProductName.Text.Trim()
                ClientCautionMoneyID = this.CurrentEntityID
            };
            uiSearchObj.WorkflowStatusIDs = includeWorkflowStatusIDs.ToArray();



            int totalRecords = 0;

            var uiSupplierCautionMoneys = PageClientCautionMoneyReturnApplicationRepository.GetUIList(uiSearchObj, rgClientCautionMoneyReturnApplications.CurrentPageIndex, rgClientCautionMoneyReturnApplications.PageSize, out totalRecords);

            rgClientCautionMoneyReturnApplications.VirtualItemCount = totalRecords;

            rgClientCautionMoneyReturnApplications.DataSource = uiSupplierCautionMoneys;


            if (isNeedRebind)
                rgClientCautionMoneyReturnApplications.Rebind();
        }


        protected void rgClientCautionMoneyReturnApplications_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindClientCautionMoneyReturnApplication(false);
        }

        protected void rgClientCautionMoneyReturnApplications_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var supplierCautionMoneyID = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (supplierCautionMoneyID.BiggerThanZero())
            {
                PageClientCautionMoneyReturnApplicationRepository.DeleteByID(supplierCautionMoneyID);
                PageClientCautionMoneyReturnApplicationRepository.Save();
                rgClientCautionMoneyReturnApplications.Rebind();
            }


        }

        protected void rgClientCautionMoneyReturnApplications_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgClientCautionMoneyReturnApplications_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }
        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSupplierCautionMoneyApply);

                return _CanEditUserIDs;
            }
        }
        protected void rgClientCautionMoneyReturnApplications_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
              || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIClientCautionMoneyReturnApplication)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    string linkHtml = "<a href=\"javascript:void(0);\" onclick=\"redirectToClientCautionMoneyReturnApplyMaintenancePage(" + uiEntity.ID + ")\">";

                    var canAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID((int)EWorkflow.ClientCautionMoneyReturnApply, uiEntity.WorkflowStatusID);

                    bool isCanAccessUser = false;
                    if (canAccessUserIDs.Contains(CurrentUser.UserID))
                        isCanAccessUser = true;

                    bool isCanEditUser = false;
                    if (CanEditUserIDs.Contains(CurrentUser.UserID)
                        || uiEntity.CreatedByUserID == CurrentUser.UserID)
                        isCanEditUser = true;

                    bool isShowDeleteLink = false;
                    bool isShowStopLink = false;

                    EWorkflowStatus workflowStatus = (EWorkflowStatus)uiEntity.WorkflowStatusID;

                    if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    {
                        linkHtml += "编辑";

                        switch (workflowStatus)
                        {
                            case EWorkflowStatus.TemporarySave:
                            case EWorkflowStatus.ReturnBasicInfo:
                                isShowDeleteLink = true;
                                break;
                        }
                    }
                    else
                    {
                        if (isCanAccessUser)
                        {
                            switch (workflowStatus)
                            {
                                case EWorkflowStatus.TemporarySave:
                                case EWorkflowStatus.ReturnBasicInfo:
                                    if (isCanEditUser)
                                    {
                                        linkHtml += "编辑";
                                        isShowDeleteLink = true;
                                    }
                                    else
                                        linkHtml += "查看";
                                    break;

                                case EWorkflowStatus.Submit:
                                case EWorkflowStatus.ApprovedByDeptManagers:
                                    linkHtml += "审核";
                                    break;
                                case EWorkflowStatus.ApprovedByTreasurers:
                                    linkHtml += "支付";
                                    break;
                            }
                        }
                        else
                            linkHtml += "查看";
                    }

                    linkHtml += "</a>";

                    //if (this.CanStopUserIDs.Contains(CurrentUser.UserID)
                    //    && uiEntity.IsStop == false)
                    //{
                    //    switch (workflowStatus)
                    //    {
                    //        case EWorkflowStatus.ApprovedBasicInfo:
                    //        case EWorkflowStatus.Shipping:
                    //            isShowStopLink = true;
                    //            break;
                    //    }
                    //}

                    var editColumn = rgClientCautionMoneyReturnApplications.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
                    }

                    var deleteColumn = rgClientCautionMoneyReturnApplications.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE);

                    if (deleteColumn != null)
                    {
                        var deleteCell = gridDataItem.Cells[deleteColumn.OrderIndex];

                        if (deleteCell != null && !isShowDeleteLink)
                            deleteCell.Text = string.Empty;
                    }

                    //var stopColumn = rgClientCautionMoneyReturnApplications.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_STOP);

                    //if (stopColumn != null)
                    //{
                    //    var stopCell = gridDataItem.Cells[stopColumn.OrderIndex];

                    //    if (stopCell != null && !isShowStopLink)
                    //        stopCell.Text = string.Empty;
                    //}
                }
            }
        }
        #endregion


    }
}