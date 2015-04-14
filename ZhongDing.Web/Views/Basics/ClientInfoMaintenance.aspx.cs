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

namespace ZhongDing.Web.Views.Basics
{
    public partial class ClientInfoMaintenance : BasePage
    {
        #region Members

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

        private IClientInfoContactRepository _PageClientInfoContactRepository;
        private IClientInfoContactRepository PageClientInfoContactRepository
        {
            get
            {
                if (_PageClientInfoContactRepository == null)
                    _PageClientInfoContactRepository = new ClientInfoContactRepository();

                return _PageClientInfoContactRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientInfoManage;

            if (!IsPostBack)
            {
                BindClientUsers();
                BindClientCompanies();

                LoadClientInfo();
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

        private void BindClientCompanies()
        {
            var clientCompanies = PageClientCompanyRepository.GetDropdownItems();
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadClientInfo()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var clientInfo = PageClientInfoRepository.GetByID(this.CurrentEntityID);

                if (clientInfo != null)
                {
                    hdnCurrentEntityID.Value = clientInfo.ID.ToString();

                    txtClientCode.Text = clientInfo.ClientCode;
                    rcbxClientUser.SelectedValue = clientInfo.ClientUserID.ToString();
                    rcbxClientCompany.SelectedValue = clientInfo.ClientCompanyID.ToString();
                    txtReceiverName.Text = clientInfo.ReceiverName;
                    txtPhoneNumber.Text = clientInfo.PhoneNumber;
                    txtFax.Text = clientInfo.Fax;
                    txtReceiverAddress.Text = clientInfo.ReceiverAddress;
                    txtReceiptAddress.Text = clientInfo.ReceiptAddress;

                    if (clientInfo.ClientUser.BankAccount != null)
                    {
                        var bankAccount = clientInfo.ClientUser.BankAccount;

                        txtAccountName.Text = bankAccount.AccountName;
                        txtBankBranchName.Text = bankAccount.BankBranchName;
                        txtAccount.Text = bankAccount.Account;
                        txtComment.Text = bankAccount.Comment;
                    }
                }
                else
                {
                    divOtherSections.Visible = false;
                    btnDelete.Visible = false;

                    txtClientCode.Text = Utility.GenerateAutoSerialNo(PageClientInfoRepository.GetMaxEntityID(),
                        GlobalConst.EntityAutoSerialNo.SerialNoPrefix.CLIENT_INFO);
                }
            }
            else
            {
                divOtherSections.Visible = false;
                btnDelete.Visible = false;

                txtClientCode.Text = Utility.GenerateAutoSerialNo(PageClientInfoRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.CLIENT_INFO);
            }
        }

        protected void rgBankAccounts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgBankAccounts.DataSource = PageClientInfoRepository.GetBankAccounts(this.CurrentEntityID);
        }

        protected void rgBankAccounts_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    IBankAccountRepository bankAccountRepository = new BankAccountRepository();
                    IClientInfoBankAccountRepository clientInfoBankAccountRepository = new ClientInfoBankAccountRepository();

                    bankAccountRepository.SetDbModel(db);
                    clientInfoBankAccountRepository.SetDbModel(db);

                    var clientBankAccount = clientInfoBankAccountRepository.GetByID(id);

                    if (clientBankAccount != null)
                        bankAccountRepository.Delete(clientBankAccount.BankAccount);

                    clientInfoBankAccountRepository.Delete(clientBankAccount);

                    unitOfWork.SaveChanges();
                }
            }

            rgBankAccounts.Rebind();
        }

        protected void cvClientCompany_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(rcbxClientCompany.Text))
            {
                UISearchDropdownItem uiSearchObj = new UISearchDropdownItem()
                {
                    ItemText = rcbxClientCompany.Text.Trim()
                };

                var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);

                if (clientCompanies.Count <= 0)
                    args.IsValid = false;
            }
        }

        protected void cvAccount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value.Trim()))
            {
                if (!Utility.IsValidAccountNumber(args.Value.Trim()))
                {
                    cvAccount.ErrorMessage = "帐号无效，请重新输入";

                    args.IsValid = false;
                }
            }
        }

        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int clientUserID;

                if (int.TryParse(e.Value, out clientUserID))
                {
                    var clientUser = PageClientUserRepository.GetByID(clientUserID);

                    if (clientUser != null && clientUser.BankAccount != null)
                    {
                        var bankAccount = clientUser.BankAccount;

                        txtAccountName.Text = bankAccount.AccountName;
                        txtBankBranchName.Text = bankAccount.BankBranchName;
                        txtAccount.Text = bankAccount.Account;
                        txtComment.Text = bankAccount.Comment;
                    }
                    else
                    {
                        txtAccountName.Text = string.Empty;
                        txtBankBranchName.Text = string.Empty;
                        txtAccount.Text = string.Empty;
                        txtComment.Text = string.Empty;
                    }
                }
            }
            else
            {
                txtAccountName.Text = string.Empty;
                txtBankBranchName.Text = string.Empty;
                txtAccount.Text = string.Empty;
                txtComment.Text = string.Empty;
            }
        }

        protected void rgContacts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgContacts.DataSource = PageClientInfoRepository.GetContacts(this.CurrentEntityID);
        }

        protected void rgContacts_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {

                PageClientInfoContactRepository.DeleteByID(id);
                PageClientInfoContactRepository.Save();
            }

            rgContacts.Rebind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rcbxClientUser.SelectedValue)
                && string.IsNullOrEmpty(hdnCustomClientName.Value.Trim()))
                cvClientName.IsValid = false;

            int selClientUserID = GlobalConst.INVALID_INT;
            if (!int.TryParse(rcbxClientUser.SelectedValue, out selClientUserID))
            {
                var clientUser = PageClientUserRepository
                    .GetList(x => x.ClientName.ToLower() == hdnCustomClientName.Value.Trim().ToLower())
                    .FirstOrDefault();

                if (clientUser != null)
                    selClientUserID = clientUser.ID;
            }

            int selClientCompanyID;
            if (int.TryParse(rcbxClientCompany.SelectedValue, out selClientCompanyID))
            {
                if (PageClientInfoRepository.GetList(x => x.ID != this.CurrentEntityID && x.ClientUserID == selClientUserID
                    && x.ClientCompanyID == selClientCompanyID).Count() > 0)
                    cvDiffClientCompany.IsValid = false;
            }

            if (!string.IsNullOrEmpty(txtAccountName.Text.Trim())
                || !string.IsNullOrEmpty(txtBankBranchName.Text.Trim())
                || !string.IsNullOrEmpty(txtAccount.Text.Trim())
                || !string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                if (string.IsNullOrEmpty(txtAccountName.Text.Trim()))
                    cvAccountName.IsValid = false;

                if (string.IsNullOrEmpty(txtBankBranchName.Text.Trim()))
                    cvBankBranchName.IsValid = false;

                if (string.IsNullOrEmpty(txtAccount.Text.Trim()))
                {
                    cvAccount.IsValid = false;
                    cvAccount.ErrorMessage = "请输入银行账号";
                }
            }

            if (!IsValid) return;

            ClientInfo clientInfo = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                clientInfo = PageClientInfoRepository.GetByID(this.CurrentEntityID);

            if (clientInfo == null)
            {
                clientInfo = new ClientInfo()
                {
                    ClientCode = Utility.GenerateAutoSerialNo(PageClientInfoRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.CLIENT_INFO)
                };

                PageClientInfoRepository.Add(clientInfo);
            }

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    clientInfo.ClientUserID = clientUserID;
            }
            else
            {
                ClientUser clientUser = new ClientUser();
                clientUser.ClientName = hdnCustomClientName.Value.Trim();

                clientInfo.ClientUser = clientUser;
            }

            if (!string.IsNullOrEmpty(rcbxClientCompany.SelectedValue))
            {
                int clientCompanyID;
                if (int.TryParse(rcbxClientCompany.SelectedValue, out clientCompanyID))
                    clientInfo.ClientCompanyID = clientCompanyID;
            }

            clientInfo.ReceiverName = txtReceiverName.Text.Trim();
            clientInfo.PhoneNumber = txtPhoneNumber.Text.Trim();
            clientInfo.Fax = txtFax.Text.Trim();
            clientInfo.ReceiverAddress = txtReceiverAddress.Text.Trim();
            clientInfo.ReceiptAddress = txtReceiptAddress.Text.Trim();

            if (!string.IsNullOrEmpty(txtAccountName.Text.Trim())
                && !string.IsNullOrEmpty(txtBankBranchName.Text.Trim())
                && !string.IsNullOrEmpty(txtAccount.Text.Trim()))
            {
                var bankAccount = clientInfo.ClientUser.BankAccount;

                if (bankAccount == null)
                {
                    bankAccount = new BankAccount()
                    {
                        OwnerTypeID = (int)EOwnerType.Client
                    };

                    clientInfo.ClientUser.BankAccount = bankAccount;
                }

                bankAccount.AccountName = txtAccountName.Text.Trim();
                bankAccount.BankBranchName = txtBankBranchName.Text.Trim();
                bankAccount.Account = txtAccount.Text.Trim();
                bankAccount.Comment = txtComment.Text.Trim();
            }

            PageClientInfoRepository.Save();

            hdnCurrentEntityID.Value = clientInfo.ID.ToString();

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    IClientInfoRepository clientInfoRepository = new ClientInfoRepository();
                    IClientInfoBankAccountRepository clientInfoBankAccountRepository = new ClientInfoBankAccountRepository();
                    IBankAccountRepository bankAccountRepository = new BankAccountRepository();
                    IClientInfoContactRepository clientInfoContactRepository = new ClientInfoContactRepository();
                    clientInfoRepository.SetDbModel(db);
                    clientInfoBankAccountRepository.SetDbModel(db);
                    bankAccountRepository.SetDbModel(db);
                    clientInfoContactRepository.SetDbModel(db);

                    var clientInfo = clientInfoRepository.GetByID(this.CurrentEntityID);

                    if (clientInfo != null)
                    {
                        foreach (var clientInfoBA in clientInfo.ClientInfoBankAccount)
                        {
                            if (clientInfoBA != null)
                            {
                                if (clientInfoBA.BankAccount != null)
                                    bankAccountRepository.Delete(clientInfoBA.BankAccount);

                                clientInfoBankAccountRepository.Delete(clientInfoBA);
                            }
                        }

                        foreach (var clientInfoContact in clientInfo.ClientInfoContact)
                        {
                            clientInfoContactRepository.Delete(clientInfoContact);
                        }

                        clientInfoRepository.Delete(clientInfo);
                    }

                    unitOfWork.SaveChanges();
                }

                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }



    }
}