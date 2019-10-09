/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;

namespace MultiThreading.Task1._100Tasks
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();

            var watch = new Stopwatch();
            watch.Start();
            HundredThread();
            watch.Stop();
            var threadTime = watch.ElapsedMilliseconds;

            watch.Reset();

            watch.Start();
            HundredTasks();
            watch.Stop();

            Console.WriteLine($"Threads. Total time {threadTime} ms");
            Console.WriteLine($"Tasks. Total time {watch.ElapsedMilliseconds} ms");

            Console.ReadLine();
        }

        /// <summary>
        /// Hundreds the thread.
        /// </summary>
        private static void HundredThread()
        {
            var array = new List<Thread>();

            for (var i = 0; i < TaskAmount; i++)
            {
                int index = i;
                array.Add(new Thread(() =>
                       {
                           for (var j = 0; j < MaxIterationsCount; j++)
                           {
                               Output(index, j);
                           }
                       }));
            }

            array.ForEach(t =>
                {
                    t.Start();
                });

            array.AsParallel().ForAll(t => { t.Join(); });
        }

        /// <summary>
        /// Hundreds the tasks.
        /// </summary>
        static void HundredTasks()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < TaskAmount; i++)
            {
                int taskIndex = i;
                tasks.Add(Task.Run(() => Process(taskIndex)));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <summary>
        /// Tasks the calculation.
        /// </summary>
        /// <param name="taskIndex">Index of the task.</param>
        private static void Process(int taskIndex)
        {
            for (var j = 0; j < MaxIterationsCount; j++)
            {
                Output(taskIndex, j);
            }
        }

        /// <summary>
        /// Outputs the specified task number.
        /// </summary>
        /// <param name="taskNumber">The task number.</param>
        /// <param name="iterationNumber">The iteration number.</param>
        private static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
