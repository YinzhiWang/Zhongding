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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Refunds.Printers
{
    public partial class PrintFMRefundApp : BasePage
    {
        #region Members

        private IFactoryManagerRefundAppRepository _PageFMRefundAppRepository;
        private IFactoryManagerRefundAppRepository PageFMRefundAppRepository
        {
            get
            {
                if (_PageFMRefundAppRepository == null)
                    _PageFMRefundAppRepository = new FactoryManagerRefundAppRepository();

                return _PageFMRefundAppRepository;
            }
        }

        private IApplicationPaymentRepository _PageAppPaymentRepository;
        private IApplicationPaymentRepository PageAppPaymentRepository
        {
            get
            {
                if (_PageAppPaymentRepository == null)
                    _PageAppPaymentRepository = new ApplicationPaymentRepository();

                return _PageAppPaymentRepository;
            }
        }

        private FactoryManagerRefundApplication _CurrentEntity;
        private FactoryManagerRefundApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageFMRefundAppRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
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

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {

                lblCreatedOn.Text = CurrentEntity.CreatedOn.ToString("yyyy/MM/dd");
                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);

                lblCompany.Text = CurrentEntity.Company.CompanyName;

                if (CurrentEntity.ClientUser != null)
                    lblClientUser.Text = CurrentEntity.ClientUser.ClientName;

                lblProductName.Text = CurrentEntity.Product.ProductName;
                lblProductSpecification.Text = CurrentEntity.ProductSpecification.Specification;

                lblBeginDate.Text = CurrentEntity.BeginDate.ToString("yyyy/MM/dd");
                lblEndDate.Text = CurrentEntity.EndDate.ToString("yyyy/MM/dd");

                lblStockInQty.Text = CurrentEntity.StockInQty.ToString();

                if (CurrentEntity.StockOutQty.HasValue)
                    lblStockOutQty.Text = CurrentEntity.StockOutQty.ToString();

                lblRefundPrice.Text = CurrentEntity.RefundPrice.ToString("C2");
                lblRefundAmount.Text = CurrentEntity.RefundAmount.ToString("C2");
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);
            }
        }

        protected void rgAppPayments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationPayment
            {
                WorkflowID = (int)EWorkflow.FactoryManagerRefunds,
                ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;
        }
    }
}