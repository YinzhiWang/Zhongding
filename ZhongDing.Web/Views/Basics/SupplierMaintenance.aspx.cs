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

namespace ZhongDing.Web.Views.Basics
{
    public partial class SupplierMaintenance : BasePage
    {
        #region Members

        public int? SupplierID
        {
            get
            {
                string sSupplierID = Request.QueryString["SupplierID"];

                int iSupplierID;

                if (int.TryParse(sSupplierID, out iSupplierID))
                    return iSupplierID;
                else
                    return null;
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


        private ISupplierCertificateRepository _PageSupplierCertificateRepository;
        private ISupplierCertificateRepository PageSupplierCertificateRepository
        {
            get
            {
                if (_PageSupplierCertificateRepository == null)
                    _PageSupplierCertificateRepository = new SupplierCertificateRepository();

                return _PageSupplierCertificateRepository;
            }
        }

        private Supplier _PageSupplier;
        private Supplier PageSupplier
        {
            get
            {
                if (_PageSupplier == null)
                    if (this.SupplierID.HasValue && this.SupplierID > 0)
                        _PageSupplier = PageSupplierRepository.GetByID(this.SupplierID);

                return _PageSupplier;
            }
        }

        private ISupplierContractRepository _PageSupplierContractRepository;
        private ISupplierContractRepository PageSupplierContractRepository
        {
            get
            {
                if (_PageSupplierContractRepository == null)
                    _PageSupplierContractRepository = new SupplierContractRepository();

                return _PageSupplierContractRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = 5;

            //新增时隐藏其他sections
            if (!this.SupplierID.HasValue
                || this.SupplierID <= 0)
            {
                divOtherSections.Visible = false;
            }

            if (!IsPostBack)
            {
                LoadSupplier();
            }
        }

        #region Private Methods

        private void LoadSupplier()
        {
            if (this.PageSupplier != null)
            {
                hdnSupplierID.Value = this.PageSupplier.ID.ToString();

                txtSupplierCode.Text = this.PageSupplier.SupplierCode;
                txtSupplierName.Text = this.PageSupplier.SupplierName;
                cbxIsProducer.Checked = this.PageSupplier.IsProducer;
                txtFactoryName.Text = this.PageSupplier.FactoryName;
                txtContactPerson.Text = this.PageSupplier.ContactPerson;
                txtPhoneNumber.Text = this.PageSupplier.PhoneNumber;
                txtFax.Text = this.PageSupplier.Fax;
                txtDistrict.Text = this.PageSupplier.District;
                txtPostalCode.Text = this.PageSupplier.PostalCode;
                txtContactAddress.Text = this.PageSupplier.ContactAddress;

                var tabSupplier = tabStripCertificates.FindTabByValue("tabSupplier", true);

                if (this.PageSupplier.IsProducer)
                {
                    if (tabSupplier != null)
                        tabSupplier.Visible = false;
                }
                else
                {
                    if (tabSupplier != null)
                        tabSupplier.Visible = true;
                }


            }
            else
                txtSupplierCode.Text = Utility.GenerateAutoSerialNo(PageSupplierRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.SUPPLIER);
        }

        /// <summary>
        /// 删除证照
        /// </summary>
        /// <param name="supplierCertificateID">The supplier certificate ID.</param>
        private static void DeleteSupplierCertificate(int supplierCertificateID)
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                DbModelContainer db = unitOfWork.GetDbModel();

                ICertificateRepository certificateRepository = new CertificateRepository();
                ISupplierCertificateRepository supplierCertificateRepository = new SupplierCertificateRepository();

                certificateRepository.SetDbModel(db);
                supplierCertificateRepository.SetDbModel(db);

                var supplierCertificate = supplierCertificateRepository.GetByID(supplierCertificateID);

                if (supplierCertificate != null)
                    certificateRepository.Delete(supplierCertificate.Certificate);

                supplierCertificateRepository.Delete(supplierCertificate);

                unitOfWork.SaveChanges();
            }
        }

        #endregion

        #region Events

        protected void cvSupplierName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //args.IsValid = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            Supplier supplier = null;

            if (this.SupplierID.HasValue
                && this.SupplierID > 0)
                supplier = PageSupplierRepository.GetByID(this.SupplierID);
            else
            {
                supplier = new Supplier();
                supplier.SupplierCode = Utility.GenerateAutoSerialNo(PageSupplierRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.SUPPLIER);

                supplier.CompanyID = CurrentUser.CompanyID;

                PageSupplierRepository.Add(supplier);
            }

            if (supplier != null)
            {
                supplier.SupplierName = txtSupplierName.Text.Trim();

                supplier.IsProducer = cbxIsProducer.Checked;

                if (cbxIsProducer.Checked)
                    supplier.FactoryName = txtFactoryName.Text.Trim();
                else
                    supplier.FactoryName = string.Empty;

                supplier.ContactPerson = txtContactPerson.Text.Trim();
                supplier.PhoneNumber = txtPhoneNumber.Text.Trim();
                supplier.Fax = txtFax.Text.Trim();
                supplier.District = txtDistrict.Text.Trim();
                supplier.PostalCode = txtPostalCode.Text.Trim();
                supplier.ContactAddress = txtContactAddress.Text.Trim();

                PageSupplierRepository.Save();

                hdnSupplierID.Value = supplier.ID.ToString();

                if (this.SupplierID.HasValue
                    && this.SupplierID > 0)
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

        protected void rgBankAccounts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgBankAccounts.DataSource = PageSupplierRepository.GetBankAccounts(this.SupplierID);
        }

        protected void rgBankAccounts_DeleteCommand(object sender, GridCommandEventArgs e)
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
                    ISupplierBankAccountRepository supplierBankAccountRepository = new SupplierBankAccountRepository();

                    bankAccountRepository.SetDbModel(db);
                    supplierBankAccountRepository.SetDbModel(db);

                    var supplierBankAccount = supplierBankAccountRepository.GetByID(id);

                    if (supplierBankAccount != null)
                        bankAccountRepository.Delete(supplierBankAccount.BankAccount);

                    supplierBankAccountRepository.Delete(supplierBankAccount);

                    unitOfWork.SaveChanges();
                }
            }

            rgBankAccounts.Rebind();
        }

        protected void rgProducerCertificates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            UISearchCertificate uiSearchObj = new UISearchCertificate()
            {
                SupplierID = this.SupplierID.Value,
                OwnerTypeID = (int)EOwnerType.Producer
            };

            int totalRecords;

            rgProducerCertificates.DataSource = PageSupplierCertificateRepository
                .GetUIList(uiSearchObj, rgProducerCertificates.CurrentPageIndex, rgProducerCertificates.PageSize, out totalRecords);

            rgProducerCertificates.VirtualItemCount = totalRecords;
        }

        protected void rgProducerCertificates_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                DeleteSupplierCertificate(id);
            }

            rgProducerCertificates.Rebind();
        }

        protected void rgSupplierCertificates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            UISearchCertificate uiSearchObj = new UISearchCertificate()
            {
                SupplierID = this.SupplierID.Value,
                OwnerTypeID = (int)EOwnerType.Supplier
            };

            int totalRecords;

            rgSupplierCertificates.DataSource = PageSupplierCertificateRepository
                .GetUIList(uiSearchObj, rgSupplierCertificates.CurrentPageIndex, rgSupplierCertificates.PageSize, out totalRecords);

            rgSupplierCertificates.VirtualItemCount = totalRecords;
        }

        protected void rgSupplierCertificates_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                DeleteSupplierCertificate(id);
            }

            rgSupplierCertificates.Rebind();
        }

        #endregion

        protected void cbxIsProducer_CheckedChanged(object sender, EventArgs e)
        {
            var tabSupplier = tabStripCertificates.FindTabByValue("tabSupplier", true);

            if (cbxIsProducer.Checked)
            {
                if (tabSupplier != null)
                    tabSupplier.Visible = false;
            }
            else
            {
                if (tabSupplier != null)
                    tabSupplier.Visible = true;
            }
        }

        protected void rgContracts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            UISearchSupplierContract uiSearchObj = new UISearchSupplierContract()
            {
                SupplierID = this.SupplierID.Value
            };

            int totalRecords;

            rgContracts.DataSource = PageSupplierContractRepository.GetUIList(uiSearchObj, rgContracts.CurrentPageIndex, rgContracts.PageSize, out totalRecords);

            rgContracts.VirtualItemCount = totalRecords;
        }

        protected void rgContracts_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    ISupplierContractRepository contractRepository = new SupplierContractRepository();
                    ISupplierContractFileRepository contractFileRepository = new SupplierContractFileRepository();

                    contractRepository.SetDbModel(db);
                    contractFileRepository.SetDbModel(db);

                    var supplierContract = PageSupplierContractRepository.GetByID(id);

                    if (supplierContract != null)
                    {
                        foreach (var contractFile in supplierContract.SupplierContractFile)
                        {
                            contractFileRepository.Delete(contractFile);
                        }
                    }

                    contractRepository.Delete(supplierContract);

                    unitOfWork.SaveChanges();
                }
            }

            rgContracts.Rebind();
        }
    }
}