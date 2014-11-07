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

namespace ZhongDing.Web.Views.Basics
{
    public partial class SupplierMaintenance : BasePage
    {
        private const string PREFIX_OF_SUPPLIERCODE = "GYS";

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
            }
            else
                txtSupplierCode.Text = Utility.GenerateAutoSerialNo(PageSupplierRepository.GetMaxEntityID(), PREFIX_OF_SUPPLIERCODE);
        }

        protected void cvSupplierName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //args.IsValid = false;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cbxIsProducer.Checked
                && string.IsNullOrEmpty(txtFactoryName.Text.Trim()))
                cvFactoryName.IsValid = false;

            if (!IsValid) return;

            Supplier supplier = null;

            if (this.SupplierID.HasValue
                && this.SupplierID > 0)
                supplier = PageSupplierRepository.GetByID(this.SupplierID);
            else
            {
                supplier = new Supplier();
                supplier.SupplierCode = Utility.GenerateAutoSerialNo(PageSupplierRepository.GetMaxEntityID(), PREFIX_OF_SUPPLIERCODE);

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
                    this.Master.BaseNotification.Show("保存成功，页面将自动跳转");
                }
                else
                {
                    this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                    this.Master.BaseNotification.Show("保存成功，页面将自动刷新");
                }
            }

        }

        protected void rgProducerCertificates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgProducerCertificates.DataSource = PageSupplierRepository.GetCertificates(this.SupplierID, (int)EOwnerType.Producer);
        }

        protected void rgProducerCertificates_DeleteCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void rgSupplierCertificates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgSupplierCertificates.DataSource = PageSupplierRepository.GetCertificates(this.SupplierID, (int)EOwnerType.Supplier);
        }

        protected void rgSupplierCertificates_DeleteCommand(object sender, GridCommandEventArgs e)
        {

        }
    }
}