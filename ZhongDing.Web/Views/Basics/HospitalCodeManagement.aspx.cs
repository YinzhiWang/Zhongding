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
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Basics
{
    public partial class HospitalCodeManagement : BasePage
    {
        #region Members

        private IHospitalCodeRepository _PageHospitalCodeRepository;
        private IHospitalCodeRepository PageHospitalCodeRepository
        {
            get
            {
                if (_PageHospitalCodeRepository == null)
                    _PageHospitalCodeRepository = new HospitalCodeRepository();

                return _PageHospitalCodeRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.HospitalManage;

        }

        private void BindHospitalCodes(bool isNeedRebind)
        {
            UISearchHospitalCode uiSearchObj = new UISearchHospitalCode()
            {
                //CompanyID = CurrentUser.CompanyID,
                //HospitalCodeCode = txtSerialNo.Text.Trim(),
                Name = txtName.Text.Trim(),
                //OnlyParent = true
                
            };

            int totalRecords;

            var uiHospitalCodes = PageHospitalCodeRepository.GetUIList(uiSearchObj, rgHospitalCodes.CurrentPageIndex, rgHospitalCodes.PageSize, out totalRecords);

            rgHospitalCodes.VirtualItemCount = totalRecords;

            rgHospitalCodes.DataSource = uiHospitalCodes;

            if (isNeedRebind)
                rgHospitalCodes.Rebind();
        }


        protected void rgHospitalCodes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindHospitalCodes(false);
        }

        protected void rgHospitalCodes_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();
            if (id.BiggerThanZero())
            {
                PageHospitalCodeRepository.DeleteByID(id);
                PageHospitalCodeRepository.Save();
                rgHospitalCodes.Rebind();
            }
          
        }

        protected void rgHospitalCodes_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgHospitalCodes_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgHospitalCodes_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindHospitalCodes(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            txtName.Text = string.Empty;

            BindHospitalCodes(true);
        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.HospitalManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}