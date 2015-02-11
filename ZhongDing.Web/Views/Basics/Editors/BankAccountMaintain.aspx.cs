using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Basics.Editors
{
    public partial class BankAccountMaintain : BasePage
    {
        #region Fields

        /// <summary>
        /// 所有者类型ID
        /// </summary>
        /// <value>The owner type ID.</value>
        private int? OwnerTypeID
        {
            get
            {
                string sOwnerTypeID = Request.QueryString["OwnerTypeID"];

                int iOwnerTypeID;

                if (int.TryParse(sOwnerTypeID, out iOwnerTypeID))
                    return iOwnerTypeID;
                else
                    return null;
            }
        }

        /// <summary>
        /// 所属的实体ID
        /// </summary>
        /// <value>The owner entity ID.</value>
        private int? OwnerEntityID
        {
            get
            {
                string sOwnerEntityID = Request.QueryString["OwnerEntityID"];

                int iOwnerEntityID;

                if (int.TryParse(sOwnerEntityID, out iOwnerEntityID))
                    return iOwnerEntityID;
                else
                    return null;
            }
        }

        #endregion

        #region Members

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                {
                    _PageSupplierRepository = new SupplierRepository();
                }

                return _PageSupplierRepository;
            }
        }

        private IClientInfoRepository _PageClientInfoRepository;
        private IClientInfoRepository PageClientInfoRepository
        {
            get
            {
                if (_PageClientInfoRepository == null)
                {
                    _PageClientInfoRepository = new ClientInfoRepository();
                }

                return _PageClientInfoRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!this.OwnerEntityID.HasValue
                || this.OwnerEntityID <= 0)
                || (!this.OwnerTypeID.HasValue
                || this.OwnerTypeID <= 0))
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                LoadBankAccount();
            }
        }

        private void LoadBankAccount()
        {
            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                BankAccount bankAccount = null;

                EOwnerType ownerType = (EOwnerType)OwnerTypeID.Value;

                switch (ownerType)
                {
                    case EOwnerType.Company:
                        break;
                    case EOwnerType.Supplier:
                        var supplier = PageSupplierRepository.GetByID(this.OwnerEntityID);

                        if (supplier != null)
                            bankAccount = supplier.SupplierBankAccount.Where(x => x.IsDeleted == false
                                && x.ID == this.CurrentEntityID).Select(x => x.BankAccount).FirstOrDefault();

                        break;

                    case EOwnerType.Client:
                        var clientInfo = PageClientInfoRepository.GetByID(this.OwnerEntityID);
                        if (clientInfo != null)
                            bankAccount = clientInfo.ClientInfoBankAccount.Where(x => x.IsDeleted == false
                                && x.ID == this.CurrentEntityID).Select(x => x.BankAccount).FirstOrDefault();

                        break;

                    case EOwnerType.Producer:
                        break;
                    case EOwnerType.Product:
                        break;
                }

                if (bankAccount != null)
                {
                    txtAccountName.Text = bankAccount.AccountName;
                    txtBankBranchName.Text = bankAccount.BankBranchName;
                    txtAccount.Text = bankAccount.Account;
                    txtComment.Text = bankAccount.Comment;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            bool isSuccessSaved = false;

            BankAccount bankAccount = null;

            EOwnerType ownerType = (EOwnerType)OwnerTypeID.Value;

            switch (ownerType)
            {
                case EOwnerType.Company:
                    break;
                case EOwnerType.Supplier:

                    #region 供应商银行帐号
                    var supplier = PageSupplierRepository.GetByID(this.OwnerEntityID);

                    if (supplier == null)
                    {
                        this.Master.BaseNotification.OnClientHidden = "onError";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                        this.Master.BaseNotification.Show("无效的供应商，窗口将自动关闭");

                        return;
                    }

                    if (this.CurrentEntityID.HasValue
                        && this.CurrentEntityID > 0)
                        bankAccount = supplier.SupplierBankAccount.Where(x => x.ID == this.CurrentEntityID)
                            .Select(x => x.BankAccount).FirstOrDefault();
                    else
                    {
                        bankAccount = new BankAccount()
                        {
                            CompanyID = CurrentUser.CompanyID,
                            OwnerTypeID = (int)EOwnerType.Supplier
                        };

                        SupplierBankAccount supplierBankAccount = new SupplierBankAccount();

                        supplierBankAccount.BankAccount = bankAccount;

                        supplier.SupplierBankAccount.Add(supplierBankAccount);
                    }

                    if (bankAccount != null)
                    {
                        bankAccount.AccountName = txtAccountName.Text.Trim();
                        bankAccount.BankBranchName = txtBankBranchName.Text.Trim();
                        bankAccount.Account = txtAccount.Text.Trim();
                        bankAccount.AccountTypeID = (int)EAccountType.Company;
                        bankAccount.Comment = txtComment.Text.Trim();
                    }

                    supplier.LastModifiedOn = DateTime.Now;
                    supplier.LastModifiedBy = CurrentUser.UserID;

                    PageSupplierRepository.Save();

                    isSuccessSaved = true;
                    #endregion

                    break;

                case EOwnerType.Client:
                    #region 客户银行帐号
                    var clientInfo = PageClientInfoRepository.GetByID(this.OwnerEntityID);

                    if (clientInfo == null)
                    {
                        this.Master.BaseNotification.OnClientHidden = "onError";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                        this.Master.BaseNotification.Show("无效的客户，窗口将自动关闭");

                        return;
                    }

                    if (this.CurrentEntityID.HasValue
                        && this.CurrentEntityID > 0)
                        bankAccount = clientInfo.ClientInfoBankAccount.Where(x => x.ID == this.CurrentEntityID)
                            .Select(x => x.BankAccount).FirstOrDefault();
                    else
                    {
                        bankAccount = new BankAccount()
                        {
                            CompanyID = CurrentUser.CompanyID,
                            OwnerTypeID = (int)EOwnerType.Client
                        };

                        ClientInfoBankAccount clientInfoBankAccount = new ClientInfoBankAccount();

                        clientInfoBankAccount.BankAccount = bankAccount;

                        clientInfo.ClientInfoBankAccount.Add(clientInfoBankAccount);
                    }

                    if (bankAccount != null)
                    {
                        bankAccount.AccountName = txtAccountName.Text.Trim();
                        bankAccount.BankBranchName = txtBankBranchName.Text.Trim();
                        bankAccount.Account = txtAccount.Text.Trim();
                        bankAccount.AccountTypeID = (int)EAccountType.Company;
                        bankAccount.Comment = txtComment.Text.Trim();
                    }

                    clientInfo.LastModifiedOn = DateTime.Now;
                    clientInfo.LastModifiedBy = CurrentUser.UserID;

                    PageClientInfoRepository.Save();

                    isSuccessSaved = true;
                    #endregion
                    break;
                case EOwnerType.Producer:
                    break;
                case EOwnerType.Product:
                    break;
                default:
                    break;
            }

            if (isSuccessSaved)
            {
                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_FAILED_SAEVED_CLOSE_WIN);
            }
        }

        protected void cvAccount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string inputAccountNo = txtAccount.Text.Trim();

            if (!string.IsNullOrEmpty(inputAccountNo))
            {
                if (!Utility.IsValidAccountNumber(inputAccountNo))
                    args.IsValid = false;
            }
        }
    }
}