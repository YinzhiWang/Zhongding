using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Procures.Printers
{
    public partial class PrintStockIn : BasePage
    {
        #region Members

        private IStockInRepository _PageStockInRepository;
        private IStockInRepository PageStockInRepository
        {
            get
            {
                if (_PageStockInRepository == null)
                    _PageStockInRepository = new StockInRepository();

                return _PageStockInRepository;
            }
        }

        private IStockInDetailRepository _PageStockInDetailRepository;
        private IStockInDetailRepository PageStockInDetailRepository
        {
            get
            {
                if (_PageStockInDetailRepository == null)
                    _PageStockInDetailRepository = new StockInDetailRepository();

                return _PageStockInDetailRepository;
            }
        }

        private StockIn _CurrentEntity;
        private StockIn CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageStockInRepository.GetByID(this.CurrentEntityID);

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
                Page.Title = "入库单" + this.CurrentEntity.Code;

                lblCode.Text = this.CurrentEntity.Code;
                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);
                lblEntryDate.Text = !this.CurrentEntity.EntryDate.HasValue
                    ? string.Empty : this.CurrentEntity.EntryDate.Value.ToString("yyyy/MM/dd");
                lblSupplier.Text = this.CurrentEntity.Supplier.SupplierName;

                lblWorkflowStatus.Text = this.CurrentEntity.WorkflowStatus.StatusName;

                lblPrintTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);
            }
        }

        protected void rgStockInDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchStockInDetailObj = new UISearchStockInDetail()
            {
                StockInID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
            };

            var stockInDetails = PageStockInDetailRepository.GetUIList(uiSearchStockInDetailObj);

            rgStockInDetails.DataSource = stockInDetails;
        }
    }
}