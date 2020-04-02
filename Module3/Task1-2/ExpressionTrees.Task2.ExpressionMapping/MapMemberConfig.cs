using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    /// <summary>
    /// Represent a class for configuration custom mapping between two types
    /// </summary>
    /// <typeparam name="TSource">source</typeparam>
    /// <typeparam name="TDestination">destination</typeparam>
    public class MapMemberConfig<TSource, TDestination>
    {
        public Type SourceType { get; }

        public Type DestinationType { get; }

        private List<MemberBinding> MemberBindings { get; set; }

        private List<PropertyInfo> DestinationPropInfos { get; set; }

        /// <summary>
        /// The SourceParam to for mapping
        /// NOTE: if we try to use different object for MemberBinding and for Expression.Lambda()  
        /// it will throw the InvalidOperationException:
        /// variable 'source' of type 'ExpressionTrees.Task2.ExpressionMapping.Tests.Models.Foo' referenced
        /// from scope '', but it is not defined
        /// </summary>
        private readonly ParameterExpression _sourceParam;

        public MapMemberConfig()
        {
            SourceType = typeof(TSource);
            DestinationType = typeof(TDestination);
            MemberBindings = new List<MemberBinding>();
            DestinationPropInfos = new List<PropertyInfo>(DestinationType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty));
            _sourceParam = Expression.Parameter(SourceType, "source");
        }


        /// <summary>
        /// Maps fields of two classes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void Map(Expression<Func<TSource, object>> source, Expression<Func<TDestination, object>> destination)
        {
            var sFieldName = ((MemberExpression)source.Body).Member.Name;
            var dFieldName = ((MemberExpression)destination.Body).Member.Name;

            var mapInfo = DestinationPropInfos.FirstOrDefault(p => p.Name.Equals(dFieldName));

            if (mapInfo == null)
            {
                //possible ways: write in log or throw new ArgumentNullException()
                return;
            }

            var memberAccess = Expression.PropertyOrField(_sourceParam, sFieldName);
            MemberBinding mb = Expression.Bind(mapInfo.GetSetMethod(), memberAccess);
            MemberBindings.Add(mb);
        }

        /// <summary>
        /// Create function for mapping 
        /// </summary>
        /// <returns>not compiled function</returns>
        public Expression<Func<TSource, TDestination>> GetMapFunc()
        {
            var ctor = Expression.New(DestinationType);
            return Expression.Lambda<Func<TSource, TDestination>>(
                Expression.MemberInit(ctor, MemberBindings), _sourceParam);
        }
    }
}
