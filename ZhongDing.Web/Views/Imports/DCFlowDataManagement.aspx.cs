﻿using System;
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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Imports
{
    public partial class DCFlowDataManagement : BasePage
    {
        #region Members

        private IDCFlowDataRepository _PageDCFlowDataRepository;
        private IDCFlowDataRepository PageDCFlowDataRepository
        {
            get
            {
                if (_PageDCFlowDataRepository == null)
                    _PageDCFlowDataRepository = new DCFlowDataRepository();

                return _PageDCFlowDataRepository;
            }
        }

        private IDistributionCompanyRepository _PageDistributionCompanyRepository;
        private IDistributionCompanyRepository PageDistributionCompanyRepository
        {
            get
            {
                if (_PageDistributionCompanyRepository == null)
                    _PageDistributionCompanyRepository = new DistributionCompanyRepository();

                return _PageDistributionCompanyRepository;
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
            this.Master.MenuItemID = (int)EMenuItem.DCFlowData;

            if (!IsPostBack)
            {
                BindDistributionCompanies();

                BindProducts();
            }
        }

        #region Private Methods

        private void BindDistributionCompanies()
        {
            var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems();

            rcbxDistributionCompany.DataSource = distributionCompanies;
            rcbxDistributionCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDistributionCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDistributionCompany.DataBind();

            rcbxDistributionCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts()
        {
            var products = PageProductRepository.GetDropdownItems();

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchDCFlowData()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                ExcludeOverwritten = true
            };

            if (!string.IsNullOrEmpty(rcbxDistributionCompany.SelectedValue))
            {
                int distributionCompanyID;
                if (int.TryParse(rcbxDistributionCompany.SelectedValue, out distributionCompanyID))
                    uiSearchObj.DistributionCompanyID = distributionCompanyID;
            }

            if (!string.IsNullOrEmpty(rcbxProduct.SelectedValue))
            {
                int productID;
                if (int.TryParse(rcbxProduct.SelectedValue, out productID))
                    uiSearchObj.ProductID = productID;
            }

            int totalRecords;

            var entities = PageDCFlowDataRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rcbxDistributionCompany.ClearSelection();
            rcbxProduct.ClearSelection();

            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            BindEntities(true);
        }
        protected override EPermission PagePermissionID()
        {
            return EPermission.DataImport;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Create;
        }
    }
}