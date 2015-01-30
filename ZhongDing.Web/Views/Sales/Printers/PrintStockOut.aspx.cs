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

namespace ZhongDing.Web.Views.Sales.Printers
{
    public partial class PrintStockOut : BasePage
    {
        #region Members

        private IStockOutRepository _PageStockOutRepository;
        private IStockOutRepository PageStockOutRepository
        {
            get
            {
                if (_PageStockOutRepository == null)
                    _PageStockOutRepository = new StockOutRepository();

                return _PageStockOutRepository;
            }
        }

        private IStockOutDetailRepository _PageStockOutDetailRepository;
        private IStockOutDetailRepository PageStockOutDetailRepository
        {
            get
            {
                if (_PageStockOutDetailRepository == null)
                    _PageStockOutDetailRepository = new StockOutDetailRepository();

                return _PageStockOutDetailRepository;
            }
        }



        private StockOut _CurrentEntity;
        private StockOut CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageStockOutRepository.GetByID(this.CurrentEntityID);

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
                Page.Title = "出库单" + this.CurrentEntity.Code;

                lblCode.Text = this.CurrentEntity.Code;
                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);

                if (CurrentEntity.ReceiverTypeID == (int)EReceiverType.DistributionCompany)
                {
                    divDaBaoOrder.Visible = true;
                    lblDistCompany.Text = this.CurrentEntity.DistributionCompany == null
                        ? string.Empty : this.CurrentEntity.DistributionCompany.Name;
                }
                else if (CurrentEntity.ReceiverTypeID == (int)EReceiverType.ClientUser)
                {
                    divClientUserOrder.Visible = true;

                    lblClientUser.Text = this.CurrentEntity.ClientUser == null
                        ? string.Empty : this.CurrentEntity.ClientUser.ClientName;

                    lblClientCompany.Text = this.CurrentEntity.ClientCompany == null
                        ? string.Empty : this.CurrentEntity.ClientCompany.Name;
                }

                lblBillDate.Text = CurrentEntity.BillDate.ToString("yyyy/MM/dd");
                lblWorkflowStatus.Text = this.CurrentEntity.WorkflowStatus.StatusName;
                lblOutDate.Text = CurrentEntity.OutDate.HasValue
                    ? CurrentEntity.OutDate.Value.ToString("yyyy/MM/dd") : string.Empty;
                lblPrintTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                lblReceiverName.Text = CurrentEntity.ReceiverName;
                lblReceiverPhone.Text = CurrentEntity.ReceiverPhone;
                lblReceiverAddress.Text = CurrentEntity.ReceiverAddress;
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);
            }
        }

        protected void rgStockOutDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchStockOutDetailObj = new UISearchStockOutDetail()
            {
                StockOutID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
            };

            var stockOutDetails = PageStockOutDetailRepository.GetUIList(uiSearchStockOutDetailObj);

            rgStockOutDetails.DataSource = stockOutDetails;
        }
    }
}