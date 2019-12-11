using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    //убрать зависимость от кол-ва параметров
    //убрать статик метод
    //тесты на все случаи
    public class ReplaceParameterVisitor : ExpressionVisitor
    {
        private Dictionary<string, int> _mappingDictionary;

        private List<ParameterExpression> _paramsExpression;


        public ReplaceParameterVisitor(Dictionary<string, int> mappingDictionary)
        {
            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            _mappingDictionary = mappingDictionary;
            _paramsExpression = new List<ParameterExpression>();
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expression leftConstant = null;
            Expression rightConstant = null;

            if (node.Left.NodeType == ExpressionType.Parameter)
            {
                if (_mappingDictionary.TryGetValue(node.Left.ToString(), out var constant))
                {
                    leftConstant = Expression.Constant(constant);
                }
            }

            if (node.Right.NodeType == ExpressionType.Parameter)
            {
                if (_mappingDictionary.TryGetValue(node.Right.ToString(), out var constant))
                {
                    rightConstant = Expression.Constant(constant);
                }
            }

            if (leftConstant == null && rightConstant == null)
            {
                return base.VisitBinary(node);
            }

            var convertedNode = node.Update(leftConstant ?? node.Left, node.Conversion, rightConstant ?? node.Right);

            if (NeedBaseProcessing(leftConstant, rightConstant, node))
            {
                return base.VisitBinary(convertedNode);
            }

            return convertedNode;
        }

        private bool NeedBaseProcessing(Expression leftConstant, Expression rightConstant, BinaryExpression node)
        {
            if (leftConstant == null && !IsParamOrConstantType(node.Left))
            {
                return true;
            }

            if (rightConstant == null && !IsParamOrConstantType(node.Right))
            {
                return true;
            }

            return false;
        }

        private bool IsParamOrConstantType(Expression node)
        {
            return node.NodeType == ExpressionType.Parameter || node.NodeType == ExpressionType.Constant;
        }
    }
}
