using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Concurrent
{
   public static class ConcurrentBagHelper
    {
        public static void Clear<T>(this ConcurrentBag<T> bag)
        {
            T item;
            while (bag.TryTake(out item))
            {
                // do nothing.
            }
        }

        public static void ForEach<T>(this ConcurrentBag<T> bag, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var item in bag)
            {
                action(item);
            }
        }
    }
}
