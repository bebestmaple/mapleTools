using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Utils.HttpEx
{
    public sealed class HttpHelper
    {
        /// <summary>
        /// 获取指定Url的HTML
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            try
            {
                //创建一个请求
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
                webReq.KeepAlive = false;
                webReq.Method = "GET";
                webReq.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:19.0) Gecko/20100101 Firefox/19.0";
                webReq.ServicePoint.Expect100Continue = false;
                webReq.Timeout = 5000;
                webReq.AllowAutoRedirect = true;//是否允许302
                ServicePointManager.DefaultConnectionLimit = 20;
                //获取响应
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                string content = string.Empty;
                using (System.IO.Stream stream = webRes.GetResponseStream())
                {
                    using (System.IO.StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8")))
                    {
                        content = reader.ReadToEnd();
                    }
                }
                webReq.Abort();
                return content;
            }
            catch (Exception)
            {
                return "";
            }

        }


        /// <summary>   
        /// 取得HTML中所有图片的 URL
        /// </summary>   
        /// <param name="sHtmlText">HTML代码</param>   
        /// <returns>图片的URL列表</returns>   
        public static List<string> GetImageUrlListFromHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return new List<string>();
            }
            // 定义正则表达式用来匹配 img 标签   
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(html);
            List<string> sUrlList = new List<string>();

            // 取得匹配项列表   
            foreach (Match match in matches)
                sUrlList.Add(match.Groups["imgUrl"].Value);
            return sUrlList;
        }

        /// <summary>
        /// 提取页面链接
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<string> GetUrlFromHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return new List<string>();
            }
            List<string> links = new List<string>();

            const string urlReg = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
            Regex r2 = new Regex(urlReg, RegexOptions.IgnoreCase);

            //获得匹配结果
            MatchCollection m2 = r2.Matches(html);
            
            foreach (Match url in m2)
            {
                links.Add(url.ToString());
            }

            //匹配href里面的链接
            const string hrefReg = @"(?i)<a\s[^>]*?href=(['""]?)(?!javascript|__doPostBack)(?<url>[^'""\s*#<>]+)[^>]*>"; ;
            Regex r = new Regex(hrefReg, RegexOptions.IgnoreCase);
            //获得匹配结果
            MatchCollection m = r.Matches(html);
            foreach (Match url in m)
            {
                links.Add(url.Groups["url"].Value);
            }
            return links;
        }

    }
}
