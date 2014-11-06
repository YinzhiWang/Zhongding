﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class BankAccountManagement : BasePage
    {
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = 4;

            if (!IsPostBack)
            {
                BindAccountTypes();
                BindCompanies();
            }

        }

        #region Private Methods

        private void BindAccountTypes()
        {
            var accountTypes = PageAccountTypeRepository.GetUIList();

            ddlAccountType.DataSource = accountTypes;
            ddlAccountType.DataTextField = "AccountTypeName";
            ddlAccountType.DataValueField = "ID";
            ddlAccountType.DataBind();

            ddlAccountType.Items.Insert(0, new DropDownListItem() { });
        }

        private void BindCompanies()
        {
            var accountTypes = PageCompanyRepository.GetUIList();

            comboxCompany.DataSource = accountTypes;
            comboxCompany.DataTextField = "CompanyName";
            comboxCompany.DataValueField = "ID";
            comboxCompany.DataBind();

            comboxCompany.DefaultItem.Text = "--请选择--";
            comboxCompany.DefaultItem.Value = "";
        }

        private void BindBankAccounts(bool isNeedRebind)
        {
            UISearchBankAccount uiSearchObj = new UISearchBankAccount()
            {
                AccountName = txtAccountName.Text.Trim(),
                BankBranchName = txtBankBranchName.Text.Trim(),
                Account = txtAccount.Text.Trim(),
                IsNeedPaging = true,
                PageIndex = rgBankAccounts.CurrentPageIndex,
                PageSize = rgBankAccounts.PageSize,
                IsNeedMaskedAccount = true
            };

            if (!string.IsNullOrEmpty(ddlAccountType.SelectedValue))
                uiSearchObj.AccountTypeID = Convert.ToInt32(ddlAccountType.SelectedValue);

            if (!string.IsNullOrEmpty(comboxCompany.SelectedValue))
                uiSearchObj.CompanyID = Convert.ToInt32(comboxCompany.SelectedValue);

            int totalRecords;

            var companies = PageBankAccountRepository.GetUIList(uiSearchObj, out totalRecords);

            rgBankAccounts.VirtualItemCount = totalRecords;

            rgBankAccounts.DataSource = companies;

            if (isNeedRebind)
                rgBankAccounts.Rebind();
        }


        #endregion

        #region Events

        protected void rgBankAccounts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindBankAccounts(false);
        }

        protected void rgBankAccounts_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageBankAccountRepository.DeleteByID(id);
                PageBankAccountRepository.Save();
            }

            rgBankAccounts.Rebind();
        }

        protected void rgBankAccounts_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgBankAccounts_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgBankAccounts_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindBankAccounts(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtAccountName.Text = string.Empty;
            txtBankBranchName.Text = string.Empty;
            txtAccount.Text = string.Empty;

            ddlAccountType.ClearSelection();
            comboxCompany.ClearSelection();

            BindBankAccounts(true);
        }

        #endregion
    }
}