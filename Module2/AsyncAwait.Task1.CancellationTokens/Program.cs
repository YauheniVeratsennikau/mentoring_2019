/*
 * Изучите код данного приложения для расчета суммы целых чисел от 0 до N, а затем
 * измените код приложения таким образом, чтобы выполнялись следующие требования:
 * 1. Расчет должен производиться асинхронно.
 * 2. N задается пользователем из консоли. Пользователь вправе внести новую границу в процессе вычислений,
 * что должно привести к перезапуску расчета.
 * 3. При перезапуске расчета приложение должно продолжить работу без каких-либо сбоев.
 */

using System;

namespace AsyncAwait.Task1.CancellationTokens
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represent the main program
    /// </summary>
    class Program
    {
        private static CancellationTokenSource _tokenSource;

        private static CancellationTokenSource TokenSource {
            get
            {
                _tokenSource = _tokenSource ?? new CancellationTokenSource();
                return _tokenSource;
            }

            set => _tokenSource = value;
        }

        private static CancellationToken Token { get; set; }

        /// <summary>
        /// The Main method should not be changed at all.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
            Console.WriteLine("Calculating the sum of integers from 0 to N.");
            Console.WriteLine("Use 'q' key to exit...");
            Console.WriteLine();

            Console.WriteLine("Info. Enter N: ");
            
            string input = Console.ReadLine();
            while (input.Trim().ToUpper() != "Q")
            {
                if (int.TryParse(input, out int n))
                {
                    CalculateSum(n);
                }
                else
                {
                    Console.WriteLine($"Error. Invalid integer: '{input}'. Please try again.");
                    Console.WriteLine("Info. Enter N: ");
                }

                input = Console.ReadLine();
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        private static async void CalculateSum(int n)
        {
            Token = TokenSource.Token;
            TokenSource.Cancel();

            try
            {
                if (TokenSource.Token.IsCancellationRequested)
                {
                    TokenSource.Dispose();
                    TokenSource = new CancellationTokenSource();
                }

                Token = TokenSource.Token;
                long sum = await Task.Run(
                               () =>
                                   {
                                       Console.WriteLine($"Info.The task for {n} started... Enter N to cancel the request:");
                                       return Calculator.Calculate(n, Token);
                                   },
                               Token);
                Console.WriteLine($"Result. Sum for {n} = {sum}.");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Warn. Sum for {n} canceled... {ex.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error. General exception: {e.Message}");
            }
        }
    }
}