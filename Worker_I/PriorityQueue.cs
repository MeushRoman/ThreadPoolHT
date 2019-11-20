using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker_I
{
    public enum PriorityEnum
    {
        High, Medium, Low
    }
    public class PriorityQueue<T> where T : IJob
    {
        private Dictionary<PriorityEnum, Queue<T>> _jobs;
        private object _locker = new object();
        public PriorityQueue()
        {
            _jobs = new Dictionary<PriorityEnum, Queue<T>>()
            {
                { PriorityEnum.High, new Queue<T>() },
                { PriorityEnum.Medium, new Queue<T>() },
                { PriorityEnum.Low, new Queue<T>() },
            };
        }

        public (T, PriorityEnum) Dequeue()
        {
            lock (_locker)
            {
                if (_jobs[PriorityEnum.High].Any())
                    return (_jobs[PriorityEnum.High].Dequeue(), PriorityEnum.High);
                else if (_jobs[PriorityEnum.Medium].Any())
                    return (_jobs[PriorityEnum.Medium].Dequeue(), PriorityEnum.Medium);
                else if (_jobs[PriorityEnum.Low].Any())
                    return (_jobs[PriorityEnum.Low].Dequeue(), PriorityEnum.Low);

                else return (default(T), PriorityEnum.Low);
            }     
        }

        public void Enqueue(T job, PriorityEnum priority)
        {
            lock (_locker)
            {
                _jobs[priority].Enqueue(job);
            }  
        } 
    }
}
