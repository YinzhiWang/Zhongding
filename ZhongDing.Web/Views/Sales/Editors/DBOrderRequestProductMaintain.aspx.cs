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
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Sales.Editors
{
    public partial class DBOrderRequestProductMaintain : BasePage
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

        private IDaBaoRequestApplicationRepository _PageDaBaoRequestAppRepository;
        private IDaBaoRequestApplicationRepository PageDaBaoRequestAppRepository
        {
            get
            {
                if (_PageDaBaoRequestAppRepository == null)
                    _PageDaBaoRequestAppRepository = new DaBaoRequestApplicationRepository();

                return _PageDaBaoRequestAppRepository;
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

        private IProductSpecificationRepository _PageProductSpecificationRepository;
        private IProductSpecificationRepository PageProductSpecificationRepository
        {
            get
            {
                if (_PageProductSpecificationRepository == null)
                    _PageProductSpecificationRepository = new ProductSpecificationRepository();

                return _PageProductSpecificationRepository;
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

                LoadCurrentEntity();
            }
        }

        #region Private Methods


        private void BindProducts(int departmentID, IList<int> excludeItemValues = null)
        {
            rcbxProduct.ClearSelection();
            rcbxProduct.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem()
            {
                Extension = new UISearchExtension { DepartmentID = departmentID }
            };

            if (excludeItemValues != null)
                uiSearchObj.ExcludeItemValues = excludeItemValues;

            var products = PageProductRepository.GetDropdownItems(uiSearchObj);

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProductSpecifications(int productID)
        {
            if (productID > 0)
            {
                ddlProductSpecification.Items.Clear();

                var productSpecifications = PageProductSpecificationRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension { ProductID = productID }
                });

                ddlProductSpecification.DataSource = productSpecifications;
                ddlProductSpecification.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                ddlProductSpecification.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                ddlProductSpecification.DataBind();
            }
        }

        private void LoadCurrentEntity()
        {
            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var dbRequestApp = PageDaBaoRequestAppRepository.GetByID(this.OwnerEntityID);

                if (dbRequestApp != null)
                {
                    var dbRequestAppDetail = dbRequestApp.DaBaoRequestAppDetail
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (dbRequestAppDetail != null)
                    {
                        BindProducts(dbRequestApp.DepartmentID);

                        rcbxProduct.SelectedValue = dbRequestAppDetail.ProductID.ToString();

                        BindProductSpecifications(dbRequestAppDetail.ProductID);

                        ddlProductSpecification.SelectedValue = dbRequestAppDetail.ProductSpecificationID.ToString();

                        txtSalesPrice.DbValue = dbRequestAppDetail.SalesPrice;
                        txtCount.DbValue = dbRequestAppDetail.Count;
                        txtTotalSalesAmount.DbValue = dbRequestAppDetail.TotalSalesAmount;
                    }
                    else
                    {
                        BindProducts(dbRequestApp.DepartmentID, dbRequestApp.DaBaoRequestAppDetail
                            .Where(x => x.IsDeleted == false).Select(x => x.ProductID).ToList());
                    }
                }
            }
        }

        #endregion

        protected void rcbxProduct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int productID;

                if (int.TryParse(e.Value, out productID))
                    BindProductSpecifications(productID);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (this.OwnerEntityID.HasValue
                && this.OwnerEntityID > 0)
            {
                var dbRequestApp = PageDaBaoRequestAppRepository.GetByID(this.OwnerEntityID);

                if (dbRequestApp != null)
                {
                    var dbRequestAppDetail = dbRequestApp.DaBaoRequestAppDetail
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (dbRequestAppDetail == null)
                    {
                        dbRequestAppDetail = new DaBaoRequestAppDetail();

                        dbRequestApp.DaBaoRequestAppDetail.Add(dbRequestAppDetail);
                    }

                    dbRequestAppDetail.ProductID = Convert.ToInt32(rcbxProduct.SelectedValue);
                    dbRequestAppDetail.ProductSpecificationID = Convert.ToInt32(ddlProductSpecification.SelectedValue);
                    dbRequestAppDetail.SalesPrice = (decimal)txtSalesPrice.Value;
                    dbRequestAppDetail.Count = (int)txtCount.Value;
                    dbRequestAppDetail.TotalSalesAmount = dbRequestAppDetail.SalesPrice * dbRequestAppDetail.Count;

                    PageDaBaoRequestAppRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
                }
            }
        }

    }
}