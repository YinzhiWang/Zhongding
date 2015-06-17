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
using ZhongDing.Common;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Basics
{
    public partial class FixedAssetsManagement : BasePage
    {
        #region Members

        private IFixedAssetsRepository _PageFixedAssetsRepository;
        private IFixedAssetsRepository PageFixedAssetsRepository
        {
            get
            {
                if (_PageFixedAssetsRepository == null)
                    _PageFixedAssetsRepository = new FixedAssetsRepository();

                return _PageFixedAssetsRepository;
            }
        }
        private IStorageLocationRepository _PageStorageLocationRepository;
        private IStorageLocationRepository PageStorageLocationRepository
        {
            get
            {
                if (_PageStorageLocationRepository == null)
                    _PageStorageLocationRepository = new StorageLocationRepository();

                return _PageStorageLocationRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.FixedAssetsManage;

        }

        private void BindFixedAssets(bool isNeedRebind)
        {
            UISearchFixedAssets uiSearchObj = new UISearchFixedAssets()
            {
                Name = txtName.Text.Trim()
            };

            int totalRecords = 0;

            var uiFixedAssetss = PageFixedAssetsRepository.GetUIList(uiSearchObj, rgFixedAssetss.CurrentPageIndex, rgFixedAssetss.PageSize, out totalRecords);

            rgFixedAssetss.VirtualItemCount = totalRecords;

            rgFixedAssetss.DataSource = uiFixedAssetss;

            if (isNeedRebind)
                rgFixedAssetss.Rebind();
        }


        protected void rgFixedAssetss_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindFixedAssets(false);
        }

        protected void rgFixedAssetss_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {

                PageFixedAssetsRepository.DeleteByID(id);
                PageFixedAssetsRepository.Save();
            }

            rgFixedAssetss.Rebind();
        }

        protected void rgFixedAssetss_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgFixedAssetss_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgFixedAssetss_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindFixedAssets(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            txtName.Text = string.Empty;

            BindFixedAssets(true);
        }





        #region rgStorageLocations

        protected void rgStorageLocations_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchStorageLocation
           {

           };

            int totalRecords;

            var storageLocations = PageStorageLocationRepository.GetUIList(uiSearchObj, rgStorageLocations.CurrentPageIndex, rgStorageLocations.PageSize, out totalRecords);

            rgStorageLocations.DataSource = storageLocations;
            rgStorageLocations.VirtualItemCount = totalRecords;

        }
        protected void rgStorageLocations_ItemDataBound(object sender, GridItemEventArgs e)
        {


            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UIStorageLocation uiEntity = null;
                if (e.Item.ItemIndex >= 0)
                    uiEntity = (UIStorageLocation)gridDataItem.DataItem;

                var txtName = (RadTextBox)e.Item.FindControl("txtName");
                if (txtName != null && uiEntity != null)
                    txtName.Text = uiEntity.Name;

                var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                if (txtComment != null && uiEntity != null)
                    txtComment.Text = uiEntity.Comment;

            }
        }

        protected void rgStorageLocations_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {

                StorageLocation storageLocation = new StorageLocation();
                var txtName = (RadTextBox)e.Item.FindControl("txtName");
                if (txtName != null)
                    storageLocation.Name = txtName.Text.Trim();



                if (PageStorageLocationRepository.GetList(x => x.Name == storageLocation.Name).Count() > 0)
                {
                    var rfvName = ((RequiredFieldValidator)e.Item.FindControl("rfvName"));
                    rfvName.IsValid = false;
                    rfvName.ErrorMessage = "地点已存在";
                    e.Canceled = true;
                    return;
                }

                var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                if (txtComment != null)
                    storageLocation.Comment = txtComment.Text.Trim();

                PageStorageLocationRepository.Add(storageLocation);
                PageStorageLocationRepository.Save();

            }

            rgStorageLocations.Rebind();


        }

        protected void rgStorageLocations_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (id.BiggerThanZero())
            {
                StorageLocation storageLocation = PageStorageLocationRepository.GetByID(id);
                var txtName = (RadTextBox)e.Item.FindControl("txtName");
                if (txtName != null)
                    storageLocation.Name = txtName.Text.Trim();


                if (PageStorageLocationRepository.GetList(x => x.Name == storageLocation.Name && x.ID != id).Count() > 0)
                {
                    var rfvName = ((RequiredFieldValidator)e.Item.FindControl("rfvName"));
                    rfvName.IsValid = false;
                    rfvName.ErrorMessage = "地点已存在";
                    e.Canceled = true;
                    return;
                }

                var txtComment = (RadTextBox)e.Item.FindControl("txtComment");
                if (txtComment != null)
                    storageLocation.Comment = txtComment.Text.Trim();
                PageStorageLocationRepository.Save();

            }
            rgStorageLocations.Rebind();
        }

        protected void rgStorageLocations_ItemCreated(object sender, GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgStorageLocations_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgStorageLocations_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (id.BiggerThanZero())
            {
                PageStorageLocationRepository.DeleteByID(id);
                PageStorageLocationRepository.Save();
                rgStorageLocations.Rebind();
            }

        }

        #endregion




        protected override EPermission PagePermissionID()
        {
            return EPermission.FixedAssetsManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}