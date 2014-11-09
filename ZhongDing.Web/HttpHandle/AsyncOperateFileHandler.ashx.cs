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

                                    if (uploadedFile.SupplierContractFileID > 0)
                                    {
                                        var supplierContractFile = PageSupplierContractFileRepository.GetByID(uploadedFile.SupplierContractFileID);

                                        if (supplierContractFile != null)
                                        {
                                            supplierContractFile.FilePath = string.Empty;

                                            PageSupplierContractFileRepository.Save();
                                        }
                                    }

                                    break;
                                case EOwnerType.Customer:
                                    break;
                                case EOwnerType.Producer:
                                    break;
                                case EOwnerType.Product:
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