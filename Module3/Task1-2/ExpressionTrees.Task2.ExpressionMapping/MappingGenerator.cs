using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> GenerateDefaultMapping<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));

            Type sourceType = typeof(TSource);
            Type destinationType = typeof(TDestination);

            var sourceInfos = new List<PropertyInfo>(sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty));
            var destinationInfos = new List<PropertyInfo>(destinationType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty));

            var mapInfos = destinationInfos.Intersect(sourceInfos, new PropertyInfoComparer()).ToList();

            var ctor = Expression.New(destinationType);
            var list = new List<MemberBinding>();
            foreach (var mapInfo in mapInfos)
            {
                var memberAccess = Expression.PropertyOrField(sourceParam, mapInfo.Name);
                MemberBinding mb = Expression.Bind(mapInfo.GetSetMethod(), memberAccess);
                list.Add(mb);
            }

            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    Expression.MemberInit(ctor, list), sourceParam);

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        /// <summary>
        /// Create mapper with custom fields mapping or with default mapping if no fields are defined
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="config">the config</param>
        /// <returns>the available mapping</returns>
        public Mapper<TSource, TDestination> GenerateCustomMapping<TSource, TDestination>(MapMemberConfig<TSource, TDestination> config)
        {
            var mapFunction = config.GetMapFunc();

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        /// <summary>
        /// Get config to configure custom mapping
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public MapMemberConfig<TSource, TDestination> GetMappingConfig<TSource, TDestination>()
        {
            return new MapMemberConfig<TSource, TDestination>();
        }
    }
}
