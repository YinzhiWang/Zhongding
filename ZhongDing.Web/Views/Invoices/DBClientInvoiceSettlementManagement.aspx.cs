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
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Invoices
{
    public partial class DBClientInvoiceSettlementManagement : WorkflowBasePage
    {
        #region Members

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

        private IDBClientInvoiceSettlementRepository _PageDBClientInvoiceSettlementRepository;
        private IDBClientInvoiceSettlementRepository PageDBClientInvoiceSettlementRepository
        {
            get
            {
                if (_PageDBClientInvoiceSettlementRepository == null)
                    _PageDBClientInvoiceSettlementRepository = new DBClientInvoiceSettlementRepository();

                return _PageDBClientInvoiceSettlementRepository;
            }
        }

        private IList<int> _CanAddUserIDs;
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewDBClientInvoiceSettlement);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditDBClientInvoiceSettlement);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.DBClientInvoiceSettlement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBClientInvoiceSettlementManage;

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
            var uiSearchObj = new UISearchDBClientInvoiceSettlement()
            {
                CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                ExcludeCanceled = true,
            };

            if (!string.IsNullOrEmpty(rcbxDistributionCompany.SelectedValue))
            {
                uiSearchObj.DistributionCompanyID = Convert.ToInt32(rcbxDistributionCompany.SelectedValue);
            }

            int totalRecords;

            var uiEntities = PageDBClientInvoiceSettlementRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;
            rgEntities.DataSource = uiEntities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CanAddUserIDs.Contains(CurrentUser.UserID))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIDBClientInvoiceSettlement)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    string linkHtml = "<a href=\"javascript:void(0);\" onclick=\"redirectToMaintenancePage(" + uiEntity.ID + ")\">";

                    if (CanEditUserIDs.Contains(CurrentUser.UserID)
                        || CanAddUserIDs.Contains(CurrentUser.UserID))
                        linkHtml += "撤销";
                    else
                        linkHtml += "查看";

                    linkHtml += "</a>";

                    var editColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            rcbxDistributionCompany.ClearSelection();

            BindEntities(true);
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.InvoiceManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}