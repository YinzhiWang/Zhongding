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
    public partial class CertificateMaintain : BasePage
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
        /// 供应商ID
        /// </summary>
        /// <value>The supplier ID.</value>
        private int? SupplierID
        {
            get
            {
                string sSupplierID = Request.QueryString["SupplierID"];

                int iSupplierID;

                if (int.TryParse(sSupplierID, out iSupplierID))
                    return iSupplierID;
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

        private ICertificateTypeRepository _PageCertificateTypeRepository;
        private ICertificateTypeRepository PageCertificateTypeRepository
        {
            get
            {
                if (_PageCertificateTypeRepository == null)
                    _PageCertificateTypeRepository = new CertificateTypeRepository();

                return _PageCertificateTypeRepository;
            }
        }

        private ISupplierCertificateRepository _PageSupplierCertificateRepository;
        private ISupplierCertificateRepository PageSupplierCertificateRepository
        {
            get
            {
                if (_PageSupplierCertificateRepository == null)
                    _PageSupplierCertificateRepository = new SupplierCertificateRepository();

                return _PageSupplierCertificateRepository;
            }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!this.SupplierID.HasValue
                || this.SupplierID <= 0)
                || (!this.OwnerTypeID.HasValue
                || this.OwnerTypeID <= 0))
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                BindCertificateTypes();

                LoadCertificate();
            }
        }

        private void BindCertificateTypes()
        {
            var certificateTypes = PageCertificateTypeRepository.GetUIList(this.OwnerTypeID);

            rcbxCertificateType.DataSource = certificateTypes;
            rcbxCertificateType.DataTextField = "CertificateTypeName";
            rcbxCertificateType.DataValueField = "ID";
            rcbxCertificateType.DataBind();
        }

        private void LoadCertificate()
        {
            EOwnerType ownerType = (EOwnerType)OwnerTypeID.Value;

            switch (ownerType)
            {
                case EOwnerType.Supplier:
                case EOwnerType.Producer:
                    if (this.SupplierID.HasValue
                        && this.SupplierID > 0)
                    {
                        var supplier = PageSupplierRepository.GetByID(this.SupplierID);

                        if (supplier != null)
                        {
                            var certificate = supplier.SupplierCertificate
                                  .Where(x => x.ID == this.CurrentEntityID)
                                  .Select(x => x.Certificate).FirstOrDefault();

                            if (certificate != null)
                            {
                                if (certificate.CertificateTypeID.HasValue)
                                    rcbxCertificateType.SelectedValue = certificate.CertificateTypeID.ToString();

                                if (certificate.IsGotten.HasValue)
                                {
                                    if (certificate.IsGotten.Value)
                                        radioIsGotten.Checked = true;
                                    else
                                        radioIsNoGotten.Checked = true;
                                }

                                rdpEffectiveFrom.SelectedDate = certificate.EffectiveFrom;
                                rdpEffectiveTo.SelectedDate = certificate.EffectiveTo;

                                cbxIsNeedAlert.Checked = certificate.IsNeedAlert.HasValue ? certificate.IsNeedAlert.Value : false;
                                txtAlertBeforeDays.Value = (double?)certificate.AlertBeforeDays;

                                txtComment.Text = certificate.Comment;
                            }
                        }

                    }

                    break;

                case EOwnerType.Product:
                    break;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cbxIsNeedAlert.Checked)
            {
                if (string.IsNullOrEmpty(txtAlertBeforeDays.Text))
                    cvAlertBeforeDays.IsValid = false;

                if (!rdpEffectiveFrom.SelectedDate.HasValue)
                    cvRequiredEffectiveFrom.IsValid = false;

                if (!rdpEffectiveTo.SelectedDate.HasValue)
                    cvRequiredEffectiveTo.IsValid = false;
            }

            if (!IsValid) return;

            bool isSuccessSaved = false;

            EOwnerType ownerType = (EOwnerType)OwnerTypeID.Value;

            switch (ownerType)
            {
                //供应商证照（生产企业和供货商）
                case EOwnerType.Supplier:
                case EOwnerType.Producer:

                    SupplierCertificate supplierCertificate = null;

                    if (this.SupplierID.HasValue
                        && this.SupplierID > 0)
                    {
                        var supplier = PageSupplierRepository.GetByID(this.SupplierID);

                        if (supplier != null)
                            supplierCertificate = supplier.SupplierCertificate
                                .Where(x => x.ID == this.CurrentEntityID && x.Certificate != null && x.Certificate.OwnerTypeID == this.OwnerTypeID).FirstOrDefault();

                        if (supplierCertificate == null)
                        {
                            Certificate certificate = new Certificate() { OwnerTypeID = this.OwnerTypeID, };

                            supplierCertificate = new SupplierCertificate() { Certificate = certificate };

                            supplier.SupplierCertificate.Add(supplierCertificate);
                        }

                        supplierCertificate.Certificate.CertificateTypeID = Convert.ToInt32(rcbxCertificateType.SelectedValue);

                        if (radioIsGotten.Checked)
                            supplierCertificate.Certificate.IsGotten = true;
                        else if (radioIsNoGotten.Checked)
                            supplierCertificate.Certificate.IsGotten = false;

                        supplierCertificate.Certificate.EffectiveFrom = rdpEffectiveFrom.SelectedDate;
                        supplierCertificate.Certificate.EffectiveTo = rdpEffectiveTo.SelectedDate;
                        supplierCertificate.Certificate.IsNeedAlert = cbxIsNeedAlert.Checked;

                        if (cbxIsNeedAlert.Checked)
                            supplierCertificate.Certificate.AlertBeforeDays = Convert.ToInt32(txtAlertBeforeDays.Value);
                        else
                            supplierCertificate.Certificate.AlertBeforeDays = null;

                        supplierCertificate.Certificate.Comment = txtComment.Text.Trim();

                        PageSupplierRepository.Save();

                        isSuccessSaved = true;
                    }

                    break;

                //货品证照
                case EOwnerType.Product:

                    break;
            }

            if (isSuccessSaved)
            {
                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
            }
        }

        protected void cvCompany_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rdpEffectiveFrom.SelectedDate.HasValue
                && rdpEffectiveTo.SelectedDate.HasValue)
            {
                if (rdpEffectiveFrom.SelectedDate.Value
                    > rdpEffectiveTo.SelectedDate.Value)
                {
                    args.IsValid = false;
                }
            }
        }
    }
}