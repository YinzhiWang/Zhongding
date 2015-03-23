using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.HttpHandle
{
    /// <summary>
    /// Summary description for AsyncOperateFileHandler
    /// </summary>
    public class AsyncOperateFileHandler : BaseHttpHandler, IHttpHandler
    {
        #region Members

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

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            bool isSucess = false;

            string sReturnData = string.Empty;

            if (this.RequestData != null)
            {
                switch (this.RequestData.AjaxActionType)
                {
                    case EAjaxActionType.DeleteFile:
                        sReturnData = DeleteFile(ref isSucess);
                        break;
                }
            }

            context.Response.Write(Utility.JsonSeralize(new AjaxResponseData { IsSuccess = isSucess, ReturnData = sReturnData }));
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="isSucess">if set to <c>true</c> [is sucess].</param>
        /// <returns>System.String.</returns>
        private string DeleteFile(ref bool isSucess)
        {
            isSucess = false;
            string returnData = string.Empty;

            try
            {
                if (RequestData.UploadedFile != null)
                {
                    var uploadedFile = RequestData.UploadedFile;

                    isSucess = Utility.DeleteFile(uploadedFile.FilePath);

                    if (isSucess)
                    {
                        returnData = Utility.JsonSeralize(new { DeletedFilePath = uploadedFile.FilePath });

                        if (RequestData.UploadedFile.OwnerTypeID > 0)
                        {
                            EOwnerType ownerType = (EOwnerType)uploadedFile.OwnerTypeID;

                            switch (ownerType)
                            {
                                case EOwnerType.Company:
                                    break;
                                case EOwnerType.Supplier:

                                    if (uploadedFile.CurrentEntityID > 0)
                                    {
                                        var supplierContractFile = PageSupplierContractFileRepository.GetByID(uploadedFile.CurrentEntityID);

                                        if (supplierContractFile != null)
                                        {
                                            supplierContractFile.FilePath = string.Empty;

                                            PageSupplierContractFileRepository.Save();
                                        }
                                    }

                                    break;
                                case EOwnerType.Client:
                                    break;
                                case EOwnerType.Producer:
                                    break;
                                case EOwnerType.Product:
                                    break;
                            }
                        }
                        else if (RequestData.UploadedFile.ImportDataTypeID > 0)
                        {
                            var importDataType = (EImportDataType)RequestData.UploadedFile.ImportDataTypeID;

                            switch (importDataType)
                            {
                                case EImportDataType.DCFlowData:
                                    if (uploadedFile.CurrentEntityID > 0)
                                    {
                                        var dcImportFileLog = PageDCImportFileLogRepository.GetByID(uploadedFile.CurrentEntityID);

                                        if (dcImportFileLog != null)
                                        {
                                            dcImportFileLog.ImportFileLog.FilePath = string.Empty;

                                            PageDCImportFileLogRepository.Save();
                                        }
                                    }
                                    break;
                                case EImportDataType.DCInventoryData:
                                    break;
                                case EImportDataType.ProcureOrderData:
                                    break;
                                case EImportDataType.StockInData:
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                Utility.WriteExceptionLog(exp);
            }

            return returnData;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}