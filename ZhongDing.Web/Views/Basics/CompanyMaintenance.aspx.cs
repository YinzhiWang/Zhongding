using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Basics
{
    public partial class CompanyMaintenance : System.Web.UI.Page
    {
        #region Members

        public int? CompanyID
        {
            get
            {
                string sCompanyID = Request.QueryString["CompanyID"];

                int iCompanyID;

                if (int.TryParse(sCompanyID, out iCompanyID))
                    return iCompanyID;
                else
                    return null;
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
            this.Master.MenuItemID = 3;

            if (!IsPostBack)
            {
                LoadCompany();
            }

        }

        private void LoadCompany()
        {
            if (this.CompanyID.HasValue
                && this.CompanyID > 0)
            {
                var company = PageCompanyRepository.GetByID(this.CompanyID);

                if (company != null)
                {
                    txtCompanyCode.Text = company.CompanyCode;
                    txtCompanyName.Text = company.CompanyName;
                    txtProviderTexRatio.DbValue = company.ProviderTexRatio;
                    txtClientTaxHighRatio.DbValue = company.ClientTaxHighRatio;
                    txtClientTaxLowRatio.DbValue = company.ClientTaxLowRatio;
                    cbxEnableTaxDeduction.Checked = company.EnableTaxDeduction.HasValue
                        ? company.EnableTaxDeduction.Value : false;
                    txtClientTaxDeductionRatio.DbValue = company.ClientTaxDeductionRatio;
                }
            }
            else
            {
                btnDelete.Visible = false;
                txtCompanyCode.Text = Utility.GenerateAutoSerialNo(PageCompanyRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.COMPANY);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            Company company = null;

            if (this.CompanyID.HasValue
                && this.CompanyID > 0)
                company = PageCompanyRepository.GetByID(this.CompanyID);
            else
            {
                company = new Company();
                company.CompanyCode = Utility.GenerateAutoSerialNo(PageCompanyRepository.GetMaxEntityID(),
                    GlobalConst.EntityAutoSerialNo.SerialNoPrefix.COMPANY);

                PageCompanyRepository.Add(company);
            }

            if (company != null)
            {
                company.CompanyName = txtCompanyName.Text;
                company.ProviderTexRatio = Convert.ToDecimal(txtProviderTexRatio.DbValue);
                company.ClientTaxHighRatio = Convert.ToDecimal(txtClientTaxHighRatio.DbValue);
                company.ClientTaxLowRatio = Convert.ToDecimal(txtClientTaxLowRatio.DbValue);
                company.EnableTaxDeduction = cbxEnableTaxDeduction.Checked;
                company.ClientTaxDeductionRatio = Convert.ToDecimal(txtClientTaxDeductionRatio.DbValue);

                PageCompanyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CompanyID.HasValue
                   && this.CompanyID > 0)
            {
                PageCompanyRepository.DeleteByID(this.CompanyID);
                PageCompanyRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }

        protected void cvCompanyName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageCompanyRepository.GetList(x => x.ID != this.CompanyID
                && x.CompanyName.Equals(txtCompanyName.Text.Trim())).Count() > 0)
            {
                args.IsValid = false;
            }
        }
    }
}