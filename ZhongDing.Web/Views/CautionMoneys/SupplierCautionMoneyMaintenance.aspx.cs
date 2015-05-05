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
    public partial class SupplierCautionMoneyMaintenance : WorkflowBasePage
    {
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.SupplierCautionMoneyApply;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Master.MenuItemID = (int)EMenuItem.SupplierCautionMoneyApplyManage;


            if (!IsPostBack)
            {

                LoadCurrentEntity();

            }
            this.Master.MenuItemID = (int)EMenuItem.SupplierCautionMoneyApplyManage;
            if (CurrentEntity != null && CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.Paid)
            {
                this.Master.MenuItemID = (int)EMenuItem.SupplierCautionMoneyManage;
            }

        }
        private SupplierCautionMoney _CurrentEntity;
        private SupplierCautionMoney CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageSupplierCautionMoneyRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewSupplierCautionMoneyApply);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSupplierCautionMoneyApply);

                return _CanEditUserIDs;
            }
        }



        private IList<int> _CanAuditByDeptManagersUserIDs;
        private IList<int> CanAuditByDeptManagersUserIDs
        {
            get
            {
                if (_CanAuditByDeptManagersUserIDs == null)
                    _CanAuditByDeptManagersUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditSupplierCautionMoneyApplyByDeptManagers);

                return _CanAuditByDeptManagersUserIDs;
            }
        }

        private IList<int> _CanAuditByTreasurersUserIDs;
        private IList<int> CanAuditByTreasurersUserIDs
        {
            get
            {
                if (_CanAuditByTreasurersUserIDs == null)
                    _CanAuditByTreasurersUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditSupplierCautionMoneyApplyByTreasurers);

                return _CanAuditByTreasurersUserIDs;
            }
        }
        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();


                rdpApplyDate.SelectedDate = this.CurrentEntity.ApplyDate;
                txtPaymentCautionMoney.Value = this.CurrentEntity.PaymentCautionMoney.ToDouble();
                rdpEndDate.SelectedDate = this.CurrentEntity.EndDate;
                //txtRemark.Text = this.CurrentEntity.Remark;

                lblOperator.Text += PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);
                BindProducts();
                rcbxProduct.SelectedValue = CurrentEntity.ProductID.ToString();
                BindCautionMoneyTypes();
                rcbxCautionMoneyType.SelectedValue = CurrentEntity.CautionMoneyTypeID.ToString();
                BindProductSpecifications(CurrentEntity.ProductID);
                rcbxProductSpecification.SelectedValue = CurrentEntity.ProductSpecificationID.ToString();
                BindSuppliers();
                rcbxSupplier.SelectedValue = CurrentEntity.SupplierID.ToString();


                BindPaymentSummary();


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

                            if (CurrentUser.UserID == this.CurrentEntity.CreatedBy
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
                            divRefund.Visible = false;
                            #endregion

                            break;
                        case EWorkflowStatus.Submit:
                            #region 已提交，待审核

                            DisabledBasicInfoControls();
                          
                            divAppPayments.Visible = false;
                            divRefund.Visible = false;
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
                            divRefund.Visible = false;

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
                            divRefund.Visible = false;
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
                            divRefund.Visible = true;
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
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("您没有权限新增客户订单");
                }
                else
                {
                    BindProducts();
                    BindCautionMoneyTypes();
                    BindSuppliers();
                }


            }
        }
        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();
                    ISupplierCautionMoneyRepository supplierCautionMoneyRepository = new SupplierCautionMoneyRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    appPaymentRepository.SetDbModel(db);
                    supplierCautionMoneyRepository.SetDbModel(db);

                    var currentEntity = supplierCautionMoneyRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {

                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == currentEntity.ID).ToList();

                        var totalPaymentCautionMoney = currentEntity.PaymentCautionMoney;

                        var totalPayAmount = appPayments.Sum(x => x.Amount.HasValue ? x.Amount.Value : 0M);

                        if (totalPayAmount != totalPaymentCautionMoney)
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("支付总额不等于保证金总金额，不能确认支付");
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
            rdpApplyDate.Enabled = rcbxProduct.Enabled = rcbxProductSpecification.Enabled = rcbxSupplier.Enabled
                = txtPaymentCautionMoney.Enabled = rcbxCautionMoneyType.Enabled = rdpEndDate.Enabled = txtRemark.Enabled
                = false;

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
            divRefund.Visible = false;
            //divComment.Visible = false;
            divComments.Visible = false;
            //divOtherSections.Visible = false;
            //divStop.Visible = false;

            //txtOrderCode.Text = Utility.GenerateAutoSerialNo(PageClientSaleAppRepository.GetMaxEntityID(),
            //    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.CLIENT_ORDER);

            //rdpOrderDate.SelectedDate = DateTime.Now;
            //lblCreateBy.Text = CurrentUser.FullName;
            lblOperator.Text += CurrentUser.FullName;
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

            if (rcbxSupplier.SelectedValue.IsNullOrEmpty())
                cvSupplier.IsValid = false;
            if (rcbxCautionMoneyType.SelectedValue.IsNullOrEmpty())
                cvCautionMoneyType.IsValid = false;



            if (!IsValid) return;
            //ClientSaleApplication currentEntity = this.CurrentEntity;

            SupplierCautionMoney currentEntity = this.CurrentEntity;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageSupplierCautionMoneyRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new SupplierCautionMoney()
                {
                    CompanyID = SiteUser.GetCurrentSiteUser().CompanyID
                };

                PageSupplierCautionMoneyRepository.Add(currentEntity);
            }
            currentEntity.ApplyDate = rdpApplyDate.SelectedDate.Value;
            currentEntity.CautionMoneyTypeID = rcbxCautionMoneyType.SelectedValue.ToInt();
            currentEntity.EndDate = rdpEndDate.SelectedDate.Value;
            currentEntity.IsStop = false;
            currentEntity.PaymentCautionMoney = txtPaymentCautionMoney.Value.ToDecimal();
            currentEntity.ProductID = rcbxProduct.SelectedValue.ToInt();
            currentEntity.ProductSpecificationID = rcbxProductSpecification.SelectedValue.ToInt();
            currentEntity.Remark = txtRemark.Text.Trim();
            currentEntity.SupplierID = rcbxSupplier.SelectedValue.ToInt();
            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;

            //currentEntity.Remark = txtRemark.Text;


            PageSupplierCautionMoneyRepository.Save();

            if (!string.IsNullOrEmpty(txtRemark.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.SupplierCautionMoneyApply;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    appNote.WorkflowStepID = (int)EWorkflowStep.EditClientOrder;
                else
                    appNote.WorkflowStepID = (int)EWorkflowStep.NewClientOrder;

                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtRemark.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }
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

                            SaveSupplierCautionMoneyBasicData(this.CurrentEntity);

                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
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

        private void SaveSupplierCautionMoneyBasicData(SupplierCautionMoney supplierCautionMoney)
        {
            PageSupplierCautionMoneyRepository.Save();
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

                    ISupplierCautionMoneyRepository supplierCautionMoneyRepository = new SupplierCautionMoneyRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    appNoteRepository.SetDbModel(db);
                    supplierCautionMoneyRepository.SetDbModel(db);


                    var currentEntity = supplierCautionMoneyRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditSupplierCautionMoneyApplyByDeptManagers;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                            case (int)EWorkflowStatus.ApprovedByDeptManagers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditSupplierCautionMoneyApplyByTreasurers;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
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

                    ISupplierCautionMoneyRepository supplierCautionMoneyRepository = new SupplierCautionMoneyRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    appNoteRepository.SetDbModel(db);
                    supplierCautionMoneyRepository.SetDbModel(db);



                    var supplierCautionMoney = supplierCautionMoneyRepository.GetByID(this.CurrentEntityID);
                    var appNote = new ApplicationNote();
                    appNote.WorkflowID = CurrentWorkFlowID;
                    appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                    appNoteRepository.Add(appNote);

                    switch (supplierCautionMoney.WorkflowStatusID)
                    {
                        case (int)EWorkflowStatus.Submit:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditSupplierCautionMoneyApplyByDeptManagers;
                            appNote.ApplicationID = supplierCautionMoney.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            supplierCautionMoney.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDeptManagers;
                            break;
                        case (int)EWorkflowStatus.ApprovedByDeptManagers:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditSupplierCautionMoneyApplyByTreasurers;
                            appNote.ApplicationID = supplierCautionMoney.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            supplierCautionMoney.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByTreasurers;
                            break;
                    }
                    unitOfWork.SaveChanges();

                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
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
                PageSupplierCautionMoneyRepository.DeleteByID(this.CurrentEntityID);
                PageSupplierCautionMoneyRepository.Save();

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
                            CompanyID = CurrentEntity.CompanyID
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

                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                if (rcbxToAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Company,
                            SupplierID = CurrentEntity.SupplierID
                        }
                    };

                    var bankAccounts = PageSupplierBankAccountRepository.GetDropdownItems(uiSearchObj);
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

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                {
                    int fromAccountID;
                    if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                        appPayment.FromBankAccountID = fromAccountID;
                    appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                }
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

                    var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                    if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                    {
                        int fromAccountID;
                        if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                            appPayment.FromBankAccountID = fromAccountID;
                        appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                    }

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

                if (plAddCommand != null)
                {
                    if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                            || (this.CanAuditByTreasurersUserIDs.Contains(CurrentUser.UserID)
                    && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByTreasurers)))
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


        #region 抵扣 返款
        protected void rgSupplierRefunds_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UIApplicationPayment uiEntity = null;

                if (e.Item.ItemIndex >= 0)
                    uiEntity = (UIApplicationPayment)gridDataItem.DataItem;

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

                var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");

                if (rdpPayDate != null)
                    rdpPayDate.MaxDate = DateTime.Now;

                if (uiEntity != null)
                {
                    if (rdpPayDate != null)
                        rdpPayDate.SelectedDate = uiEntity.PayDate;

                    var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    if (txtAmount != null)
                        txtAmount.DbValue = uiEntity.Amount;

                    var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                    if (txtFee != null)
                        txtFee.DbValue = uiEntity.Fee;

                    var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                    if (txtComment != null)
                        txtComment.Text = uiEntity.Comment;
                }
            }
        }

        protected void rgSupplierRefunds_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                if (!IsValid)
                {
                    e.Canceled = true;
                }
                else
                {
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;

                        ApplicationPayment appPayment = new ApplicationPayment();

                        var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                        if (rdpPayDate != null)
                            appPayment.PayDate = rdpPayDate.SelectedDate;

                        var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                        if (!string.IsNullOrEmpty(rcbxToAccount.SelectedValue))
                        {
                            int toAccountID;
                            if (int.TryParse(rcbxToAccount.SelectedValue, out toAccountID))
                                appPayment.ToBankAccountID = toAccountID;
                            appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                        }

                        var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                        if (txtAmount != null)
                            appPayment.Amount = (decimal?)txtAmount.Value;

                        var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                        if (txtFee != null)
                            appPayment.Fee = (decimal?)txtFee.Value;

                        {
                            var uiSearchObj = new UISearchSupplierCautionMoney()
                            {
                                ID = CurrentEntityID.Value,
                                NeedStatistics = true
                            };

                            int totalRecords;

                            var uiSupplierRefundApp = PageSupplierCautionMoneyRepository.GetUIList(uiSearchObj, 0, 1, out totalRecords).FirstOrDefault();

                            if (uiSupplierRefundApp != null)
                            {
                                if ((uiSupplierRefundApp.TakeBackCautionMoney
                                    + appPayment.Amount.GetValueOrDefault(0)
                                    + appPayment.Fee.GetValueOrDefault(0)) > uiSupplierRefundApp.PaymentCautionMoney)
                                {
                                    ((CustomValidator)e.Item.FindControl("cvAmount")).IsValid = false;
                                    ((CustomValidator)e.Item.FindControl("cvFee")).IsValid = false;
                                    e.Canceled = true;
                                    return;
                                }
                            }
                        }


                        var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                        if (txtComment != null)
                            appPayment.Comment = txtComment.Text;

                        appPayment.ApplicationID = this.CurrentEntityID.Value;

                        appPayment.WorkflowID = this.CurrentWorkFlowID;
                        appPayment.PaymentStatusID = (int)EPaymentStatus.Paid;
                        appPayment.PaymentTypeID = (int)EPaymentType.Income;

                        PageAppPaymentRepository.Add(appPayment);

                        PageAppPaymentRepository.Save();

                        //hdnNeedRefreshPage.Value = true.ToString();

                        rgSupplierRefunds.Rebind();
                    }
                }
            }

            else if (e.CommandName == "Delete")
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    PageAppPaymentRepository.DeleteByID(dataItem.GetDataKeyValue("ID").ToInt());
                    PageAppPaymentRepository.Save();
                }
            }
        }

        protected void btnSearchRefund_Click(object sender, EventArgs e)
        {
            BindSupplierRefunds(true);
        }

        protected void btnResetRefund_Click(object sender, EventArgs e)
        {
            rdpRefundBeginDate.Clear();
            rdpRefundEndDate.Clear();

            BindSupplierRefunds(true);
        }
        private void BindSupplierRefunds(bool isNeedRebind)
        {
            IList<UIApplicationPayment> appPayments = new List<UIApplicationPayment>();

            int totalRecords = 0;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                var uiSearchObj = new UISearchApplicationPayment
                {
                    WorkflowID = this.CurrentWorkFlowID,
                    ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                    BeginDate = rdpRefundBeginDate.SelectedDate,
                    EndDate = rdpRefundEndDate.SelectedDate,
                    PaymentTypeID = (int)EPaymentType.Income
                };

                appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgSupplierRefunds.CurrentPageIndex, rgSupplierRefunds.PageSize, out totalRecords);
            }

            rgSupplierRefunds.DataSource = appPayments;
            rgSupplierRefunds.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgSupplierRefunds.Rebind();
        }

        private void BindSupplierDeductions(bool isNeedRebind)
        {
            IList<UISupplierCautionMoneyDeduction> supplierDeductions = new List<UISupplierCautionMoneyDeduction>();

            int totalRecords = 0;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                var uiSearchObj = new UISearchSupplierCautionMoneyDeduction
                {
                    SupplierCautionMoneyID = this.CurrentEntityID.Value,
                    BeginDate = rdpDeductionBeginDate.SelectedDate,
                    EndDate = rdpDeductionEndDate.SelectedDate,
                };

                supplierDeductions = PageSupplierCautionMoneyDeductionRepository.GetUIList(uiSearchObj, rgSupplierDeduction.CurrentPageIndex, rgSupplierDeduction.PageSize, out totalRecords);
            }


            rgSupplierDeduction.DataSource = supplierDeductions;
            rgSupplierDeduction.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgSupplierDeduction.Rebind();
        }
        protected void btnSearchDeduction_Click(object sender, EventArgs e)
        {
            BindSupplierDeductions(true);
        }

        protected void btnResetDeduction_Click(object sender, EventArgs e)
        {
            rdpDeductionBeginDate.Clear();
            rdpDeductionEndDate.Clear();

            BindSupplierDeductions(true);
        }
        protected void cvAmount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value))
            {
                decimal curAmount;

                if (decimal.TryParse(args.Value, out curAmount))
                {
                    var uiSearchObj = new UISearchSupplierCautionMoney()
                    {
                        ID = CurrentEntityID.Value,
                        NeedStatistics = true
                    };

                    int totalRecords;

                    var uiSupplierRefundApp = PageSupplierCautionMoneyRepository.GetUIList(uiSearchObj, 0, 1, out totalRecords).FirstOrDefault();

                    if (uiSupplierRefundApp != null)
                    {
                        if ((uiSupplierRefundApp.TakeBackCautionMoney + curAmount) > uiSupplierRefundApp.PaymentCautionMoney)
                        {
                            args.IsValid = false;
                        }
                    }
                }
            }
        }
        protected void cvFee_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value))
            {
                decimal curFee;

                if (decimal.TryParse(args.Value, out curFee))
                {
                    var uiSearchObj = new UISearchSupplierCautionMoney()
                    {
                        ID = CurrentEntityID.Value,
                        NeedStatistics = true
                    };


                    int totalRecords;

                    var uiSupplierRefundApp = PageSupplierCautionMoneyRepository.GetUIList(uiSearchObj, 0, 1, out totalRecords).FirstOrDefault();

                    if (uiSupplierRefundApp != null)
                    {
                        if ((uiSupplierRefundApp.TakeBackCautionMoney + curFee) > uiSupplierRefundApp.PaymentCautionMoney)
                        {
                            args.IsValid = false;
                        }
                    }
                }
            }
        }
        protected void rgSupplierRefunds_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindSupplierRefunds(false);
        }
        protected void rgSupplierDeduction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindSupplierDeductions(false);
        }

        protected void rgSupplierDeduction_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UISupplierDeduction uiEntity = null;

                if (e.Item.ItemIndex >= 0)
                    uiEntity = (UISupplierDeduction)gridDataItem.DataItem;

                var rcbxSupplier = (RadComboBox)e.Item.FindControl("rcbxSupplier");

                if (rcbxSupplier != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            CompanyID = CurrentEntity.CompanyID
                        }
                    };

                    //if (e.Item.ItemIndex < 0)
                    //{
                    //    var excludeItemValues = PageSupplierDeductionRepository
                    //        .GetList(x => x.SupplierRefundAppID == this.CurrentEntityID)
                    //        .Select(x => x.SupplierID)
                    //        .ToList();

                    //    if (excludeItemValues.Count > 0)
                    //        uiSearchObj.ExcludeItemValues = excludeItemValues;
                    //}

                    var suppliers = PageSupplierRepository.GetDropdownItems(uiSearchObj);
                    rcbxSupplier.DataSource = suppliers;
                    rcbxSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxSupplier.DataBind();

                    if (uiEntity != null)
                        rcbxSupplier.SelectedValue = uiEntity.SupplierID.ToString();
                }

                var rdpDeductedDate = (RadDatePicker)e.Item.FindControl("rdpDeductedDate");

                if (rdpDeductedDate != null)
                    rdpDeductedDate.MaxDate = DateTime.Now;

                if (uiEntity != null)
                {
                    if (rdpDeductedDate != null)
                        rdpDeductedDate.SelectedDate = uiEntity.DeductedDate;

                    var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    if (txtAmount != null)
                        txtAmount.DbValue = uiEntity.Amount;

                    var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                    if (txtComment != null)
                        txtComment.Text = uiEntity.Comment;
                }
            }
        }

        protected void rgSupplierDeduction_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                if (!IsValid)
                {
                    e.Canceled = true;
                }
                else
                {
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;

                        SupplierCautionMoneyDeduction supplierDeduction = new SupplierCautionMoneyDeduction();

                        var rdpDeductedDate = (RadDatePicker)e.Item.FindControl("rdpDeductedDate");
                        if (rdpDeductedDate != null)
                            supplierDeduction.DeductedDate = rdpDeductedDate.SelectedDate;

                        var rcbxSupplier = (RadComboBox)e.Item.FindControl("rcbxSupplier");

                        if (!string.IsNullOrEmpty(rcbxSupplier.SelectedValue))
                        {
                            int supplierID;
                            if (int.TryParse(rcbxSupplier.SelectedValue, out supplierID))
                                supplierDeduction.SupplierID = supplierID;
                        }

                        var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                        if (txtAmount != null && txtAmount.Value.HasValue)
                            supplierDeduction.Amount = (decimal)txtAmount.Value;

                        {
                            var uiSearchObj = new UISearchSupplierCautionMoney()
                            {
                                ID = CurrentEntityID.Value,
                                NeedStatistics = true
                            };

                            int totalRecords;

                            var uiSupplierRefundApp = PageSupplierCautionMoneyRepository.GetUIList(uiSearchObj, 0, 1, out totalRecords).FirstOrDefault();

                            if (uiSupplierRefundApp != null)
                            {
                                if ((uiSupplierRefundApp.TakeBackCautionMoney
                                    + supplierDeduction.Amount) > uiSupplierRefundApp.PaymentCautionMoney)
                                {
                                    ((CustomValidator)e.Item.FindControl("cvAmount")).IsValid = false;
                                    e.Canceled = true;
                                    return;
                                }
                            }
                        }

                        var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                        if (txtComment != null)
                            supplierDeduction.Comment = txtComment.Text;

                        int currentEntityID;
                        int.TryParse(hdnCurrentEntityID.Value, out currentEntityID);

                        var supplierRefundApp = PageSupplierCautionMoneyRepository.GetByID(currentEntityID);

                        supplierRefundApp.SupplierCautionMoneyDeduction.Add(supplierDeduction);

                        PageSupplierCautionMoneyRepository.Save();

                        hdnCurrentEntityID.Value = supplierRefundApp.ID.ToString();

                        //hdnNeedRefreshPage.Value = true.ToString();

                        //rgSupplierRefunds.Rebind();
                        rgSupplierDeduction.Rebind();
                    }
                }
            }
            else if (e.CommandName == "Delete")
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    PageSupplierCautionMoneyDeductionRepository.DeleteByID(dataItem.GetDataKeyValue("ID").ToInt());
                    PageSupplierCautionMoneyDeductionRepository.Save();
                }
            }
        }

        #endregion
    }
}