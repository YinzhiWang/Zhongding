namespace ZhongDing.Common.NPOIHelper.Excel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class HeadInfo
    {
        /// <summary>
        /// height默认不要设置值，如果设置请设置为 x * 256 x为字符数量 256是一个字符的高度，因为这里不是以像素为单位的，字符的1/256为单位
        /// </summary>
        public int? defaultheight { get; set; }
        /// <summary>
        /// width的单位和 height一样，设置的时候要注意了
        /// </summary>
        public int? defaultwidth { get; set; }
        /// <summary>
        /// 具体的表头单元格信息
        /// </summary>
        public IList<AttributeList> head { get; set; }
        /// <summary>
        /// 表头的行数
        /// </summary>
        public int? rowspan { get; set; }
        /// <summary>
        /// excel sheet的名称
        /// </summary>
        public string sheetname { get; set; }
    }
}

