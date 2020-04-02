/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    partial class Program
    {
        static void Main(string[] args)
        {
           // Run_Expression(args);
            Run_Mapping(args);
        }
    }
}
