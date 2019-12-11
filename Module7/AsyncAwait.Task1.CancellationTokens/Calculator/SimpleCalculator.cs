using System.Threading;
using System.Threading.Tasks;

namespace AOP.AsyncAwait.Task1.CancellationTokens.Calculator
{
    /// <summary>
    /// Represent a calculator
    /// </summary>
    public class SimpleCalculator: ICalculator
    {
        /// <summary>
        /// The calculate.
        /// </summary>
        /// <param name="n">
        /// The n.
        /// </param>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public long Calculate(int n, CancellationToken token)
        {
            long sum = 0;

            for (int i = 0; i < n; i++)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                // i + 1 is to allow 2147483647 (Max(Int32)) 
                sum = sum + (i + 1);
                Thread.Sleep(10);
            }

            return sum;
        }
    }
}
