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
    public partial class DistributionCompanyManagement : BasePage
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DistributionCompanyManage;

        }

        private void BindEntities(bool isNeedRebind)
        {
            UISearchDistributionCompany uiSearchObj = new UISearchDistributionCompany()
            {
                SerialNo = txtSerialNo.Text.Trim(),
                Name = txtName.Text.Trim()
            };

            int totalRecords;

            var uiEntities = PageDistributionCompanyRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;
            rgEntities.DataSource = uiEntities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {

                PageDistributionCompanyRepository.DeleteByID(id);
                PageDistributionCompanyRepository.Save();
            }

            rgEntities.Rebind();
        }

        protected void rgEntities_ItemCreated(object sender, GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgEntities_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgEntities_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSerialNo.Text = string.Empty;
            txtName.Text = string.Empty;

            BindEntities(true);
        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.DistributionCompanyManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}