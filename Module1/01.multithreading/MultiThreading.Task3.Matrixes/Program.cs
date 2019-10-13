/*
 * 3. Write a program, which multiplies two matrices and uses class Parallel.
 * a. Implement logic of MatricesMultiplierParallel.cs
 *    Make sure that all the tests within MultiThreading.Task3.MatrixMultiplier.Tests.csproj run successfully.
 * b. Create a test inside MultiThreading.Task3.MatrixMultiplier.Tests.csproj to check which multiplier runs faster.
 *    Find out the size which makes parallel multiplication more effective than the regular one.
 */

using System;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier
{
    using System.Collections.Generic;
    using System.Diagnostics;

    class Program
    {
        private static List<string> lstWatchedActionTimes = new List<string>();

        private static int defaultSize = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("3.	Write a program, which multiplies two matrices and uses class Parallel. ");
            Console.WriteLine();
            int matrixSize = defaultSize;

            while (true)
            {
                Console.WriteLine("Enter a Matrix Size:");
                var strSize = Console.ReadLine();

                if (!int.TryParse(strSize, out matrixSize))
                {
                    break;
                }

                CreateAndProcessMatrices(matrixSize);
            }

            Console.ReadLine();
        }

        private static void CreateAndProcessMatrices(int sizeOfMatrix)
        {
            lstWatchedActionTimes.Clear();
            Console.WriteLine("Multiplying...");
            var firstMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            var secondMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            IMatrix resultMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix);
            IMatrix resultMatrixParallel = new Matrix(sizeOfMatrix, sizeOfMatrix);

            var actions = new List<Action>
                              {
                                  () => ProcessMatricesMultiplierMultiply(
                                      firstMatrix,
                                      secondMatrix,
                                      out resultMatrix),
                                  () => ProcessMatricesMultiplierParallelMultiply(
                                      firstMatrix,
                                      secondMatrix,
                                      out resultMatrixParallel)
                              };

            actions.ForEach(DoWatchedAction);

            PrintMatrixes(firstMatrix, secondMatrix, resultMatrix);

            PrintMatrixes(firstMatrix, secondMatrix, resultMatrixParallel);

            lstWatchedActionTimes.ForEach(Console.WriteLine);
        }

        private static void ProcessMatricesMultiplierMultiply(IMatrix firstMatrix, IMatrix secondMatrix, out IMatrix resultMatrix)
        {
            resultMatrix = new MatricesMultiplier().Multiply(firstMatrix, secondMatrix);
        }

        private static void ProcessMatricesMultiplierParallelMultiply(IMatrix firstMatrix, IMatrix secondMatrix, out IMatrix resultMatrix)
        {
            Console.WriteLine("Parallel");
            resultMatrix = new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix);
        }
        
        private static void PrintMatrixes(Matrix firstMatrix, Matrix secondMatrix, IMatrix resultMatrix)
        {
            Console.WriteLine("firstMatrix:");
            firstMatrix.Print();
            Console.WriteLine("secondMatrix:");
            secondMatrix.Print();
            Console.WriteLine("resultMatrix:");
            resultMatrix.Print();
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
