using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Add && CanBeConverted(node, out var incParam))
            {
                return Expression.Increment(incParam);
            }

            if (node.NodeType == ExpressionType.Subtract && CanBeConverted(node, out var decParam))
            {
                return Expression.Decrement(decParam);
            }

            return base.VisitBinary(node);
        }
        
        /// <summary>
        /// Determines whether this instance [can be converted] the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can be converted] the specified node; otherwise, <c>false</c>.
        /// </returns>
        private bool CanBeConverted(BinaryExpression node, out ParameterExpression param)
        {
            param = null;
            ConstantExpression constant = null;

            if (node != null
                && node.Left.NodeType == ExpressionType.Parameter
                && node.Right.NodeType == ExpressionType.Constant)
            {
                param = node.Left as ParameterExpression;
                constant = node.Right as ConstantExpression;
            }
            else if (node != null
                     && node.Right.NodeType == ExpressionType.Parameter
                     && node.Left.NodeType == ExpressionType.Constant)
            {
                param = node.Right as ParameterExpression;
                constant = node.Left as ConstantExpression;
            }

            return param != null && constant != null && (int)constant.Value == 1; 
        }
    }
}
