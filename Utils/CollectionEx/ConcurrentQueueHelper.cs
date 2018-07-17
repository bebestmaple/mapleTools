
namespace System.Collections.Concurrent
{
    public static class ConcurrentQueueHelper
    {
       /// <summary>
       /// 清空
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="queue"></param>
       public static void Clear<T>(this ConcurrentQueue<T> queue)
       {
           T item;
           while (queue.TryDequeue(out item))
           {
               // do nothing
           }
       }
    }
}
