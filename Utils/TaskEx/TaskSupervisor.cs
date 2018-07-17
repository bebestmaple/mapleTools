using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utils.TaskEx
{
    /// <summary>
    /// 任务管理器
    /// </summary>
    public  class TaskSupervisor
    {

        /// <summary>
        /// 构造
        /// </summary>
        public TaskSupervisor()
        {
            MaxTaskNum = 10;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="maxTaskNum">最大线程数</param>
        public TaskSupervisor(int maxTaskNum)
        {
            MaxTaskNum = maxTaskNum;
        }

        /// <summary>
        /// 最大线程数量
        /// </summary>
        public int MaxTaskNum { get; private set; }

        /// <summary>
        /// Task List
        /// </summary>
        private ConcurrentBag<Task> TaskList = new ConcurrentBag<Task>();

        /// <summary>
        /// 释放所有无效任务
        /// </summary>
        public void DisposeInvalidTask()
        {
            var disposeList = TaskList.Where(task => task.IsCompleted || task.IsCanceled || task.IsFaulted).ToList();

            foreach (var task in disposeList)
            {
                task.Dispose();
                Task disposeTask;
                TaskList.TryTake(out disposeTask);
            }
        }

        /// <summary>
        /// 释放线程
        /// </summary>
        public void DisposeAllTask()
        {
            foreach (var task in TaskList)
            {
                while (task.IsCanceled || task.IsCompleted || task.IsFaulted)
                {
                    task.Dispose();
                }
            }
        }

        /// <summary>
        /// 新增一个任务
        /// </summary>
        /// <param name="task">任务</param>
        public bool Add(Task task)
        {
            if (TaskList.Count < MaxTaskNum)
            {
                if (task.Status == TaskStatus.Created)
                {
                    TaskList.Add(task);
                    task.Start();
                    return true;
                }
            }

            DisposeInvalidTask();
            return false;
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <returns>数量</returns>
        public int Count()
        {
            DisposeInvalidTask();
            return TaskList.Count;
        }

        /// <summary>
        /// 开始全部
        /// </summary>
        public void StartAll()
        {
            foreach (var task in TaskList)
            {
                task.Start();
            }
        }
    }
}
