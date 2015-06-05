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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class ReimbursementTypeManagement : BasePage
    {
        #region Members

        private IReimbursementTypeRepository _PageReimbursementTypeRepository;
        private IReimbursementTypeRepository PageReimbursementTypeRepository
        {
            get
            {
                if (_PageReimbursementTypeRepository == null)
                    _PageReimbursementTypeRepository = new ReimbursementTypeRepository();

                return _PageReimbursementTypeRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ReimbursementTypeManage;

        }

        private void BindReimbursementTypes(bool isNeedRebind)
        {
            UISearchReimbursementType uiSearchObj = new UISearchReimbursementType()
            {
                //CompanyID = CurrentUser.CompanyID,
                //ReimbursementTypeCode = txtSerialNo.Text.Trim(),
                Name = txtName.Text.Trim(),
                OnlyParent = true
            };

            int totalRecords;

            var uiReimbursementTypes = PageReimbursementTypeRepository.GetUIList(uiSearchObj, rgReimbursementTypes.CurrentPageIndex, rgReimbursementTypes.PageSize, out totalRecords);

            rgReimbursementTypes.VirtualItemCount = totalRecords;

            rgReimbursementTypes.DataSource = uiReimbursementTypes;

            if (isNeedRebind)
                rgReimbursementTypes.Rebind();
        }


        protected void rgReimbursementTypes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindReimbursementTypes(false);
        }

        protected void rgReimbursementTypes_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {

                PageReimbursementTypeRepository.DeleteByID(id);
                PageReimbursementTypeRepository.Save();
            }

            rgReimbursementTypes.Rebind();
        }

        protected void rgReimbursementTypes_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgReimbursementTypes_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgReimbursementTypes_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindReimbursementTypes(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            txtName.Text = string.Empty;

            BindReimbursementTypes(true);
        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.ReimbursementTypeManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}