using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker_I
{
    class Program
    {
        private static Random _random = new Random();
        private static int _queuedJobsCount = 1;
        private static PriorityQueue<IJob> priorityQueue = new PriorityQueue<IJob>();
        private static ConsoleColor DefaultForegroundColor = Console.ForegroundColor;
        private static int _jobsLeft = 5000;

        static void CreateScheduledTask()
        {
            List<ScheduledTask> sTasks = new List<ScheduledTask>();

            sTasks.Add(new ScheduledTask(DateTime.Now.AddSeconds(5), 10));
            sTasks.Add(new ScheduledTask(DateTime.Now.AddSeconds(28), 0));
                       
            ThreadPool.QueueUserWorkItem(work =>
            {
                while (true)
                {
                    for (int i = 0; i < sTasks.Count; i++)
                    {
                        ScheduledTask st = sTasks[i];
                        if (st.DT <= DateTime.Now)
                        {                            
                            Console.WriteLine($"Создана задача {st.GetName()}");

                            ThreadPool.QueueUserWorkItem(w => st.Execute());

                            if (st.interval > 0)
                            {
                                sTasks.Add(new ScheduledTask(st.DT.AddSeconds(st.interval), st.interval));
                            }

                            sTasks.Remove(st);
                        }
                    }
                }
            });


        }


        static void CreateRandomJob()
        {
            IJob[] jobs = new IJob[]
            {
                new CalculatePiJob(),
                new CalculateExpJob(),
                new GetNewGuid()
            };


            while (_jobsLeft != 0)
            {
                var nextPriority = (PriorityEnum)_random.Next(0, 3);
                var nextJob = jobs[_random.Next(0, 3)];

                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"Создана задача # {_queuedJobsCount}");
                Console.WriteLine($"Тип задачи - {nextJob.GetName()}");
                Console.WriteLine($"Приоритет задачи - {nextPriority}");

                Console.ForegroundColor = DefaultForegroundColor;

                priorityQueue.Enqueue(nextJob, nextPriority);

                _queuedJobsCount++;
                _jobsLeft--;

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }



        public static void ProcessJobsQueue()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            while (true)
            {
                var jobToProcess = priorityQueue.Dequeue();
                if (jobToProcess.Item1 != null)
                {
                    Console.WriteLine($"Поток # {Thread.CurrentThread.ManagedThreadId} получил" +
                    $"на обработку задачу {jobToProcess.Item1.GetName()} с приоритетом {jobToProcess.Item2}");

                    ThreadPool.QueueUserWorkItem(work => jobToProcess.Item1.Execute());
                }
                else
                {
                    Console.WriteLine("Задач не найдено, ожидаем!");
                }

                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }
        static void Main(string[] args)
        {
            //ThreadPool.QueueUserWorkItem(work => CreateRandomJob());
            //ThreadPool.QueueUserWorkItem(work => CreateRandomJob());
            //ThreadPool.QueueUserWorkItem(work => CreateRandomJob());
            //ThreadPool.QueueUserWorkItem(work => CreateRandomJob());
            //ThreadPool.QueueUserWorkItem(work => CreateRandomJob());

            //Thread.Sleep(15000);

            //ThreadPool.QueueUserWorkItem(work => ProcessJobsQueue());
            //ThreadPool.QueueUserWorkItem(work => ProcessJobsQueue());
            //ThreadPool.QueueUserWorkItem(work => ProcessJobsQueue());
            //ThreadPool.QueueUserWorkItem(work => ProcessJobsQueue());
            //ThreadPool.QueueUserWorkItem(work => ProcessJobsQueue());

            CreateScheduledTask();

            Console.ReadLine();
        }
    }
}
