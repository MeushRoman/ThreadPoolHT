using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker_I
{
    public interface IJob
    {
        void Execute();
        string GetName();
    }

    public class CalculatePiJob : IJob
    {       

        public void Execute()
        {
            Thread.Sleep(5000);
            Console.WriteLine("PI is 4!");
        }

        public string GetName()
        {
            return "Считаем PI";
        }
    }

    public class CalculateExpJob : IJob
    {
        public void Execute()
        {
            Thread.Sleep(15000);
            Console.WriteLine("E is 2!");
        }

        public string GetName()
        {
            return "Считаем EXP";
        }
    }

    public class GetNewGuid : IJob
    {
        public void Execute()
        {
            Console.WriteLine(Guid.NewGuid().ToString());
        }

        public string GetName()
        {
            return "Считаем GUID";
        }
    }

    public class ScheduledTask : IJob
    {
        public DateTime DT { get; set; }
        public int interval { get; set; }

        public ScheduledTask():this(DateTime.Now, 0) { }        
        public ScheduledTask(DateTime dateTime) : this(dateTime, 0) { }
        public ScheduledTask(DateTime dateTime, int interval)
        {
            this.DT = dateTime;
            this.interval = interval;           
        }

        public void Execute()
        {
            Thread.Sleep(5000);
            Console.WriteLine($"{DT}");
        }

        public string GetName()
        {
            return $"Выполняется задача {DT}" ;
        }
    }
}