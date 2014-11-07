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

        private string GridClientID
        {
            get
            {
                return Request.QueryString["GridClientID"];
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cbxIsNeedAlert.Checked
                && string.IsNullOrEmpty(txtAlertBeforeDays.Text))
                cvAlertBeforeDays.IsValid = false;

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
                        supplierCertificate.Certificate.IsGotten = cbxIsGotten.Checked;
                        supplierCertificate.Certificate.EffectiveFrom = rdpEffectiveFrom.SelectedDate;
                        supplierCertificate.Certificate.EffectiveTo = rdpEffectiveTo.SelectedDate;
                        supplierCertificate.Certificate.IsNeedAlert = cbxIsNeedAlert.Checked;
                        supplierCertificate.Certificate.AlertBeforeDays = Convert.ToInt32(txtAlertBeforeDays.Value);
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
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED);
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