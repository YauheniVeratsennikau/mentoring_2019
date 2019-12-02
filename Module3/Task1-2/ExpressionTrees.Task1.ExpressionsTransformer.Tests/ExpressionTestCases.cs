namespace ExpressionTrees.Task1.ExpressionsTransformer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class ExpressionTestCases
    {
        public static IEnumerable<object[]> IncrementTestCases
        {
            get
            {
                Expression<Func<int, int>> expSource = (x) => (x + 1) + (x - x) * (x + 1) + 12 * x;
                Expression<Func<int, int, int, int>> expMultiSource = (x, y, z) => (x + y + 2 + 1) + (1 + y) * (z + 1) + 12 * x;
                Expression<Func<int, int, int, int>> expMultiSource2 = (x, y, z) => (x + 1 + z + 1) + (1 + y) * (z + 1) + 12 * x;

                yield return new object[] { expSource, "((Increment(x) + ((x - x) * Increment(x))) + (12 * x))" };
                yield return new object[] { expMultiSource, "(((((x + y) + 2) + 1) + (Increment(y) * Increment(z))) + (12 * x))" };
                yield return new object[] { expMultiSource2, "((((Increment(x) + z) + 1) + (Increment(y) * Increment(z))) + (12 * x))" };
            }
        }

        public static IEnumerable<object[]> DecrementTestCases
        {
            get
            {
                Expression<Func<int, int>> expSource = (x) => (1 - x) + (x - 1) * (x + x) + 12 * x;
                Expression<Func<int, int, int, int>> expMultiSource = (x, y, z) => (x + y + 2 + 1) + (1 - y) * (z - 1) + 12 * x;
                Expression<Func<int, int, int, int>> expMultiSource2 = (x, y, z) => (x - 1 + z + 1) + (1 - y) * (z - 1) + 12 * x;

                yield return new object[] { expSource, "((Decrement(x) + (Decrement(x) * (x + x))) + (12 * x))" };
                yield return new object[] { expMultiSource, "(((((x + y) + 2) + 1) + (Decrement(y) * Decrement(z))) + (12 * x))" };
                yield return new object[] { expMultiSource2, "((((Decrement(x) + z) + 1) + (Decrement(y) * Decrement(z))) + (12 * x))" };
            }
        }

        public static IEnumerable<object[]> ReplaceParameterTestCases
        {
            get
            {
                var dicValues = new Dictionary<string, int>
                                    {
                                        { "x", 2 },
                                        { "y", 15 },
                                        { "z", 6 }
                                    };
                Expression<Func<int, int>> expSource = (x) => (1 - x) + (x - 1) * (x + x) + 12 * x;
                Expression<Func<int, int, int, int>> expMultiSource = (x, y, d) => (x + y + 2 + 1) + (1 - y) * (d - 1) + 12 * x;
                Expression<Func<int, int, int, int>> expMultiSource2 = (x, d, z) => (x - 1 + z + 1) + (1 - d) * (z - 1) + 12 * x;
                Expression<Func<int, int, int, int, int>> expMultiSource3 = (x, d, z, k) => (k - 1 + z + 1) + (1 - d) * (z - 1) + 12 * x;
                Expression<Func<int, int, int, int, int>> expMultiSource4 = (x, d, z, k) => (k - 1 + d + z + 1) + (1 - z - 2 * d) * (z - 1) + 12 * x;

                yield return new object[] { expSource, dicValues };
                yield return new object[] { expMultiSource, dicValues };
                yield return new object[] { expMultiSource2, dicValues };
                yield return new object[] { expMultiSource3, dicValues };
                yield return new object[] { expMultiSource4, dicValues };
            }
        }
    }
}
