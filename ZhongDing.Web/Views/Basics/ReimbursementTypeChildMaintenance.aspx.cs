using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Basics
{
    public partial class ReimbursementTypeChildMaintenance : BasePage
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
            this.Master.MenuItemID = (int)EMenuItem.ReimbursementTypeManage;

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
                var currentEntity = PageReimbursementTypeRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    txtName.Text = currentEntity.Name;
                    txtComment.Text = currentEntity.Comment;

                }
            }
            else
            {
                btnDelete.Visible = false;
            }

        }


        protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageReimbursementTypeRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.Name.Equals(txtName.Text.Trim())).Count() > 0)
            {
                args.IsValid = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            ReimbursementType currentEntity = null;

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
                currentEntity = PageReimbursementTypeRepository.GetByID(this.CurrentEntityID);
            else
            {
                currentEntity = new ReimbursementType();

                PageReimbursementTypeRepository.Add(currentEntity);
            }

            if (currentEntity != null)
            {
                currentEntity.Name = txtName.Text.Trim();
                currentEntity.Comment = txtComment.Text.Trim();
                currentEntity.ParentID = IntParameter("ParentID");
                PageReimbursementTypeRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntityID.HasValue
                   && this.CurrentEntityID > 0)
            {
                PageReimbursementTypeRepository.DeleteByID(this.CurrentEntityID);
                PageReimbursementTypeRepository.Save();

                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_DELETED_REDIRECT);
            }
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