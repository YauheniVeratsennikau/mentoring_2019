using Expressions.Task3.E3SQueryProvider.Models.Entitites;
using Expressions.Task3.E3SQueryProvider.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Expressions.Task3.E3SQueryProvider.QueryProvider
{
    public class E3SEntitySet<T> : IQueryable<T> where T : BaseE3SEntity
    {
        protected readonly Expression expression;
        protected readonly IQueryProvider provider;

        public E3SEntitySet(E3SSearchService client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            expression = Expression.Constant(this);
            provider = new E3SLinqProvider(client);
        }

        #region public properties

        public Type ElementType => typeof(T);

        public Expression Expression => expression;

        public IQueryProvider Provider => provider;

        #endregion

        #region public methods

        public IEnumerator<T> GetEnumerator()
        {
            return provider.Execute<IEnumerable<T>>(expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return provider.Execute<IEnumerable>(expression).GetEnumerator();
        }

        #endregion
    }
}