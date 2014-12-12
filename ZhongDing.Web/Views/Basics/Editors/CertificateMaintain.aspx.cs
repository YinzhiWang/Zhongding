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
        /// 证照所属的实体ID
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

        private IClientCompanyRepository _PageClientCompanyRepository;
        private IClientCompanyRepository PageClientCompanyRepository
        {
            get
            {
                if (_PageClientCompanyRepository == null)
                    _PageClientCompanyRepository = new ClientCompanyRepository();

                return _PageClientCompanyRepository;
            }
        }

        private IClientCompanyCertificateRepository _PageClientCompanyCertificateRepository;
        private IClientCompanyCertificateRepository PageClientCompanyCertificateRepository
        {
            get
            {
                if (_PageClientCompanyCertificateRepository == null)
                    _PageClientCompanyCertificateRepository = new ClientCompanyCertificateRepository();

                return _PageClientCompanyCertificateRepository;
            }
        }

        private IProductRepository _PageProductRepository;
        private IProductRepository PageProductRepository
        {
            get
            {
                if (_PageProductRepository == null)
                    _PageProductRepository = new ProductRepository();

                return _PageProductRepository;
            }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!this.OwnerEntityID.HasValue
                || this.OwnerEntityID <= 0)
                || (!this.OwnerTypeID.HasValue
                || this.OwnerTypeID <= 0))
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

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

            Certificate certificate = null;

            EOwnerType ownerType = (EOwnerType)OwnerTypeID.Value;

            switch (ownerType)
            {
                case EOwnerType.Supplier:
                case EOwnerType.Producer:
                    if (this.OwnerEntityID.HasValue
                        && this.OwnerEntityID > 0)
                    {
                        var supplier = PageSupplierRepository.GetByID(this.OwnerEntityID);

                        if (supplier != null)
                        {
                            certificate = supplier.SupplierCertificate
                                 .Where(x => x.ID == this.CurrentEntityID)
                                 .Select(x => x.Certificate).FirstOrDefault();
                        }
                    }

                    break;

                case EOwnerType.Client:
                    if (this.OwnerEntityID.HasValue
                        && this.OwnerEntityID > 0)
                    {
                        var clientCompany = PageClientCompanyRepository.GetByID(this.OwnerEntityID);

                        if (clientCompany != null)
                        {
                            certificate = clientCompany.ClientCompanyCertificate
                                  .Where(x => x.ID == this.CurrentEntityID)
                                  .Select(x => x.Certificate).FirstOrDefault();
                        }
                    }

                    break;

                case EOwnerType.Product:
                    if (this.OwnerEntityID.HasValue
                        && this.OwnerEntityID > 0)
                    {
                        var product = PageProductRepository.GetByID(this.OwnerEntityID);

                        if (product != null)
                        {
                            certificate = product.ProductCertificate
                                  .Where(x => x.ID == this.CurrentEntityID)
                                  .Select(x => x.Certificate).FirstOrDefault();
                        }
                    }
                    break;
            }

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
                    #region 供应商证照（生产企业和供货商）

                    SupplierCertificate supplierCertificate = null;

                    if (this.OwnerEntityID.HasValue
                        && this.OwnerEntityID > 0)
                    {
                        var supplier = PageSupplierRepository.GetByID(this.OwnerEntityID);

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

                    #endregion

                    break;

                //客户商业单位证照
                case EOwnerType.Client:

                    #region 客户商业单位证照

                    ClientCompanyCertificate clientCompanyCertificate = null;

                    if (this.OwnerEntityID.HasValue
                        && this.OwnerEntityID > 0)
                    {
                        var clientCompany = PageClientCompanyRepository.GetByID(this.OwnerEntityID);

                        if (clientCompany != null)
                            clientCompanyCertificate = clientCompany.ClientCompanyCertificate
                                .Where(x => x.ID == this.CurrentEntityID && x.Certificate != null && x.Certificate.OwnerTypeID == this.OwnerTypeID).FirstOrDefault();

                        if (clientCompanyCertificate == null)
                        {
                            Certificate certificate = new Certificate() { OwnerTypeID = this.OwnerTypeID, };

                            clientCompanyCertificate = new ClientCompanyCertificate() { Certificate = certificate };

                            clientCompany.ClientCompanyCertificate.Add(clientCompanyCertificate);
                        }

                        clientCompanyCertificate.Certificate.CertificateTypeID = Convert.ToInt32(rcbxCertificateType.SelectedValue);

                        if (radioIsGotten.Checked)
                            clientCompanyCertificate.Certificate.IsGotten = true;
                        else if (radioIsNoGotten.Checked)
                            clientCompanyCertificate.Certificate.IsGotten = false;

                        clientCompanyCertificate.Certificate.EffectiveFrom = rdpEffectiveFrom.SelectedDate;
                        clientCompanyCertificate.Certificate.EffectiveTo = rdpEffectiveTo.SelectedDate;
                        clientCompanyCertificate.Certificate.IsNeedAlert = cbxIsNeedAlert.Checked;

                        if (cbxIsNeedAlert.Checked)
                            clientCompanyCertificate.Certificate.AlertBeforeDays = Convert.ToInt32(txtAlertBeforeDays.Value);
                        else
                            clientCompanyCertificate.Certificate.AlertBeforeDays = null;

                        clientCompanyCertificate.Certificate.Comment = txtComment.Text.Trim();

                        PageClientCompanyRepository.Save();

                        isSuccessSaved = true;
                    }

                    #endregion

                    break;

                //货品证照
                case EOwnerType.Product:

                    #region 货品证照

                    ProductCertificate productCertificate = null;

                    if (this.OwnerEntityID.HasValue
                        && this.OwnerEntityID > 0)
                    {
                        var product = PageProductRepository.GetByID(this.OwnerEntityID);

                        if (product != null)
                            productCertificate = product.ProductCertificate
                                .Where(x => x.ID == this.CurrentEntityID && x.Certificate != null && x.Certificate.OwnerTypeID == this.OwnerTypeID).FirstOrDefault();

                        if (productCertificate == null)
                        {
                            Certificate certificate = new Certificate() { OwnerTypeID = this.OwnerTypeID, };

                            productCertificate = new ProductCertificate() { Certificate = certificate };

                            product.ProductCertificate.Add(productCertificate);
                        }

                        productCertificate.Certificate.CertificateTypeID = Convert.ToInt32(rcbxCertificateType.SelectedValue);

                        if (radioIsGotten.Checked)
                            productCertificate.Certificate.IsGotten = true;
                        else if (radioIsNoGotten.Checked)
                            productCertificate.Certificate.IsGotten = false;

                        productCertificate.Certificate.EffectiveFrom = rdpEffectiveFrom.SelectedDate;
                        productCertificate.Certificate.EffectiveTo = rdpEffectiveTo.SelectedDate;
                        productCertificate.Certificate.IsNeedAlert = cbxIsNeedAlert.Checked;

                        if (cbxIsNeedAlert.Checked)
                            productCertificate.Certificate.AlertBeforeDays = Convert.ToInt32(txtAlertBeforeDays.Value);
                        else
                            productCertificate.Certificate.AlertBeforeDays = null;

                        productCertificate.Certificate.Comment = txtComment.Text.Trim();

                        PageProductRepository.Save();

                        isSuccessSaved = true;
                    }

                    #endregion

                    break;
            }

            if (isSuccessSaved)
            {
                this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
            }
        }

        protected void cvEffectiveDate_ServerValidate(object source, ServerValidateEventArgs args)
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