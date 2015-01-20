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
    public partial class DBOrderProductMaintain : BasePage
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

        private IDaBaoApplicationRepository _PageDaBaoAppRepository;
        private IDaBaoApplicationRepository PageDaBaoAppRepository
        {
            get
            {
                if (_PageDaBaoAppRepository == null)
                    _PageDaBaoAppRepository = new DaBaoApplicationRepository();

                return _PageDaBaoAppRepository;
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
                var dbOrderApp = PageDaBaoAppRepository.GetByID(this.OwnerEntityID);

                if (dbOrderApp != null)
                {
                    BindProducts(dbOrderApp.DepartmentID);

                    if (dbOrderApp.SalesOrderApplication != null)
                    {
                        var salesOrderAppDetails = dbOrderApp.SalesOrderApplication.SalesOrderAppDetail
                            .Where(x => x.IsDeleted == false);

                        var salesOrderAppDetail = salesOrderAppDetails.Where(x => x.ID == this.CurrentEntityID).FirstOrDefault();

                        if (salesOrderAppDetail != null)
                        {
                            BindProducts(dbOrderApp.DepartmentID);

                            rcbxProduct.SelectedValue = salesOrderAppDetail.ProductID.ToString();

                            BindProductSpecifications(salesOrderAppDetail.ProductID);

                            ddlProductSpecification.SelectedValue = salesOrderAppDetail.ProductSpecificationID.ToString();

                            txtSalesPrice.DbValue = salesOrderAppDetail.SalesPrice;
                            txtCount.DbValue = salesOrderAppDetail.Count;
                            txtTotalSalesAmount.DbValue = salesOrderAppDetail.TotalSalesAmount;
                        }
                        else
                        {
                            BindProducts(dbOrderApp.DepartmentID, salesOrderAppDetails.Select(x => x.ProductID).ToList());
                        }
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
                var dbOrderApp = PageDaBaoAppRepository.GetByID(this.OwnerEntityID);

                if (dbOrderApp != null && dbOrderApp.SalesOrderApplication != null)
                {
                    var salesOrderAppDetail = dbOrderApp.SalesOrderApplication.SalesOrderAppDetail
                        .Where(x => x.IsDeleted == false && x.ID == this.CurrentEntityID).FirstOrDefault();

                    if (salesOrderAppDetail == null)
                    {
                        salesOrderAppDetail = new SalesOrderAppDetail();

                        dbOrderApp.SalesOrderApplication.SalesOrderAppDetail.Add(salesOrderAppDetail);
                    }

                    salesOrderAppDetail.ProductID = Convert.ToInt32(rcbxProduct.SelectedValue);
                    salesOrderAppDetail.ProductSpecificationID = Convert.ToInt32(ddlProductSpecification.SelectedValue);
                    salesOrderAppDetail.SalesPrice = (decimal)txtSalesPrice.Value;
                    salesOrderAppDetail.Count = (int)txtCount.Value;
                    salesOrderAppDetail.TotalSalesAmount = salesOrderAppDetail.SalesPrice * salesOrderAppDetail.Count;

                    PageDaBaoAppRepository.Save();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
                }
            }
        }

    }
}