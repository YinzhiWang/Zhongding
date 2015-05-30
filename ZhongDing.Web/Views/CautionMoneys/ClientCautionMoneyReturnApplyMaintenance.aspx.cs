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
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.CautionMoneys
{
    public partial class ClientCautionMoneyReturnApplyMaintenance : WorkflowBasePage
    {
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.ClientCautionMoneyReturnApply;
        }
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientCautionMoneyReturnApply;
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

        private ISupplierCautionMoneyRepository _PageSupplierCautionMoneyRepository;
        private ISupplierCautionMoneyRepository PageSupplierCautionMoneyRepository
        {
            get
            {
                if (_PageSupplierCautionMoneyRepository == null)
                    _PageSupplierCautionMoneyRepository = new SupplierCautionMoneyRepository();
                return _PageSupplierCautionMoneyRepository;
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
        private ISupplierCautionMoneyDeductionRepository _PageSupplierCautionMoneyDeductionRepository;
        private ISupplierCautionMoneyDeductionRepository PageSupplierCautionMoneyDeductionRepository
        {
            get
            {
                if (_PageSupplierCautionMoneyDeductionRepository == null)
                    _PageSupplierCautionMoneyDeductionRepository = new SupplierCautionMoneyDeductionRepository();

                return _PageSupplierCautionMoneyDeductionRepository;
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientCautionMoneyManage;
            if (!IsPostBack)
            {
                LoadCurrentEntity();
            }
        }

        /// <summary>
        /// 当前实体ClientCautionMoneyID
        /// </summary>
        /// <value>The ClientCautionMoneyID.</value>
        public int? ClientCautionMoneyID
        {
            get
            {
                return IntParameter("ClientCautionMoneyID");
            }
        }

        private ClientCautionMoney _CurrentClientCautionMoneyEntity;
        private ClientCautionMoney CurrentClientCautionMoneyEntity
        {
            get
            {
                if (_CurrentClientCautionMoneyEntity == null)
                    if (this.ClientCautionMoneyID.BiggerThanZero())
                        _CurrentClientCautionMoneyEntity = PageClientCautionMoneyRepository.GetByID(this.ClientCautionMoneyID);

                return _CurrentClientCautionMoneyEntity;
            }
        }

        private ClientCautionMoneyReturnApplication _CurrentEntity;
        private ClientCautionMoneyReturnApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageClientCautionMoneyReturnApplicationRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null || _CanAccessUserIDs.Count == 0)
                {
                    if (this.CurrentEntity == null)
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientCautionMoneyReturnApply);
                    else
                        _CanAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID(this.CurrentWorkFlowID, this.CurrentEntity.WorkflowStatusID);
                }

                return _CanAccessUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientCautionMoneyReturnApply);

                return _CanEditUserIDs;
            }
        }



        private IList<int> _CanAuditByDeptManagersUserIDs;
        private IList<int> CanAuditByDeptManagersUserIDs
        {
            get
            {
                if (_CanAuditByDeptManagersUserIDs == null)
                    _CanAuditByDeptManagersUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditClientCautionMoneyReturnApplyByDeptManagers);

                return _CanAuditByDeptManagersUserIDs;
            }
        }

        private IList<int> _CanAuditByTreasurersUserIDs;
        private IList<int> CanAuditByTreasurersUserIDs
        {
            get
            {
                if (_CanAuditByTreasurersUserIDs == null)
                    _CanAuditByTreasurersUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditClientCautionMoneyReturnApplyByTreasurers);

                return _CanAuditByTreasurersUserIDs;
            }
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
        private void LoadCurrentEntity()
        {
            DisabledBasicInfoFiledsControls();

            if (this.CurrentClientCautionMoneyEntity != null)
            {
                var uiClientCautionMoney = PageClientCautionMoneyRepository.GetUIClientCautionMoneyByID(this.ClientCautionMoneyID.Value);
                if (uiClientCautionMoney != null)
                {
                    if (uiClientCautionMoney.NotReturnCautionMoney <= 0)
                    {
                        this.Master.BaseNotification.OnClientHidden = "redirectClientCautionMoneyMaintenancePage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                        this.Master.BaseNotification.AutoCloseDelay = 1000;
                        this.Master.BaseNotification.Show("未退保证金等于￥0.00，不可以申请退回");
                        return;
                    }
                    txtNotReturnCautionMoney.Value = uiClientCautionMoney.NotReturnCautionMoney.ToDoubleOrNull();
                }

                rdpApplyDate.SelectedDate = DateTime.Now;
                rdpEndDate.SelectedDate = this.CurrentClientCautionMoneyEntity.EndDate;
                lblOperator.Text += PageUsersRepository.GetUserFullNameByID(this.CurrentClientCautionMoneyEntity.CreatedBy.HasValue
                    ? this.CurrentClientCautionMoneyEntity.CreatedBy.Value : GlobalConst.INVALID_INT);
                BindProducts();
                rcbxProduct.SelectedValue = CurrentClientCautionMoneyEntity.ProductID.ToString();
                BindCautionMoneyTypes();
                rcbxCautionMoneyType.SelectedValue = CurrentClientCautionMoneyEntity.CautionMoneyTypeID.ToString();
                BindProductSpecifications(CurrentClientCautionMoneyEntity.ProductID);
                rcbxProductSpecification.SelectedValue = CurrentClientCautionMoneyEntity.ProductSpecificationID.ToString();

                BindClientUsers();
                rcbxClientUser.SelectedValue = CurrentClientCautionMoneyEntity.ClientUserID.ToString();


                BindPaymentSummary();



            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);
            }


            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();


                txtReason.Text = CurrentEntity.Reason;
                txtAmount.Value = CurrentEntity.Amount.ToDoubleOrNull();

                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);

                    //if (CurrentEntity.DeliveryModeID != (int)EDeliveryMode.GuaranteeDelivery)
                    //    divAppPayments.Visible = true;

                    //#region 审核通过和发货中的订单，只能中止
                    //switch (workfolwStatus)
                    //{
                    //    case EWorkflowStatus.ApprovedBasicInfo:
                    //    case EWorkflowStatus.Shipping:
                    //        //if (CanStopUserIDs.Contains(CurrentUser.UserID))
                    //        //    cbxIsStop.Enabled = true;
                    //        break;
                    //}
                    //#endregion
                }
                else
                {
                    switch (workfolwStatus)
                    {
                        case EWorkflowStatus.TemporarySave:
                        case EWorkflowStatus.ReturnBasicInfo:
                            #region 暂存和基础信息退回（订单创建者才能修改）

                            if (CurrentUser.UserID == this.CurrentClientCautionMoneyEntity.CreatedBy
                                || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                                ShowSaveButtons(true);
                            else
                                DisabledBasicInfoControls();

                            btnAudit.Visible = false;
                            btnReturn.Visible = false;
                            divAudit.Visible = false;
                            //divAuditAll.Visible = false;
                            divAppPayments.Visible = false;
                            //txtAuditComment.Visible = false;

                            #endregion

                            break;
                        case EWorkflowStatus.Submit:
                            #region 已提交，待审核

                            DisabledBasicInfoControls();

                            divAppPayments.Visible = false;

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                ShowAuditControls(true);

                                //if (CurrentEntity.DeliveryModeID != (int)EDeliveryMode.GuaranteeDelivery)
                                //    divAppPayments.Visible = true;
                                //else
                                //    divAppPayments.Visible = false;

                            }
                            else
                            {
                                ShowAuditControls(false);
                                divAuditAll.Visible = true;
                                divAudit.Visible = false;
                            }

                            #endregion

                            break;
                        case EWorkflowStatus.ApprovedByDeptManagers:
                            #region 已提交或需进入下一级审核，待审核

                            DisabledBasicInfoControls();

                            divAppPayments.Visible = false;


                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowAuditControls(true);
                            else
                                ShowAuditControls(false);

                            #endregion
                            break;

                        case EWorkflowStatus.ApprovedByTreasurers:
                            #region 审核通过，待支付

                            DisabledBasicInfoControls();
                            ShowAuditControls(false);
                            divAuditAll.Visible = true;

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                btnPay.Visible = true;
                            }
                            else
                                rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                            #endregion

                            break;


                        case EWorkflowStatus.Paid:
                            #region 已支付，只能撤销

                            DisabledBasicInfoControls();

                            ShowSaveButtons(false);

                            ShowAuditControls(false);
                            divAuditAll.Visible = true;

                            rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

                            if (CurrentEntity.PaidBy == CurrentUser.UserID)
                            {
                                //divCancel.Visible = true;
                                //btnCancel.Visible = true;
                            }

                            #endregion
                            break;

                    }
                }
            }
            else
            {
                InitDefaultData();

                if (!this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectClientCautionMoneyMaintenancePage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("您没有权限新增客户订单");
                }
            }
        }

        private void DisabledBasicInfoFiledsControls()
        {
            rdpApplyDate.Enabled = rcbxProduct.Enabled = rcbxProductSpecification.Enabled = rcbxClientUser.Enabled
                  = rcbxCautionMoneyType.Enabled = rdpEndDate.Enabled = rcbxDepartment.Enabled = rcbxClientUser.Enabled
                  = txtNotReturnCautionMoney.Enabled
                  = false;
        }
        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (CurrentClientCautionMoneyEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();
                    IClientCautionMoneyReturnApplicationRepository clientCautionMoneyReturnApplicationRepository = new ClientCautionMoneyReturnApplicationRepository();

                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();
                    appPaymentRepository.SetDbModel(db);
                    clientCautionMoneyReturnApplicationRepository.SetDbModel(db);
                    var currentEntity = clientCautionMoneyReturnApplicationRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {

                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == currentEntity.ID).ToList();

                        var needReturnCautionMoney = currentEntity.Amount;

                        var totalPayAmount = appPayments.Sum(x => x.Amount.HasValue ? x.Amount.Value : 0M);

                        if (totalPayAmount != needReturnCautionMoney)
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("支付总额不等于退回申请退回金额，不能确认支付");
                        }
                        else
                        {
                            foreach (var item in appPayments)
                            {
                                item.PaymentStatusID = (int)EPaymentStatus.Paid;
                            }

                            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.Paid;
                            currentEntity.PaidDate = DateTime.Now;
                            currentEntity.PaidBy = CurrentUser.UserID;

                            unitOfWork.SaveChanges();

                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
                    }
                }
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);
            }
        }

        protected void rgAuditNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationNote()
            {
                WorkflowID = this.CurrentWorkFlowID,
                NoteTypeID = (int)EAppNoteType.AuditOpinion,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            var appNotes = PageAppNoteRepository.GetUIList(uiSearchObj);

            rgAuditNotes.DataSource = appNotes;
        }
        /// <summary>
        /// 显示或隐藏保存和提交按钮
        /// </summary>
        private void ShowSaveButtons(bool isShow)
        {
            btnSave.Visible = isShow;
            btnSubmit.Visible = isShow;
        }

        /// <summary>
        /// 显示或隐藏审核相关控件
        /// </summary>
        private void ShowAuditControls(bool isShow)
        {
            divAudit.Visible = isShow;
            //divAppPayments.Visible = isShow;
            divAuditAll.Visible = isShow;
            btnAudit.Visible = isShow;
            btnReturn.Visible = isShow;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            txtReason.Enabled = txtAmount.Enabled = false;

            btnSave.Visible = false;
            btnSubmit.Visible = false;
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        private void InitDefaultData()
        {
            btnSubmit.Visible = false;
            btnAudit.Visible = false;
            btnReturn.Visible = false;
            divAudit.Visible = false;
            divAuditAll.Visible = false;
            divAppPayments.Visible = false;

            divComments.Visible = false;
            lblOperator.Text = "操作人：" + CurrentUser.FullName;
            //var saleOrderType = PageSaleOrderTypeRepository.GetByID(SaleOrderTypeID);
            //if (saleOrderType != null)
            //    lblSalesOrderType.Text = saleOrderType.TypeName;

            //hdnSaleOrderTypeID.Value = this.SaleOrderTypeID.ToString();
        }
        protected void rgAppNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationNote()
            {
                WorkflowID = this.CurrentWorkFlowID,
                NoteTypeID = (int)EAppNoteType.Comment,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            var appNotes = PageAppNoteRepository.GetUIList(uiSearchObj);

            rgAppNotes.DataSource = appNotes;
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

        private void BindCautionMoneyTypes()
        {
            var items = PageCautionMoneyTypeRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {

                }
            });

            rcbxCautionMoneyType.DataSource = items;
            rcbxCautionMoneyType.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxCautionMoneyType.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxCautionMoneyType.DataBind();

            rcbxCautionMoneyType.Items.Insert(0, new RadComboBoxItem("", ""));
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


            if (rcbxCautionMoneyType.SelectedValue.IsNullOrEmpty())
                cvCautionMoneyType.IsValid = false;



            if (!IsValid) return;
            //ClientSaleApplication currentEntity = this.CurrentEntity;

            var uiClientCautionMoney = PageClientCautionMoneyRepository.GetUIClientCautionMoneyByID(this.ClientCautionMoneyID.Value);
            if (uiClientCautionMoney != null)
            {
                if (txtAmount.Value.ToDecimal() > uiClientCautionMoney.NotReturnCautionMoney.Value)
                {
                    //this.Master.BaseNotification.OnClientHidden = "redirectClientCautionMoneyMaintenancePage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("申请退回金额不能大于未退保证金￥"+uiClientCautionMoney.NotReturnCautionMoney.ToString("f2")+"");
                    return;
                }
                txtNotReturnCautionMoney.Value = uiClientCautionMoney.NotReturnCautionMoney.ToDoubleOrNull();
            }


            ClientCautionMoneyReturnApplication currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageClientCautionMoneyReturnApplicationRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new ClientCautionMoneyReturnApplication()
                {

                };

                PageClientCautionMoneyReturnApplicationRepository.Add(currentEntity);
            }
            currentEntity.ApplyDate = rdpApplyDate.SelectedDate.Value;
            currentEntity.ClientCautionMoneyID = this.ClientCautionMoneyID.Value;
            currentEntity.IsStop = false;
            currentEntity.Amount = txtAmount.Value.ToDecimal();
            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
            currentEntity.Reason = txtReason.Text.Trim();


            PageClientCautionMoneyReturnApplicationRepository.Save();


            hdnCurrentEntityID.Value = currentEntity.ID.ToString();
            if (this.CurrentEntity != null)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectClientCautionMoneyMaintenancePage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REFRESH);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ValidControls();


            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                switch (workfolwStatus)
                {
                    case EWorkflowStatus.TemporarySave:
                    case EWorkflowStatus.ReturnBasicInfo:
                        if (this.CurrentEntity.ID > 0)
                        {
                            this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;

                            SaveClientCautionMoneyReturnApplicationBasicData(this.CurrentEntity);

                            this.Master.BaseNotification.OnClientHidden = "redirectClientCautionMoneyMaintenancePage";
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
                        else
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("供应商担保金申请不存在，不能提交");
                        }

                        break;
                }
            }
        }

        private void SaveClientCautionMoneyReturnApplicationBasicData(ClientCautionMoneyReturnApplication clientCautionMoney)
        {
            PageClientCautionMoneyReturnApplicationRepository.Save();
        }
        private void ValidControls()
        {

        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientCautionMoneyReturnApplicationRepository clientCautionMoneyReturnApplicationRepository = new ClientCautionMoneyReturnApplicationRepository();

                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    appNoteRepository.SetDbModel(db);
                    clientCautionMoneyReturnApplicationRepository.SetDbModel(db);


                    var currentEntity = clientCautionMoneyReturnApplicationRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditClientCautionMoneyReturnApplyByDeptManagers;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                            case (int)EWorkflowStatus.ApprovedByDeptManagers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditClientCautionMoneyReturnApplyByTreasurers;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectClientCautionMoneyMaintenancePage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        protected void btnAudit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientCautionMoneyReturnApplicationRepository clientCautionMoneyReturnApplicationRepository = new ClientCautionMoneyReturnApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    appNoteRepository.SetDbModel(db);
                    clientCautionMoneyReturnApplicationRepository.SetDbModel(db);



                    var clientCautionMoneyReturnApplication = clientCautionMoneyReturnApplicationRepository.GetByID(this.CurrentEntityID);
                    var appNote = new ApplicationNote();
                    appNote.WorkflowID = CurrentWorkFlowID;
                    appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                    appNoteRepository.Add(appNote);

                    switch (clientCautionMoneyReturnApplication.WorkflowStatusID)
                    {
                        case (int)EWorkflowStatus.Submit:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditSupplierCautionMoneyApplyByDeptManagers;
                            appNote.ApplicationID = clientCautionMoneyReturnApplication.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            clientCautionMoneyReturnApplication.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDeptManagers;
                            break;
                        case (int)EWorkflowStatus.ApprovedByDeptManagers:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditSupplierCautionMoneyApplyByTreasurers;
                            appNote.ApplicationID = clientCautionMoneyReturnApplication.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            clientCautionMoneyReturnApplication.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByTreasurers;
                            break;
                    }
                    unitOfWork.SaveChanges();

                    this.Master.BaseNotification.OnClientHidden = "redirectClientCautionMoneyMaintenancePage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageClientCautionMoneyReturnApplicationRepository.DeleteByID(this.CurrentEntityID);
                PageClientCautionMoneyReturnApplicationRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }

        #region rgAppPayments

        protected void rgAppPayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //var uiSearchObj = new UISearchApplicationPayment
            //{
            //    WorkflowID = this.CurrentWorkFlowID,
            //    ApplicationID = this.CurrentEntityID.HasValue
            //    ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            //};

            //int totalRecords;

            //var appPayments = PageSupplierCautionMoneyRepository.GetPayments(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            //rgAppPayments.DataSource = appPayments;
            //rgAppPayments.VirtualItemCount = totalRecords;

            var uiSearchObj = new UISearchApplicationPayment
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                PaymentTypeID = (int)EPaymentType.Expend
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

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (rcbxFromAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Company,
                            CompanyID = CurrentClientCautionMoneyEntity.CompanyID
                        }
                    };

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
                    rcbxFromAccount.DataSource = bankAccounts;
                    rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxFromAccount.DataBind();

                    if (uiEntity != null)
                        rcbxFromAccount.SelectedValue = uiEntity.FromBankAccountID.ToString();
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

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                {
                    int fromAccountID;
                    if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                        appPayment.FromBankAccountID = fromAccountID;
                    appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                }

                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                appPayment.Amount = (decimal?)txtAmount.Value;

                var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                appPayment.Fee = (decimal?)txtFee.Value;

                var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                if (txtComment != null)
                    appPayment.Comment = txtComment.Text;

                appPayment.ApplicationID = this.CurrentEntityID.Value;
                appPayment.WorkflowID = this.CurrentWorkFlowID;
                appPayment.PaymentStatusID = (int)EPaymentStatus.ToBePaid;
                appPayment.PaymentTypeID = (int)EPaymentType.Expend;

                PageAppPaymentRepository.Add(appPayment);

                PageAppPaymentRepository.Save();
            }

            rgAppPayments.Rebind();

            BindPaymentSummary();
        }
        private void BindPaymentSummary()
        {
            var appPaymentAmounts = PageAppPaymentRepository
                .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == CurrentClientCautionMoneyEntity.ID && x.IsDeleted == false)
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

                    var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                    if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                    {
                        int fromAccountID;
                        if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                            appPayment.FromBankAccountID = fromAccountID;
                        appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                    }


                    var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    appPayment.Amount = (decimal?)txtAmount.Value;

                    var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                    appPayment.Fee = (decimal?)txtFee.Value;


                    var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                    if (txtComment != null)
                        appPayment.Comment = txtComment.Text;

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

                if (plAddCommand != null)
                {
                    if (this.CurrentClientCautionMoneyEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                            || (this.CanAuditByTreasurersUserIDs.Contains(CurrentUser.UserID)
                    && this.CurrentClientCautionMoneyEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByTreasurers)))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        protected void rgAppPayments_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                || (this.CanAuditByTreasurersUserIDs.Contains(CurrentUser.UserID)
                    && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByTreasurers)))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
            }
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

    }
}