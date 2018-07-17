using System;
using System.Text.RegularExpressions;

namespace Utils.HttpEx
{
    public sealed class UrlHelper
    {
        /// <summary>
        /// 判断一个字符串是否为url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUrl(string str)
        {
            try
            {
                string reg = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                return Regex.IsMatch(str, reg);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool IsJs(string url)
        {
            return url.EndsWith(".js",StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsCss(string url)
        {
            return url.EndsWith(".css", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsImg(string url)
        {
            string reg = @".*(\.png|\.jpg|\.jpeg|\.gif)$";
            return Regex.IsMatch(url, reg);
        }
    }
}
