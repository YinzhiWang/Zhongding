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

namespace ZhongDing.Web.Views.Basics.Editors
{
    public partial class CertificateFileMaintain : BasePage
    {
        #region Fields

        /// <summary>
        /// 合同ID.
        /// </summary>
        /// <value>The contract ID.</value>
        private int? ContractID
        {
            get
            {
                string sContractID = Request.QueryString["ContractID"];

                int iContractID;

                if (int.TryParse(sContractID, out iContractID))
                    return iContractID;
                else
                    return null;
            }
        }

        #endregion

        #region Members
        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                    _PageSupplierRepository = new SupplierRepository();

                return _PageSupplierRepository;
            }
        }

        private ISupplierContractRepository _PageSupplierContractRepository;
        private ISupplierContractRepository PageSupplierContractRepository
        {
            get
            {
                if (_PageSupplierContractRepository == null)
                    _PageSupplierContractRepository = new SupplierContractRepository();

                return _PageSupplierContractRepository;
            }
        }

        private ISupplierContractFileRepository _PageSupplierContractFileRepository;
        private ISupplierContractFileRepository PageSupplierContractFileRepository
        {
            get
            {
                if (_PageSupplierContractFileRepository == null)
                    _PageSupplierContractFileRepository = new SupplierContractFileRepository();

                return _PageSupplierContractFileRepository;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ContractID.HasValue
                || this.ContractID <= 0)
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = base.GridClientID;

                InitUploadConfiguration();

                LoadContracFile();
            }
        }

        private void LoadContracFile()
        {
            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
            {
                var contractFile = PageSupplierContractFileRepository.GetByID(this.CurrentEntityID);

                if (contractFile != null)
                {
                    txtFileName.Text = contractFile.FileName;
                    txtComment.Text = contractFile.Comment;

                    if (!string.IsNullOrWhiteSpace(contractFile.FilePath))
                    {
                        string filePath = contractFile.FilePath;

                        if (!filePath.StartsWith("~"))
                            filePath = "~" + filePath;

                        filePath = HttpContext.Current.Server.MapPath(filePath);

                        if (File.Exists(filePath))
                        {
                            FileInfo fileInfo = new FileInfo(filePath);

                            if (fileInfo != null)
                            {
                                string fileName = contractFile.FileName;

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

                    hdnEntityID.Value = contractFile.ID.ToString();

                    hdnFilePath.Value = contractFile.FilePath;

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

            config.OwnerTypeID = (int)EOwnerType.Supplier;
            config.SupplierContractID = this.ContractID.Value;
            config.UploadFilePath = WebConfig.UploadFilePathCommon;

            if (config.SupplierContractID > 0)
            {
                var supplierContract = PageSupplierContractRepository.GetByID(config.SupplierContractID);

                if (supplierContract != null)
                {
                    config.SupplierID = supplierContract.SupplierID.HasValue ? supplierContract.SupplierID.Value : GlobalConst.INVALID_INT;

                    config.UploadFilePath = WebConfig.UploadFilePathSupplierContract;
                }
            }

            // The upload configuration will be available in the handler
            radUploadFile.UploadConfiguration = config;

            radUploadFile.FileUploaded += new FileUploadedEventHandler(radUploadFile_FileUploaded);
        }

        protected void radUploadFile_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            ZDAsyncUploadResult result = e.UploadResult as ZDAsyncUploadResult;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            SupplierContractFile contractFile = null;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                contractFile = PageSupplierContractFileRepository.GetByID(this.CurrentEntityID);

            if (contractFile == null)
            {
                contractFile = new SupplierContractFile()
                {
                    ContractID = this.ContractID
                };

                PageSupplierContractFileRepository.Add(contractFile);
            }

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

            contractFile.FileName = fileName;
            contractFile.FilePath = hdnFilePath.Value;
            contractFile.Comment = txtComment.Text.Trim();

            PageSupplierContractFileRepository.Save();

            this.Master.BaseNotification.OnClientHidden = "onClientHidden";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_CLOSE_WIN);
        }

    }
}