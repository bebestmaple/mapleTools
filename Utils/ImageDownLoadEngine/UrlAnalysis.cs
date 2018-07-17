using System;
using System.Collections.Generic;
using Utils.HttpEx;

namespace Utils.ImageDownLoadEngine
{
    internal sealed class UrlAnalysis
    {
        /// <summary>
        /// 分析该URL下面所有的url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="PushWaitUrlList"></param>
        /// <param name="PushWaitImagesList"></param>
       public static void AnalysisUrls(string url, Action<IEnumerable<string>> PushWaitUrlList, Action<IEnumerable<string>> PushWaitImagesList)
        {
            string originalHtml = HttpHelper.GetHtml(url);

            //// 解析原始的url
            PushWaitUrlList(HttpHelper.GetUrlFromHtml(originalHtml));

            //// 解析原始的图片URL
            PushWaitImagesList(HttpHelper.GetImageUrlListFromHtml(originalHtml));
        }
    }
}
