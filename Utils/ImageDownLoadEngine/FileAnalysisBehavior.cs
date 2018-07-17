using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Utils.HttpEx;
using Utils.TaskEx;

namespace Utils.ImageDownLoadEngine
{
    public sealed class FileAnalysisBehavior : BaseBehavior
    {
        /// <summary>
        /// 待下载的图片Url队列
        /// </summary>
       private ConcurrentQueue<string> WaitImageUrlQueue { get; set; }


        public string FilePath { get; private set; }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="filePath">存储url的文件</param>
       /// <param name="imageQueue">存放图片url的队列</param>
        public FileAnalysisBehavior(string filePath, ConcurrentQueue<string> imageQueue)
        {
            Progress("[文件分析器]正在初始化...");
            // 初始化任务管理器
            WaitImageUrlQueue = imageQueue;
            FilePath = filePath;
        }

        /// <summary>
        /// 行动
        /// </summary>
        public override async Task Action()
        {
            while (!CancelTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (File.Exists(FilePath))
                    {
                        string url;
                        using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (StreamReader read = new StreamReader(fs, Encoding.Default))
                            {
                                Progress("[文件分析器]正在读取文件...");
                                int count = 1;
                                while (!string.IsNullOrWhiteSpace((url = await read.ReadLineAsync())))
                                {
                                    while (true)
                                    {
                                        if (WaitImageUrlQueue.Count <= 500)
                                        {
                                            Progress("[文件分析器]正在读取行[{0}]：{1}", count, url);
                                            if (UrlHelper.IsImg(url))
                                            {
                                                WaitImageUrlQueue.Enqueue(url);
                                            }
                                            else
                                            {
                                                Progress("[文件分析器]行({0})：{1}不是合法的图片链接！", count, url);
                                            }
                                            count++;
                                            break;
                                        }
                                    }
                                }
                                Progress("[文件分析器]读取文件结束...");
                            }
                        }
                    }
                    else
                    {
                        Progress("[文件分析器]文件({0})不存在！", FilePath);
                    }
                }
                catch (Exception ex)
                {
                    Progress("[文件分析器]发生异常：" + ex.ToString());
                }
            }

        }
    }

}
