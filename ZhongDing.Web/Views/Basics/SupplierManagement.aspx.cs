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
    public partial class SupplierManagement : BasePage
    {
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

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierManage;
        }

        #region Private Methods

        private void BindSuppliers(bool isNeedRebind)
        {
            UISearchSupplier uiSearchObj = new UISearchSupplier()
            {
                SupplierCode = txtSupplierCode.Text.Trim(),
                SupplierName = txtSupplierName.Text.Trim(),
                CompanyID = CurrentUser.CompanyID
            };

            int totalRecords;

            var companies = PageSupplierRepository.GetUIList(uiSearchObj, rgSuppliers.CurrentPageIndex, rgSuppliers.PageSize, out totalRecords);

            rgSuppliers.VirtualItemCount = totalRecords;

            rgSuppliers.DataSource = companies;

            if (isNeedRebind)
                rgSuppliers.Rebind();
        }

        #endregion

        protected void rgSuppliers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindSuppliers(false);
        }

        protected void rgSuppliers_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    ISupplierRepository supplierRepository = new SupplierRepository();
                    ISupplierBankAccountRepository supplierBankAccountRepository = new SupplierBankAccountRepository();
                    ISupplierCertificateRepository supplierCertificateRepository = new SupplierCertificateRepository();
                    ISupplierContractFileRepository supplierContractFileRepository = new SupplierContractFileRepository();
                    ISupplierContractRepository supplierContractRepository = new SupplierContractRepository();
                    ISupplierTaskAssignmentRepository supplierTaskAssignmentRepository = new SupplierTaskAssignmentRepository();
                    IBankAccountRepository bankAccountRepository = new BankAccountRepository();
                    ICertificateRepository certificateRepository = new CertificateRepository();

                    supplierRepository.SetDbModel(db);
                    supplierBankAccountRepository.SetDbModel(db);
                    supplierCertificateRepository.SetDbModel(db);
                    supplierContractRepository.SetDbModel(db);
                    supplierContractFileRepository.SetDbModel(db);
                    supplierTaskAssignmentRepository.SetDbModel(db);
                    bankAccountRepository.SetDbModel(db);
                    certificateRepository.SetDbModel(db);

                    var supplier = supplierRepository.GetByID(id);

                    if (supplier != null)
                    {
                        foreach (var supplierBankAccount in supplier.SupplierBankAccount)
                        {
                            if (supplierBankAccount.BankAccount != null)
                                bankAccountRepository.Delete(supplierBankAccount.BankAccount);

                            supplierBankAccountRepository.Delete(supplierBankAccount);
                        }

                        foreach (var supplierCertificate in supplier.SupplierCertificate)
                        {
                            if (supplierCertificate.Certificate != null)
                                certificateRepository.Delete(supplierCertificate.Certificate);

                            supplierCertificateRepository.Delete(supplierCertificate);
                        }

                        foreach (var contract in supplier.SupplierContract)
                        {
                            foreach (var contractFile in contract.SupplierContractFile)
                            {
                                supplierContractFileRepository.Delete(contractFile);
                            }

                            foreach (var taskAssignment in contract.SupplierTaskAssignment)
                            {
                                supplierTaskAssignmentRepository.Delete(taskAssignment);
                            }

                            supplierContractRepository.Delete(contract);
                        }

                        supplierRepository.Delete(supplier);
                    }

                    unitOfWork.SaveChanges();
                }
            }

            rgSuppliers.Rebind();
        }

        protected void rgSuppliers_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgSuppliers_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgSuppliers_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSuppliers(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSupplierCode.Text = string.Empty;
            txtSupplierName.Text = string.Empty;

            BindSuppliers(true);
        }
    }
}