/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;

namespace MultiThreading.Task4.Threads.Join
{
    using System.Threading;

    class Program
    {
        private static int queueSize = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            ProcessReqursiveThreads(queueSize);
            ProcessReqursiveThreadPool(queueSize);
            Console.ReadLine();
        }

        private static void ProcessReqursiveThreads(int queueIndex)
        {
            var thread = new Thread(() => StartReqursiveThread(queueIndex));
            thread.Start();
            thread.Join();
        }

        private static void StartReqursiveThread(int queueIndex)
        {
            queueIndex--;
            Console.WriteLine($"Current index of thread: {queueIndex}");
            if (queueIndex > 0)
            {
                ProcessReqursiveThreads(queueIndex);
            }
        }

        private static Semaphore Sync = new Semaphore(3,3);

        private static void ProcessReqursiveThreadPool(int queueIndex)
        {
            ThreadPool.QueueUserWorkItem(ThreadSyncWithSemaphore, queueIndex);
        }

        private static void ThreadSyncWithSemaphore(object oQueueIndex)
        {
            if (int.TryParse(oQueueIndex?.ToString(), out var queueIndex))
            {
                queueIndex--;
                Console.WriteLine($"wants to access: {queueIndex}");
                Sync.WaitOne(1000);
                Console.WriteLine($"Current index of thread: {queueIndex}");
                if (queueIndex > 0)
                {
                    Thread.Sleep(3000);
                    ProcessReqursiveThreadPool(queueIndex);
                }

                Console.WriteLine($"dispose resource: {queueIndex}");
                Sync.Release(1);
            }
        }
    }
}
