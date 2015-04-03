namespace ZhongDing.Common.NPOIHelper.Excel
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 使用说明见下边注释,下边注释是详细字段使用范例 类似于css属性的设置
    /// </summary>
    /*"title": "排序", 
        "align": "center", 
        "bgcolor": "#820C5F", 
        "fontsize": 14, 
        "fontcolor": "#FBEC00", 
        "cellregion": "0,1,0,0", 
        "width": 30*/
    public class AttributeList
    {
        public string align { get; set; }

        public string bgcolor { get; set; }

        public string cellregion { get; set; }

        public string fontcolor { get; set; }

        public string fontName { get; set; }

        public short? fontsize { get; set; }

        public int? height { get; set; }

        public bool? IsItalic { get; set; }

        public bool? IsStrikeout { get; set; }

        public string title { get; set; }

        public bool? Underline { get; set; }

        public string valign { get; set; }

        public int? width { get; set; }
    }
}

