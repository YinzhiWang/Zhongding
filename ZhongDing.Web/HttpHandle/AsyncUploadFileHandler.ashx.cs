using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Telerik.Web.UI;
using ZhongDing.Common.Enums;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.HttpHandle
{
    /// <summary>
    /// Summary description for AsyncUploadFileHandler
    /// </summary>
    public class AsyncUploadFileHandler : AsyncUploadHandler, IRequiresSessionState
    {

        protected override IAsyncUploadResult Process(UploadedFile file, HttpContext context, IAsyncUploadConfiguration configuration, string tempFileName)
        {

            // Call the base Process method to save the file to the temporary folder

            // base.Process(file, context, configuration, tempFileName);

            // Populate the default (base) result into an object of type ZDAsyncUploadResult

            ZDAsyncUploadResult result = CreateDefaultUploadResult<ZDAsyncUploadResult>(file);

            string uploadFilePath = string.Empty;

            // You can obtain any custom information passed from the page via casting the configuration parameter to your custom class

            ZDAsyncUploadConfiguration asynUploadConfiguration = configuration as ZDAsyncUploadConfiguration;

            if (asynUploadConfiguration != null)
            {
                uploadFilePath = asynUploadConfiguration.UploadFilePath.Replace("\\", "/");

                if (!uploadFilePath.EndsWith("/"))
                    uploadFilePath += "/";

                if (asynUploadConfiguration.OwnerTypeID > 0)
                {
                    EOwnerType ownerType = (EOwnerType)asynUploadConfiguration.OwnerTypeID;

                    switch (ownerType)
                    {
                        case EOwnerType.Company:
                            break;
                        case EOwnerType.Supplier:

                            if (asynUploadConfiguration.SupplierID > 0)
                            {
                                uploadFilePath += asynUploadConfiguration.SupplierID + "/";

                                if (asynUploadConfiguration.SupplierContractID > 0)
                                    uploadFilePath += asynUploadConfiguration.SupplierContractID + "/";
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
            }

            if (!string.IsNullOrEmpty(uploadFilePath))
            {
                string serverUploadFilePath = HttpContext.Current.Server.MapPath(uploadFilePath).Replace("/", "\\");

                if (!Directory.Exists(serverUploadFilePath))
                {
                    Directory.CreateDirectory(serverUploadFilePath);
                }

                if (!serverUploadFilePath.EndsWith("\\"))
                {
                    serverUploadFilePath += "\\";
                }

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + file.GetExtension();

                //string fileName = file.GetName();

                string fileUploadPath = serverUploadFilePath + fileName;

                file.SaveAs(fileUploadPath);

                result.FileNameWithoutExtension = file.GetNameWithoutExtension();

                result.FilePath = uploadFilePath.Replace("~", "") + fileName;

            }

            return result;
        }
    }
}