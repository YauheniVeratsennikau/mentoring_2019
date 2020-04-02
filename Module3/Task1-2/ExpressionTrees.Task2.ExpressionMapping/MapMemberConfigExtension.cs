using System;
using System.Linq.Expressions;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public static class MapMemberConfigExtension
    {
        /// <summary>
        /// Configure custom mapping
        /// </summary>
        /// <typeparam name="TSource">source</typeparam>
        /// <typeparam name="TDestination">destination</typeparam>
        /// <param name="config">config</param>
        /// <param name="source">source field</param>
        /// <param name="destination">destination field</param>
        /// <returns>updated config</returns>
        public static MapMemberConfig<TSource, TDestination> ForMember<TSource, TDestination>(this MapMemberConfig<TSource, TDestination> config, Expression<Func<TSource, object>> source, Expression<Func<TDestination, object>> destination)
        {
            config.Map(source, destination);

            return config;
        }
    }
}
