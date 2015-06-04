using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Business.Repositories.Reports;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class CompanyManagement : BasePage
    {
        #region Members

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

        private IOwnerTypeRepository _PageOwnerTypeRepository;
        private IOwnerTypeRepository PageOwnerTypeRepository
        {
            get
            {
                if (_PageOwnerTypeRepository == null)
                {
                    _PageOwnerTypeRepository = new OwnerTypeRepository();
                }

                return _PageOwnerTypeRepository;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.CompanyManage;

            //报表测试代码
            //IReportRepository reportRepository = new ReportRepository();
            //var companies = reportRepository.GetCompanyReport();

            //Unit of work 测试代码
            //IUnitOfWork unitOfWork = new UnitOfWork();

            //DbModelContainer db = unitOfWork.GetDbModel();

            //PageCompanyRepository.SetDbModel(db);
            //PageOwnerTypeRepository.SetDbModel(db);

            //Company company = new Company()
            //{
            //    CompanyCode = "ZT00002",
            //    CompanyName = "测试账套2",
            //    ProviderTexRatio = (decimal)0.18,
            //    ClientTaxHighRatio = (decimal)0.13,
            //    ClientTaxLowRatio = (decimal)0.11,
            //    EnableTaxDeduction = true,
            //    ClientTaxDeductionRatio = (decimal)0.17
            //};
            //PageCompanyRepository.Add(company);

            //OwnerType ownerType = new OwnerType() { OwnerTypeName = "生产企业" };
            //PageOwnerTypeRepository.Add(ownerType);

            //unitOfWork.SaveChanges();

        }

        #region Private Methods

        private void BindCompanies(bool isNeedRebind)
        {
            UISearchCompany uiSearchObj = new UISearchCompany()
            {
                CompanyCode = txtCompanyCode.Text.Trim(),
                CompanyName = txtCompanyName.Text.Trim()
            };

            int totalRecords;

            var companies = PageCompanyRepository.GetUIList(uiSearchObj, rgCompanies.CurrentPageIndex, rgCompanies.PageSize, out totalRecords);

            rgCompanies.VirtualItemCount = totalRecords;

            rgCompanies.DataSource = companies;

            if (isNeedRebind)
                rgCompanies.Rebind();
        }

        #endregion

        #region Events

        protected void rgCompanies_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindCompanies(false);
        }

        protected void rgCompanies_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageCompanyRepository.DeleteByID(id);
                PageCompanyRepository.Save();
            }

            rgCompanies.Rebind();
        }
        protected void rgCompanies_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }


        protected void rgCompanies_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgCompanies_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindCompanies(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtCompanyCode.Text = string.Empty;
            txtCompanyName.Text = string.Empty;

            BindCompanies(true);
        }

        #endregion

        protected override EPermission PagePermissionID()
        {
            return EPermission.CompanyManagement;
        }
        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}