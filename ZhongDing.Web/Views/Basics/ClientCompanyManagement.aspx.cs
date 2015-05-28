using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class ClientCompanyManagement : BasePage
    {

        #region Members

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
            this.Master.MenuItemID = (int)EMenuItem.ClientCompanyManage;
        }

        private void BindEntities(bool isNeedRebind)
        {
            UISearchClientCompany uiSearchObj = new UISearchClientCompany()
            {
                Name = txtName.Text.Trim(),
                District = txtDistrict.Text.Trim()
            };

            int totalRecords;

            var uiEntities = PageClientCompanyRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

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

                    IClientCompanyRepository clientCompanyRepository = new ClientCompanyRepository();
                    IClientCompanyCertificateRepository clientCompanyCertificateRepository = new ClientCompanyCertificateRepository();
                    ICertificateRepository certificateRepository = new CertificateRepository();

                    clientCompanyRepository.SetDbModel(db);
                    clientCompanyCertificateRepository.SetDbModel(db);
                    certificateRepository.SetDbModel(db);

                    var clientCompany = clientCompanyRepository.GetByID(id);

                    if (clientCompany != null)
                    {
                        foreach (var clientCompanyCertificate in clientCompany.ClientCompanyCertificate)
                        {
                            if (clientCompanyCertificate != null && clientCompanyCertificate.Certificate != null)
                                certificateRepository.Delete(clientCompanyCertificate.Certificate);

                            clientCompanyCertificateRepository.Delete(clientCompanyCertificate);
                        }

                        clientCompanyRepository.Delete(clientCompany);
                    }

                    unitOfWork.SaveChanges();
                }
            }

            rgEntities.Rebind();
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
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
            txtName.Text = string.Empty;
            txtDistrict.Text = string.Empty;

            BindEntities(true);
        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.ClientCompanyManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}