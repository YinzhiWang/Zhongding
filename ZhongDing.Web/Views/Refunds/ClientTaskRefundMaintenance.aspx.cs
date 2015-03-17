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

namespace ZhongDing.Web.Views.Refunds
{
    public partial class ClientTaskRefundMaintenance : WorkflowBasePage
    {

        #region Members

        private IClientTaskRefundAppRepository _PageClientTaskRefundAppRepository;
        private IClientTaskRefundAppRepository PageClientTaskRefundAppRepository
        {
            get
            {
                if (_PageClientTaskRefundAppRepository == null)
                    _PageClientTaskRefundAppRepository = new ClientTaskRefundAppRepository();

                return _PageClientTaskRefundAppRepository;
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

        private IClientInfoRepository _PageClientInfoRepository;
        private IClientInfoRepository PageClientInfoRepository
        {
            get
            {
                if (_PageClientInfoRepository == null)
                    _PageClientInfoRepository = new ClientInfoRepository();

                return _PageClientInfoRepository;
            }
        }

        private IClientInfoProductSettingRepository _PageClientInfoPSRepository;
        private IClientInfoProductSettingRepository PageClientInfoPSRepository
        {
            get
            {
                if (_PageClientInfoPSRepository == null)
                    _PageClientInfoPSRepository = new ClientInfoProductSettingRepository();

                return _PageClientInfoPSRepository;
            }
        }

        private ICompanyRepository _PageCompanyRepository;
        private ICompanyRepository PageCompanyRepository
        {
            get
            {
                if (_PageCompanyRepository == null)
                    _PageCompanyRepository = new CompanyRepository();

                return _PageCompanyRepository;
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

        private IStockInDetailRepository _PageStockInDetailRepository;
        private IStockInDetailRepository PageStockInDetailRepository
        {
            get
            {
                if (_PageStockInDetailRepository == null)
                    _PageStockInDetailRepository = new StockInDetailRepository();

                return _PageStockInDetailRepository;
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

        private IStockOutRepository _PageStockOutRepository;
        private IStockOutRepository PageStockOutRepository
        {
            get
            {
                if (_PageStockOutRepository == null)
                    _PageStockOutRepository = new StockOutRepository();

                return _PageStockOutRepository;
            }
        }

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

        private ClientTaskRefundApplication _CurrentEntity;
        private ClientTaskRefundApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageClientTaskRefundAppRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientTaskRefund);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientTaskRefund);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanAuditUserIDs;
        private IList<int> CanAuditUserIDs
        {
            get
            {
                if (_CanAuditUserIDs == null)
                    _CanAuditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByIDs(new List<int> {
                        (int)EWorkflowStep.AuditCTRefundByDistrictManagers,
                        (int)EWorkflowStep.AuditCTRefundByMarketManagers,
                        (int)EWorkflowStep.AuditCTRefundByTreasurers,
                        (int)EWorkflowStep.AuditCTRefundByDeptManagers
                    });

                return _CanAuditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientTaskRefunds;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientTaskRefundsManage;

            if (!IsPostBack)
            {
                BindCompanies();

                BindClientUsers();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindCompanies()
        {
            var companies = PageCompanyRepository.GetDropdownItems();
            rcbxCompany.DataSource = companies;
            rcbxCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxCompany.DataBind();

            rcbxCompany.Items.Insert(0, new RadComboBoxItem("", ""));
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
        private void BindClientCompanies()
        {
            rcbxClientCompany.ClearSelection();
            rcbxClientCompany.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.Extension = new UISearchExtension { ClientUserID = clientUserID };
            }

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts()
        {
            rcbxProduct.ClearSelection();
            rcbxProduct.Items.Clear();

            int companyID;

            if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
            {
                var products = PageProductRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension { CompanyID = companyID }
                });

                rcbxProduct.DataSource = products;
                rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxProduct.DataBind();

                rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
            }
        }

        private void BindProductSpecifications()
        {
            ddlProductSpecification.ClearSelection();
            ddlProductSpecification.Items.Clear();

            if (!string.IsNullOrEmpty(rcbxProduct.SelectedValue))
            {
                int productID;
                if (int.TryParse(rcbxProduct.SelectedValue, out productID))
                {
                    var productSpecifications = PageProductSpecificationRepository.GetDropdownItems(new UISearchDropdownItem()
                    {
                        Extension = new UISearchExtension { ProductID = productID }
                    });

                    ddlProductSpecification.DataSource = productSpecifications;
                    ddlProductSpecification.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    ddlProductSpecification.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    ddlProductSpecification.DataBind();
                }
            }
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                lblCreatedOn.Text = CurrentEntity.CreatedOn.ToString("yyyy/MM/dd");
                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);

                rcbxCompany.SelectedValue = CurrentEntity.CompanyID.ToString();

                if (CurrentEntity.ClientUserID > 0)
                    rcbxClientUser.SelectedValue = CurrentEntity.ClientUserID.ToString();

                BindClientCompanies();
                if (CurrentEntity.ClientCompanyID > 0)
                    rcbxClientCompany.SelectedValue = CurrentEntity.ClientCompanyID.ToString();

                BindProducts();
                rcbxProduct.SelectedValue = CurrentEntity.ProductID.ToString();

                BindProductSpecifications();
                ddlProductSpecification.SelectedValue = CurrentEntity.ProductSpecificationID.ToString();

                rmypRefundDate.SelectedDate = CurrentEntity.RefundDate;

                if (CurrentEntity.TaskQty >= 0)
                    lblTaskQty.Text = CurrentEntity.TaskQty.ToString();

                if (CurrentEntity.StockOutQty >= 0)
                    lblStockOutQty.Text = CurrentEntity.StockOutQty.ToString();

                if (CurrentEntity.UseFlowData == true)
                {
                    lblUseFlowData.Text = GlobalConst.BoolChineseDescription.TRUE;

                    if (CurrentEntity.BackQty >= 0)
                        lblBackQty.Text = CurrentEntity.BackQty.ToString();
                }
                else if (CurrentEntity.UseFlowData == false)
                    lblUseFlowData.Text = GlobalConst.BoolChineseDescription.FALSE;

                txtRefundPrice.DbValue = CurrentEntity.RefundPrice;
                txtRefundAmount.DbValue = CurrentEntity.RefundAmount;

                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);
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
                            divAppPayments.Visible = false;

                            #endregion

                            break;
                        case EWorkflowStatus.Submit:
                        case EWorkflowStatus.ApprovedByDistrictManagers:
                        case EWorkflowStatus.ApprovedByMarketManagers:
                        case EWorkflowStatus.ApprovedByTreasurers:
                            #region 已提交或财务审核同，待审核

                            DisabledBasicInfoControls();
                            divAppPayments.Visible = false;

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                ShowAuditControls(true);

                                btnSave.Visible = true;
                                txtRefundPrice.Enabled = true;
                            }
                            else
                                ShowAuditControls(false);
                            #endregion

                            break;

                        case EWorkflowStatus.ApprovedByDeptManagers:
                            #region 审核通过，待支付

                            DisabledBasicInfoControls();
                            ShowAuditControls(false);

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                btnSave.Visible = true;
                                btnSubmit.Visible = false;
                                btnPay.Visible = true;
                            }
                            else
                                rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                            #endregion
                            break;

                        case EWorkflowStatus.Paid:
                            #region 已支付的订单，不能修改

                            DisabledBasicInfoControls();

                            ShowSaveButtons(false);

                            ShowAuditControls(false);

                            rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

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
                    this.Master.BaseNotification.Show("您没有权限新增厂家经理返款");
                }
            }
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
            btnAudit.Visible = isShow;
            btnReturn.Visible = isShow;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rcbxCompany.Enabled = false;
            rcbxClientUser.Enabled = false;
            rcbxClientCompany.Enabled = false;
            rcbxProduct.Enabled = false;
            ddlProductSpecification.Enabled = false;
            rmypRefundDate.Enabled = false;
            txtRefundPrice.Enabled = false;

            txtComment.Enabled = false;

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
            divComment.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;

            lblCreatedOn.Text = DateTime.Now.ToString("yyyy/MM/dd");
            lblCreateBy.Text = CurrentUser.FullName;
        }

        private void BindProductOtherInfos(bool isNeedInitPrice = true)
        {
            if (!string.IsNullOrEmpty(rcbxCompany.SelectedValue)
                && rmypRefundDate.SelectedDate.HasValue
                && !string.IsNullOrEmpty(rcbxClientUser.SelectedValue)
                && !string.IsNullOrEmpty(rcbxClientCompany.SelectedValue)
                && !string.IsNullOrEmpty(rcbxProduct.SelectedValue)
                && !string.IsNullOrEmpty(ddlProductSpecification.SelectedValue))
            {
                int companyID = Convert.ToInt32(rcbxCompany.SelectedValue);
                int clientUserID = Convert.ToInt32(rcbxClientUser.SelectedValue);
                int clientCompanyID = Convert.ToInt32(rcbxClientCompany.SelectedValue);
                int productID = Convert.ToInt32(rcbxProduct.SelectedValue);
                int productSpecificationID = Convert.ToInt32(ddlProductSpecification.SelectedValue);

                var clientProductSetting = PageClientInfoPSRepository.GetOneByCondistions(new UISearchClientInfoProductSetting
                {
                    ClientUserID = clientUserID,
                    ClientCompanyID = clientCompanyID,
                    ProductID = productID,
                    ProductSpecificationID = productSpecificationID
                });

                if (clientProductSetting != null)
                {
                    lblTaskQty.Text = clientProductSetting.MonthlyTask.HasValue
                        ? clientProductSetting.MonthlyTask.ToString() : string.Empty;

                    decimal? refundPrice;

                    if (isNeedInitPrice)
                        txtRefundPrice.DbValue = clientProductSetting.RefundPrice;

                    refundPrice = (decimal?)txtRefundPrice.Value;

                    if (clientProductSetting.UseFlowData)
                    {
                        lblUseFlowData.Text = GlobalConst.BoolChineseDescription.TRUE;

                        //获取流回数量
                        lblBackQty.Text = "0";
                    }
                    else
                    {
                        lblUseFlowData.Text = GlobalConst.BoolChineseDescription.FALSE;
                    }

                    var refundDate = rmypRefundDate.SelectedDate.Value;
                    var beginDate = new DateTime(refundDate.Year, refundDate.Month, 1);
                    var endDate = beginDate.AddMonths(1);

                    int? stockOutQty = PageStockOutRepository.GetStockOutQty(new UISearchStockOut
                    {
                        CompanyID = companyID,
                        ClientUserID = clientUserID,
                        ClientCompanyID = clientCompanyID,
                        ProductID = productID,
                        ProductSpecificationID = productSpecificationID,
                        BeginDate = beginDate,
                        EndDate = endDate
                    });

                    lblStockOutQty.Text = (stockOutQty ?? 0).ToString();

                    if (stockOutQty.HasValue && stockOutQty > clientProductSetting.MonthlyTask)
                    {
                        //计算返款金额
                        if (clientProductSetting.UseFlowData)
                        {
                            //实际本月流回数量* 返款价
                            txtRefundAmount.DbValue = (refundPrice ?? 0M) * 0; //实际本月流回数量暂时用0
                        }
                        else
                        {
                            //月出库数量 * 返款价
                            txtRefundAmount.DbValue = (refundPrice ?? 0M) * (stockOutQty ?? 0);
                        }
                    }
                    else
                        txtRefundAmount.DbValue = 0;
                }
            }
        }

        private void ResetProductOtherInfos()
        {
            lblTaskQty.Text = string.Empty;
            lblStockOutQty.Text = string.Empty;
            lblUseFlowData.Text = string.Empty;
            lblBackQty.Text = string.Empty;

            txtRefundPrice.DbValue = null;
            txtRefundAmount.DbValue = null;
        }

        private bool SaveCTRefundAppBasicData(ClientTaskRefundApplication currentEntity)
        {
            bool isSucceedSaved = false;

            int companyID = int.Parse(rcbxCompany.SelectedValue);
            int clientUserID = int.Parse(rcbxClientUser.SelectedValue);
            int clientCompanyID = int.Parse(rcbxClientCompany.SelectedValue);
            int productID = int.Parse(rcbxProduct.SelectedValue);
            int productSpecificationID = int.Parse(ddlProductSpecification.SelectedValue);
            decimal refundPrice = (decimal)(txtRefundPrice.Value ?? 0D);

            DateTime tempRefundDate = rmypRefundDate.SelectedDate.Value;
            DateTime refundDate = new DateTime(tempRefundDate.Year, tempRefundDate.Month, 1);

            int taskQty = 0;
            int stockOutQty = 0;
            decimal refundAmount = 0M;

            var clientProductSetting = PageClientInfoPSRepository.GetOneByCondistions(new UISearchClientInfoProductSetting
            {
                ClientUserID = clientUserID,
                ClientCompanyID = clientCompanyID,
                ProductID = productID,
                ProductSpecificationID = productSpecificationID
            });

            if (clientProductSetting != null)
            {
                taskQty = clientProductSetting.MonthlyTask ?? 0;

                var beginDate = refundDate;
                var endDate = beginDate.AddMonths(1);

                var tempStockOutQty = PageStockOutRepository.GetStockOutQty(new UISearchStockOut
                {
                    CompanyID = companyID,
                    ClientUserID = clientUserID,
                    ClientCompanyID = clientCompanyID,
                    ProductID = productID,
                    ProductSpecificationID = productSpecificationID,
                    BeginDate = beginDate,
                    EndDate = endDate
                });

                stockOutQty = tempStockOutQty ?? 0;

                if (stockOutQty > taskQty)
                {
                    //计算返款金额
                    if (clientProductSetting.UseFlowData)
                    {
                        //实际本月流回数量* 返款价
                        refundAmount = refundPrice * 0; //实际本月流回数量暂时用0
                    }
                    else
                    {
                        //月出库数量 * 返款价
                        refundAmount = refundPrice * stockOutQty;
                    }
                }

                currentEntity.UseFlowData = clientProductSetting.UseFlowData;
                currentEntity.BackQty = 0;//暂时用0
            }

            if (refundAmount <= 0M)
            {
                cvRefundAmount.IsValid = false;
                cvRefundAmount.ErrorMessage = "返款金额必须大于0";
                return isSucceedSaved;
            }

            var tempCTRefundAppCount = PageClientTaskRefundAppRepository.GetList(x => x.ID != currentEntity.ID
                && x.CompanyID == companyID && x.ClientUserID == clientUserID && x.ClientCompanyID == clientCompanyID
                && x.ProductID == productID && x.ProductSpecificationID == productSpecificationID
                && x.RefundDate == refundDate).Count();

            if (tempCTRefundAppCount > 0)
            {
                cvRefundDate.IsValid = false;
                cvRefundDate.ErrorMessage = "奖励年月跟系统已有数据有重叠，请重新选择日期或其他条件";

                return isSucceedSaved;
            }

            currentEntity.CompanyID = companyID;
            currentEntity.ClientUserID = clientUserID;
            currentEntity.ClientCompanyID = clientCompanyID;
            currentEntity.ProductID = productID;
            currentEntity.ProductSpecificationID = productSpecificationID;
            currentEntity.TaskQty = taskQty;
            currentEntity.StockOutQty = stockOutQty;
            currentEntity.RefundPrice = refundPrice;
            currentEntity.RefundAmount = refundAmount;
            currentEntity.RefundDate = refundDate;

            PageClientTaskRefundAppRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.ClientTaskRefunds;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    appNote.WorkflowStepID = (int)EWorkflowStep.EditClientTaskRefund;
                else
                    appNote.WorkflowStepID = (int)EWorkflowStep.NewClientTaskRefund;

                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }

            if (!string.IsNullOrEmpty(txtAuditComment.Text.Trim())
                && CanAuditUserIDs.Contains(CurrentUser.UserID))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.ClientTaskRefunds;
                appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;

                if (currentEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit)
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditCTRefundByDistrictManagers;
                else if (currentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByDistrictManagers)
                {
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditCTRefundByMarketManagers;
                }
                else if (currentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByMarketManagers)
                {
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditCTRefundByTreasurers;
                }
                else if (currentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByTreasurers)
                {
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditCTRefundByDeptManagers;
                }

                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtAuditComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }

            isSucceedSaved = true;

            return isSucceedSaved;
        }

        private bool UpdateClientTaskRefundApp(ClientTaskRefundApplication currentEntity, int oldWorkflowStatusID)
        {
            bool isSucceedSaved = false;

            decimal refundPrice = (decimal)(txtRefundPrice.Value ?? 0D);

            decimal refundAmount = 0M;

            //计算返款金额
            if (currentEntity.UseFlowData.HasValue
                && currentEntity.UseFlowData == true)
            {
                //实际本月流回数量* 返款价
                refundAmount = refundPrice * currentEntity.BackQty ?? 0; //实际本月流回数量暂时用0
            }
            else
            {
                //月出库数量 * 返款价
                refundAmount = refundPrice * currentEntity.StockOutQty;
            }

            if (refundAmount <= 0M)
            {
                cvRefundAmount.IsValid = false;
                cvRefundAmount.ErrorMessage = "返款金额必须大于0";
                return isSucceedSaved;
            }

            currentEntity.RefundPrice = refundPrice;
            currentEntity.RefundAmount = refundAmount;

            PageClientTaskRefundAppRepository.Save();

            if (!string.IsNullOrEmpty(txtAuditComment.Text.Trim())
                && CanAuditUserIDs.Contains(CurrentUser.UserID))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.ClientTaskRefunds;
                appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;

                if (oldWorkflowStatusID == (int)EWorkflowStatus.Submit)
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditCTRefundByDistrictManagers;
                else if (oldWorkflowStatusID == (int)EWorkflowStatus.ApprovedByDistrictManagers)
                {
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditCTRefundByMarketManagers;
                }
                else if (oldWorkflowStatusID == (int)EWorkflowStatus.ApprovedByMarketManagers)
                {
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditCTRefundByTreasurers;
                }
                else if (oldWorkflowStatusID == (int)EWorkflowStatus.ApprovedByTreasurers)
                {
                    appNote.WorkflowStepID = (int)EWorkflowStep.AuditCTRefundByDeptManagers;
                }

                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtAuditComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }

            isSucceedSaved = true;

            return isSucceedSaved;
        }
        #endregion

        protected void rcbxCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetProductOtherInfos();

            ddlProductSpecification.ClearSelection();
            ddlProductSpecification.Items.Clear();

            BindProducts();
        }

        protected void rcbxClientUser_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetProductOtherInfos();

            BindClientCompanies();
        }

        protected void rcbxProduct_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetProductOtherInfos();

            BindProductSpecifications();
        }

        protected void ddlProductSpecification_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            BindProductOtherInfos();
        }

        protected void rcbxClientCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindProductOtherInfos();
        }

        protected void rmypRefundDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindProductOtherInfos();
        }

        protected void txtRefundPrice_TextChanged(object sender, EventArgs e)
        {
            BindProductOtherInfos(false);
        }

        #region Grid events

        #region App notes events

        protected void rgAppNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

        protected void rgAuditNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

        #endregion

        #region rgAppPayments

        protected void rgAppPayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
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

                    //if (e.Item.ItemIndex < 0)
                    //{
                    //    var excludeItemValues = PageAppPaymentRepository
                    //    .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == this.CurrentEntityID)
                    //    .Select(x => x.FromBankAccountID.HasValue ? x.FromBankAccountID.Value : GlobalConst.INVALID_INT)
                    //    .ToList();

                    //    if (excludeItemValues.Count > 0)
                    //        uiSearchObj.ExcludeItemValues = excludeItemValues;
                    //}

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

                var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                if (txtComment != null && uiEntity != null)
                    txtComment.Text = uiEntity.Comment;
            }
        }

        protected void rgAppPayments_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                || (this.CanAccessUserIDs.Contains(CurrentUser.UserID)
                    && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByDeptManagers)))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
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
                appPayment.Comment = txtComment.Text;

                appPayment.ApplicationID = this.CurrentEntityID.Value;
                appPayment.WorkflowID = this.CurrentWorkFlowID;
                appPayment.PaymentStatusID = (int)EPaymentStatus.ToBePaid;
                appPayment.PaymentTypeID = (int)EPaymentType.Expend;

                PageAppPaymentRepository.Add(appPayment);

                PageAppPaymentRepository.Save();

                rgAppPayments.Rebind();
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
                    appPayment.Comment = txtComment.Text;

                    appPayment.ApplicationID = this.CurrentEntityID.Value;
                    appPayment.WorkflowID = this.CurrentWorkFlowID;
                    appPayment.PaymentStatusID = (int)EPaymentStatus.ToBePaid;
                    appPayment.PaymentTypeID = (int)EPaymentType.Expend;

                    PageAppPaymentRepository.Save();

                    rgAppPayments.Rebind();
                }
            }
        }

        #endregion

        #endregion

        #region Button events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if ((txtRefundAmount.Value ?? 0D) <= 0)
            {
                cvRefundAmount.IsValid = false;
                cvRefundAmount.ErrorMessage = "返款金额必须大于0";
            }

            if (!IsValid) return;

            ClientTaskRefundApplication currentEntity = this.CurrentEntity;

            if (currentEntity == null)
            {
                currentEntity = new ClientTaskRefundApplication();
                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;

                PageClientTaskRefundAppRepository.Add(currentEntity);
            }

            bool isSaved = SaveCTRefundAppBasicData(currentEntity);

            if (isSaved)
            {
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

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;

                bool isSaved = SaveCTRefundAppBasicData(this.CurrentEntity);

                if (isSaved)
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
                }
            }
        }

        protected void btnAudit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            var oldWorkflowStatusID = this.CurrentEntity.WorkflowStatusID;

            switch (oldWorkflowStatusID)
            {
                case (int)EWorkflowStatus.Submit:
                    CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDistrictManagers;
                    break;

                case (int)EWorkflowStatus.ApprovedByDistrictManagers:
                    CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByMarketManagers;
                    break;

                case (int)EWorkflowStatus.ApprovedByMarketManagers:
                    CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByTreasurers;
                    break;

                case (int)EWorkflowStatus.ApprovedByTreasurers:
                    CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDeptManagers;
                    break;
            }

            bool isSaved = UpdateClientTaskRefundApp(CurrentEntity, oldWorkflowStatusID);

            if (isSaved)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            var oldWorkflowStatusID = this.CurrentEntity.WorkflowStatusID;

            CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

            bool isSaved = UpdateClientTaskRefundApp(CurrentEntity, oldWorkflowStatusID);

            if (isSaved)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientTaskRefundAppRepository ctRefundAppRepository = new ClientTaskRefundAppRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    ctRefundAppRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var currentEntity = ctRefundAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == CurrentEntity.ID).ToList();

                        var totalRefundAmount = CurrentEntity.RefundAmount ?? 0M;

                        var totalPayAmount = appPayments.Sum(x => (x.Amount ?? 0M) + (x.Fee ?? 0M));

                        if (totalPayAmount != totalRefundAmount)
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("支付总额不等于应返款总额，不能确认支付");
                        }
                        else
                        {
                            foreach (var item in appPayments)
                            {
                                item.PaymentStatusID = (int)EPaymentStatus.Paid;
                            }

                            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.Paid;

                            unitOfWork.SaveChanges();

                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
                    }
                }
            }
        }

        #endregion

    }
}