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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Imports
{
    public partial class ImportDCInventoryData : BasePage
    {

        #region Members

        private IDCImportFileLogRepository _PageDCImportFileLogRepository;
        private IDCImportFileLogRepository PageDCImportFileLogRepository
        {
            get
            {
                if (_PageDCImportFileLogRepository == null)
                    _PageDCImportFileLogRepository = new DCImportFileLogRepository();

                return _PageDCImportFileLogRepository;
            }
        }

        private IDistributionCompanyRepository _PageDistributionCompanyRepository;
        private IDistributionCompanyRepository PageDistributionCompanyRepository
        {
            get
            {
                if (_PageDistributionCompanyRepository == null)
                    _PageDistributionCompanyRepository = new DistributionCompanyRepository();

                return _PageDistributionCompanyRepository;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DCInventoryData;

            if (!IsPostBack)
            {
                BindDistributionCompanies();
            }
        }

        #region Private Methods

        private void BindDistributionCompanies()
        {
            var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems();

            rcbxDistributionCompany.DataSource = distributionCompanies;
            rcbxDistributionCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDistributionCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDistributionCompany.DataBind();

            rcbxDistributionCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchDCImportFileLog()
            {
                ImportDataTypeID = (int)EImportDataType.DCInventoryData,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate
            };

            if (rdpSettlementDate.SelectedDate.HasValue)
            {
                uiSearchObj.SettlementDate = rdpSettlementDate.SelectedDate;
            }

            if (!string.IsNullOrEmpty(rcbxDistributionCompany.SelectedValue))
            {
                int distributionCompanyID;
                if (int.TryParse(rcbxDistributionCompany.SelectedValue, out distributionCompanyID))
                    uiSearchObj.DistributionCompanyID = distributionCompanyID;
            }

            int totalRecords;

            var entities = PageDCImportFileLogRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpSettlementDate.Clear();

            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            rcbxDistributionCompany.ClearSelection();

            BindEntities(true);
        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.DataImport;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Create;
        }
    }
}