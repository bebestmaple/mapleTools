using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Utils.HttpEx;
using Utils.TaskEx;

namespace Utils.ImageDownLoadEngine
{
    public sealed class ImageDownloadBehavior : BaseBehavior
    {
        /// <summary>
        /// 待下载的图片列表
        /// </summary>
        private ConcurrentQueue<string> WaitDownLoadImageUrlQueue = new ConcurrentQueue<string>();

        /// <summary>
        /// 已存在的图片列表
        /// </summary>
        private ConcurrentBag<string> ExistImageUrlList = new ConcurrentBag<string>();

        /// <summary>
        /// 任务管理器
        /// </summary>
        private TaskEx.TaskSupervisor TaskSupervisor { get; set; }

        private string SaveFolder { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxTaskNum">最大任务数</param>
        /// <param name="imageQueue">待下载</param>
        public ImageDownloadBehavior(int maxTaskNum, ConcurrentQueue<string> imageQueue, string saveFolder)
        {
            Progress("[图片下载器]正在初始化...");
            WaitDownLoadImageUrlQueue = imageQueue;
            TaskSupervisor = new TaskEx.TaskSupervisor(maxTaskNum);
            SaveFolder = saveFolder;
        }

        /// <summary>
        /// 开始分析
        /// </summary>
        public override async Task Action()
        {
            try
            {
                DownloadHelper downloadHelper = new DownloadHelper();
                while (!CancelTokenSource.IsCancellationRequested)
                {
                    TaskSupervisor.DisposeInvalidTask();

                    if (WaitDownLoadImageUrlQueue.Count < 1)
                    {
                        Progress("[图片下载器]图片队列为空...");
                        await Task.Delay(1000);
                        continue;
                    }

                    string imageUrl = Dequeue();
                    if (!ExistImageUrlList.Contains(imageUrl))
                    {

                        Task task = new Task(() => { Progress("[图片下载器]正在下载图片URL:{0}\t保存路径为:{1}", imageUrl, downloadHelper.DownLoadImage(imageUrl, SaveFolder)); }, CancelTokenSource.Token);
                        bool result = TaskSupervisor.Add(task);
                        if (!result)
                        {
                            Progress("[图片下载器]下载队列已满,等待...", WaitDownLoadImageUrlQueue.Count);
                            await Task.Delay(1000);
                            WaitDownLoadImageUrlQueue.Enqueue(imageUrl);
                            continue;
                        }
                        ExistImageUrlList.Add(imageUrl);
                    }
                }

                TaskSupervisor.DisposeInvalidTask();
            }
            catch (Exception ex)
            {
                Progress("[图片下载器]发生异常：" + ex.ToString());
            }

        }

        /// <summary>
        /// 获取下一个目标
        /// </summary>
        /// <returns>图片地址</returns>
        private string Dequeue()
        {
            string result = "";
            WaitDownLoadImageUrlQueue.TryDequeue(out result);
            return result;
        }
    }
}
