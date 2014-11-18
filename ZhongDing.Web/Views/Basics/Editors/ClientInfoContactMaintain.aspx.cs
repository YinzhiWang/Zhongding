using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;

namespace ZhongDing.Web.Views.Basics.Editors
{
    public partial class ClientInfoContactMaintain : BasePage
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

        private IClientInfoRepository _PageClientInfoRepository;
        private IClientInfoRepository PageClientInfoRepository
        {
            get
            {
                if (_PageClientInfoRepository == null)
                    _PageClientInfoRepository = new ClientInfoRepository();

                return _PageClientInfoRepository;
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
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR);

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
                var clientInfo = PageClientInfoRepository.GetByID(this.OwnerEntityID);

                if (clientInfo != null)
                {
                    var clientInfoContact = clientInfo.ClientInfoContact
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

            ClientInfoContact contact = null;

            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var clientInfo = PageClientInfoRepository.GetByID(this.OwnerEntityID);

                if (clientInfo != null)
                {
                    contact = clientInfo.ClientInfoContact
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (contact == null)
                    {
                        contact = new ClientInfoContact();

                        clientInfo.ClientInfoContact.Add(contact);
                    }

                    contact.ContactName = txtContactName.Text.Trim();
                    contact.PhoneNumber = txtPhoneNumber.Text.Trim();
                    contact.Address = txtAddress.Text.Trim();
                    contact.Comment = txtComment.Text.Trim();

                    clientInfo.LastModifiedOn = DateTime.Now;
                    clientInfo.LastModifiedBy = CurrentUser.UserID;

                    PageClientInfoRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);

                }
            }
        }
    }
}