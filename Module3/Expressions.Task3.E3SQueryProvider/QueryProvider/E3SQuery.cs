using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Expressions.Task3.E3SQueryProvider.QueryProvider
{
    public class E3SQuery<T> : IQueryable<T>
    {
        private readonly Expression _expression;
        private readonly E3SLinqProvider _provider;
        
        public E3SQuery(Expression expression, E3SLinqProvider provider)
        {
            _expression = expression;
            _provider = provider;
        }

        #region public properties

        public Type ElementType => typeof(T);

        public Expression Expression => _expression;

        public IQueryProvider Provider => _provider;

        #endregion

        #region public methods

        public IEnumerator<T> GetEnumerator()
        {
            return _provider.Execute<IEnumerable<T>>(_expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _provider.Execute<IEnumerable>(_expression).GetEnumerator();
        }

        #endregion
    }
}
