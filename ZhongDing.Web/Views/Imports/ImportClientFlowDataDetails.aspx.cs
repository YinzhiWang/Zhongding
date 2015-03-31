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

namespace ZhongDing.Web.Views.Imports
{
    public partial class ImportClientFlowDataDetails : BasePage
    {
        #region Members

        private IImportFileLogRepository _PageImportFileLogRepository;
        private IImportFileLogRepository PageImportFileLogRepository
        {
            get
            {
                if (_PageImportFileLogRepository == null)
                    _PageImportFileLogRepository = new ImportFileLogRepository();

                return _PageImportFileLogRepository;
            }
        }

        private IImportErrorLogRepository _PageImportErrorLogRepository;
        private IImportErrorLogRepository PageImportErrorLogRepository
        {
            get
            {
                if (_PageImportErrorLogRepository == null)
                    _PageImportErrorLogRepository = new ImportErrorLogRepository();

                return _PageImportErrorLogRepository;
            }
        }

        private IClientFlowDataRepository _PageClientFlowDataRepository;
        private IClientFlowDataRepository PageClientFlowDataRepository
        {
            get
            {
                if (_PageClientFlowDataRepository == null)
                    _PageClientFlowDataRepository = new ClientFlowDataRepository();

                return _PageClientFlowDataRepository;
            }
        }

        private ImportFileLog _CurrentEntity;
        private ImportFileLog CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                    {
                        _CurrentEntity = PageImportFileLogRepository.GetByID(this.CurrentEntityID);

                        if (_CurrentEntity != null && _CurrentEntity.ImportDataTypeID != (int)EImportDataType.ClientFlowData)
                            _CurrentEntity = null;
                    }

                return _CurrentEntity;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.CurrentEntity == null)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);

                return;
            }

            this.Master.MenuItemID = (int)EMenuItem.ClientSaleFlowData;

            if (!IsPostBack)
            {
                LoadCurrentEntity();
            }
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                lblFileName.Text = CurrentEntity.FileName;
                lblFilePath.Text = CurrentEntity.FilePath;
                lblClientUser.Text = (CurrentEntity.ClientImportFileLog != null && CurrentEntity.ClientImportFileLog.ClientUser != null)
                    ? CurrentEntity.ClientImportFileLog.ClientUser.ClientName : string.Empty;

                lblClientCompany.Text = (CurrentEntity.ClientImportFileLog != null && CurrentEntity.ClientImportFileLog.ClientCompany != null)
                    ? CurrentEntity.ClientImportFileLog.ClientCompany.Name : string.Empty;

                lblImportBeginDate.Text = CurrentEntity.ImportBeginDate.HasValue
                    ? CurrentEntity.ImportBeginDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;
                lblImportEndDate.Text = CurrentEntity.ImportEndDate.HasValue
                    ? CurrentEntity.ImportEndDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;

                lblTotalCount.Text = CurrentEntity.TotalCount.HasValue
                    ? CurrentEntity.TotalCount.ToString() : string.Empty;
                lblSucceedCount.Text = CurrentEntity.SucceedCount.HasValue
                    ? CurrentEntity.SucceedCount.ToString() : string.Empty;

                lblFailedCount.Text = CurrentEntity.FailedCount.HasValue
                    ? CurrentEntity.FailedCount.ToString() : string.Empty;
            }
        }

        protected void rgSucceedLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchClientFlowData()
            {
                ImportFileLogID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var entities = PageClientFlowDataRepository.GetUIList(uiSearchObj, rgSucceedLogs.CurrentPageIndex, rgSucceedLogs.PageSize, out totalRecords);

            rgSucceedLogs.VirtualItemCount = totalRecords;

            rgSucceedLogs.DataSource = entities;
        }

        protected void rgFailedLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchImportErrorLog()
            {
                ImportFileLogID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var entities = PageImportErrorLogRepository.GetUIList(uiSearchObj, rgFailedLogs.CurrentPageIndex, rgFailedLogs.PageSize, out totalRecords);

            rgFailedLogs.VirtualItemCount = totalRecords;

            rgFailedLogs.DataSource = entities;
        }
    }
}