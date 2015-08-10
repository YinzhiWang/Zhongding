using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Web.Views.HRM
{
    public partial class SalarySettleMaintenance : WorkflowBasePage
    {
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.SalarySettleManagement;
        }
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.SalarySettleManagement;
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
        private ISalarySettleRepository _PageSalarySettleRepository;
        private ISalarySettleRepository PageSalarySettleRepository
        {
            get
            {
                if (_PageSalarySettleRepository == null)
                    _PageSalarySettleRepository = new SalarySettleRepository();

                return _PageSalarySettleRepository;
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

        private ISalarySettleDetailRepository _PageSalarySettleDetailRepository;
        private ISalarySettleDetailRepository PageSalarySettleDetailRepository
        {
            get
            {
                if (_PageSalarySettleDetailRepository == null)
                    _PageSalarySettleDetailRepository = new SalarySettleDetailRepository();

                return _PageSalarySettleDetailRepository;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SalarySettleManage;
            if (!IsPostBack)
            {
                LoadCurrentEntity();
            }
        }

        private SalarySettle _CurrentEntity;
        private SalarySettle CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageSalarySettleRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewSalarySettle);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSalarySettle);

                return _CanEditUserIDs;
            }
        }



        private IList<int> _CanAuditByDeptManagersUserIDs;
        private IList<int> CanAuditByDeptManagersUserIDs
        {
            get
            {
                if (_CanAuditByDeptManagersUserIDs == null)
                    _CanAuditByDeptManagersUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditSalarySettleByDeptManagers);

                return _CanAuditByDeptManagersUserIDs;
            }
        }

        private IList<int> _CanAuditByTreasurersUserIDs;
        private IList<int> CanAuditByTreasurersUserIDs
        {
            get
            {
                if (_CanAuditByTreasurersUserIDs == null)
                    _CanAuditByTreasurersUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditSalarySettleByTreasurers);

                return _CanAuditByTreasurersUserIDs;
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
        private void LoadCurrentEntity()
        {
            BindDepartments();

            if (this.CurrentEntity != null)
            {
                DisabledBasicInfoFiledsControls();
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                rmypSettleDate.SelectedDate = CurrentEntity.SettleDate;
                rcbxDepartment.SelectedValue = CurrentEntity.DepartmentID.ToString();


                BindSalarySettleDetails(true);

                divTotalSalary.Visible = true;


                var appPaymentAmounts = PageSalarySettleDetailRepository.GetList(x => x.SalarySettleID == this.CurrentEntityID).Select(x => new { x.NeedPaySalary, x.RealPaySalary }).ToList();
                if (appPaymentAmounts.Count > 0)
                {
                    decimal totalNeedPaySalary = appPaymentAmounts.Sum(x => x.NeedPaySalary);
                    decimal totalRealPaySalary = appPaymentAmounts.Sum(x => x.RealPaySalary);
                    lblTotalNeedPaySalary.Text = totalNeedPaySalary.ToString("C2");
                    lblTotalRealPaySalary.Text = totalRealPaySalary.ToString("C2");
                }


                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);
                    rgSalarySettleDetails.Enabled = true;
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
                            {
                                rgSalarySettleDetails.Enabled = true;
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
                        case EWorkflowStatus.ApprovedByAdministrations:
                            #region 已提交或需进入下一级审核，待审核

                            DisabledBasicInfoControls();

                            divAppPayments.Visible = false;


                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowAuditControls(true);
                            else
                            {
                                ShowAuditControls(false);
                            }
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

                        case EWorkflowStatus.ApprovedByDeptManagers:
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
                }
            }
        }

        private void DisabledBasicInfoFiledsControls()
        {
            rcbxDepartment.Enabled = false;
            rmypSettleDate.Enabled = false;
            rgSalarySettleDetails.Enabled = false;
        }
        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (CurrentEntity != null)
            {
                var isAllPayed = !CurrentEntity.SalarySettleDetail.Any(x => x.IsPayed == false && x.IsDeleted == false);
                if (!isAllPayed)
                {
                    ShowErrorMessage("付款明细没有全部付款完毕，不能确认支付");
                    rgAppPayments.Rebind();
                    return;
                }
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();
                    ISalarySettleDetailRepository salarySettleDetailRepository = new SalarySettleDetailRepository();
                    ISalarySettleRepository salarySettleRepository = new SalarySettleRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();
                    appPaymentRepository.SetDbModel(db);
                    salarySettleDetailRepository.SetDbModel(db);
                    salarySettleRepository.SetDbModel(db);
                    var currentEntity = salarySettleRepository.GetByID(this.CurrentEntityID);

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


                        //var salarySettleDetails = currentEntity.SalarySettleDetail;
                        //foreach (var item in salarySettleDetails)
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
            //divAuditAll.Visible = isShow;
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
            if (rcbxDepartment.SelectedValue.IsNullOrEmpty())
                cvDepartment.IsValid = false;
            if (!IsValid) return;

            SalarySettle currentEntity = this.CurrentEntity;
            if (!this.CurrentEntityID.BiggerThanZero())
            {
                currentEntity = new SalarySettle()
                {
                    DepartmentID = rcbxDepartment.SelectedValue.ToInt(),
                    SettleDate = rmypSettleDate.SelectedDate.Value.AddDays(-(rmypSettleDate.SelectedDate.Value.Day - 1)),
                    WorkflowStatusID = (int)EWorkflowStatus.TemporarySave,
                };
                PageSalarySettleRepository.Add(currentEntity);

            }
            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
            PageSalarySettleRepository.Save();
            hdnCurrentEntityID.Value = currentEntity.ID.ToString();
            var selectedItems = rgSalarySettleDetails.Items;
            foreach (var item in selectedItems)
            {
                var editableItem = ((GridEditableItem)item);
                var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();
                var userID = editableItem.GetDataKeyValue("UserID").ToIntOrNull();

                var realPaySalary = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtRealPaySalary").GetValueOrDefault(0);

                if (realPaySalary != 0)
                {
                    var basicSalary = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtBasicSalary").GetValueOrDefault(0);
                    var workDay = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtWorkDay").ToIntOrNull().GetValueOrDefault(0);
                    var mealAllowance = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtMealAllowance").GetValueOrDefault(0);
                    var positionSalary = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtPositionSalary").GetValueOrDefault(0);
                    var bonusPay = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtBonusPay").GetValueOrDefault(0);
                    var workAgeSalary = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtWorkAgeSalary").GetValueOrDefault(0);
                    var phoneAllowance = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtPhoneAllowance").GetValueOrDefault(0);
                    var officeExpense = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtOfficeExpense").GetValueOrDefault(0);
                    var otherAllowance = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtOtherAllowance").GetValueOrDefault(0);

                    var needPaySalary = basicSalary + mealAllowance + positionSalary + bonusPay + workAgeSalary + phoneAllowance + officeExpense + otherAllowance;

                    var needDeduct = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtNeedDeduct").GetValueOrDefault(0);
                    var holidayDeductOfSalary = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtHolidayDeductOfSalary").GetValueOrDefault(0);
                    var holidayDeductOfMealAllowance = GetGridEditableItemRadNumericTextBoxValue(editableItem, "txtHolidayDeductOfMealAllowance").GetValueOrDefault(0);

                    realPaySalary = basicSalary + mealAllowance + positionSalary + bonusPay + workAgeSalary + phoneAllowance + officeExpense + otherAllowance
                        - needDeduct - holidayDeductOfSalary - holidayDeductOfMealAllowance;

                    var salarySettleDetail = PageSalarySettleDetailRepository.GetByID(id);
                    if (salarySettleDetail == null)
                    {
                        salarySettleDetail = new SalarySettleDetail()
                        {
                            SalarySettleID = currentEntity.ID,
                        };
                        PageSalarySettleDetailRepository.Add(salarySettleDetail);
                    }

                    salarySettleDetail.BasicSalary = basicSalary;
                    salarySettleDetail.WorkDay = workDay;
                    salarySettleDetail.MealAllowance = mealAllowance;
                    salarySettleDetail.MealAllowance = mealAllowance;
                    salarySettleDetail.PositionSalary = positionSalary;
                    salarySettleDetail.BonusPay = bonusPay;
                    salarySettleDetail.WorkAgeSalary = workAgeSalary;
                    salarySettleDetail.PhoneAllowance = phoneAllowance;
                    salarySettleDetail.OfficeExpense = officeExpense;
                    salarySettleDetail.OtherAllowance = otherAllowance;

                    salarySettleDetail.NeedPaySalary = needPaySalary;

                    salarySettleDetail.NeedDeduct = needDeduct;
                    salarySettleDetail.HolidayDeductOfSalary = holidayDeductOfSalary;
                    salarySettleDetail.HolidayDeductOfMealAllowance = holidayDeductOfMealAllowance;

                    salarySettleDetail.RealPaySalary = realPaySalary;

                    salarySettleDetail.UserID = userID.Value;
                    PageSalarySettleDetailRepository.Save();
                }
                else
                {
                    // 删除被抛弃的
                    if (id.BiggerThanZero())
                    {
                        PageSalarySettleDetailRepository.DeleteByID(id);
                    }
                }

            }

            PageSalarySettleDetailRepository.Save();
            hdnBasicPricesCellValueChangedCount.Value = "0";


            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);

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
                        SaveSalarySettleBasicData(this.CurrentEntity);
                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        break;
                }
            }
        }

        private void SaveSalarySettleBasicData(SalarySettle clientCautionMoney)
        {
            PageSalarySettleRepository.Save();
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

                    ISalarySettleRepository salarySettleRepository = new SalarySettleRepository();

                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    appNoteRepository.SetDbModel(db);
                    salarySettleRepository.SetDbModel(db);


                    var currentEntity = salarySettleRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditSalarySettleByAdministrations;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                            case (int)EWorkflowStatus.ApprovedByAdministrations:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditSalarySettleByTreasurers;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                            case (int)EWorkflowStatus.ApprovedByTreasurers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditSalarySettleByDeptManagers;
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

                    ISalarySettleRepository salarySettleRepository = new SalarySettleRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    appNoteRepository.SetDbModel(db);
                    salarySettleRepository.SetDbModel(db);



                    var salarySettle = salarySettleRepository.GetByID(this.CurrentEntityID);
                    var appNote = new ApplicationNote();
                    appNote.WorkflowID = CurrentWorkFlowID;
                    appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                    appNoteRepository.Add(appNote);

                    switch (salarySettle.WorkflowStatusID)
                    {
                        case (int)EWorkflowStatus.Submit:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditSalarySettleByAdministrations;
                            appNote.ApplicationID = salarySettle.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            salarySettle.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByAdministrations;
                            break;
                        case (int)EWorkflowStatus.ApprovedByAdministrations:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditSalarySettleByTreasurers;
                            appNote.ApplicationID = salarySettle.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            salarySettle.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByTreasurers;
                            break;
                        case (int)EWorkflowStatus.ApprovedByTreasurers:
                            appNote.WorkflowStepID = (int)EWorkflowStep.AuditSalarySettleByDeptManagers;
                            appNote.ApplicationID = salarySettle.ID;
                            appNote.Note = txtAuditComment.Text.Trim();
                            salarySettle.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDeptManagers;
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
                PageSalarySettleRepository.DeleteByID(this.CurrentEntityID);
                PageSalarySettleRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }

        #region rgAppPayments

        protected void rgAppPayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchSalarySettleDetail
            {
                SalarySettleID = this.CurrentEntityID
            };

            int totalRecords;

            var appPayments = PageSalarySettleDetailRepository.GetUIListForAppPayment(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;

            BindPaymentSummary();
        }
        protected void rgAppPayments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            bool hasPermission = this.CanAccessUserIDs.Contains(CurrentUser.UserID);


            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UISalarySettleDetail uiEntity = (UISalarySettleDetail)gridDataItem.DataItem;

                var editColumn = rgAppPayments.MasterTableView.GetColumn("Pay");
                var editCell = gridDataItem.Cells[editColumn.OrderIndex];
                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");
                if (!hasPermission)
                {
                    rcbxFromAccount.Enabled = false;
                }
                if (uiEntity.IsPayed)
                {
                    rcbxFromAccount.Text = uiEntity.FromAccount;
                    rcbxFromAccount.Enabled = false;
                    editCell.Text = "已付款";

                }
                else
                {
                    if (!hasPermission)
                    {
                        editCell.Text = "";
                    }
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Company,
                            CompanyID = CurrentUser.CompanyID
                        }
                    };

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
                    rcbxFromAccount.DataSource = bankAccounts;
                    rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxFromAccount.DataBind();
                    rcbxFromAccount.Items.Insert(0, new RadComboBoxItem("", ""));
                }



            }


        }

        protected void rgAppPayments_InsertCommand(object sender, GridCommandEventArgs e)
        {

        }
        private void BindPaymentSummary()
        {
            var appPaymentAmounts = PageSalarySettleDetailRepository.GetList(x => x.SalarySettleID == this.CurrentEntityID).Select(x => x.RealPaySalary).ToList();

            if (appPaymentAmounts.Count > 0)
            {
                decimal totalPaymentAmount = appPaymentAmounts.Sum();
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
            if (e.Item is GridCommandItem)
            {
                //GridCommandItem commandItem = e.Item as GridCommandItem;
                //Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

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
            //var editableItem = ((GridEditableItem)e.Item);
            //var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            //if (id.BiggerThanZero())
            //{
            //    PageAppPaymentRepository.DeleteByID(id);
            //    PageAppPaymentRepository.Save();

            //    rgAppPayments.Rebind();
            //    BindPaymentSummary();
            //}

        }
        protected void rgAppPayments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Pay")
            {
                var editableItem = ((GridEditableItem)e.Item);
                var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();
                SalarySettleDetail uiEntity = PageSalarySettleDetailRepository.GetByID(id);

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");
                if (rcbxFromAccount.SelectedValue.ToIntOrNull().BiggerThanZero())
                {
                    ApplicationPayment appPayment = new ApplicationPayment();


                    appPayment.FromBankAccountID = rcbxFromAccount.SelectedValue.ToIntOrNull();
                    appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;

                    appPayment.Amount = uiEntity.RealPaySalary;

                    appPayment.ApplicationID = this.CurrentEntityID.Value;
                    appPayment.WorkflowID = this.CurrentWorkFlowID;
                    appPayment.PaymentStatusID = (int)EPaymentStatus.ToBePaid;
                    appPayment.PaymentTypeID = (int)EPaymentType.Expend;
                    PageAppPaymentRepository.Add(appPayment);
                    PageAppPaymentRepository.Save();
                    uiEntity.IsPayed = true;
                    uiEntity.ApplicationPaymentID = appPayment.ID;
                    PageSalarySettleDetailRepository.Save();
                    rgAppPayments.Rebind();
                }
                else
                {
                    var rfvFromAccount = (CustomValidator)e.Item.FindControl("rfvFromAccount");
                    rfvFromAccount.IsValid = false;
                    e.Canceled = true;
                }

            }
        }
        #endregion


        private void BindSalarySettleDetails(bool isNeedRebind)
        {
            if (rcbxDepartment.SelectedValue.ToIntOrNull().BiggerThanZero() && rmypSettleDate.SelectedDate.HasValue)
            {
                rgSalarySettleDetails.Visible = true;

                UISearchSalarySettleDetail uiSearchObj = new UISearchSalarySettleDetail()
                {
                    DepartmentID = rcbxDepartment.SelectedValue.ToIntOrNull(),
                    SettleDate = rmypSettleDate.SelectedDate.Value
                };

                int totalRecords;

                var basicPrices = PageSalarySettleDetailRepository.GetUIList(uiSearchObj,
                    rgSalarySettleDetails.CurrentPageIndex, rgSalarySettleDetails.PageSize, out totalRecords);

                rgSalarySettleDetails.DataSource = basicPrices;
                rgSalarySettleDetails.VirtualItemCount = totalRecords;
                if (isNeedRebind)
                    rgSalarySettleDetails.Rebind();
            }
            else
            {
                rgSalarySettleDetails.Visible = false;

            }
            hdnBasicPricesCellValueChangedCount.Value = "0";
        }

        protected void rgSalarySettleDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindSalarySettleDetails(false);
        }

        protected void rgSalarySettleDetails_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {

        }

        protected void rgSalarySettleDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                UISalarySettleDetail uiEntity = (UISalarySettleDetail)gridDataItem.DataItem;
                var txtBasicSalary = (RadNumericTextBox)e.Item.FindControl("txtBasicSalary");
                txtBasicSalary.Value = uiEntity.BasicSalary.ToDoubleOrNull();

                var txtWorkDay = (RadNumericTextBox)e.Item.FindControl("txtWorkDay");
                txtWorkDay.Value = uiEntity.WorkDay.ToDoubleOrNull();

                var txtMealAllowance = (RadNumericTextBox)e.Item.FindControl("txtMealAllowance");
                txtMealAllowance.Value = uiEntity.MealAllowance.ToDoubleOrNull();

                var txtPositionSalary = (RadNumericTextBox)e.Item.FindControl("txtPositionSalary");
                txtPositionSalary.Value = uiEntity.PositionSalary.ToDoubleOrNull();

                var txtBonusPay = (RadNumericTextBox)e.Item.FindControl("txtBonusPay");
                txtBonusPay.Value = uiEntity.BonusPay.ToDoubleOrNull();

                var txtWorkAgeSalary = (RadNumericTextBox)e.Item.FindControl("txtWorkAgeSalary");
                txtWorkAgeSalary.Value = uiEntity.WorkAgeSalary.ToDoubleOrNull();

                var txtPhoneAllowance = (RadNumericTextBox)e.Item.FindControl("txtPhoneAllowance");
                txtPhoneAllowance.Value = uiEntity.PhoneAllowance.ToDoubleOrNull();

                var txtOfficeExpense = (RadNumericTextBox)e.Item.FindControl("txtOfficeExpense");
                txtOfficeExpense.Value = uiEntity.OfficeExpense.ToDoubleOrNull();

                var txtOtherAllowance = (RadNumericTextBox)e.Item.FindControl("txtOtherAllowance");
                txtOtherAllowance.Value = uiEntity.OtherAllowance.ToDoubleOrNull();

                var txtNeedPaySalary = (RadNumericTextBox)e.Item.FindControl("txtNeedPaySalary");
                txtNeedPaySalary.Value = uiEntity.NeedPaySalary.ToDoubleOrNull();

                var txtNeedDeduct = (RadNumericTextBox)e.Item.FindControl("txtNeedDeduct");
                txtNeedDeduct.Value = uiEntity.NeedDeduct.ToDoubleOrNull();

                var txtHolidayDeductOfSalary = (RadNumericTextBox)e.Item.FindControl("txtHolidayDeductOfSalary");
                txtHolidayDeductOfSalary.Value = uiEntity.HolidayDeductOfSalary.ToDoubleOrNull();

                var txtHolidayDeductOfMealAllowance = (RadNumericTextBox)e.Item.FindControl("txtHolidayDeductOfMealAllowance");
                txtHolidayDeductOfMealAllowance.Value = uiEntity.HolidayDeductOfMealAllowance.ToDoubleOrNull();

                var txtRealPaySalary = (RadNumericTextBox)e.Item.FindControl("txtRealPaySalary");
                txtRealPaySalary.Value = uiEntity.RealPaySalary.ToDoubleOrNull();

            }
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

        protected void rmypSettleDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (CheckParamenter())
                BindSalarySettleDetails(true);
        }

        protected void rcbxDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (CheckParamenter())
                BindSalarySettleDetails(true);
        }

        private bool CheckParamenter()
        {
            if (rcbxDepartment.SelectedValue.ToIntOrNull().BiggerThanZero() && rmypSettleDate.SelectedDate.HasValue)
            {
                bool hasSalarySettle = PageSalarySettleRepository.HasSalarySettle(rcbxDepartment.SelectedValue.ToInt(), rmypSettleDate.SelectedDate.Value);
                if (!hasSalarySettle)
                {
                }
                else
                {
                    ShowErrorMessage("当前月份工资结算已经存在");
                    rmypSettleDate.SelectedDate = null;
                    rgSalarySettleDetails.Visible = false;
                    return false;
                }
            }
            return true;
        }




    }
}