/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;

namespace MultiThreading.Task1._100Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

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

            HundredTasks();

            Console.ReadLine();
        }

        //NEW AC
        //переделать на Task
        //merge time Task vs Thread
        //----
        //cache for Task
        static void HundredTasks()
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

        static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
