using System;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.TaskEx
{
    public abstract  class BaseBehavior
    {
        /// <summary>
        /// 行动！
        /// </summary>
        public abstract Task Action();

        /// <summary>
        /// 进度同步
        /// </summary>
        /// <param name="message">同步消息</param>
        protected void Progress(string format, params object[] args)
        {
            if (TaskProgress != null)
            {
                IProgress<string> progress = TaskProgress;
                string message = string.Format("[{0}]{1}", DateTime.Now.ToString("HH:mm:ssssss"), string.Format(format, args));
                progress.Report(message + "\r\n");
            }
        }

        /// <summary>
        /// cancelToken
        /// </summary>
        public static  CancellationTokenSource CancelTokenSource = new CancellationTokenSource();

        /// <summary>
        /// 进度通知
        /// </summary>
        public  Progress<string> TaskProgress = new Progress<string>();
    }
}
