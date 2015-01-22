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

namespace ZhongDing.Web.Views.Products.Editors
{
    public partial class ProductSpecificationMaintain : BasePage
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

        private IUnitOfMeasurementRepository _PageUnitOfMeasurementRepository;
        private IUnitOfMeasurementRepository PageUnitOfMeasurementRepository
        {
            get
            {
                if (_PageUnitOfMeasurementRepository == null)
                    _PageUnitOfMeasurementRepository = new UnitOfMeasurementRepository();

                return _PageUnitOfMeasurementRepository;
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

                BindUnits();

                LoadCurrentEntity();
            }
        }

        private void BindUnits()
        {
            var units = PageUnitOfMeasurementRepository.GetDropdownItems();
            ddlUnitOfMeasurement.DataSource = units;
            ddlUnitOfMeasurement.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            ddlUnitOfMeasurement.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            ddlUnitOfMeasurement.DataBind();
        }

        private void LoadCurrentEntity()
        {
            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var product = PageProductRepository.GetByID(this.OwnerEntityID);

                if (product != null)
                {
                    var productSpecification = product.ProductSpecification
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (productSpecification != null)
                    {
                        txtSpecification.Text = productSpecification.Specification;

                        if (productSpecification.UnitOfMeasurementID.HasValue
                            && productSpecification.UnitOfMeasurementID > 0)
                            ddlUnitOfMeasurement.SelectedValue = productSpecification.UnitOfMeasurementID.ToString();

                        txtNumberInSmallPackage.Value = (double?)productSpecification.NumberInSmallPackage;
                        txtNumberInLargePackage.Value = (double?)productSpecification.NumberInLargePackage;

                        txtLicenseNumber.Text = productSpecification.LicenseNumber;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            ProductSpecification productSpecification = null;

            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var product = PageProductRepository.GetByID(this.OwnerEntityID);

                if (product != null)
                {
                    productSpecification = product.ProductSpecification
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (productSpecification == null)
                    {
                        productSpecification = new ProductSpecification();

                        product.ProductSpecification.Add(productSpecification);
                    }

                    productSpecification.Specification = txtSpecification.Text.Trim();

                    if (!string.IsNullOrEmpty(ddlUnitOfMeasurement.SelectedValue))
                        productSpecification.UnitOfMeasurementID = Convert.ToInt32(ddlUnitOfMeasurement.SelectedValue);

                    productSpecification.NumberInSmallPackage = (int?)txtNumberInSmallPackage.Value;
                    productSpecification.NumberInLargePackage = (int?)txtNumberInLargePackage.Value;
                    productSpecification.LicenseNumber = txtLicenseNumber.Text.Trim();

                    product.LastModifiedOn = DateTime.Now;
                    product.LastModifiedBy = CurrentUser.UserID;

                    PageProductRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);

                }
            }
        }
    }
}