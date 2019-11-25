using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    using System;
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            
            if (ShouldBeConverted(node) && TryGetParam(node, out var param) && TryGetConstant(node, out var constant))
            {
                if (CanBeConvertedToIncrementExp(node.NodeType, param, constant))
                {
                    return Expression.Increment(param);
                }

                if (CanBeConvertedToDecrementExp(node.NodeType, param, constant))
                {
                    return Expression.Decrement(param);
                }

                throw new Exception($"Node is not converted, but it should be converted.Node: {node}");
            }

            return base.VisitBinary(node);
        }

        private bool ShouldBeConverted(BinaryExpression node)
        {
            return node.NodeType == ExpressionType.Add || node.NodeType == ExpressionType.Subtract;
        }

        /// <summary>
        /// Determines whether this instance can convert to decrement expression the specified node node type.
        /// </summary>
        /// <param name="nodeType">Type of the node </param>
        /// <param name="param">The parameter expression.</param>
        /// <param name="constant">The constant expression.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert to decrement exp the specified node type; otherwise, <c>false</c>.
        /// </returns>
        private bool CanBeConvertedToDecrementExp(ExpressionType nodeType, ParameterExpression param, ConstantExpression constant)
        {
            return nodeType == ExpressionType.Subtract && param != null && constant != null && (int)constant.Value == 1;
        }

        /// <summary>
        /// Determines whether this instance can convert to increment expression the specified node node type.
        /// </summary>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="param">The parameter expression.</param>
        /// <param name="constant">The constant expression.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can convert to increment exp] the specified node node type; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private bool CanBeConvertedToIncrementExp(ExpressionType nodeType, ParameterExpression param, ConstantExpression constant)
        {
            return nodeType == ExpressionType.Add && param != null && constant != null && (int)constant.Value == 1;
        }

        /// <summary>
        /// Tries the get constant.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="constant">The constant expression.</param>
        /// <returns></returns>
        private bool TryGetConstant(BinaryExpression node, out ConstantExpression constant)
        {
            constant = null;
            if (node.Left.NodeType == ExpressionType.Constant && node.Right.NodeType != ExpressionType.Constant)
            {
                constant = node.Left as ConstantExpression;
                return true;
            }

            if (node.Left.NodeType != ExpressionType.Constant && node.Right.NodeType == ExpressionType.Constant)
            {
                constant = node.Right as ConstantExpression;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries the get parameter.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="param">The parameter expression.</param>
        /// <returns></returns>
        private bool TryGetParam(BinaryExpression node, out ParameterExpression param)
        {
            param = null;
            if (node.Left.NodeType == ExpressionType.Parameter && node.Right.NodeType != ExpressionType.Parameter)
            {
                param = node.Left as ParameterExpression;
                return true;
            }

            if (node.Left.NodeType != ExpressionType.Parameter && node.Right.NodeType == ExpressionType.Parameter)
            {
                param = node.Right as ParameterExpression;
                return true;
            }

            return false;
        }
    }
}
