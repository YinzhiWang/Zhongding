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
    public partial class SupplierInvoiceSettlementManagement : WorkflowBasePage
    {
        #region Members

        private ISupplierInvoiceSettlementRepository _PageSupplierInvoiceSettlementRepository;
        private ISupplierInvoiceSettlementRepository PageSupplierInvoiceSettlementRepository
        {
            get
            {
                if (_PageSupplierInvoiceSettlementRepository == null)
                    _PageSupplierInvoiceSettlementRepository = new SupplierInvoiceSettlementRepository();

                return _PageSupplierInvoiceSettlementRepository;
            }
        }

        private IList<int> _CanAddUserIDs;
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewSupplierInvoiceSettlement);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSupplierInvoiceSettlement);

                return _CanEditUserIDs;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierInvoiceSettlementManage;

        }

        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchSupplierInvoiceSettlement()
            {
                CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                ExcludeCanceled = true
            };

            int totalRecords;

            var uiEntities = PageSupplierInvoiceSettlementRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;
            rgEntities.DataSource = uiEntities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

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

        protected void rgEntities_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UISupplierInvoiceSettlement)gridDataItem.DataItem;

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

            BindEntities(true);

        }
    }
}