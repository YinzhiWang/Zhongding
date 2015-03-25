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
using ZhongDing.Domain.UISearchObjects;

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

        private IDCFlowDataDetailRepository _PageDCFlowDataDetailRepository;
        private IDCFlowDataDetailRepository PageDCFlowDataDetailRepository
        {
            get
            {
                if (_PageDCFlowDataDetailRepository == null)
                    _PageDCFlowDataDetailRepository = new DCFlowDataDetailRepository();

                return _PageDCFlowDataDetailRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!this.CurrentEntityID.HasValue
                || this.CurrentEntityID <= 0))
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);

                return;
            }

            this.Master.MenuItemID = (int)EMenuItem.DCFlowData;

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

                    if (currentEntity.IsCorrectlyFlow == true)
                    {
                        btnCorrect.Visible = false;
                        btnImport.Visible = false;
                    }
                }
            }
        }


        #endregion

        protected void rgDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchDCFlowDataDetail
            {
                DCFlowDataID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var dcFlowDataDetails = PageDCFlowDataDetailRepository.GetUIList(uiSearchObj,
                rgDetails.CurrentPageIndex, rgDetails.PageSize, out  totalRecords);

            rgDetails.DataSource = dcFlowDataDetails;
            rgDetails.VirtualItemCount = totalRecords;
        }

        protected void rgDetails_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    var dcFlowDataDetail = PageDCFlowDataDetailRepository.GetByID(id);

                    if (dcFlowDataDetail != null)
                    {
                        var txtComment = dataItem.FindControl("txtComment") as RadTextBox;
                        dcFlowDataDetail.Comment = txtComment.Text;

                        PageDCFlowDataDetailRepository.Save();
                    }
                }
            }
        }

    }
}