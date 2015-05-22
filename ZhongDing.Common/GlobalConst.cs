using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common
{
    /// <summary>
    /// 公共常量类
    /// </summary>
    public class GlobalConst
    {
        #region General Consts

        /// <summary>
        /// 无效的Int:-1
        /// </summary>
        public static readonly int INVALID_INT = -1;
        
        /// <summary>
        /// 系统母版页主标题
        /// </summary>
        public static readonly string WEBSITE_MASTER_PAGE_MAIN_TITLE = "众鼎医药咨询信息系统";

        /// <summary>
        /// 是否有某种证照：有
        /// </summary>
        public static readonly string GOTTEN_DESC_HAVE = "有";
        /// <summary>
        /// 是否有某种证照：无
        /// </summary>
        public static readonly string GOTTEN_DESC_NONHAVE = "无";

        /// <summary>
        /// 默认用户头像
        /// </summary>
        public static readonly string DEFAULT_USER_AVATAR = "~/Images/defaultAvatar.gif";

        /// <summary>
        /// 下拉列表控件默认文本字段属性名
        /// </summary>
        public static readonly string DEFAULT_DROPDOWN_DATATEXTFIELD = "ItemText";

        /// <summary>
        /// 下拉列表控件默认值字段属性名
        /// </summary>
        public static readonly string DEFAULT_DROPDOWN_DATAVALUEFIELD = "ItemValue";

        /// <summary>
        /// 默认最大担保金额：5万元RMB
        /// </summary>
        public static readonly decimal DEFAULT_MAX_GUARANTEE_AMOUNT = 50000;

        public static readonly DateTime? DATETIME_NULL_VALUE = null;
        #endregion

        #region Win service consts
        /// <summary>
        /// 服务默认运行周期：24小时
        /// </summary>
        public const int WIN_SERVICE_DEFAULT_INTERVAl_HOUR = 24;

        /// <summary>
        /// 服务默认运行周期：1个月
        /// </summary>
        public const int WIN_SERVICE_DEFAULT_INTERVAl_MONTH = 1;
        #endregion

        #region System Security

        /// <summary>
        /// 密码恢复时格式默认长度：8
        /// </summary>
        public static readonly int DEFAULT_PASSWORD_RESET_LENGTH = 8;

        /// <summary>
        /// 密码恢复时格式中特殊字符的默认个数：1
        /// </summary>
        public static readonly int DEFAULT_PASSWORD_RESET_NONALPHANUMERIC_COUNT = 1;

        /// <summary>
        /// 默认锁定用户超时时间：20分钟
        /// </summary>
        public static readonly int DEFAULT_LOCKEDOUT_TIMEOUT = 20;

        /// <summary>
        /// 系统管理员默认用户名ID
        /// </summary>
        public static readonly int DEFAULT_SYSTEM_ADMIN_USERID = 1;

        #endregion

        #region Rad Notification提示框 属性设置

        public class NotificationSettings
        {
            /// <summary>
            /// Title:提示
            /// </summary>
            public static readonly string TITLE_SUCCESS = "提示";
            /// <summary>
            /// Title:错误
            /// </summary>
            public static readonly string TITLE_ERROR = "错误";

            /// <summary>
            /// ContentIcon：成功的Icon
            /// </summary>
            public static readonly string CONTENT_ICON_SUCCESS = "~/Content/icons/32/tick.png";
            /// <summary>
            /// ContentIcon：错误的
            /// </summary>
            public static readonly string CONTENT_ICON_ERROR = "~/Content/icons/32/cross.png";

            /// <summary>
            /// 提示消息：保存成功，窗口将自动关闭
            /// </summary>
            public static readonly string MSG_SUCCESS_SAEVED_CLOSE_WIN = "保存成功，窗口将自动关闭";

            /// <summary>
            /// 提示消息：操作成功，窗口将自动关闭
            /// </summary>
            public static readonly string MSG_SUCCESS_OPERATE_CLOSE_WIN = "操作成功，窗口将自动关闭";

            /// <summary>
            /// 提示消息：保存失败，稍后再试，窗口将自动关闭
            /// </summary>
            public static readonly string MSG_FAILED_SAEVED_CLOSE_WIN = "保存失败，稍后再试，窗口将自动关闭";

            /// <summary>
            /// 提示消息：保存失败，稍后再试
            /// </summary>
            public static readonly string MSG_FAILED_SAEVED = "保存失败，稍后再试";


            /// <summary>
            /// 提示消息：保存成功，页面将自动刷新
            /// </summary>
            public static readonly string MSG_SUCCESS_SAEVED_REFRESH = "保存成功，页面将自动刷新";

            /// <summary>
            /// 提示消息：保存成功，页面将自动跳转
            /// </summary>
            public static readonly string MSG_SUCCESS_SAEVED_REDIRECT = "保存成功，页面将自动跳转";

            /// <summary>
            /// 提示消息：操作成功，页面将自动跳转
            /// </summary>
            public static readonly string MSG_SUCCESS_OPERATE_REDIRECT = "操作成功，页面将自动跳转";

            /// <summary>
            /// 提示消息：提交成功，待审核，页面将自动跳转
            /// </summary>
            public static readonly string MSG_SUCCESS_SUBMITED_TO_AUDITTING_REDIRECT = "提交成功，待审核，页面将自动跳转";

            /// <summary>
            /// 提示消息：删除成功，页面将自动跳转
            /// </summary>
            public static readonly string MSG_SUCCESS_DELETED_REDIRECT = "删除成功，页面将自动跳转";

            /// <summary>
            /// 提示消息：参数错误，窗口将自动关闭
            /// </summary>
            public static readonly string MSG_PARAMETER_ERROR_CLOSE_WIN = "参数错误，窗口将自动关闭";

            /// <summary>
            /// 提示消息：参数错误，页面将自动跳转
            /// </summary>
            public static readonly string MSG_PARAMETER_ERROR_REDIRECT = "参数错误，页面将自动跳转";

            /// <summary>
            /// 提示消息："没有权限，窗口将自动关闭"
            /// </summary>
            public static readonly string MSG_NO_PERMISSION_CLOSE_WIN = "没有权限，窗口将自动关闭";

            /// <summary>
            /// 提示消息："没有权限，页面将自动跳转"
            /// </summary>
            public static readonly string MSG_NO_PERMISSION_REDIRECT = "没有权限，页面将自动跳转";

        }


        #endregion

        #region 实体自动编号前缀和后缀

        /// <summary>
        /// 实体自动编号前后缀
        /// </summary>
        public class EntityAutoSerialNo
        {
            /// <summary>
            /// 编号前缀
            /// </summary>
            public class SerialNoPrefix
            {
                /// <summary>
                /// 账套编号前缀
                /// </summary>
                public static readonly string COMPANY = "ZT";

                /// <summary>
                /// 供应商编号前缀
                /// </summary>
                public static readonly string SUPPLIER = "GYS";

                /// <summary>
                /// 供应商合同编号前缀
                /// </summary>
                public static readonly string SUPPLIER_CONTRACT = "GYSHT";

                /// <summary>
                /// 仓库编号前缀
                /// </summary>
                public static readonly string WAREHOUSE = "CK";

                /// <summary>
                /// 配送公司编号前缀
                /// </summary>
                public static readonly string DISTRIBUTION_COMPANY = "PSGS";

                /// <summary>
                /// 客户编号前缀
                /// </summary>
                public static readonly string CLIENT_INFO = "KH";

                /// <summary>
                /// 大包客户协议
                /// </summary>
                public static readonly string DB_CONTRACT = "DBKHXY";

                /// <summary>
                /// 采购订单编号
                /// </summary>
                public static readonly string PROCURE_ORDER = "CG-" + DateTime.Now.ToString("yyyy-MM-dd") + "-";

                /// <summary>
                /// 入库单编号
                /// </summary>
                public static readonly string STOCK_IN = "IN-" + DateTime.Now.ToString("yyyy-MM-dd") + "-";

                /// <summary>
                /// 出库单编号
                /// </summary>
                public static readonly string STOCK_OUT = "OUT-" + DateTime.Now.ToString("yyyy-MM-dd") + "-";

                /// <summary>
                /// 大包出库单编号
                /// </summary>
                public static readonly string STOCK_OUT_DABAO = "OUT-DB-" + DateTime.Now.ToString("yyyy-MM-dd") + "-";

                /// <summary>
                /// 大包配送订单编号
                /// </summary>
                public static readonly string DABAO_ORDER = "DB-" + DateTime.Now.ToString("yyyy-MM-dd") + "-";

                /// <summary>
                /// 客户订单编号
                /// </summary>
                public static readonly string CLIENT_ORDER = "KH-" + DateTime.Now.ToString("yyyy-MM-dd") + "-";

                /// <summary>
                /// 客户订单出库单编号
                /// </summary>
                public static readonly string STOCK_OUT_CLIENT = "OUT-KH-" + DateTime.Now.ToString("yyyy-MM-dd") + "-";
            }

        }

        #endregion

        #region 部门类型

        /// <summary>
        /// 常量类：部门类型
        /// </summary>
        public class DepartmentTypes
        {
            /// <summary>
            /// 基药
            /// </summary>
            public static readonly string BASE_MEDICINE = "基药";

            /// <summary>
            /// 招商
            /// </summary>
            public static readonly string BUSINESS_MEDICINE = "招商";

            /// <summary>
            /// 其他（如：人事部，行政部等）
            /// </summary>
            public static readonly string OTHER = "其他";
        }

        #endregion

        #region 部门产品销售提成类型

        /// <summary>
        /// 常量类：部门产品销售提成类型
        /// </summary>
        public class DeptProductSalesBonusTypes
        {
            /// <summary>
            /// 固定的
            /// </summary>
            public static readonly string FIXED = "固定";

            /// <summary>
            /// 浮动的
            /// </summary>
            public static readonly string FLOATED = "浮动";
        }

        #endregion

        #region 比较操作类型

        /// <summary>
        /// 常量类：比较操作类型
        /// </summary>
        public class CompareOperatorTypes
        {
            /// <summary>
            /// 大于
            /// </summary>
            public static readonly string GREATER_THAN = ">";
            /// <summary>
            /// 等于
            /// </summary>
            public static readonly string EQUAL_TO = "=";
            /// <summary>
            /// 小于
            /// </summary>
            public static readonly string LESS_THAN = "<";
        }

        #endregion

        #region Grid Column Unique Names

        /// <summary>
        /// 常量类：Grid列名
        /// </summary>
        public class GridColumnUniqueNames
        {
            /// <summary>
            /// 列名：View
            /// </summary>
            public static readonly string COLUMN_VIEW = "View";

            /// <summary>
            /// 列名：Edit
            /// </summary>
            public static readonly string COLUMN_EDIT = "Edit";

            /// <summary>
            /// 列名：Print
            /// </summary>
            public static readonly string COLUMN_PRINT = "Print";

            /// <summary>
            /// 列名：Stop
            /// </summary>
            public static readonly string COLUMN_STOP = "Stop";

            /// <summary>
            /// 列名：Delete
            /// </summary>
            public static readonly string COLUMN_DELETE = "Delete";

            /// <summary>
            /// 列名：ClientSelect
            /// </summary>
            public static readonly string COLUMN_CLIENT_SELECT = "ClientSelect";
        }


        #endregion

        #region Icons

        /// <summary>
        /// 常量类：静态图标
        /// </summary>
        public class Icons
        {
            /// <summary>
            /// 客户订单发货模式为担保模式，且担保金额未收回
            /// </summary>
            public static readonly string ICON_GUARANTEE_NOT_RECEIPTED = "~/Images/guarantee.png";

            /// <summary>
            /// 客户订单发货模式为担保模式，且担保金额已收回
            /// </summary>
            public static readonly string ICON_GUARANTEE_RECEIPTED = "~/Images/guarantee_grey.png";
        }
        #endregion

        #region 发货模式

        /// <summary>
        /// 常量类：发货模式
        /// </summary>
        public class DeliveryModes
        {
            /// <summary>
            /// 查款发货
            /// </summary>
            public static readonly string RECEIPTED_DELIVERY = "查款发货";

            /// <summary>
            /// 担保发货
            /// </summary>
            public static readonly string GUARANTEE_DELIVERY = "担保发货";
        }

        #endregion

        #region 收款方式

        /// <summary>
        /// 常量类：收款方式
        /// </summary>
        public class PaymentMethods
        {
            /// <summary>
            /// 银行转账
            /// </summary>
            public static readonly string BANK_TRANSFER = "银行转账";

            /// <summary>
            /// 客户余额抵扣
            /// </summary>
            public static readonly string DEDUCATION = "抵扣";
        }

        #endregion

        #region 排序语句

        /// <summary>
        /// 常量类：排序语句
        /// </summary>
        public class OrderByExpression
        {
            /// <summary>
            /// 创建时间倒序：CreatedOn DESC
            /// </summary>
            public static readonly string CREATEDON_DESC = "CreatedOn DESC";

            /// <summary>
            /// 结算日期倒序：SettlementDate DESC
            /// </summary>
            public static readonly string SETTLEMENTDATE_DESC = "SettlementDate DESC";


        }

        #endregion

        #region 收款单据类型

        /// <summary>
        /// 常量类：收款单据类型
        /// </summary>
        public class InvoiceCategories
        {
            /// <summary>
            /// 收据
            /// </summary>
            public static readonly string RECEIPT = "收据";

            /// <summary>
            /// 发票
            /// </summary>
            public static readonly string INVOICE = "发票";
        }

        #endregion

        #region 医院性质

        /// <summary>
        /// 常量类：医院性质
        /// </summary>
        public class HospitalTypes
        {
            /// <summary>
            /// 基药
            /// </summary>
            public static readonly string BASE_MEDICINE = "基药";

            /// <summary>
            /// 招商
            /// </summary>
            public static readonly string BUSINESS_MEDICINE = "招商";

        }

        #endregion

        #region 布尔类型汉语描述

        /// <summary>
        /// 常量类：布尔类型汉语描述
        /// </summary>
        public class BoolChineseDescription
        {
            /// <summary>
            /// True：是
            /// </summary>
            public static readonly string TRUE = "是";

            /// <summary>
            /// False：否
            /// </summary>
            public static readonly string FALSE = "否";
        }

        #endregion

        #region 支付状态

        /// <summary>
        /// 常量类：支付状态
        /// </summary>
        public class PaymentStatus
        {
            /// <summary>
            /// 待支付
            /// </summary>
            public static readonly string TO_BE_PAY = "待支付";

            /// <summary>
            /// 已支付
            /// </summary>
            public static readonly string PAID = "已支付";
        }

        #endregion

        #region 导入数据Excel列名

        /// <summary>
        /// 导入数据Excel列名
        /// </summary>
        public class ImportDataColumns
        {
            #region 流向数据列名
            /// <summary>
            /// 货品编码列
            /// </summary>
            public static readonly string PRODUCT_CODE = "货品编码";

            /// <summary>
            /// 货品名称
            /// </summary>
            public static readonly string PRODUCT_NAME = "货品名称";

            /// <summary>
            /// 规格
            /// </summary>
            public static readonly string PRODUCT_SPECIFICATION = "规格";

            /// <summary>
            /// 基本单位
            /// </summary>
            public static readonly string UNIT_NAME = "基本单位";

            /// <summary>
            /// 生产企业
            /// </summary>
            public static readonly string FACTORY_NAME = "生产企业";

            /// <summary>
            /// 销售日期
            /// </summary>
            public static readonly string SALE_DATE = "销售日期";

            /// <summary>
            /// 销售数量
            /// </summary>
            public static readonly string SALE_QTY = "销售数量";

            /// <summary>
            /// 流向
            /// </summary>
            public static readonly string FLOW_TO = "流向";

            /// <summary>
            /// 备注
            /// </summary>
            public static readonly string COMMENT = "备注";

            /// <summary>
            /// 医院
            /// </summary>
            public static readonly string HOSPITAL_NAME = "医院";

            /// <summary>
            /// 医院性质
            /// </summary>
            public static readonly string HOSPITAL_TYPE = "医院性质";

            /// <summary>
            /// 区域
            /// </summary>
            public static readonly string MARKET_NAME = "区域";

            /// <summary>
            /// 库存数量
            /// </summary>
            public static readonly string INVENTORY_BALANCE_QTY = "库存数量";

            /// <summary>
            /// 订单号
            /// </summary>
            public static readonly string ORDER_CODE = "订单号";


            #endregion

            /// <summary>
            /// 订单日期
            /// </summary>
            public static readonly string ORDER_DATE = "订单日期";
            /// <summary>
            /// 供应商
            /// </summary>
            public static readonly string SUPPLIER_NAME = "供应商";
            /// <summary>
            /// 交货日期
            /// </summary>
            public static readonly string ESTDELIVERY_DATE = "交货日期";
            /// <summary>
            /// 入库仓库
            /// </summary>
            public static readonly string STOCKIN_WAREHOUSE_NAME = "入库仓库";

            /// <summary>
            /// 基本数量
            /// </summary>
            public static readonly string PROCURE_COUNT = "基本数量";

            /// <summary>
            /// 采购单价
            /// </summary>
            public static readonly string PROCURE_PRICE = "采购单价";

            /// <summary>
            /// 采购金额
            /// </summary>
            public static readonly string PROCURE_TOTAL_AMOUNT = "采购金额";


            /// <summary>
            /// 入库单编号
            /// </summary>
            public static readonly string STOCKIN_ORDER_CODE = "入库单编号";

            /// <summary>
            /// 入库日期
            /// </summary>
            public static readonly string STOCKIN_ORDER_DATE = "入库日期";

             /// <summary>
            /// 采购订单编号
            /// </summary>
            public static readonly string PROCURE_ORDER_CODE = "采购订单编号";

            /// <summary>
            /// 货品批号
            /// </summary>
            public static readonly string BATCH_NUMBER = "货品批号";

            /// <summary>
            /// 过期日期
            /// </summary>
            public static readonly string EXPIRATION_DATE = "过期日期";

            /// <summary>
            /// 批准文号
            /// </summary>
            public static readonly string LICENSE_NUMBER = "批准文号";

             /// <summary>
            /// 抵款货物
            /// </summary>
            public static readonly string MORTGAGED_PRODUCT = "抵款货物";

            /// <summary>
            /// 基本数量
            /// </summary>
            public static readonly string STOCKIN_COUNT = "基本数量";
            

        }

        #endregion

    }
}
