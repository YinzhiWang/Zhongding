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
    public partial class ClientCompanyMaintenance : BasePage
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

        private ICertificateRepository _PageCertificateRepository;
        private ICertificateRepository PageCertificateRepository
        {
            get
            {
                if (_PageCertificateRepository == null)
                    _PageCertificateRepository = new CertificateRepository();

                return _PageCertificateRepository;
            }
        }

        private ClientCompany _CurrentEntity;
        private ClientCompany CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageClientCompanyRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientCompanyManage;

            //新增时隐藏其他sections
            if (this.CurrentEntity == null)
                divOtherSections.Visible = false;

            if (!IsPostBack)
            {
                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var clientCompany = PageClientCompanyRepository.GetByID(this.CurrentEntityID);

                if (clientCompany != null)
                {
                    hdnCurrentEntityID.Value = clientCompany.ID.ToString();

                    txtName.Text = clientCompany.Name;
                    txtDistrict.Text = clientCompany.District;
                    txtPostalCode.Text = clientCompany.PostalCode;
                    txtAddress.Text = clientCompany.Address;
                }
            }
        }

        #endregion

        protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageClientCompanyRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.Name.Equals(txtName.Text.Trim())).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void rgCertificates_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            UISearchCertificate uiSearchObj = new UISearchCertificate()
            {
                OwnerEntityID = this.CurrentEntityID.Value,
                OwnerTypeID = (int)EOwnerType.Client
            };

            int totalRecords;

            rgCertificates.DataSource = PageCertificateRepository
                .GetUIList(uiSearchObj, rgCertificates.CurrentPageIndex, rgCertificates.PageSize, out totalRecords);

            rgCertificates.VirtualItemCount = totalRecords;
        }

        protected void rgCertificates_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("OwnerEntityID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    ICertificateRepository certificateRepository = new CertificateRepository();
                    IClientCompanyCertificateRepository clientCompanyCertificateRepository = new ClientCompanyCertificateRepository();

                    certificateRepository.SetDbModel(db);
                    clientCompanyCertificateRepository.SetDbModel(db);

                    var clientCompanyCertificate = clientCompanyCertificateRepository.GetByID(id);

                    if (clientCompanyCertificate != null)
                        certificateRepository.Delete(clientCompanyCertificate.Certificate);

                    clientCompanyCertificateRepository.Delete(clientCompanyCertificate);

                    unitOfWork.SaveChanges();
                }
            }

            rgCertificates.Rebind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            ClientCompany clientCompany = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                clientCompany = PageClientCompanyRepository.GetByID(this.CurrentEntityID);
            else
            {
                clientCompany = new ClientCompany();
                PageClientCompanyRepository.Add(clientCompany);
            }

            if (clientCompany != null)
            {
                clientCompany.Name = txtName.Text.Trim();
                clientCompany.District = txtDistrict.Text.Trim();
                clientCompany.PostalCode = txtPostalCode.Text.Trim();
                clientCompany.Address = txtAddress.Text.Trim();

                PageClientCompanyRepository.Save();

                hdnCurrentEntityID.Value = clientCompany.ID.ToString();

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
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
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

                    var clientCompany = clientCompanyRepository.GetByID(this.CurrentEntityID);

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

                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }
    }
}