using System;
using System.Collections;
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
using ZhongDing.Web.Extensions;
using ZhongDing.Common.Extension;
using System.Text;

namespace ZhongDing.Web.Views.Sales.Editors
{
    public partial class GuaranteeLogManagement : WorkflowBasePage
    {
        protected override EPermission PagePermissionID()
        {
            return EPermission.GuaranteeReceiptManagement;
        }


        #region Members

        private IClientSaleApplicationRepository _PageClientSalesAppRepository;
        private IClientSaleApplicationRepository PageClientSalesAppRepository
        {
            get
            {
                if (_PageClientSalesAppRepository == null)
                    _PageClientSalesAppRepository = new ClientSaleApplicationRepository();

                return _PageClientSalesAppRepository;
            }
        }

        private IClientUserRepository _PageClientUserRepository;
        private IClientUserRepository PageClientUserRepository
        {
            get
            {
                if (_PageClientUserRepository == null)
                    _PageClientUserRepository = new ClientUserRepository();

                return _PageClientUserRepository;
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


        private IGuaranteeReceiptRepository _PageGuaranteeReceiptRepository;
        private IGuaranteeReceiptRepository PageGuaranteeReceiptRepository
        {
            get
            {
                if (_PageGuaranteeReceiptRepository == null)
                    _PageGuaranteeReceiptRepository = new GuaranteeReceiptRepository();

                return _PageGuaranteeReceiptRepository;
            }
        }


        private IGuaranteeLogRepository _PageGuaranteeLogRepository;
        private IGuaranteeLogRepository PageGuaranteeLogRepository
        {
            get
            {
                if (_PageGuaranteeLogRepository == null)
                    _PageGuaranteeLogRepository = new GuaranteeLogRepository();

                return _PageGuaranteeLogRepository;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.GuaranteeReceiptManagement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
       
            if (!IsPostBack)
            {

            }
        }

        #region Private Methods



        #endregion

        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }


        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchGuaranteeReceipt
            {
                GuaranteeReceiptID = this.CurrentEntityID
            };

            int totalRecords;

            var uiEntities = PageGuaranteeLogRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;
            rgEntities.DataSource = uiEntities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                //GridCommandItem commandItem = e.Item as GridCommandItem;
                //Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                //if (plAddCommand != null)
                //{
                //    if (this.CanAddUserIDs.Contains(CurrentUser.UserID))
                //        plAddCommand.Visible = true;
                //    else
                //        plAddCommand.Visible = false;
                //}
            }
        }

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            //if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || CanEditUserIDs.Contains(CurrentUser.UserID))
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;

            //if (this.CanStopUserIDs.Contains(CurrentUser.UserID))
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = false;
        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                //GridDataItem gridDataItem = e.Item as GridDataItem;
                //var uiEntity = (UIClientSaleApplication)gridDataItem.DataItem;


            }
        }

        protected void rgEntities_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

        }

        protected void rgEntities_ItemCommand(object sender, GridCommandEventArgs e)
        {
        }



    }
}