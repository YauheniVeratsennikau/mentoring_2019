/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;

namespace MultiThreading.Task6.Continuation
{
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();
            Console.WriteLine("Put the letter to choose a way of processing:");

            var letter = string.Empty;
            while (!letter.Equals("e"))
            {
                letter = Console.ReadLine();
                var taskFactory = new TaskFactory();
                switch (letter)
                {
                    case "a":
                        taskFactory.StartNew(PrintTenNumbers)
                            .ContinueWith(PrintString, TaskContinuationOptions.DenyChildAttach)
                            .ContinueWith(ContinuousActionMessage);
                        break;

                    case "b":
                        var ctSource = new CancellationTokenSource();
                        var ct = ctSource.Token;
                        taskFactory.StartNew(
                            () =>
                                {
                                    Console.WriteLine("Print ten thousands numbers by main task");

                                    var random = new Random();

                                    for (var i = 0; i <= 10000; i++)
                                    {
                                        Console.WriteLine(random.Next(10));
                                        if (ct.IsCancellationRequested)
                                        {
                                            Console.WriteLine("Task was canceled");
                                            break;
                                        }

                                        // ct.ThrowIfCancellationRequested();
                                    }
                                },
                            ct).ContinueWith(PrintSecondString, default(CancellationToken))
                               .ContinueWith(ContinuousActionMessage, default(CancellationToken));
                        Thread.Sleep(100);
                        ctSource.Cancel();
                        break;
                        //переходит или не переходит и идентификатор потока.
                        //подумать про unit test
                    case "c":
                        taskFactory.StartNew(
                            () =>
                                {
                                    Console.WriteLine($"Start processing main task. ManagedThreadId ={Thread.CurrentThread.ManagedThreadId}");
                                    Thread.Sleep(10);
                                    Console.WriteLine("Something wrong has happened. Throw an exception");
                                    throw new Exception("something wrong");
                                },
                            ct).ContinueWith(PrintStringWhenFailed, TaskContinuationOptions.OnlyOnFaulted)
                               .ContinueWith(ContinuousActionMessage, ct); 
                        break;

                    case "d":
                        var cTokenSource = new CancellationTokenSource();
                        var cToken = cTokenSource.Token;
                        taskFactory.StartNew(
                            () =>
                                {
                                    Console.WriteLine("Print ten thousands numbers by main task");

                                    var random = new Random();

                                    for (var i = 0; i <= 10000; i++)
                                    {
                                        Console.WriteLine(random.Next(10));
                                        if (cToken.IsCancellationRequested)
                                        {
                                            Console.WriteLine("Task was canceled");
                                            break;
                                        }

                                        // ct.ThrowIfCancellationRequested();
                                    }
                                },
                            cToken).ContinueWith(PrintSecondString, TaskContinuationOptions.RunContinuationsAsynchronously)
                                   .ContinueWith(ContinuousActionMessage, TaskContinuationOptions.AttachedToParent);
                        Thread.Sleep(100);
                        cTokenSource.Cancel();

                        Console.WriteLine("Continue processing part 'd'");
                        break;
                }
            }

            Console.WriteLine("You exit from demo successfully :)");
            Console.ReadLine();
        }

        private static void ContinuousActionMessage(Task task)
        {
            Console.WriteLine("Put the letter to choose a way of processing:");
        }

        static void PrintTenNumbers()
        {
            Console.WriteLine($"Print ten numbers by main task. ManagedThreadId ={Thread.CurrentThread.ManagedThreadId}");

            var random = new Random();

            for (var i = 0; i <= 10; i++)
            {
                Console.WriteLine(random.Next(100));
            }
        }

        private static void PrintString(Task obj)
        {
            Console.WriteLine($"Continue with PrintString method. ManagedThreadId ={Thread.CurrentThread.ManagedThreadId}");
        }

        private static void PrintSecondString(Task obj)
        {
            if (obj.IsCanceled || obj.IsCompleted)
            {
                Console.WriteLine($"Main Task has finished with following reason: {obj.Status}");
            }
        }

        private static void PrintStringWhenFailed(Task obj)
        {
            if (obj.IsFaulted)
            {
                Console.WriteLine($"Main Task has finished with following reason: {obj.Status}");
                Console.WriteLine($"Processing failure case in thread ManagedThreadId ={Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }
}
