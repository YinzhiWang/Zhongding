﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Products
{
    public partial class ProductManagement : BasePage
    {
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ProductManage;
        }

        #region Private Methods

        private void BindProducts(bool isNeedRebind)
        {
            UISearchProduct uiSearchObj = new UISearchProduct()
            {
                ProductCode = txtProductCode.Text.Trim(),
                ProductName = txtProductName.Text.Trim()
            };

            int totalRecords;

            var entities = PageProductRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindProducts(false);
        }

        protected void rgEntities_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    DbModelContainer db = unitOfWork.GetDbModel();

                    IProductRepository productRepository = new ProductRepository();
                    IProductSpecificationRepository productSpecificationRepository = new ProductSpecificationRepository();
                    IProductCertificateRepository productCertificateRepository = new ProductCertificateRepository();
                    ICertificateRepository certificateRepository = new CertificateRepository();
                    productRepository.SetDbModel(db);
                    productSpecificationRepository.SetDbModel(db);
                    productCertificateRepository.SetDbModel(db);
                    certificateRepository.SetDbModel(db);

                    var product = productRepository.GetByID(id);

                    if (product != null)
                    {
                        foreach (var productSpecification in product.ProductSpecification)
                        {
                            if (productSpecification != null)
                                productSpecificationRepository.Delete(productSpecification);
                        }

                        foreach (var productCertificate in product.ProductCertificate)
                        {
                            if (productCertificate != null)
                            {
                                if (productCertificate.Certificate != null)
                                    certificateRepository.Delete(productCertificate.Certificate);

                                productCertificateRepository.Delete(productCertificate);
                            }
                        }

                        productRepository.Delete(product);
                    }

                    unitOfWork.SaveChanges();
                }
            }

            rgEntities.Rebind();
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProducts(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtProductCode.Text = string.Empty;
            txtProductName.Text = string.Empty;

            BindProducts(true);
        }
    }
}