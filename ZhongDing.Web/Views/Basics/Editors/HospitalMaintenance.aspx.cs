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

namespace ZhongDing.Web.Views.Basics.Editors
{
    public partial class HospitalMaintenance : BasePage
    {
        #region Fields

        /// <summary>
        /// 所有者类型ID
        /// </summary>
        /// <value>The owner type ID.</value>
        private int? OwnerTypeID
        {
            get
            {
                string sOwnerTypeID = Request.QueryString["OwnerTypeID"];

                int iOwnerTypeID;

                if (int.TryParse(sOwnerTypeID, out iOwnerTypeID))
                    return iOwnerTypeID;
                else
                    return null;
            }
        }

        /// <summary>
        /// 所属的实体ID
        /// </summary>
        /// <value>The owner entity ID.</value>
        private int? OwnerEntityID
        {
            get
            {
                string sOwnerEntityID = Request.QueryString["OwnerEntityID"];

                int iOwnerEntityID;

                if (int.TryParse(sOwnerEntityID, out iOwnerEntityID))
                    return iOwnerEntityID;
                else
                    return null;
            }
        }

        #endregion

        #region Members


        private IHospitalRepository _PageHospitalRepository;
        private IHospitalRepository PageHospitalRepository
        {
            get
            {
                if (_PageHospitalRepository == null)
                {
                    _PageHospitalRepository = new HospitalRepository();
                }

                return _PageHospitalRepository;
            }
        }



        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                LoadEntityData();
            }
        }


        private void LoadEntityData()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageHospitalRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    txtName.Text = currentEntity.HospitalName;
                }
            }
        }


        protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PageHospitalRepository.GetList(x => x.ID != this.CurrentEntityID
                && x.HospitalName == txtName.Text.Trim()).Count() > 0)
            {
                args.IsValid = false;
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            Hospital currentEntity = null;

            if (this.CurrentEntityID.BiggerThanZero())
            {
                currentEntity = PageHospitalRepository.GetByID(this.CurrentEntityID);
            }
            else
            {
                currentEntity = new Hospital();
                currentEntity.HospitalCodeID = this.OwnerEntityID;
                PageHospitalRepository.Add(currentEntity);
            }
            currentEntity.HospitalName = txtName.Text.Trim();

            PageHospitalRepository.Save();
            if (currentEntity != null)
            {
                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_FAILED_SAEVED_CLOSE_WIN);
            }
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