using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;

namespace ZhongDing.Web.Views.Imports.Editors
{
    public partial class ViewOverwrittenDCFlowData : BasePage
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
            if ((!this.CurrentEntityID.HasValue
                || this.CurrentEntityID <= 0))
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);

                return;
            }

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
                    lblSaleDate.Text = currentEntity.SaleDate.ToString("yyyy/MM/dd");

                    lblProductCode.Text = currentEntity.ProductCode;
                    lblProductName.Text = currentEntity.ProductName;

                    lblSpecification.Text = currentEntity.ProductSpecification;
                    lblFactoryName.Text = currentEntity.FactoryName;

                    lblSaleQty.Text = currentEntity.SaleQty.ToString();
                    lblFlow.Text = currentEntity.FlowTo;
                }
            }
        }


        #endregion
    }
}