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
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Imports.Editors
{
    public partial class UploadClientFlowDataFile : BasePage
    {
        #region Members
        private IClientImportFileLogRepository _PageClientImportFileLogRepository;
        private IClientImportFileLogRepository PageClientImportFileLogRepository
        {
            get
            {
                if (_PageClientImportFileLogRepository == null)
                    _PageClientImportFileLogRepository = new ClientImportFileLogRepository();

                return _PageClientImportFileLogRepository;
            }
        }

        private IClientUserRepository _PageClientUserRepository;
        private IClientUserRepository PageClientUserRepository
        {
            get
            {
                if (_PageClientUserRepository == null)
                    _PageClientUserRepository = new ClientUserRepository();

                return _PageClientUserRepository;
            }
        }

        private IClientCompanyRepository _PageClientCompanyRepository;
        private IClientCompanyRepository PageClientCompanyRepository
        {
            get
            {
                if (_PageClientCompanyRepository == null)
                    _PageClientCompanyRepository = new ClientCompanyRepository();

                return _PageClientCompanyRepository;
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

                BindClientUsers();

                LoadClientFlowDataFile();
            }
        }

        #region Private Methods

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension { OnlyIncludeValidClientUser = true }
            });

            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindClientCompanies()
        {
            rcbxClientCompany.ClearSelection();
            rcbxClientCompany.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.Extension = new UISearchExtension { ClientUserID = clientUserID };
            }

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadClientFlowDataFile()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var clientImportFile = PageClientImportFileLogRepository.GetByID(this.CurrentEntityID);

                if (clientImportFile != null)
                {
                    string fileName = clientImportFile.ImportFileLog.FileName;

                    txtFileName.Text = fileName;

                    if (!string.IsNullOrWhiteSpace(clientImportFile.ImportFileLog.FilePath))
                    {
                        string filePath = clientImportFile.ImportFileLog.FilePath;

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

                    rmypSettlementDate.SelectedDate = clientImportFile.SettlementDate;

                    if (clientImportFile.ClientUserID > 0)
                        rcbxClientUser.SelectedValue = clientImportFile.ClientUserID.ToString();

                    BindClientCompanies();

                    if (clientImportFile.ClientCompanyID > 0)
                        rcbxClientCompany.SelectedValue = clientImportFile.ClientCompanyID.ToString();

                    hdnEntityID.Value = clientImportFile.ImportFileLogID.ToString();

                    hdnFilePath.Value = clientImportFile.ImportFileLog.FilePath;

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

            config.ImportDataTypeID = (int)EImportDataType.ClientFlowData;
            config.UploadFilePath = WebConfig.UploadFilePathClientFlowData;

            // The upload configuration will be available in the handler
            radUploadFile.UploadConfiguration = config;

            radUploadFile.FileUploaded += new FileUploadedEventHandler(radUploadFile_FileUploaded);
        }

        #endregion

        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientCompanies();
        }

        protected void radUploadFile_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            ZDAsyncUploadResult result = e.UploadResult as ZDAsyncUploadResult;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (rmypSettlementDate.SelectedDate.HasValue
                && !string.IsNullOrEmpty(rcbxClientUser.SelectedValue)
                && !string.IsNullOrEmpty(rcbxClientCompany.SelectedValue))
            {
                var vSettlementDate = rmypSettlementDate.SelectedDate.Value;
                var settlementDate = new DateTime(vSettlementDate.Year, vSettlementDate.Month, 1);
                var clientUserID = int.Parse(rcbxClientUser.SelectedValue);
                var clientCompanyID = int.Parse(rcbxClientCompany.SelectedValue);

                var tempImportFileLogCount = PageClientImportFileLogRepository
                    .GetList(x => x.IsDeleted == false && x.ImportFileLogID != this.CurrentEntityID
                        && x.ClientUserID == clientUserID && x.ClientCompanyID == clientCompanyID
                        && x.ImportFileLog.ImportDataTypeID == (int)EImportDataType.ClientFlowData
                        && x.ImportFileLog.ImportStatusID != (int)EImportStatus.ImportError
                        && x.SettlementDate == settlementDate).Count();

                if (tempImportFileLogCount > 0)
                {
                    cvSettlementDate.IsValid = false;
                    cvSettlementDate.ErrorMessage = "该结算年月在当前选择的客户和商业单位下已存在，请选择其他结算年月";
                }
            }

            if (string.IsNullOrEmpty(hdnFilePath.Value))
            {
                cvFileName.IsValid = false;
                cvFileName.ErrorMessage = "请上传需导入的文件";
            }

            if (!IsValid) return;

            ClientImportFileLog clientImportFileLog = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                clientImportFileLog = PageClientImportFileLogRepository.GetByID(this.CurrentEntityID);

            if (clientImportFileLog == null)
            {
                clientImportFileLog = new ClientImportFileLog();

                var importFileLog = new ImportFileLog()
                {
                    ImportDataTypeID = (int)EImportDataType.ClientFlowData,
                    ImportStatusID = (int)EImportStatus.ToBeImport
                };

                clientImportFileLog.ImportFileLog = importFileLog;

                PageClientImportFileLogRepository.Add(clientImportFileLog);
            }

            clientImportFileLog.ClientUserID = int.Parse(rcbxClientUser.SelectedValue);
            clientImportFileLog.ClientCompanyID = int.Parse(rcbxClientCompany.SelectedValue);

            var tempSettlementDate = rmypSettlementDate.SelectedDate.Value;

            clientImportFileLog.SettlementDate = new DateTime(tempSettlementDate.Year, tempSettlementDate.Month, 1);

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

            clientImportFileLog.ImportFileLog.FileName = fileName;
            clientImportFileLog.ImportFileLog.FilePath = hdnFilePath.Value;

            PageClientImportFileLogRepository.Save();

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