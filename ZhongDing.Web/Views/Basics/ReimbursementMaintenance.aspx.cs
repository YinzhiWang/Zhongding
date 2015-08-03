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
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Basics
{
    public partial class ReimbursementMaintenance : WorkflowBasePage
    {
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.ReimbursementManagement;
        }
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ReimbursementManagement;
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
        private IReimbursementRepository _PageReimbursementRepository;
        private IReimbursementRepository PageReimbursementRepository
        {
            get
            {
                if (_PageReimbursementRepository == null)
                    _PageReimbursementRepository = new ReimbursementRepository();

                return _PageReimbursementRepository;
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

        private IReimbursementDetailRepository _PageReimbursementDetailRepository;
        private IReimbursementDetailRepository PageReimbursementDetailRepository
        {
            get
            {
                if (_PageReimbursementDetailRepository == null)
                    _PageReimbursementDetailRepository = new ReimbursementDetailRepository();

                return _PageReimbursementDetailRepository;
            }
        }

        private IReimbursementDetailTransportFeeRepository _PagePageReimbursementDetailTransportFeeRepository;
        private IReimbursementDetailTransportFeeRepository PageReimbursementDetailTransportFeeRepository
        {
            get
            {
                if (_PagePageReimbursementDetailTransportFeeRepository == null)
                    _PagePageReimbursementDetailTransportFeeRepository = new ReimbursementDetailTransportFeeRepository();

                return _PagePageReimbursementDetailTransportFeeRepository;
            }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ReimbursementManage;
            if (!IsPostBack)
            {
                LoadCurrentEntity();
            }
        }

        private Reimbursement _CurrentEntity;
        private Reimbursement CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageReimbursementRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewReimbursement);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditReimbursement);

                return _CanEditUserIDs;
            }
        }



        private IList<int> _CanAuditByDeptManagersUserIDs;
        private IList<int> CanAuditByDeptManagersUserIDs
        {
            get
            {
                if (_CanAuditByDeptManagersUserIDs == null)
                    _CanAuditByDeptManagersUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditReimbursementByDeptManagers);

                return _CanAuditByDeptManagersUserIDs;
            }
        }

        private IList<int> _CanAuditByTreasurersUserIDs;
        private IList<int> CanAuditByTreasurersUserIDs
        {
            get
            {
                if (_CanAuditByTreasurersUserIDs == null)
                    _CanAuditByTreasurersUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditReimbursementByTreasurers);

                return _CanAuditByTreasurersUserIDs;
            }
        }


        private void LoadCurrentEntity()
        {

            if (this.CurrentEntity != null)
            {
                DisabledBasicInfoFiledsControls();
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                txtApplyDate.SelectedDate = CurrentEntity.ApplyDate;
                txtDepartment.Text = CurrentEntity.DepartmentID.ToString();
                txtCreatedByFullName.Text = PageUsersRepository.GetByID(CurrentEntity.CreatedBy).FullName;
                txtDepartment.Text = PageDepartmentRepository.GetByID(CurrentEntity.DepartmentID).DepartmentName;
                BindReimbursementDetails(true);

                //divTotalSalary.Visible = true;


                //var appPaymentAmounts = PageReimbursementDetailRepository.GetList(x => x.ReimbursementID == this.CurrentEntityID).Select(x => new { x.NeedPaySalary, x.RealPaySalary }).ToList();
                //if (appPaymentAmounts.Count > 0)
                //{
                //    decimal totalNeedPaySalary = appPaymentAmounts.Sum(x => x.NeedPaySalary);
                //    decimal totalRealPaySalary = appPaymentAmounts.Sum(x => x.RealPaySalary);
                //    lblTotalNeedPaySalary.Text = totalNeedPaySalary.ToString("C2");
                //    lblTotalRealPaySalary.Text = totalRealPaySalary.ToString("C2");
                //}


                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);
                    rgReimbursementDetails.Enabled = true;
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

                            if (CurrentUser.UserID == this.CurrentEntity.CreatedBy
                                || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                            {
                                rgReimbursementDetails.Enabled = true;
                                txtApplyDate.Enabled = true;
                                ShowSaveButtons(true);
                            }
                            else
                                DisabledBasicInfoControls();

                            btnAudit.Visible = false;
                            btnReturn.Visible = false;
                            divAudit.Visible = false;
                            //divAuditAll.Visible = false;
                            divAppPayments.Visible = false;
                            //txtAuditComment.Visible = false;


                            break;
                        case EWorkflowStatus.Submit:

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

                            #region 已提交或需进入下一级审核，待审核

                            DisabledBasicInfoControls();

                            divAppPayments.Visible = false;


                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowAuditControls(true);
                            else
                                ShowAuditControls(false);

                            #endregion
                            break;

                        case EWorkflowStatus.ApprovedByManagers:
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
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("您没有权限");
                    return;
                }
                txtCreatedByFullName.Text = CurrentUser.FullName;
                txtDepartment.Text = PageDepartmentRepository.GetByID(CurrentUser.DepartmentID).DepartmentName;
            }
        }

        private void DisabledBasicInfoFiledsControls()
        {
            txtApplyDate.Enabled = false;
            rgReimbursementDetails.Enabled = false;

        }
        protected void btnPay_Click(object sender, EventArgs e)
        {
            {
                var uiSearchObj = new UISearchApplicationPayment
                {
                    WorkflowID = this.CurrentWorkFlowID,
                    ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                    PaymentTypeID = (int)EPaymentType.Expend
                };
                var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj);
                decimal nowTotalAmount = (appPayments.Any() ? appPayments.Sum(x => x.Amount).Value : 0M) + (appPayments.Any() ? appPayments.Sum(x => x.Fee).Value : 0M);
                var currentEntity = this.CurrentEntity;
                decimal currentEntityAmount = currentEntity.ReimbursementDetail.Where(x => x.IsDeleted == false).Any()
                    ? currentEntity.ReimbursementDetail.Where(x => x.IsDeleted == false).Sum(x => x.Amount) : 0M;


                if (nowTotalAmount != currentEntityAmount)
                {
                    ShowErrorMessage("支付金额必须等于报销总金额");
                    return;
                }
            }

            if (CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();
                    IReimbursementDetailRepository ReimbursementDetailRepository = new ReimbursementDetailRepository();
                    IReimbursementRepository ReimbursementRepository = new ReimbursementRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();
                    appPaymentRepository.SetDbModel(db);
                    ReimbursementDetailRepository.SetDbModel(db);
                    ReimbursementRepository.SetDbModel(db);
                    var currentEntity = ReimbursementRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {

                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == currentEntity.ID).ToList();


                        foreach (var item in appPayments)
                        {
                            item.PaymentStatusID = (int)EPaymentStatus.Paid;
                            item.PayDate = DateTime.Now;
                        }

                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.Paid;
                        currentEntity.PaidDate = DateTime.Now;
                        currentEntity.PaidBy = CurrentUser.UserID;


                        //var ReimbursementDetails = currentEntity.ReimbursementDetail;
                        //foreach (var item in ReimbursementDetails)
                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);

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


        }


        protected void cvCompanyName_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        private decimal? GetGridEditableItemRadNumericTextBoxValue(GridEditableItem item, string name)
        {
            var control = (RadNumericTextBox)item.FindControl(name);
            return control.Value.ToDecimalOrNull();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (!IsValid) return;

            Reimbursement currentEntity = this.CurrentEntity;
            if (currentEntity == null)
            {
                currentEntity = new Reimbursement()
                {
                    DepartmentID = CurrentUser.DepartmentID,
                    ApplyDate = txtApplyDate.SelectedDate.Value,
                    WorkflowStatusID = (int)EWorkflowStatus.TemporarySave,
                };
                PageReimbursementRepository.Add(currentEntity);

            }
            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
            PageReimbursementRepository.Save();
            hdnCurrentEntityID.Value = currentEntity.ID.ToString();

            if (this.CurrentEntity == null)
            {
                this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);

            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
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
                        this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;
                        SaveReimbursementBasicData(this.CurrentEntity);
                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        break;
                }
            }
        }

        private void SaveReimbursementBasicData(Reimbursement clientCautionMoney)
        {
            PageReimbursementRepository.Save();
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

                    IReimbursementRepository ReimbursementRepository = new ReimbursementRepository();

                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    appNoteRepository.SetDbModel(db);
                    ReimbursementRepository.SetDbModel(db);


                    var currentEntity = ReimbursementRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditReimbursementByDeptManagers;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                            case (int)EWorkflowStatus.ApprovedByDeptManagers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditReimbursementByTreasurers;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                            case (int)EWorkflowStatus.ApprovedByTreasurers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditReimbursementByManagers;
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

                    IReimbursementRepository reimbursementRepository = new ReimbursementRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    appNoteRepository.SetDbModel(db);
                    reimbursementRepository.SetDbModel(db);



                    var Reimbursement = reimbursementRepository.GetByID(this.CurrentEntityID);
                    var appNote = new ApplicationNote();
                    appNote.WorkflowID = CurrentWorkFlowID;
                    appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                    appNoteRepository.Add(appNote);

                    switch (Reimbursement.WorkflowStatusID)
                    {
                        case (int)EWorkflowStatus.Submit:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditReimbursementByDeptManagers;
                            appNote.ApplicationID = Reimbursement.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            Reimbursement.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDeptManagers;
                            break;
                        case (int)EWorkflowStatus.ApprovedByDeptManagers:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditReimbursementByTreasurers;
                            appNote.ApplicationID = Reimbursement.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            Reimbursement.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByTreasurers;
                            break;
                        case (int)EWorkflowStatus.ApprovedByTreasurers:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditReimbursementByManagers;
                            appNote.ApplicationID = Reimbursement.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            Reimbursement.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByManagers;
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
                PageReimbursementRepository.DeleteByID(this.CurrentEntityID);
                PageReimbursementRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }


        protected void rgAppPayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationPayment
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
            };

            int totalRecords;

            var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);
            //foreach (var item in appPayments)
            //{
            //    if (item.PaymentTypeID == (int)EPaymentType.Expend)
            //    {
            //        item.PaymentType = "借款";
            //        item.ToAccount = item.FromAccount;
            //        item.ToBankAccountID = item.ToBankAccountID;
            //    }
            //    else
            //    {
            //        item.PaymentType = "还款";
            //    }
            //}
            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;

            BindPaymentSummary();
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
                if (rdpPayDate != null)
                    rdpPayDate.MinDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                if (rdpPayDate != null && uiEntity != null)
                    rdpPayDate.SelectedDate = uiEntity.PayDate;
                else
                    rdpPayDate.SelectedDate = DateTime.Now;


                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (rcbxToAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Company,
                            CompanyID = CurrentUser.CompanyID
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


        private void BindPaymentSummary()
        {
            var appPaymentAmounts = PageAppPaymentRepository
                .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == this.CurrentEntityID && x.IsDeleted == false)
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

                    //var appPayment = PageAppPaymentRepository.GetByID(id);

                    //var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                    //appPayment.PayDate = rdpPayDate.SelectedDate;

                    //var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                    //if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                    //{
                    //    int fromAccountID;
                    //    if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                    //        appPayment.FromBankAccountID = fromAccountID;
                    //    appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                    //}


                    //var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    //appPayment.Amount = (decimal?)txtAmount.Value;

                    //var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                    //appPayment.Fee = (decimal?)txtFee.Value;


                    //var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                    //if (txtComment != null)
                    //    appPayment.Comment = txtComment.Text;

                    //PageAppPaymentRepository.Save();

                    //rgAppPayments.Rebind();
                }
            }

            BindPaymentSummary();
        }

        protected void rgAppPayments_ItemCreated(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridCommandItem)
            //{
            //    GridCommandItem commandItem = e.Item as GridCommandItem;
            //    Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

            //    if (plAddCommand != null)
            //    {
            //        if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
            //                || (this.CanAuditByTreasurersUserIDs.Contains(CurrentUser.UserID)
            //        && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByManagers)))
            //            plAddCommand.Visible = true;
            //        else
            //            plAddCommand.Visible = false;
            //    }
            //}
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
        protected void rgAppPayments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                rgAppPayments_InsertCommand(sender, e);
            }
        }
        protected void rgAppPayments_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                ApplicationPayment appPayment = new ApplicationPayment();

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");
                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                var cvFee = (CustomValidator)e.Item.FindControl("cvFee");
                var rfvAmount = ((RequiredFieldValidator)e.Item.FindControl("rfvAmount"));

                appPayment.ToBankAccountID = rcbxFromAccount.SelectedValue.ToIntOrNull();
                appPayment.ToAccount = rcbxFromAccount.SelectedItem.Text;
                appPayment.PaymentTypeID = (int)EPaymentType.Expend;
                appPayment.Amount = (decimal?)txtAmount.Value;
                appPayment.Fee = (decimal?)txtFee.Value;

                var uiSearchObj = new UISearchApplicationPayment
                {
                    WorkflowID = this.CurrentWorkFlowID,
                    ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                    PaymentTypeID = (int)EPaymentType.Expend
                };
                var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj);
                decimal nowTotalAmount = (appPayments.Any() ? appPayments.Sum(x => x.Amount).Value : 0M) + (appPayments.Any() ? appPayments.Sum(x => x.Fee).Value : 0M);
                decimal totalAmount = nowTotalAmount + appPayment.Amount.GetValueOrDefault(0) + appPayment.Fee.GetValueOrDefault(0);
                var currentEntity = this.CurrentEntity;

                decimal currentEntityAmount = currentEntity.ReimbursementDetail.Where(x => x.IsDeleted == false).Any()
                    ? currentEntity.ReimbursementDetail.Where(x => x.IsDeleted == false).Sum(x => x.Amount) : 0M;


                if (totalAmount > currentEntityAmount)
                {
                    rfvAmount.IsValid = false;
                    rfvAmount.ErrorMessage = "支付总额不能超过报销总额";
                    if (txtFee.Value.GetValueOrDefault(0) > 0)
                    {
                        cvFee.IsValid = false;
                        cvFee.ErrorMessage = "支付总额不能超过报销总额";
                    }
                    e.Canceled = true;
                    return;
                }


                var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                appPayment.PayDate = rdpPayDate.SelectedDate;


                var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                appPayment.Comment = txtComment.Text.Trim();

                appPayment.ApplicationID = this.CurrentEntityID.Value;
                appPayment.WorkflowID = this.CurrentWorkFlowID;
                appPayment.PaymentStatusID = (int)EPaymentStatus.Paid;

                PageAppPaymentRepository.Add(appPayment);

                PageAppPaymentRepository.Save();

                //LoadCurrentEntity();


                //if (appPayment.PaymentTypeID == (int)EPaymentType.Income)//支出 借款
                //{
                //    var uiSearchObj = new UISearchApplicationPayment
                //    {
                //        WorkflowID = this.CurrentWorkFlowID,
                //        ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                //        PaymentTypeID = (int)EPaymentType.Income
                //    };
                //    var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj);
                //    decimal nowTotalAmount = appPayments.Any() ? appPayments.Sum(x => x.Amount).Value : 0M;
                //    var currentEntity = PageBorrowMoneyRepository.GetByID(this.CurrentEntityID);
                //    if (nowTotalAmount >= currentEntity.BorrowAmount)
                //    {
                //        currentEntity.Status = (int)EBorrowMoneyStatus.Returned;
                //        PageBorrowMoneyRepository.Save();
                //    }

                //}
            }

            rgAppPayments.Rebind();

            BindPaymentSummary();
        }


        private void BindReimbursementDetails(bool isNeedRebind)
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                divReimbursementDetails.Visible = true;

                UISearchReimbursementDetail uiSearchObj = new UISearchReimbursementDetail()
                {
                    //DepartmentID = rcbxDepartment.SelectedValue.ToIntOrNull(),
                    //SettleDate = rmypSettleDate.SelectedDate.Value
                    ReimbursementID = this.CurrentEntityID
                };

                int totalRecords;

                var uiEntities = PageReimbursementDetailRepository.GetUIList(uiSearchObj,
                    rgReimbursementDetails.CurrentPageIndex, rgReimbursementDetails.PageSize, out totalRecords);

                rgReimbursementDetails.DataSource = uiEntities;
                rgReimbursementDetails.VirtualItemCount = totalRecords;
                if (isNeedRebind)
                    rgReimbursementDetails.Rebind();
            }
            else
            {
                divReimbursementDetails.Visible = false;
            }
        }

        protected void rgReimbursementDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindReimbursementDetails(false);
        }

        protected void rgReimbursementDetails_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {

        }
        protected void rgReimbursementDetails_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();
            if (id.BiggerThanZero())
            {
                var reimbursementDetail = PageReimbursementDetailRepository.GetByID(id);
                var reimbursementDetailTransportFees = reimbursementDetail.ReimbursementDetailTransportFee.Where(x => x.IsDeleted == false).ToList();

                reimbursementDetailTransportFees.ForEach(x =>
                {
                    PageReimbursementDetailTransportFeeRepository.DeleteByID(x.ID);
                });
                PageReimbursementDetailTransportFeeRepository.Save();
                PageReimbursementDetailRepository.DeleteByID(id);
                PageReimbursementDetailRepository.Save();
                BindReimbursementDetails(true);
            }
        }
        protected void rgReimbursementDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {

            }
        }

        protected void rgReimbursementDetails_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {

                    bool canEdit = false;
                    if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    {
                        canEdit = true;
                    }
                    else
                    {
                        switch ((EWorkflowStatus)CurrentEntity.WorkflowStatusID)
                        {
                            case EWorkflowStatus.TemporarySave:
                            case EWorkflowStatus.ReturnBasicInfo:

                                if (CurrentUser.UserID == this.CurrentEntity.CreatedBy
                                    || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                                {
                                    canEdit = true;
                                }
                                else
                                {
                                    canEdit = false;
                                }
                                break;
                        }
                    }

                    plAddCommand.Visible = canEdit;

                }
            }
        }

        protected void rgReimbursementDetails_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            bool canEdit = false;
            if (CanEditUserIDs.Contains(CurrentUser.UserID))
            {
                canEdit = true;
            }
            else
            {
                switch ((EWorkflowStatus)CurrentEntity.WorkflowStatusID)
                {
                    case EWorkflowStatus.TemporarySave:
                    case EWorkflowStatus.ReturnBasicInfo:

                        if (CurrentUser.UserID == this.CurrentEntity.CreatedBy
                            || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                        {
                            canEdit = true;
                        }
                        else
                        {
                            canEdit = false;
                        }
                        break;
                }
            }
            rgReimbursementDetails.Enabled = canEdit;
            e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = canEdit;
            e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = canEdit;


        }

        private int GetDataKeyValue(GridBatchEditingCommand command, string dataKeyName)
        {
            int dataKeyValue = GlobalConst.INVALID_INT;

            if (command.NewValues[dataKeyName] != null)
            {
                string sdataKeyValue = command.NewValues[dataKeyName].ToString();

                if (int.TryParse(sdataKeyValue, out dataKeyValue))
                    return dataKeyValue;
            }

            return dataKeyValue;
        }

    }
}