using System;
using System.Threading;
using System.Threading.Tasks;
using AOP.AsyncAwait.Task1.CancellationTokens.Calculator;

namespace AOP.AsyncAwait.Task1.CancellationTokens
{
    public class Application
    {
        private static CancellationTokenSource _tokenSource;

        private readonly ICalculator _calculator;

        private CancellationTokenSource TokenSource
        {
            get
            {
                _tokenSource = _tokenSource ?? new CancellationTokenSource();
                return _tokenSource;
            }

            set => _tokenSource = value;
        }

        private CancellationToken Token { get; set; }

        public Application(ICalculator calculator)
        {
            if (calculator == null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            _calculator = calculator;
        }

        public void Run()
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

        private async void CalculateSum(int n)
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
                        return _calculator.Calculate(n, Token);
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
