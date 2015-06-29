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

namespace ZhongDing.Web.Views.Basics.Editors
{
    public partial class SupplierContactMaintain : BasePage
    {
        #region Fields

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

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                    _PageSupplierRepository = new SupplierRepository();

                return _PageSupplierRepository;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!this.OwnerEntityID.HasValue
                || this.OwnerEntityID <= 0))
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                LoadClientContact();
            }
        }

        private void LoadClientContact()
        {
            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var clientInfo = PageSupplierRepository.GetByID(this.OwnerEntityID);

                if (clientInfo != null)
                {
                    var clientInfoContact = clientInfo.SupplierContact
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (clientInfoContact != null)
                    {
                        txtContactName.Text = clientInfoContact.ContactName;
                        txtPhoneNumber.Text = clientInfoContact.PhoneNumber;
                        txtAddress.Text = clientInfoContact.Address;
                        txtComment.Text = clientInfoContact.Comment;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            SupplierContact contact = null;

            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var clientInfo = PageSupplierRepository.GetByID(this.OwnerEntityID);

                if (clientInfo != null)
                {
                    contact = clientInfo.SupplierContact
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (contact == null)
                    {
                        contact = new SupplierContact();

                        clientInfo.SupplierContact.Add(contact);
                    }

                    contact.ContactName = txtContactName.Text.Trim();
                    contact.PhoneNumber = txtPhoneNumber.Text.Trim();
                    contact.Address = txtAddress.Text.Trim();
                    contact.Comment = txtComment.Text.Trim();

                    clientInfo.LastModifiedOn = DateTime.Now;
                    clientInfo.LastModifiedBy = CurrentUser.UserID;

                    PageSupplierRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);

                }
            }
        }


        protected override EPermission PagePermissionID()
        {
            return EPermission.SupplierManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}