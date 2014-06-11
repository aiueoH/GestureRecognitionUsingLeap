/*
 * Fixed Sized Queue
 * Automatically dequeus oldest element whene the queue is full.
 * 
 * ref : http://stackoverflow.com/questions/5852863/fixed-size-queue-which-automatically-dequeues-old-values-upon-new-enques
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR
{
    public class FixedSizedQueue<T>
    {
        //private ConcurrentQueue<T> q = new ConcurrentQueue<T>();
        //private ConcurrentBag<T> b;

        private Queue<T> q = new Queue<T>();
        private Queue<T> b;

        public int Limit { get; set; }

        public FixedSizedQueue(int limit)
        {
            Limit = limit;
        }

        public void Enqueue(T obj)
        {
            q.Enqueue(obj);
            lock (this)
            {
                //T overflow;
                while (q.Count > Limit && q.Dequeue() != null) ;
            }
        }
    }
}
