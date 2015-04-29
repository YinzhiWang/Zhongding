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


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Master.MenuItemID = (int)EMenuItem.SupplierCautionMoneyApplyManage;


            if (!IsPostBack)
            {

                LoadCurrentEntity();

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
                txtRemark.Text = this.CurrentEntity.Remark;

                lblOperator.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);
                BindProducts();
                rcbxProduct.SelectedValue = CurrentEntity.ProductID.ToString();
                BindCautionMoneyTypes();
                rcbxCautionMoneyType.SelectedValue = CurrentEntity.CautionMoneyTypeID.ToString();
                BindProductSpecifications(CurrentEntity.ProductID);
                rcbxProductSpecification.SelectedValue = CurrentEntity.ProductSpecificationID.ToString();
                BindSuppliers();
                rcbxSupplier.SelectedValue = CurrentEntity.SupplierID.ToString();





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
                            divAuditAll.Visible = false;
                            //divAppPayments.Visible = false;

                            #endregion

                            break;
                        case EWorkflowStatus.Submit:
                            #region 已提交，待审核

                            DisabledBasicInfoControls();

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                ShowAuditControls(true);

                                //if (CurrentEntity.DeliveryModeID != (int)EDeliveryMode.GuaranteeDelivery)
                                //    divAppPayments.Visible = true;
                                //else
                                //    divAppPayments.Visible = false;

                            }
                            else
                                ShowAuditControls(false);

                            #endregion

                            break;
                        case EWorkflowStatus.ApprovedByDeptManagers:
                            #region 已提交或需进入下一级审核，待审核

                            DisabledBasicInfoControls();

                            //divAppPayments.Visible = false;

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
                                //btnPay.Visible = true;
                            }
                            //else
                            //    rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                            #endregion

                            break;


                        case EWorkflowStatus.Paid:
                            #region 已支付，只能撤销

                            DisabledBasicInfoControls();

                            ShowSaveButtons(false);

                            ShowAuditControls(false);

                            //rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

                            //if (CurrentEntity.PaidBy == CurrentUser.UserID)
                            //{
                            //    divCancel.Visible = true;
                            //    btnCancel.Visible = true;
                            //}

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
                = txtPaymentCautionMoney.Enabled = rcbxCautionMoneyType.Enabled = rdpEndDate.Enabled = txtRemark.Enabled = false;

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
            //divComment.Visible = false;
            //divComments.Visible = false;
            //divOtherSections.Visible = false;
            //divStop.Visible = false;

            //txtOrderCode.Text = Utility.GenerateAutoSerialNo(PageClientSaleAppRepository.GetMaxEntityID(),
            //    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.CLIENT_ORDER);

            //rdpOrderDate.SelectedDate = DateTime.Now;
            //lblCreateBy.Text = CurrentUser.FullName;

            //var saleOrderType = PageSaleOrderTypeRepository.GetByID(SaleOrderTypeID);
            //if (saleOrderType != null)
            //    lblSalesOrderType.Text = saleOrderType.TypeName;

            //hdnSaleOrderTypeID.Value = this.SaleOrderTypeID.ToString();
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

            SupplierCautionMoney currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageSupplierCautionMoneyRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new SupplierCautionMoney()
                {
                    ApplyDate = rdpApplyDate.SelectedDate.Value,
                    CautionMoneyTypeID = rcbxCautionMoneyType.SelectedValue.ToInt(),
                    EndDate = rdpEndDate.SelectedDate.Value,
                    IsStop = false,
                    PaymentCautionMoney = txtPaymentCautionMoney.Value.ToDecimal(),
                    ProductID = rcbxProduct.SelectedValue.ToInt(),
                    ProductSpecificationID = rcbxProductSpecification.SelectedValue.ToInt(),
                    Remark = txtRemark.Text.Trim(),
                    SupplierID = rcbxSupplier.SelectedValue.ToInt(),
                    WorkflowStatusID = (int)EWorkflowStatus.TemporarySave
                };

                PageSupplierCautionMoneyRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.Remark = txtRemark.Text;

                PageSupplierCautionMoneyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
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
            //if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
            //    cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientSaleApplicationRepository clientSaleAppRepository = new ClientSaleApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    clientSaleAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = clientSaleAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditClientOrder;
                                appNote.ApplicationID = currentEntity.ID;
                                //appNote.Note = txtAuditComment.Text.Trim();

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
                    //IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    //clientSaleAppRepository.SetDbModel(db);
                    //appPaymentRepository.SetDbModel(db);
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
                ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
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
                .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == CurrentEntity.ID)
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
            //if (e.Item is GridCommandItem)
            //{
            //    GridCommandItem commandItem = e.Item as GridCommandItem;
            //    Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

            //    if (plAddCommand != null)
            //    {
            //        if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
            //            || (this.CanAuditUserIDs.Contains(CurrentUser.UserID)
            //                && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit)))
            //            plAddCommand.Visible = true;
            //        else
            //            plAddCommand.Visible = false;
            //    }
            //}
        }

        protected void rgAppPayments_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            //if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
            //    || (this.CanAuditUserIDs.Contains(CurrentUser.UserID)
            //        && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit)))
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
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                string sPaymentMethodID = editableItem.GetDataKeyValue("PaymentMethodID").ToString();

                int iPaymentMethodID;

                if (int.TryParse(sPaymentMethodID, out iPaymentMethodID))
                {
                    if (iPaymentMethodID == (int)EPaymentMethod.BankTransfer)
                    {
                        PageAppPaymentRepository.DeleteByID(id);
                        PageAppPaymentRepository.Save();
                    }
                    else if (iPaymentMethodID == (int)EPaymentMethod.Deduction)
                    {

                    }
                }

                rgAppPayments.Rebind();
            }
        }

        #endregion
    }
}