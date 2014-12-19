using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchBankAccount : UISearchBase
    {
        /// <summary>
        /// 户名
        /// </summary>
        /// <value>The name of the account.</value>
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        /// <value>The name of the bank branch.</value>
        public string BankBranchName { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        /// <value>The account.</value>
        public string Account { get; set; }

        /// <summary>
        /// 账户类型ID
        /// </summary>
        /// <value>The account type ID.</value>
        public int AccountTypeID { get; set; }

        /// <summary>
        /// 所属账套ID
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID { get; set; }

        /// <summary>
        /// 所有者类型ID
        /// </summary>
        public int OwnerTypeID { get; set; }

        /// <summary>
        /// 是否遮罩帐号
        /// </summary>
        /// <value><c>true</c> if this instance is need masked account; otherwise, <c>false</c>.</value>
        public bool IsNeedMaskedAccount { get; set; }
    }
}
