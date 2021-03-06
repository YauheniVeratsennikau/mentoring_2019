﻿/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;

namespace MultiThreading.Task2.Chaining
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        /// <summary>
        /// The random size.
        /// </summary>
        private static readonly int RandomSize = 10;

        /// <summary>
        /// The array size
        /// </summary>
        private static readonly int ArraySize = 10;

        private static List<string> lstWatchedActionTimes = new List<string>();

        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var actions = new List<Action> { ProcessThread, ProcessTaskFactory };

            actions.ForEach(DoWatchedAction);
           
            lstWatchedActionTimes.ForEach(Console.WriteLine);

            Console.ReadLine();
        }

        private static void ProcessThread()
        {
            var task = new Task(
              () =>
              {
                  var rdm = new Random();
                  var t1_lst = new List<int>();
                  for (int i = 0; i < ArraySize; i++)
                  {
                      t1_lst.Add(rdm.Next(RandomSize));
                      Console.WriteLine($"Thread#1 - iteration: {i} ; random value: {t1_lst[i]}");
                  }

                  PrintConsoleDelimiter();

                  var task2 = new Task(() =>
                  {
                      for (var j = 0; j < ArraySize; j++)
                      {
                          var valueT2 = rdm.Next(RandomSize);
                          t1_lst[j] *= valueT2;
                          Console.WriteLine($"Thread#2 - iteration:{j} - random value: {valueT2} - new value: {t1_lst[j]}");
                      }

                      PrintConsoleDelimiter();

                      var task3 = new Task(
                            () =>
                                  {
                                      t1_lst = t1_lst.OrderBy(x => x).ToList();
                                      var i = 0;
                                      t1_lst.ForEach(
                                            item =>
                                              {
                                                  Console.WriteLine($"Thread#3 - iteration: {i++} - value:{item}");
                                              });
                                      PrintConsoleDelimiter();

                                      var task4 = new Task(
                                            () =>
                                              {
                                                  Console.WriteLine($"Thread#4 - Average: {t1_lst.Average()}");
                                              });

                                      task4.Start();
                                  });

                      task3.Start();
                  });

                  task2.Start();
              });

            task.Start();
        }


        private static void ProcessTaskFactory()
        {
            Task.Factory.StartNew(CreateRandomList)
                .ContinueWith(task => MultEachElementOfListByRandomValues(task.Result))
                .ContinueWith(task => OrderList(task.Result))
                .ContinueWith(task => CalcAverage(task.Result))
                .Wait();
        }

        private static List<int> CreateRandomList()
        {
            var rdm = new Random();
            var lstRandoms = new List<int>();
            for (int i = 0; i < ArraySize; i++)
            {
                lstRandoms.Add(rdm.Next(RandomSize));
                Console.WriteLine($"Task#1 - iteration: {i} ; random value: {lstRandoms[i]}");
            }
            
            PrintConsoleDelimiter();
            
            return lstRandoms;
        }

        private static List<int> MultEachElementOfListByRandomValues(List<int> lstRandomInt)
        {
            var rdm = new Random();
            for (var j = 0; j < ArraySize; j++)
            {
                var randomValue = rdm.Next(RandomSize);
                lstRandomInt[j] *= randomValue;
                Console.WriteLine($"Task#2 - iteration:{j} - random value: {randomValue} - new value: {lstRandomInt[j]}");
            }

            PrintConsoleDelimiter();

            return lstRandomInt;
        }

        private static List<int> OrderList(List<int> lstRandomInt)
        {
            var orderedLstRandomInt = lstRandomInt.OrderBy(x => x).ToList();
            var i = 0;
            orderedLstRandomInt.ForEach(
                item =>
                    {
                        Console.WriteLine($"Task3# - iteration: {i++} - value:{item}");
                    });
            PrintConsoleDelimiter();

            return orderedLstRandomInt;
        }

        private static Task CalcAverage(List<int> lstRandomInt)
        {
            Console.WriteLine($"Task4# - Average: {lstRandomInt.Average()}");
            PrintConsoleDelimiter();

            return Task.FromResult(lstRandomInt);
        }

        /// <summary>
        /// Prints the console delimiter.
        /// </summary>
        private static void PrintConsoleDelimiter()
        {
            Console.WriteLine($"//-------------------Thread: {Thread.CurrentThread.ManagedThreadId}-------------------");
        }

        /// <summary>
        /// Does the watched action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private static void DoWatchedAction(Action action)
        {
            var watch = new Stopwatch();
            watch.Start();
            action.Invoke();
            watch.Stop();
            lstWatchedActionTimes.Add($"Executed time of method '{action.Method.Name}' - {watch.ElapsedMilliseconds} ms.");
        }
    }
}

