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
    public partial class ClientInfoManagement : BasePage
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientInfoManage;

            if (!IsPostBack)
            {
                BindClientCompanies();
            }
        }

        private void BindClientCompanies()
        {
            var clientCompanies = PageClientCompanyRepository.GetDropdownItems();
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();
        }

        private void BindEntities(bool isNeedRebind)
        {
            UISearchClientInfo uiSearchObj = new UISearchClientInfo()
            {
                ClientCode = txtSerialNo.Text.Trim(),
                ClientName = txtName.Text.Trim()
            };

            if (!string.IsNullOrEmpty(rcbxClientCompany.SelectedValue))
            {
                int clientCompanyID;
                if (int.TryParse(rcbxClientCompany.SelectedValue, out clientCompanyID))
                    uiSearchObj.ClientCompanyID = clientCompanyID;
            }

            int totalRecords;

            var uiEntities = PageClientInfoRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;
            rgEntities.DataSource = uiEntities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
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

                    var clientInfo = clientInfoRepository.GetByID(id);

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
            }

            rgEntities.Rebind();
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSerialNo.Text = string.Empty;
            txtName.Text = string.Empty;
            rcbxClientCompany.ClearSelection();

            BindEntities(true);
        }
    }
}