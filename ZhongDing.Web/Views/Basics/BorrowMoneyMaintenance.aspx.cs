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
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;
using System.IO;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class BorrowMoneyMaintenance : WorkflowBasePage
    {

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.BorrowMoneyManagement;
        }
        #region Members


        private IBankAccountRepository _PageBankAccountRepository;
        private IBankAccountRepository PageBankAccountRepository
        {
            get
            {
                if (_PageBankAccountRepository == null)
                {
                    _PageBankAccountRepository = new BankAccountRepository();
                }

                return _PageBankAccountRepository;
            }
        }

        private IBorrowMoneyRepository _PageBorrowMoneyRepository;
        private IBorrowMoneyRepository PageBorrowMoneyRepository
        {
            get
            {
                if (_PageBorrowMoneyRepository == null)
                {
                    _PageBorrowMoneyRepository = new BorrowMoneyRepository();
                }

                return _PageBorrowMoneyRepository;
            }
        }

        private IApplicationPaymentRepository _PageApplicationPaymentRepository;
        private IApplicationPaymentRepository PageApplicationPaymentRepository
        {
            get
            {
                if (_PageApplicationPaymentRepository == null)
                {
                    _PageApplicationPaymentRepository = new ApplicationPaymentRepository();
                }

                return _PageApplicationPaymentRepository;
            }
        }

        private ICompanyRepository _PageCompanyRepository;
        private ICompanyRepository PageCompanyRepository
        {
            get
            {
                if (_PageCompanyRepository == null)
                {
                    _PageCompanyRepository = new CompanyRepository();
                }

                return _PageCompanyRepository;
            }
        }

        private IAccountTypeRepository _PageAccountTypeRepository;
        private IAccountTypeRepository PageAccountTypeRepository
        {
            get
            {
                if (_PageAccountTypeRepository == null)
                {
                    _PageAccountTypeRepository = new AccountTypeRepository();
                }

                return _PageAccountTypeRepository;
            }
        }

        private IAttachmentFileRepository _PageAttachmentFileRepository;
        private IAttachmentFileRepository PageAttachmentFileRepository
        {
            get
            {
                if (_PageAttachmentFileRepository == null)
                {
                    _PageAttachmentFileRepository = new AttachmentFileRepository();
                }

                return _PageAttachmentFileRepository;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.BorrowMoneyManage;

            if (!IsPostBack)
            {
                LoadCurrentEntity();
                base.PermissionOptionCheckButtonDelete(btnDelete);
            }
        }

        #region Private Methods

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                var currentEntity = PageBorrowMoneyRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    hdnCurrentEntityID.Value = currentEntity.ID.ToString();
                    txtBorrowAmount.Value = currentEntity.BorrowAmount.ToDoubleOrNull();
                    txtBorrowName.Text = currentEntity.BorrowName;
                    txtComment.Text = currentEntity.Comment;
                    rdpBorrowDate.SelectedDate = currentEntity.BorrowDate;
                    rdpReturnDate.SelectedDate = currentEntity.ReturnDate;

                    var hasApplicationPayment = PageApplicationPaymentRepository.GetList(x =>
                            x.WorkflowID == (int)EWorkflow.BorrowMoneyManagement
                            && x.ApplicationID == currentEntity.ID
                            && x.PaymentStatusID == (int)EPaymentStatus.Paid).Any();
                    if (hasApplicationPayment)
                    {
                        txtBorrowAmount.Enabled = txtBorrowName.Enabled = txtComment.Enabled = rdpBorrowDate.Enabled = rdpReturnDate.Enabled = radAsyncUpload.Enabled = rgAttachmentFiles.Enabled = false;
                    }

                }
                else
                    btnDelete.Visible = false;
            }
            else
                btnDelete.Visible = false;
        }
        #endregion

        #region Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            BorrowMoney currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageBorrowMoneyRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new BorrowMoney();

                PageBorrowMoneyRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {

                currentEntity.BorrowAmount = txtBorrowAmount.Value.ToDecimal();
                currentEntity.BorrowDate = rdpBorrowDate.SelectedDate.Value;
                currentEntity.BorrowName = txtBorrowName.Text.Trim();
                currentEntity.Comment = txtComment.Text.Trim();
                currentEntity.ReturnDate = rdpReturnDate.SelectedDate.Value;
                currentEntity.Status = (int)EBorrowMoneyStatus.NotReturn;
                PageBorrowMoneyRepository.Save();
                try
                {
                    if (radAsyncUpload.UploadedFiles.Count > 0)
                    {
                        for (int i = 0; i < radAsyncUpload.UploadedFiles.Count; i++)
                        {
                            string uploadFilePath = WebConfig.UploadFilePathBorrowMoney;
                            string uploadFileServerFolderPath = Server.MapPath(uploadFilePath);
                            if (!Directory.Exists(uploadFileServerFolderPath))
                                Directory.CreateDirectory(uploadFileServerFolderPath);
                            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + radAsyncUpload.UploadedFiles[i].GetExtension();
                            string fileNameFullPath = uploadFileServerFolderPath + fileName;
                            string relativePath = uploadFilePath + fileName;
                            radAsyncUpload.UploadedFiles[i].SaveAs(fileNameFullPath);
                            AttachmentFile attachmentFile = new AttachmentFile()
                            {
                                AttachmentHostTableID = currentEntity.ID,
                                AttachmenTypeID = (int)EAttachmenType.BorrowMoneyAttachmen,
                                FileName = radAsyncUpload.UploadedFiles[i].FileName,
                                FilePath = relativePath
                            };
                            PageAttachmentFileRepository.Add(attachmentFile);
                        }
                        PageAttachmentFileRepository.Save();

                    }
                }
                catch (Exception ex)
                { }


                hdnCurrentEntityID.Value = currentEntity.ID.ToString();
                BindAttachmentFiles(true);
                if (this.CurrentEntityID.BiggerThanZero())
                {
                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
                }
                else
                {
                    this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REFRESH);
                }

            }



        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                PageBorrowMoneyRepository.DeleteByID(this.CurrentEntityID);
                PageBorrowMoneyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }

        protected void cvAccount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //string inputAccountNo = txtAccount.Text.Trim();

            //if (!string.IsNullOrEmpty(inputAccountNo))
            //{
            //    inputAccountNo = inputAccountNo.Replace("-", "");

            //    if (!Utility.IsValidAccountNumber(inputAccountNo))
            //        args.IsValid = false;
            //}
        }


        private void BindAttachmentFiles(bool isNeedRebind)
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                rgAttachmentFiles.Visible = true;
                UISearchAttachmentFile uiSearchObj = new UISearchAttachmentFile()
                {
                    //CompanyID = CurrentUser.CompanyID,
                    //ReimbursementTypeCode = txtSerialNo.Text.Trim(),
                    //Name = txtName.Text.Trim(),
                    //ParentID = this.CurrentEntityID
                    AttachmentTypeID = (int)EAttachmenType.BorrowMoneyAttachmen,
                    AttachmentHostTableID = this.CurrentEntityID
                };

                int totalRecords;

                var uiReimbursementTypes = PageAttachmentFileRepository.GetUIList(uiSearchObj, rgAttachmentFiles.CurrentPageIndex, rgAttachmentFiles.PageSize, out totalRecords);

                rgAttachmentFiles.VirtualItemCount = totalRecords;

                rgAttachmentFiles.DataSource = uiReimbursementTypes;

                if (isNeedRebind)
                    rgAttachmentFiles.Rebind();
            }
            else
            {
                rgAttachmentFiles.Visible = false;
            }
        }


        protected void rgAttachmentFiles_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindAttachmentFiles(false);
        }


        protected void rgAttachmentFiles_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            int id = editableItem.GetDataKeyValue("ID").ToString().ToInt();


            var item = PageAttachmentFileRepository.GetByID(id);


            DownloadFile(item.FileName, Server.MapPath(item.FilePath));

        }

        protected void rgAttachmentFiles_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageAttachmentFileRepository.DeleteByID(id);
                PageAttachmentFileRepository.Save();
            }

            rgAttachmentFiles.Rebind();
        }

        protected void rgAttachmentFiles_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgAttachmentFiles_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgAttachmentFiles_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {


        }


        #endregion




        #region rgAppPayments

        protected void rgAppPayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.CurrentEntityID.BiggerThanZero())
            {
                divAppPayments.Visible = true;

                var uiSearchObj = new UISearchApplicationPayment
                {
                    WorkflowID = this.CurrentWorkFlowID,
                    ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                };

                int totalRecords;

                var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);
                foreach (var item in appPayments)
                {
                    if (item.PaymentTypeID == (int)EPaymentType.Expend)
                    {
                        item.PaymentType = "借款";
                        item.ToAccount = item.FromAccount;
                        item.ToBankAccountID = item.ToBankAccountID;
                    }
                    else
                    {
                        item.PaymentType = "还款";
                    }
                }
                rgAppPayments.DataSource = appPayments;
                rgAppPayments.VirtualItemCount = totalRecords;
            }
            else
            {
                divAppPayments.Visible = false;
            }
        }
        protected void rgAppPayments_ItemDataBound(object sender, GridItemEventArgs e)
        {

            if (e.Item.ItemType == GridItemType.Item
             || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var editColumn = rgAppPayments.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);
                if (editColumn != null)
                {
                    editColumn.Visible = false;
                    var editCell = gridDataItem.Cells[editColumn.OrderIndex];
                    if (editCell != null)
                        editCell.Text = "";
                }
            }


            if (e.Item.ItemType == GridItemType.EditItem)
            {
                var editColumn = rgAppPayments.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);
                if (editColumn != null)
                {
                    editColumn.Visible = true;
                }

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


                var rcbxPaymentType = (RadComboBox)e.Item.FindControl("rcbxPaymentType");
                if (IsNotNull(rcbxPaymentType, uiEntity))
                {
                    rcbxPaymentType.SelectedValue = uiEntity.PaymentTypeID.ToString();
                }
                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

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

        protected void rgAppPayments_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                ApplicationPayment appPayment = new ApplicationPayment();

                var rcbxPaymentType = (RadComboBox)e.Item.FindControl("rcbxPaymentType");
                appPayment.PaymentTypeID = rcbxPaymentType.SelectedValue.ToInt();
                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                appPayment.Amount = (decimal?)txtAmount.Value;
                var rfvAmount = ((RequiredFieldValidator)e.Item.FindControl("rfvAmount"));
                if (appPayment.PaymentTypeID == (int)EPaymentType.Expend)//支出 借款
                {
                    var uiSearchObj = new UISearchApplicationPayment
                    {
                        WorkflowID = this.CurrentWorkFlowID,
                        ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                        PaymentTypeID = (int)EPaymentType.Expend
                    };
                    var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj);
                    decimal nowTotalAmount = appPayments.Any() ? appPayments.Sum(x => x.Amount).Value : 0M;
                    decimal totalAmount = nowTotalAmount + appPayment.Amount.Value;
                    var currentEntity = PageBorrowMoneyRepository.GetByID(this.CurrentEntityID);
                    if (totalAmount > currentEntity.BorrowAmount)
                    {
                        rfvAmount.IsValid = false;
                        rfvAmount.ErrorMessage = "借款总额不能超过借款单据金额";
                        e.Canceled = true;
                        return;
                    }
                }
                else
                {
                    var uiSearchObj = new UISearchApplicationPayment
                    {
                        WorkflowID = this.CurrentWorkFlowID,
                        ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                        PaymentTypeID = (int)EPaymentType.Income
                    };
                    var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj);
                    decimal nowTotalAmount = appPayments.Any() ? appPayments.Sum(x => x.Amount).Value : 0M;
                    decimal totalAmount = nowTotalAmount + appPayment.Amount.Value;
                    var currentEntity = PageBorrowMoneyRepository.GetByID(this.CurrentEntityID);
                    if (totalAmount > currentEntity.BorrowAmount)
                    {
                        rfvAmount.IsValid = false;
                        rfvAmount.ErrorMessage = "还款总额不能超过借款单据金额";
                        e.Canceled = true;
                        return;
                    }
                }

                var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
                appPayment.PayDate = rdpPayDate.SelectedDate;





                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");
                if (rcbxToAccount.SelectedItem != null)
                {
                    if (appPayment.PaymentTypeID == (int)EPaymentType.Income)
                    {
                        appPayment.ToBankAccountID = rcbxToAccount.SelectedValue.ToIntOrNull();
                        appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                    }
                    else
                    {
                        appPayment.FromBankAccountID = rcbxToAccount.SelectedValue.ToIntOrNull();
                        appPayment.FromAccount = rcbxToAccount.SelectedItem.Text;
                    }

                }


                //var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
                //appPayment.Fee = (decimal?)txtFee.Value;

                appPayment.ApplicationID = this.CurrentEntityID.Value;
                appPayment.WorkflowID = this.CurrentWorkFlowID;
                appPayment.PaymentStatusID = (int)EPaymentStatus.Paid;

                PageAppPaymentRepository.Add(appPayment);

                PageAppPaymentRepository.Save();

                LoadCurrentEntity();


                if (appPayment.PaymentTypeID == (int)EPaymentType.Income)//支出 借款
                {
                    var uiSearchObj = new UISearchApplicationPayment
                    {
                        WorkflowID = this.CurrentWorkFlowID,
                        ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                        PaymentTypeID = (int)EPaymentType.Income
                    };
                    var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj);
                    decimal nowTotalAmount = appPayments.Any() ? appPayments.Sum(x => x.Amount).Value : 0M;
                    var currentEntity = PageBorrowMoneyRepository.GetByID(this.CurrentEntityID);
                    if (nowTotalAmount >= currentEntity.BorrowAmount)
                    {
                        currentEntity.Status = (int)EBorrowMoneyStatus.Returned;
                        PageBorrowMoneyRepository.Save();
                    }

                }
            }

            rgAppPayments.Rebind();

            //BindPaymentSummary();
        }
        //private void BindPaymentSummary()
        //{
        //    var appPaymentAmounts = PageAppPaymentRepository
        //        .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == this.CurrentEntityID && x.IsDeleted == false)
        //        .Select(x => x.Amount).ToList();

        //    if (appPaymentAmounts.Count > 0)
        //    {
        //        decimal totalPaymentAmount = appPaymentAmounts.Sum(x => x ?? 0);

        //        lblTotalPaymentAmount.Text = totalPaymentAmount.ToString("C2");
        //        lblCapitalTotalPaymentAmount.Text = totalPaymentAmount.ToString().ConvertToChineseMoney();
        //    }
        //}
        protected void rgAppPayments_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            //var editableItem = ((GridEditableItem)e.Item);
            //String sid = editableItem.GetDataKeyValue("ID").ToString();

            //int id = 0;
            //if (int.TryParse(sid, out id))
            //{
            //    if (e.Item is GridDataItem)
            //    {
            //        GridDataItem dataItem = e.Item as GridDataItem;

            //        var appPayment = PageAppPaymentRepository.GetByID(id);

            //        var rdpPayDate = (RadDatePicker)e.Item.FindControl("rdpPayDate");
            //        appPayment.PayDate = rdpPayDate.SelectedDate;


            //        var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");
            //        if (rcbxToAccount.SelectedItem != null)
            //        {
            //            appPayment.ToBankAccountID = rcbxToAccount.SelectedValue.ToIntOrNull();
            //            appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
            //        }
            //        var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
            //        appPayment.Amount = (decimal?)txtAmount.Value;

            //        //var txtFee = (RadNumericTextBox)e.Item.FindControl("txtFee");
            //        //appPayment.Fee = (decimal?)txtFee.Value;
            //        PageAppPaymentRepository.Save();

            //        rgAppPayments.Rebind();
            //    }
            //}

            //BindPaymentSummary();
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
                //BindPaymentSummary();
            }

        }

        #endregion



        protected override EPermission PagePermissionID()
        {
            return EPermission.BorrowMoneyManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }





    }
}