using System;
using System.Collections.Generic;
using System.IO;
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
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Imports.Editors
{
    public partial class UploadDCFlowDataFile : BasePage
    {
        #region Members
        private IDCImportFileLogRepository _PageDCImportFileLogRepository;
        private IDCImportFileLogRepository PageDCImportFileLogRepository
        {
            get
            {
                if (_PageDCImportFileLogRepository == null)
                    _PageDCImportFileLogRepository = new DCImportFileLogRepository();

                return _PageDCImportFileLogRepository;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            radUploadFile.PersistConfiguration = true;

            if (!IsPostBack)
            {
                hdnGridClientID.Value = base.GridClientID;

                InitUploadConfiguration();

                BindDistributionCompanies();

                LoadDCFlowDataFile();
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


        private void LoadDCFlowDataFile()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var dcImportFile = PageDCImportFileLogRepository.GetByID(this.CurrentEntityID);

                if (dcImportFile != null)
                {
                    string fileName = dcImportFile.ImportFileLog.FileName;

                    txtFileName.Text = fileName;

                    if (!string.IsNullOrWhiteSpace(dcImportFile.ImportFileLog.FilePath))
                    {
                        string filePath = dcImportFile.ImportFileLog.FilePath;

                        if (!filePath.StartsWith("~"))
                            filePath = "~" + filePath;

                        filePath = HttpContext.Current.Server.MapPath(filePath);

                        if (File.Exists(filePath))
                        {
                            FileInfo fileInfo = new FileInfo(filePath);

                            if (fileInfo != null)
                            {
                                if (fileName.LastIndexOf(".") >= 0)
                                {
                                    string customExtension = fileName.Substring(fileName.LastIndexOf("."));

                                    if (customExtension.ToLower() != fileInfo.Extension.ToLower())
                                        fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + fileInfo.Extension;
                                }
                                else
                                    fileName += fileInfo.Extension;

                                hdnFileName.Value = fileName;
                            }
                        }
                    }

                    rmypSettlementDate.SelectedDate = dcImportFile.SettlementDate;

                    if (dcImportFile.DistributionCompanyID > 0)
                        rcbxDistributionCompany.SelectedValue = dcImportFile.DistributionCompanyID.ToString();

                    hdnEntityID.Value = dcImportFile.ImportFileLogID.ToString();

                    hdnFilePath.Value = dcImportFile.ImportFileLog.FilePath;

                }

            }
        }

        /// <summary>
        /// Inits the upload configuration.
        /// </summary>
        private void InitUploadConfiguration()
        {
            // Populate the default (base) upload configuration into an object of type ZDAsyncUploadConfiguration
            ZDAsyncUploadConfiguration config =
                radUploadFile.CreateDefaultUploadConfiguration<ZDAsyncUploadConfiguration>();

            config.ImportDataTypeID = (int)EImportDataType.DCFlowData;
            config.UploadFilePath = WebConfig.UploadFilePathDCFlowData;

            // The upload configuration will be available in the handler
            radUploadFile.UploadConfiguration = config;

            radUploadFile.FileUploaded += new FileUploadedEventHandler(radUploadFile_FileUploaded);
        }

        #endregion

        protected void radUploadFile_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            ZDAsyncUploadResult result = e.UploadResult as ZDAsyncUploadResult;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (rmypSettlementDate.SelectedDate.HasValue
                && !string.IsNullOrEmpty(rcbxDistributionCompany.SelectedValue))
            {
                var vSettlementDate = rmypSettlementDate.SelectedDate.Value;
                var settlementDate = new DateTime(vSettlementDate.Year, vSettlementDate.Month, 1);
                var distributionCompanyID = int.Parse(rcbxDistributionCompany.SelectedValue);

                var tempImportFileLogCount = PageDCImportFileLogRepository
                    .GetList(x => x.IsDeleted == false && x.ImportFileLogID != this.CurrentEntityID
                        && x.DistributionCompanyID == distributionCompanyID
                        && x.ImportFileLog.ImportDataTypeID == (int)EImportDataType.DCFlowData
                        && x.ImportFileLog.ImportStatusID != (int)EImportStatus.ImportError
                        && x.SettlementDate == settlementDate).Count();

                if (tempImportFileLogCount > 0)
                {
                    cvSettlementDate.IsValid = false;
                    cvSettlementDate.ErrorMessage = "该结算年月在当前选择的配送公司下已存在，请选择其他结算年月";
                }
            }

            if (!IsValid) return;

            DCImportFileLog dcImportFileLog = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                dcImportFileLog = PageDCImportFileLogRepository.GetByID(this.CurrentEntityID);

            if (dcImportFileLog == null)
            {
                dcImportFileLog = new DCImportFileLog();

                var importFileLog = new ImportFileLog()
                {
                    ImportDataTypeID = (int)EImportDataType.DCFlowData,
                    ImportStatusID = (int)EImportStatus.ToBeImport
                };

                dcImportFileLog.ImportFileLog = importFileLog;

                PageDCImportFileLogRepository.Add(dcImportFileLog);
            }

            dcImportFileLog.DistributionCompanyID = int.Parse(rcbxDistributionCompany.SelectedValue);

            var tempSettlementDate = rmypSettlementDate.SelectedDate.Value;

            dcImportFileLog.SettlementDate = new DateTime(tempSettlementDate.Year, tempSettlementDate.Month, 1);

            string fileName = txtFileName.Text.Trim();

            if (!string.IsNullOrWhiteSpace(hdnFilePath.Value))
            {
                string filePath = hdnFilePath.Value;

                if (!filePath.StartsWith("~"))
                    filePath = "~" + filePath;

                filePath = HttpContext.Current.Server.MapPath(filePath);

                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    if (fileInfo != null)
                    {
                        if (fileName.LastIndexOf(".") >= 0)
                        {
                            string customExtension = fileName.Substring(fileName.LastIndexOf("."));

                            if (customExtension.ToLower() != fileInfo.Extension.ToLower())
                                fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + fileInfo.Extension;
                        }
                        else
                            fileName += fileInfo.Extension;
                    }
                }
            }

            dcImportFileLog.ImportFileLog.FileName = fileName;
            dcImportFileLog.ImportFileLog.FilePath = hdnFilePath.Value;

            PageDCImportFileLogRepository.Save();

            this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
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