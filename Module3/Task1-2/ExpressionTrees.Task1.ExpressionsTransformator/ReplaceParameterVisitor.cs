using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class ReplaceParameterVisitor : ExpressionVisitor
    {
        public static Expression<Func<TElement, int>> ReplaceParameter<TElement>(
            Expression<Func<TElement, TElement, TElement, int>> inputExpression,
            Dictionary<string, TElement> dicElements)
        {

            Expression body = inputExpression.Body;

            var inputData = ParseParams(dicElements, inputExpression);

            var mapping = inputData.Item1;
            var lstParam = inputData.Item2;

            ReplaceParameterVisitor visitor = new ReplaceParameterVisitor(mapping);

            Expression newBody = visitor.Visit(body);

            ////todo. only for test
            //if (lstParam.Count == 0)
            //{
            //    lstParam.Add(Expression.Parameter(typeof(int), "fake_param"));
            //}
            ////---------

            Expression<Func<TElement, int>> newExpression = Expression.Lambda<Func<TElement, int>>(newBody, lstParam);

            return newExpression;

        }

        private static Tuple<Dictionary<ParameterExpression, ConstantExpression>, List<ParameterExpression>> ParseParams<TElement>(Dictionary<string, TElement> dicElements,
                                                                        Expression<Func<TElement, TElement, TElement, int>> inputExpression)
        {
            var mapping = new Dictionary<ParameterExpression, ConstantExpression>();
            var paramExps = new List<ParameterExpression>();
            foreach (var param in inputExpression.Parameters)
            {
                if (param != null && dicElements.TryGetValue(param.Name, out var value))
                {
                    mapping.Add(param, Expression.Constant(value, typeof(TElement)));
                }
                else
                {
                    paramExps.Add(param);
                }
            }

            return Tuple.Create(mapping, paramExps);
        }

        public ReplaceParameterVisitor(Dictionary<ParameterExpression, ConstantExpression> mapping)
        {
            this.mapping = mapping;
        }

        private readonly Dictionary<ParameterExpression, ConstantExpression> mapping;


        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (mapping.TryGetValue(node, out var constantExp))
            {
                return constantExp;
            }
            else
            {
                return base.VisitParameter(node);
            }
        }
    }
}
