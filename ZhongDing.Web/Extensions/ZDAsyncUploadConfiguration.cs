using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;

namespace ZhongDing.Web.Extensions
{
    public class ZDAsyncUploadConfiguration : AsyncUploadConfiguration
    {
        private string uploadFilePath;
        public string UploadFilePath
        {
            get { return uploadFilePath; }

            set { uploadFilePath = value; }
        }

        private int ownerTypeID;
        public int OwnerTypeID
        {
            get { return ownerTypeID; }
            set { ownerTypeID = value; }
        }

        private int companyID;
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private int supplierID;
        public int SupplierID
        {
            get { return supplierID; }
            set { supplierID = value; }
        }

        private int supplierContractID;
        public int SupplierContractID
        {
            get { return supplierContractID; }
            set { supplierContractID = value; }
        }
    }
}