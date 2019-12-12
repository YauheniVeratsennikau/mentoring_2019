using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using log4net;
using PostSharp.Aspects;

namespace AOP.CacheLib
{
    [Serializable]
    public class CacheInterceptorAspect : OnMethodBoundaryAspect, ISerializable
    {
        [NonSerialized]
        private readonly ILog _log = LogManager.GetLogger(typeof(CacheInterceptorAspect));

        private Dictionary<string, object> _cache = new Dictionary<string, object>();
        
        private int CacheSize { get; set; }

        private static string SerializationDelimiter = "||";

        public Dictionary<string, object> CustomCache => _cache ?? new Dictionary<string, object>();

        public CacheInterceptorAspect()
        {
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            _log.Info("MethodName: " + args.Method.Name);
            var key = GetCacheKey(args.Arguments);

            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            if (CustomCache.TryGetValue(key, out var value))
            {
                args.FlowBehavior = FlowBehavior.Return;
                args.ReturnValue = value;
            }
            else
            {
                args.FlowBehavior = FlowBehavior.Default;
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            var key = GetCacheKey(args.Arguments);

            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            CustomCache.Add(key, args.ReturnValue);
            _log.Info($"arguments: {string.Join(';', args.Arguments.AsEnumerable())}. Result value: {args.ReturnValue} ");
        }

        private string GetCacheKey(Arguments arguments)
        {
            return string.Join(";", arguments.AsEnumerable());
        }

        protected CacheInterceptorAspect(SerializationInfo info, StreamingContext context)
        {
            for (int index = CacheSize - 1; index > 0; CacheSize--)
            {
                var strCacheEntry = info.GetString(index.ToString());

                var data = strCacheEntry.Split(SerializationDelimiter);
                if (data.Length == 2 && !string.IsNullOrEmpty(data[0]) && !string.IsNullOrEmpty(data[1]));
                CustomCache.Add(data[0], data[1]);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            CacheSize = 0;

            foreach (var cacheEntry in _cache)
            {
                info.AddValue(CacheSize.ToString(), $"{cacheEntry.Key}{SerializationDelimiter}{cacheEntry.Value}");
                CacheSize++;
            }
        }
    }
}
