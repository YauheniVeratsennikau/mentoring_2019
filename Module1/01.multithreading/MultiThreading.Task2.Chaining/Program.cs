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
    using System.Linq;
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


        //ContinuesWith
        //Use Code Standart

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

            var task = new Task(
                () =>
                    {
                        var rdm = new Random();
                        var t1_lst = new List<int>();
                        for (int i = 0; i < ArraySize; i++)
                        {
                            t1_lst.Add(rdm.Next(RandomSize));
                            Console.WriteLine($"Task#1 - iteration: {i} ; random value: {t1_lst[i]}");
                        }

                        PrintConsoleDelimiter();

                        var task2 = new Task(() =>
                            {
                                for (var j = 0; j < ArraySize; j++)
                                {
                                    var valueT2 = rdm.Next(RandomSize);
                                    t1_lst[j] *= valueT2;
                                    Console.WriteLine($"Task#2 - iteration:{j} - random value: {valueT2} - new value: {t1_lst[j]}");
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
                                                        Console.WriteLine($"Task3# - iteration: {i++} - value:{item}");
                                                    });
                                            PrintConsoleDelimiter();

                                            var task4 = new Task(
                                                () =>
                                                    {
                                                        Console.WriteLine($"Task4# - Average: {t1_lst.Average()}");
                                                    });

                                            task4.Start();
                                        });

                                task3.Start();
                            });

                        task2.Start();
                    });

            task.Start();

            Console.ReadLine();
        }

        /// <summary>
        /// Prints the console delimiter.
        /// </summary>
        private static void PrintConsoleDelimiter()
        {
            Console.WriteLine("//-----------------------------------------------------");
        }
    }
}
