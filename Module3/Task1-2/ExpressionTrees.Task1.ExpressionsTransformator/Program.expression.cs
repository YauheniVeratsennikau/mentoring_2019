/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    partial class Program
    {
        static void Run_Expression(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            Expression<Func<int, int>> expSource = (x) => (x + 1) + (x - 1) * (x + 1) + 12 * x;

            var convertedTree = new IncDecExpressionVisitor().VisitAndConvert(expSource, "");

            Console.WriteLine("Original expression");
            Console.WriteLine(expSource.Body);

            Console.WriteLine("Converted expression");
            Console.WriteLine(convertedTree?.Body);

            Console.WriteLine("End. Expression Visitor for increment/decrement.");

            Console.WriteLine("\r\n------------------------------------------\r\n");

            Console.WriteLine("Expression Visitor for converting param to const.");

            var dicValues = new Dictionary<string, int>
            {
                { "a", 2 },
              //  { "b", 15 },
                { "c", 6 }
            };

            Expression<Func<Int32, Int32, Int32, Int32>> source = (a, b, c) => (a + b - c * b) / (a + c);
            var result = new ReplaceParameterVisitor(dicValues).VisitAndConvert(source, "");

            var bRes = result?.Compile().Invoke(0, 4, 0);

            Console.WriteLine("Original expression");
            Console.WriteLine(source.Body);

            Console.WriteLine("Original constants");
            foreach (var item in dicValues)
            {
                Console.WriteLine($"{item.Key}={item.Value}");
            }

            Console.WriteLine("Converted expression");
            Console.WriteLine(result?.Body);
            Console.WriteLine($"b = 4 \r\nresult : {bRes}");
            Console.WriteLine("End.Expression Visitor for converting param to const.");

            Console.ReadLine();
        }
    }
}
