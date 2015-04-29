using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Invoices
{
    public partial class ClientAttachedInvoiceSettlementMaintenance : WorkflowBasePage
    {
        #region Members
        private IClientAttachedInvoiceSettlementRepository _PageClientAttachedInvoiceSettlementRepository;
        private IClientAttachedInvoiceSettlementRepository PageClientAttachedInvoiceSettlementRepository
        {
            get
            {
                if (_PageClientAttachedInvoiceSettlementRepository == null)
                    _PageClientAttachedInvoiceSettlementRepository = new ClientAttachedInvoiceSettlementRepository();

                return _PageClientAttachedInvoiceSettlementRepository;
            }
        }

        private IClientAttachedInvoiceSettlementDetailRepository _PageClientAttachedInvoiceSDRepository;
        private IClientAttachedInvoiceSettlementDetailRepository PageClientAttachedInvoiceSDRepository
        {
            get
            {
                if (_PageClientAttachedInvoiceSDRepository == null)
                    _PageClientAttachedInvoiceSDRepository = new ClientAttachedInvoiceSettlementDetailRepository();

                return _PageClientAttachedInvoiceSDRepository;
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

        private ClientAttachedInvoiceSettlement _CurrentEntity;
        private ClientAttachedInvoiceSettlement CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageClientAttachedInvoiceSettlementRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        private ICompanyRepository _PageCompanyRepository;
        private ICompanyRepository PageCompanyRepository
        {
            get
            {
                if (_PageCompanyRepository == null)
                    _PageCompanyRepository = new CompanyRepository();

                return _PageCompanyRepository;
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

        private IBankAccountRepository _PageBankAccountRepository;
        private IBankAccountRepository PageBankAccountRepository
        {
            get
            {
                if (_PageBankAccountRepository == null)
                    _PageBankAccountRepository = new BankAccountRepository();

                return _PageBankAccountRepository;
            }
        }

        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null)
                {
                    if (this.CurrentEntity == null)
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewCAISettlement);
                    else
                        _CanAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID(this.CurrentWorkFlowID, this.CurrentEntity.WorkflowStatusID);
                }

                return _CanAccessUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditCAISettlement);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}