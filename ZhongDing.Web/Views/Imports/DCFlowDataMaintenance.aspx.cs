using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;

namespace ZhongDing.Web.Views.Imports
{
    public partial class DCFlowDataMaintenance : BasePage
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var currentEntity = PageDCFlowDataRepository.GetByID(this.CurrentEntityID);

                if (currentEntity != null)
                {
                    lblDistributionCompany.Text = currentEntity.DistributionCompany == null
                        ? string.Empty : currentEntity.DistributionCompany.Name;
                    lblSaleDate.Text = currentEntity.SaleDate.ToString("{0:yyyy/MM/dd}");

                    lblProductCode.Text = currentEntity.ProductCode;
                    lblProductName.Text = currentEntity.ProductName;

                    lblSpecification.Text = currentEntity.ProductSpecification;
                    lblFactoryName.Text = "";

                    lblSaleQty.Text = currentEntity.SaleQty.ToString();
                    lblFlow.Text = "";
                }
            }
        }


        #endregion

        protected void rgDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

        }
    }
}