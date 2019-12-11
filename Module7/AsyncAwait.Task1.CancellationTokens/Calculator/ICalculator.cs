using System.Threading;

namespace AOP.AsyncAwait.Task1.CancellationTokens.Calculator
{
    public interface ICalculator
    {
        long Calculate(int n, CancellationToken token);
    }
}
