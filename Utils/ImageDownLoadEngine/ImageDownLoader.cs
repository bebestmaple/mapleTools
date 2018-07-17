using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Utils.TaskEx;

namespace Utils.ImageDownLoadEngine
{
    public sealed class ImageDownLoader : BaseBehavior
    {
          /// <summary>
        /// 分析器
        /// </summary>
        public TaskSupervisor TaskSupervisor = new TaskSupervisor();

        /// <summary>
        /// 功能总开关
        /// </summary>
        public bool Control { get; set; }

        /// <summary>
        /// 对象锁
        /// </summary>
        private object ObjLock = new object();

        /// <summary>
        /// 所有的图片队列
        /// </summary>
        public ConcurrentQueue<string> ImagesQueue { get; private set; }

        private BaseBehavior ImgDownloadBehavior { get; set; }

        private BaseBehavior AnalysisBehavior { get; set; }

        public ImageDownLoader(ConcurrentQueue<string> imgQueue, BaseBehavior analysisBehavior, BaseBehavior imgDownloadBehavior)
        {
            ImagesQueue = imgQueue;
            AnalysisBehavior = analysisBehavior;
            ImgDownloadBehavior = imgDownloadBehavior;
        }



        /// <summary>
        /// 行动！！
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <param name="maxTaskNum">最大任务数量</param>
        public override async Task Action()
        {
            CancelTokenSource = new CancellationTokenSource();
            TaskSupervisor.Add(Task.Factory.StartNew(() => AnalysisBehavior.Action(), CancelTokenSource.Token));
            TaskSupervisor.Add(Task.Factory.StartNew(() => ImgDownloadBehavior.Action(), CancelTokenSource.Token));
        }



        /// <summary>
        /// 终止
        /// </summary>
        public void End()
        {
            //// 通知关闭
            CancelTokenSource.Cancel();
            TaskSupervisor.DisposeAllTask();
        }

      
    }
}
