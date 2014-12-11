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
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Products
{
    public partial class ProductPriceManagement : BasePage
    {
        #region Consts

        private const string DATA_KEY_NAME_PRODUCT_PRICE_ID = "ID";
        private const string DATA_KEY_NAME_PRODUCT_ID = "ProductID";
        private const string DATA_KEY_NAME_PRODUCT_SPECIFICATION_ID = "ProductSpecificationID";

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

        private IProductBasicPriceRepository _PageProductBasicPriceRepository;
        private IProductBasicPriceRepository PageProductBasicPriceRepository
        {
            get
            {
                if (_PageProductBasicPriceRepository == null)
                    _PageProductBasicPriceRepository = new ProductBasicPriceRepository();

                return _PageProductBasicPriceRepository;
            }
        }

        private IProductHighPriceRepository _PageProductHighPriceRepository;
        private IProductHighPriceRepository PageProductHighPriceRepository
        {
            get
            {
                if (_PageProductHighPriceRepository == null)
                    _PageProductHighPriceRepository = new ProductHighPriceRepository();

                return _PageProductHighPriceRepository;
            }
        }

        private IProductDBPolicyPriceRepository _PageProductDBPolicyPriceRepository;
        private IProductDBPolicyPriceRepository PageProductDBPolicyPriceRepository
        {
            get
            {
                if (_PageProductDBPolicyPriceRepository == null)
                    _PageProductDBPolicyPriceRepository = new ProductDBPolicyPriceRepository();

                return _PageProductDBPolicyPriceRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ProductPriceManage;
        }

        #region 低价

        private void BindProductBasicPrices(bool isNeedRebind)
        {
            UISearchProduct uiSearchObj = new UISearchProduct()
            {
                CompanyID = CurrentUser.CompanyID,
                ProductName = txtBPProductName.Text.Trim()
            };

            int totalRecords;

            var basicPrices = PageProductBasicPriceRepository.GetUIList(uiSearchObj,
                rgProductBasicPrices.CurrentPageIndex, rgProductBasicPrices.PageSize, out totalRecords);

            rgProductBasicPrices.DataSource = basicPrices;
            rgProductBasicPrices.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgProductBasicPrices.Rebind();

            hdnBasicPricesCellValueChangedCount.Value = "0";
        }

        protected void rgProductBasicPrices_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindProductBasicPrices(false);
        }

        protected void rgProductBasicPrices_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            if (e.Commands.Count > 0)
            {
                int dkProductPriceID = GlobalConst.INVALID_INT;
                int dkProductID = GlobalConst.INVALID_INT;
                int dkProductSpecificationID = GlobalConst.INVALID_INT;

                foreach (var command in e.Commands)
                {
                    switch (command.Type)
                    {
                        case GridBatchEditingCommandType.Delete:
                            dkProductPriceID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_PRICE_ID);
                            if (dkProductPriceID > 0)
                                PageProductBasicPriceRepository.DeleteByID(dkProductPriceID);

                            break;
                        case GridBatchEditingCommandType.Insert:

                            break;
                        case GridBatchEditingCommandType.Update:

                            dkProductPriceID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_PRICE_ID);
                            dkProductID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_ID);
                            dkProductSpecificationID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_SPECIFICATION_ID);

                            ProductBasicPrice productPrice = null;

                            if (dkProductPriceID > 0)
                                productPrice = PageProductBasicPriceRepository.GetByID(dkProductPriceID);

                            if (productPrice == null)
                            {
                                productPrice = new ProductBasicPrice()
                                {
                                    ProductID = dkProductID,
                                    ProductSpecificationID = dkProductSpecificationID
                                };

                                PageProductBasicPriceRepository.Add(productPrice);
                            }

                            string sProcurePrice = command.NewValues["ProcurePrice"] == null
                                ? string.Empty : command.NewValues["ProcurePrice"].ToString();
                            string sSalePrice = command.NewValues["SalePrice"] == null
                                ? string.Empty : command.NewValues["SalePrice"].ToString();
                            string sComment = command.NewValues["Comment"] == null
                                ? string.Empty : command.NewValues["Comment"].ToString();

                            if (!string.IsNullOrEmpty(sProcurePrice))
                            {
                                decimal dProcurePrice;

                                if (decimal.TryParse(sProcurePrice, out dProcurePrice))
                                    productPrice.ProcurePrice = dProcurePrice;
                            }
                            else
                                productPrice.ProcurePrice = null;

                            if (!string.IsNullOrEmpty(sSalePrice))
                            {
                                decimal dSalePrice;

                                if (decimal.TryParse(sSalePrice, out dSalePrice))
                                    productPrice.SalePrice = dSalePrice;
                            }
                            else
                                productPrice.SalePrice = null;

                            if (!string.IsNullOrEmpty(sComment.Trim()))
                                productPrice.Comment = sComment;
                            else
                                productPrice.Comment = null;

                            break;
                    }
                }

                PageProductBasicPriceRepository.Save();

                hdnBasicPricesCellValueChangedCount.Value = "0";
            }
        }

        protected void btnSearchBasicPrice_Click(object sender, EventArgs e)
        {
            BindProductBasicPrices(true);
        }

        protected void btnResetBasicPrice_Click(object sender, EventArgs e)
        {
            txtBPProductName.Text = string.Empty;

            BindProductBasicPrices(true);
        }

        #endregion

        #region 高价

        private void BindProductHighPrices(bool isNeedRebind)
        {
            UISearchProduct uiSearchObj = new UISearchProduct()
            {
                CompanyID = CurrentUser.CompanyID,
                ProductName = txtHPProductName.Text.Trim()
            };

            int totalRecords;

            var basicPrices = PageProductHighPriceRepository.GetUIList(uiSearchObj,
                rgProductHighPrices.CurrentPageIndex, rgProductBasicPrices.PageSize, out totalRecords);

            rgProductHighPrices.DataSource = basicPrices;
            rgProductHighPrices.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgProductHighPrices.Rebind();

            hdnHighPricesCellValueChangedCount.Value = "0";
        }

        protected void rgProductHighPrices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindProductHighPrices(false);
        }

        protected void rgProductHighPrices_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            if (e.Commands.Count > 0)
            {
                int dkProductPriceID = GlobalConst.INVALID_INT;
                int dkProductID = GlobalConst.INVALID_INT;
                int dkProductSpecificationID = GlobalConst.INVALID_INT;

                foreach (var command in e.Commands)
                {
                    switch (command.Type)
                    {
                        case GridBatchEditingCommandType.Delete:
                            dkProductPriceID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_PRICE_ID);
                            if (dkProductPriceID > 0)
                                PageProductHighPriceRepository.DeleteByID(dkProductPriceID);

                            break;
                        case GridBatchEditingCommandType.Insert:

                            break;
                        case GridBatchEditingCommandType.Update:

                            dkProductPriceID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_PRICE_ID);
                            dkProductID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_ID);
                            dkProductSpecificationID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_SPECIFICATION_ID);

                            ProductHighPrice productPrice = null;

                            if (dkProductPriceID > 0)
                                productPrice = PageProductHighPriceRepository.GetByID(dkProductPriceID);

                            if (productPrice == null)
                            {
                                productPrice = new ProductHighPrice()
                                {
                                    ProductID = dkProductID,
                                    ProductSpecificationID = dkProductSpecificationID
                                };

                                PageProductHighPriceRepository.Add(productPrice);
                            }

                            string sHighPrice = command.NewValues["HighPrice"] == null
                                ? string.Empty : command.NewValues["HighPrice"].ToString();
                            string sActualProcurePrice = command.NewValues["ActualProcurePrice"] == null
                                ? string.Empty : command.NewValues["ActualProcurePrice"].ToString();
                            string sActualSalePrice = command.NewValues["ActualSalePrice"] == null
                                ? string.Empty : command.NewValues["ActualSalePrice"].ToString();

                            string sSupplierTaxRatio = command.NewValues["SupplierTaxRatio"] == null
                                ? string.Empty : command.NewValues["SupplierTaxRatio"].ToString();
                            string sClientTaxRatio = command.NewValues["ClientTaxRatio"] == null
                                ? string.Empty : command.NewValues["ClientTaxRatio"].ToString();

                            string sComment = command.NewValues["Comment"] == null
                                ? string.Empty : command.NewValues["Comment"].ToString();

                            if (!string.IsNullOrEmpty(sHighPrice))
                            {
                                decimal dHighPrice;

                                if (decimal.TryParse(sHighPrice, out dHighPrice))
                                    productPrice.HighPrice = dHighPrice;
                            }
                            else
                                productPrice.HighPrice = null;

                            if (!string.IsNullOrEmpty(sActualProcurePrice))
                            {
                                decimal dActualProcurePrice;

                                if (decimal.TryParse(sActualProcurePrice, out dActualProcurePrice))
                                    productPrice.ActualProcurePrice = dActualProcurePrice;
                            }
                            else
                                productPrice.ActualProcurePrice = null;

                            if (!string.IsNullOrEmpty(sActualSalePrice))
                            {
                                decimal dActualSalePrice;

                                if (decimal.TryParse(sActualSalePrice, out dActualSalePrice))
                                    productPrice.ActualSalePrice = dActualSalePrice;
                            }
                            else
                                productPrice.ActualSalePrice = null;

                            if (!string.IsNullOrEmpty(sSupplierTaxRatio))
                            {
                                double dSupplierTaxRatio;

                                if (double.TryParse(sSupplierTaxRatio, out dSupplierTaxRatio))
                                    productPrice.SupplierTaxRatio = dSupplierTaxRatio / 100;
                            }
                            else
                                productPrice.SupplierTaxRatio = null;

                            if (!string.IsNullOrEmpty(sClientTaxRatio))
                            {
                                double dClientTaxRatio;

                                if (double.TryParse(sClientTaxRatio, out dClientTaxRatio))
                                    productPrice.ClientTaxRatio = dClientTaxRatio / 100;
                            }
                            else
                                productPrice.ClientTaxRatio = null;

                            if (!string.IsNullOrEmpty(sComment.Trim()))
                                productPrice.Comment = sComment;
                            else
                                productPrice.Comment = null;

                            break;
                    }
                }

                PageProductHighPriceRepository.Save();

                hdnHighPricesCellValueChangedCount.Value = "0";
            }
        }

        protected void btnSearchHighPrice_Click(object sender, EventArgs e)
        {
            BindProductHighPrices(true);
        }

        protected void btnResetHighPrice_Click(object sender, EventArgs e)
        {
            txtHPProductName.Text = string.Empty;

            BindProductHighPrices(true);
        }

        #endregion

        #region 大包政策定价

        private void BindProductDBPolicyPrices(bool isNeedRebind)
        {
            UISearchProduct uiSearchObj = new UISearchProduct()
            {
                CompanyID = CurrentUser.CompanyID,
                ProductName = txtDBPProductName.Text.Trim()
            };

            int totalRecords;

            var dBPolicyPrices = PageProductDBPolicyPriceRepository.GetUIList(uiSearchObj,
                rgDBPolicyPrices.CurrentPageIndex, rgDBPolicyPrices.PageSize, out totalRecords);

            rgDBPolicyPrices.DataSource = dBPolicyPrices;
            rgDBPolicyPrices.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgDBPolicyPrices.Rebind();

            hdnDBPolicyPricesCellValueChangedCount.Value = "0";
        }

        protected void rgDBPolicyPrices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindProductDBPolicyPrices(false);
        }

        protected void btnSearchDBPolicyPrice_Click(object sender, EventArgs e)
        {
            BindProductDBPolicyPrices(true);
        }

        protected void btnResetDBPolicyPrice_Click(object sender, EventArgs e)
        {
            txtDBPProductName.Text = string.Empty;
            BindProductDBPolicyPrices(true);
        }

        protected void rgDBPolicyPrices_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            if (e.Commands.Count > 0)
            {
                int dkProductPriceID = GlobalConst.INVALID_INT;
                int dkProductID = GlobalConst.INVALID_INT;
                int dkProductSpecificationID = GlobalConst.INVALID_INT;

                foreach (var command in e.Commands)
                {
                    switch (command.Type)
                    {
                        case GridBatchEditingCommandType.Delete:
                            dkProductPriceID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_PRICE_ID);
                            if (dkProductPriceID > 0)
                                PageProductDBPolicyPriceRepository.DeleteByID(dkProductPriceID);

                            break;
                        case GridBatchEditingCommandType.Insert:

                            break;
                        case GridBatchEditingCommandType.Update:

                            dkProductPriceID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_PRICE_ID);
                            dkProductID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_ID);
                            dkProductSpecificationID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_SPECIFICATION_ID);

                            ProductDBPolicyPrice productPrice = null;

                            if (dkProductPriceID > 0)
                                productPrice = PageProductDBPolicyPriceRepository.GetByID(dkProductPriceID);

                            if (productPrice == null)
                            {
                                productPrice = new ProductDBPolicyPrice()
                                {
                                    ProductID = dkProductID,
                                    ProductSpecificationID = dkProductSpecificationID
                                };

                                PageProductDBPolicyPriceRepository.Add(productPrice);
                            }

                            string sBidPrice = command.NewValues["BidPrice"] == null
                                ? string.Empty : command.NewValues["BidPrice"].ToString();

                            string sFeeRatio = command.NewValues["FeeRatio"] == null
                                ? string.Empty : command.NewValues["FeeRatio"].ToString();

                            string sPreferredPrice = command.NewValues["PreferredPrice"] == null
                                 ? string.Empty : command.NewValues["PreferredPrice"].ToString();

                            string sPolicyPrice = command.NewValues["PolicyPrice"] == null
                                ? string.Empty : command.NewValues["PolicyPrice"].ToString();

                            string sComment = command.NewValues["Comment"] == null
                                ? string.Empty : command.NewValues["Comment"].ToString();

                            if (!string.IsNullOrEmpty(sBidPrice))
                            {
                                decimal dBidPrice;

                                if (decimal.TryParse(sBidPrice, out dBidPrice))
                                    productPrice.BidPrice = dBidPrice;
                            }
                            else
                                productPrice.BidPrice = null;

                            if (!string.IsNullOrEmpty(sFeeRatio))
                            {
                                double dFeeRatio;

                                if (double.TryParse(sFeeRatio, out dFeeRatio))
                                    productPrice.FeeRatio = dFeeRatio / 100;
                            }
                            else
                                productPrice.FeeRatio = null;

                            if (!string.IsNullOrEmpty(sPreferredPrice))
                            {
                                decimal dPreferredPrice;

                                if (decimal.TryParse(sPreferredPrice, out dPreferredPrice))
                                    productPrice.PreferredPrice = dPreferredPrice;
                            }
                            else
                                productPrice.PreferredPrice = null;

                            if (!string.IsNullOrEmpty(sPolicyPrice))
                            {
                                decimal dPolicyPrice;

                                if (decimal.TryParse(sPolicyPrice, out dPolicyPrice))
                                    productPrice.PolicyPrice = dPolicyPrice;
                            }
                            else
                                productPrice.PolicyPrice = null;

                            if (!string.IsNullOrEmpty(sComment.Trim()))
                                productPrice.Comment = sComment;
                            else
                                productPrice.Comment = null;

                            break;
                    }
                }

                PageProductDBPolicyPriceRepository.Save();

                hdnDBPolicyPricesCellValueChangedCount.Value = "0";
            }
        }


        #endregion

        #region Private Methods

        private int GetDataKeyValue(GridBatchEditingCommand command, string dataKeyName)
        {
            int dataKeyValue = GlobalConst.INVALID_INT;

            if (command.NewValues[dataKeyName] != null)
            {
                string sdataKeyValue = command.NewValues[dataKeyName].ToString();

                if (int.TryParse(sdataKeyValue, out dataKeyValue))
                    return dataKeyValue;
            }

            return dataKeyValue;
        }

        #endregion

    }
}

