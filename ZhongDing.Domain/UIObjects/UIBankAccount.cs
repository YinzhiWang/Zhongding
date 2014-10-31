using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    /// <summary>
    /// 类：银行帐号UI对象
    /// </summary>
    public class UIBankAccount : UIBase
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
        /// 类别
        /// </summary>
        /// <value>The type of the account.</value>
        public string AccountType { get; set; }

        /// <summary>
        /// 所有者类型
        /// </summary>
        /// <value>The type of the owner.</value>
        public string OwnerType { get; set; }

        /// <summary>
        /// 所属账套
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName { get; set; }

    }
}
