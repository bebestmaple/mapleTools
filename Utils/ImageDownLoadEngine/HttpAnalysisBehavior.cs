using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.TaskEx;

namespace Utils.ImageDownLoadEngine
{
    public  class HttpAnalysisBehavior : BaseBehavior
    {
        /// <summary>
        /// 等待分析的Url队列
        /// </summary>
        private ConcurrentQueue<string> WaitUrlQueue = new ConcurrentQueue<string>();

        /// <summary>
        /// 待下载的图片Url队列
        /// </summary>
        private ConcurrentQueue<string> WaitImageUrlQueue { get; set; }

        /// <summary>
        /// 已经分析过的Url
        /// </summary>
        private ConcurrentBag<string> ExistUrlList { get; set; }

        /// <summary>
        /// 任务管理器
        /// </summary>
        private TaskSupervisor TaskSupervisor { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">初始的URL</param>
        /// <param name="maxTaskNum">最大任务数</param>
        /// <param name="imageQueue">图片队列</param>
        public HttpAnalysisBehavior(string url, int maxTaskNum, ConcurrentQueue<string> imageQueue)
        {
            //// 添加第一个URL
            WaitUrlQueue.Enqueue(url);
            //// 初始化任务管理器
            TaskSupervisor = new TaskSupervisor(maxTaskNum);
            WaitImageUrlQueue = imageQueue;
            ExistUrlList = new ConcurrentBag<string>();
        }

        /// <summary>
        /// 行动
        /// </summary>
        public override async Task Action()
        {
            while (!CancelTokenSource.IsCancellationRequested)
            {
                TaskSupervisor.DisposeInvalidTask();
                if (WaitUrlQueue.Count < 1 || WaitImageUrlQueue.Count > 500)
                {
                    base.Progress("[分析器休眠]待分析URL:{0},图片待下载:{1}。", WaitUrlQueue.Count, WaitImageUrlQueue.Count);
                    await Task.Delay(2000);
                    continue;
                }
                string url = "";
                if (WaitUrlQueue.TryDequeue(out url) && !ExistUrlList.Contains(url))
                {
                    Task task = new Task(delegate { UrlAnalysis.AnalysisUrls(url, PushWaitUrlList, PushWaitImagesList); }, CancelTokenSource.Token);
                    if (!TaskSupervisor.Add(task))
                    {
                        base.Progress("[分析器休眠]分析队列已满,等待...", WaitImageUrlQueue.Count);
                        await Task.Delay(2000);
                        WaitUrlQueue.Enqueue(url);
                        continue;
                    }
                    ExistUrlList.Add(url);
                }
            }

            TaskSupervisor.DisposeInvalidTask();
        }

        /// <summary>
        /// 追加url至列表中
        /// </summary>
        /// <param name="list">列表</param>
        protected void PushWaitUrlList(IEnumerable<string> list)
        {
            foreach (string url in list)
            {
                WaitUrlQueue.Enqueue(url);
            }
        }

        /// <summary>
        /// 追加待下载的图片至数据集中
        /// </summary>
        /// <param name="list">待下载的图片</param>
        protected void PushWaitImagesList(IEnumerable<string> list)
        {
            foreach (string url in list)
            {
                WaitImageUrlQueue.Enqueue(url);
            }
        }

    }
}
