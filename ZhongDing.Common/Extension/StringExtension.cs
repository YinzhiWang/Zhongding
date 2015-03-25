using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZhongDing.Common.Extension
{
    public static class StringExtension
    {
        #region Url编码，发短信的时候加密中文消息，避免乱码 UrlEncode():string
        /// <summary>
        /// UrlEncode,加密中文，避免乱码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return System.Web.HttpUtility.UrlEncode(url);
        }
        public static string UrlDecode(this string url)
        {
            return System.Web.HttpUtility.UrlDecode(url);
        }
        #endregion

        public static string FormatLineForHtml(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return s.Replace("\r\n", "<br/>").Replace("\n", "<br/>");
        }

        public static string[] ToArrayByLine(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return new string[0];
            return s.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static bool IsNullOrEmpty(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return true;
            return false;
        }
        public static bool IsNotNullOrEmpty(this string s)
        {
            if (!string.IsNullOrEmpty(s))
                return true;
            return false;
        }
        public static bool HasValue(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            if (string.IsNullOrWhiteSpace(s))
                return false;
            return true;
        }
        public static bool IsNullOrWhiteSpace(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return true;
            return false;
        }
        public static string Substring(this string s, int start, int length, char placeholder)
        {
            placeholder = placeholder == null ? ' ' : placeholder;
            s = s ?? string.Empty;
            if (s.Length < length)
            {
                s = s.PadRight(length, placeholder);
            }
            else
            {
                s = s.Substring(start, length);
            }
            return s;
        }

        public static string LastPathFloder(this string strFloderPath)
        {
            if (string.IsNullOrEmpty(strFloderPath))
                return null;
            return strFloderPath.Substring(strFloderPath.LastIndexOf('\\') + 1);
        }
        public static string ConvertToChineseMoney(this string money)
        {
            double _money = money.ToDouble();
            if (_money < 0)
                throw new ArgumentOutOfRangeException("参数money不能为负值！");
            string s = _money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            s = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(s, ".", delegate(Match m) { return "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString(); });
        }

        public static string FileName(this string filePath)
        {
            if (filePath != null && filePath.Length > 0)
            {
                return filePath.Substring(filePath.LastIndexOf('\\') + 1);
            }
            return string.Empty;
        }
        public static string FileNameWithoutExt(this string filePath)
        {
            if (filePath != null && filePath.Length > 0)
            {
                string temp = filePath.FileName();
                return temp.Substring(0, temp.LastIndexOf('.'));
            }
            return string.Empty;
        }

        public static string TrimNewLine(this string str)
        {
            if (str != null)
            {
                return str.Trim('\r', '\n');
            }
            return str;
        }
        public static string RelativePath(this string str, string rootPath)
        {
            if (str != null)
                str = str.Replace(rootPath, "").Trim('\\');
            return str;
        }


        /// <summary>
        /// 字符串转为UniCode码字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StringToUnicode(this string s)
        {
            if (s.IsNullOrEmpty())
                return string.Empty;
            char[] charbuffers = s.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(String.Format("\\u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }
        /// <summary>
        /// Unicode字符串转为正常字符串
        /// </summary>
        /// <param name="srcText"></param>
        /// <returns></returns>
        public static string UnicodeToString(this string srcText)
        {
            if (srcText.IsNullOrEmpty())
                return string.Empty;

            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;
            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }

        public static bool IsEmail(this string str_Email)
        {
            string pattern = @"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$";
            return System.Text.RegularExpressions.Regex.IsMatch(str_Email, pattern);
        }

        public static bool IsCellPhoneNumberUsername(this string str)
        {
            string num = @"^\d{11}$";
            return Regex.IsMatch(str, num);
        }


    }

}