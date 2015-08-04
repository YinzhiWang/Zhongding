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
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Basics
{
    public partial class HospitalCodeMaintenance : BasePage
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


        private IHospitalRepository _PageHospitalRepository;
        private IHospitalRepository PageHospitalRepository
        {
            get
            {
                if (_PageHospitalRepository == null)
                    _PageHospitalRepository = new HospitalRepository();

                return _PageHospitalRepository;
            }
        }

        private ISaleTypeRepository _PageSaleTypeRepository;
        private ISaleTypeRepository PageSaleTypeRepository
        {
            get
            {
                if (_PageSaleTypeRepository == null)
                    _PageSaleTypeRepository = new SaleTypeRepository();

                return _PageSaleTypeRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.HospitalManage;

            if (!IsPostBack)
            {
                LoadEntityData();
                base.PermissionOptionCheckButtonDelete(btnDelete);
            }

        }


        private void LoadEntityData()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageHospitalCodeRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    txtName.Text = currentEntity.Code;
                    txtComment.Text = currentEntity.Comment;
                }
            }
            else
            {
                rgHospitals.Visible = false;
                btnDelete.Visible = false;
            }

        }


        protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageHospitalCodeRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.Code.Equals(txtName.Text.Trim())).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            HospitalCode currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageHospitalCodeRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new HospitalCode();

                PageHospitalCodeRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.Code = txtName.Text.Trim();
                currentEntity.Comment = txtComment.Text.Trim();
                PageHospitalCodeRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageHospitalCodeRepository.DeleteByID(this.CurrentEntityID);
                PageHospitalCodeRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
        }


        private void BindHospitals(bool isNeedRebind)
        {
            UISearchHospital uiSearchObj = new UISearchHospital()
            {
                //CompanyID = CurrentUser.CompanyID,
                //HospitalCodeCode = txtSerialNo.Text.Trim(),
                //Name = txtName.Text.Trim(),
                HospitalCodeID = this.CurrentEntityID
            };

            int totalRecords;

            var uiHospitalCodes = PageHospitalRepository.GetUIList(uiSearchObj, rgHospitals.CurrentPageIndex, rgHospitals.PageSize, out totalRecords);

            rgHospitals.VirtualItemCount = totalRecords;

            rgHospitals.DataSource = uiHospitalCodes;

            if (isNeedRebind)
                rgHospitals.Rebind();
        }


        protected void rgHospitals_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindHospitals(false);
        }

        protected void rgHospitals_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();
            if (id.BiggerThanZero())
            {
                PageHospitalRepository.DeleteByID(id);
                PageHospitalRepository.Save();
                rgHospitals.Rebind();
            }

           
        }

        protected void rgHospitals_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgHospitals_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgHospitals_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

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