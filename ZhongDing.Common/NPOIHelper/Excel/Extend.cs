namespace ZhongDing.Common.NPOIHelper.Excel
{
    using NPOI.SS.UserModel;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class Extend
    {
        public static void Export<T>(this IEnumerable<T> source, string path)
        {
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> list, string tableName)
        {
            List<PropertyInfo> pList = new List<PropertyInfo>();
            Type type = typeof(T);
            DataTable dt = new DataTable
            {
                TableName = tableName
            };
            Array.ForEach<PropertyInfo>(type.GetProperties(), delegate(PropertyInfo p)
            {
                pList.Add(p);
                dt.Columns.Add(p.Name, p.PropertyType);
            });
            using (IEnumerator<T> enumerator = list.GetEnumerator())
            {
                T item;
                while (enumerator.MoveNext())
                {
                    item = enumerator.Current;
                    DataRow row = dt.NewRow();
                    pList.ForEach(delegate(PropertyInfo p)
                    {
                        row[p.Name] = p.GetValue(item, null);
                    });
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        public static DataTable ToDataTable<T>(this List<T> list, string[] titles, params Func<T, string>[] fieldFuncs)
        {
            DataTable table;
            DataRow row;
            int num;
            DataRow row2;
            if (fieldFuncs.Length > 0)
            {
                if ((titles == null) || (fieldFuncs.Length != titles.Length))
                {
                    throw new Exception("titles不能为空且必须与导出字段一一对应");
                }
                table = new DataTable();
                row = table.NewRow();
                num = 0;
                while (num < fieldFuncs.Length)
                {
                    table.Columns.Add(new DataColumn());
                    row[num] = titles[num];
                    num++;
                }
                table.Rows.Add(row);
                foreach (T local in list)
                {
                    row2 = table.NewRow();
                    for (num = 0; num < fieldFuncs.Length; num++)
                    {
                        row2[num] = fieldFuncs[num](local);
                    }
                    table.Rows.Add(row2);
                }
                return table;
            }
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (properties.Length != titles.Length)
            {
                throw new Exception("titles不能为空且必须与导出字段一一对应");
            }
            table = new DataTable();
            row = table.NewRow();
            num = 0;
            while (num < properties.Length)
            {
                PropertyInfo info = properties[num];
                table.Columns.Add(new DataColumn());
                row[num] = titles[num];
                num++;
            }
            table.Rows.Add(row);
            foreach (T local in list)
            {
                row2 = table.NewRow();
                for (num = 0; num < table.Columns.Count; num++)
                {
                    row2[num] = properties[num].GetValue(local, null);
                }
                table.Rows.Add(row2);
            }
            return table;
        }

        public static HorizontalAlignment ToHorAlign(this string str)
        {
            switch (str.ToLower())
            {
                case "center":
                    return HorizontalAlignment.Center;

                case "left":
                    return HorizontalAlignment.Left;

                case "right":
                    return HorizontalAlignment.Right;
            }
            return HorizontalAlignment.Center;
        }

        public static int[] ToIntArray(this string[] region)
        {
            ArrayList list = new ArrayList();
            foreach (string str in region)
            {
                list.Add(Convert.ToInt32(str));
            }
            return (int[])list.ToArray(typeof(int));
        }

        public static VerticalAlignment ToVerAlign(this string str)
        {
            switch (str.ToLower())
            {
                case "center":
                    return VerticalAlignment.Center;

                case "top":
                    return VerticalAlignment.Top;

                case "bottom":
                    return VerticalAlignment.Bottom;
            }
            return VerticalAlignment.Center;
        }
    }
}

