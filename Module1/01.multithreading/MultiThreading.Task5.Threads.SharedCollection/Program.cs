/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        private static int MaxQueueCount = 100;
        private static Semaphore Sync = new Semaphore(1, 1);
        private static ConcurrentQueue<int> concurrentQueue = new ConcurrentQueue<int>();

        private static object syncObj = new object();
        private static List<int> sharedList = new List<int>();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

             SyncByShemaphore();

           // SyncByLock();

            Console.WriteLine("The End.");
            Console.ReadLine();
        }

        #region Lock
        private static void SyncByLock()
        {
            Console.WriteLine("Start processing with sync by 'lock'");

            Parallel.Invoke(FillSharedList, PrintLastElementFromSharedList);
        }

        private static void FillSharedList()
        {
            var random = new Random();
            for (var i = 0; i < MaxQueueCount; i++)
            {
                lock (syncObj)
                {
                    var value = random.Next(100);
                    Console.WriteLine($"Task1. Value was added to queue: {value}");

                    sharedList.Add(value);
                }

                Thread.Sleep(100);
            }
        }

        private static void PrintLastElementFromSharedList()
        {
            var printedIndex = 0;
            while (printedIndex != MaxQueueCount)
            {
                lock (syncObj)
                {
                    if (sharedList.Any() && printedIndex != sharedList.Count)
                    {
                        printedIndex = sharedList.Count;
                        Console.WriteLine($"Task2. Last element of queue: {sharedList.Last()}; current index: {printedIndex}");
                        Thread.Sleep(150);
                    }
                    else
                    {
                       // Console.WriteLine($"Try to print the same value from queue by index: {printedIndex}");
                        Thread.Sleep(200);
                    }
                }
            }
        }
        #endregion lock

        #region Semaphore
        private static void SyncByShemaphore()
        {
            Console.WriteLine("Start processing with sync by 'semaphore'");
            Parallel.Invoke(FillTheConcurrentQueue, PrintLastElementFromConcurrentQueue);
            //UsingTaskForProcessing();
        }

        private static void UsingTaskForProcessing()
        {
            var taskToFill = new TaskFactory().StartNew(FillTheConcurrentQueue);
            var taskToPrint = new TaskFactory().StartNew(PrintLastElementFromConcurrentQueue);

            Task.WaitAll(taskToFill, taskToPrint);
        }

        private static void FillTheConcurrentQueue()
        {
            var random = new Random();
            for (var i = 0; i < MaxQueueCount; i++)
            {
                Sync.WaitOne();
                var value = random.Next(100);
                Console.WriteLine($"Task1. Value was added to queue: {value}");
                concurrentQueue.Enqueue(value);
                Thread.Sleep(200);
                Sync.Release(1);
            }
        }

        private static void PrintLastElementFromConcurrentQueue()
        {
            var printedQueueIndex = 0;
            while (printedQueueIndex != MaxQueueCount)
            {
                Sync.WaitOne(1000);
                if (!concurrentQueue.IsEmpty && printedQueueIndex != concurrentQueue.Count)
                {
                    printedQueueIndex = concurrentQueue.Count;
                    Console.WriteLine($"Task2. Last element of queue: {concurrentQueue.Last()}; current index: {printedQueueIndex}");
                }
                else
                {
                    Console.WriteLine($"Try to print the same value from queue by index: {printedQueueIndex}");
                    Thread.Sleep(200);
                }

                Sync.Release(1);
            }
        }
        #endregion Semaphore
    }
}

