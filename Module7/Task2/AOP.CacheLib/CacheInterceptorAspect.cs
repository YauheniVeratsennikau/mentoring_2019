using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using PostSharp.Aspects;

namespace AOP.CacheLib
{
    [Serializable]
    public class CacheInterceptorAspect : OnMethodBoundaryAspect, ISerializable
    {
        private static ILogger _log;

        private Dictionary<string, object> _cache = new Dictionary<string, object>();

        private int CacheSize { get; set; }

        private static string SerializationDelimiter = "||";

        private static string ParamsDelimiter = ";";

        public Dictionary<string, object> CustomCache => _cache ?? new Dictionary<string, object>();

        public CacheInterceptorAspect()
        {
        }

        static CacheInterceptorAspect()
        {
            ConfigureNlog();
        }

        private static void ConfigureNlog()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets
            var consoleTarget = new ColoredConsoleTarget("target1")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);

            var fileTarget = new FileTarget("target2")
            {
                FileName = "${basedir}/Logs/cacheLib.log",
                Layout = "${logger}-${level}:${longdate} ---- ${message} ${exception}----",
                OpenFileFlushTimeout = 1,
                ArchiveAboveSize = 10000,
                ArchiveFileName = "${basedir}/Logs/Archives/cacheLib_${shortdate}_{##}.log"
            };
            config.AddTarget(fileTarget);

            // Step 3. Define rules
            //config.AddRuleForOneLevel(LogLevel.Error, fileTarget); // only errors to file
            config.AddRuleForAllLevels(fileTarget); // all to file

            config.AddRuleForAllLevels(consoleTarget); // all to console

            // Step 4. Activate the configuration
            LogManager.Configuration = config;
            _log = LogManager.GetLogger(nameof(CacheInterceptorAspect));
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            _log.Info("OnEntry Start. Method: " + args.Method.Name);
            
            WriteMainLogInfo(args);
            
            var key = GetCacheKey(args.Arguments);

            if (string.IsNullOrEmpty(key))
            {
                PrintCacheKeyWarning(args);
                return;
            }

            if (CustomCache.TryGetValue(key, out var value))
            {
                _log.Info($"Cache exists. key:{key} - value:{value} ");
                args.FlowBehavior = FlowBehavior.Return;
                args.ReturnValue = value;
            }
            else
            {
                args.FlowBehavior = FlowBehavior.Default;
            }

            _log.Info($"OnEntry End. Method:{args.Method.Name}");
        }

        private static void WriteMainLogInfo(MethodExecutionArgs args)
        {
            var logInfo = GetLogInfo(args);
            _log.Debug(logInfo.Serialize());
        }

        private void PrintCacheKeyWarning(MethodExecutionArgs args)
        {
            _log.Warn($"Cache key is empty. Call MethodName: {args.Method.Name}; Args: {GetArgsParamsAsString(args)}");
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            var key = GetCacheKey(args.Arguments);

            if (string.IsNullOrEmpty(key))
            {
                PrintCacheKeyWarning(args);
                return;
            }
            CustomCache.Add(key, args.ReturnValue);

            _log.Info($"Method '{args.Method.Name}' has finished successfully");
            WriteMainLogInfo(args);
        }

        private static LogInfo GetLogInfo(MethodExecutionArgs args)
        {
            return new LogInfo
            {
                Date = DateTime.UtcNow,
                MethodName = args.Method.Name,
                MethodType = args.Method.DeclaringType?.ToString(),
                MethodAttributes = GetArgsParamsAsString(args),
                MethodMemberType = args.Method.MemberType.ToString(),
                MethodModule = args.Method.Module.ToString(),
                ReturnValue = args.ReturnValue?.ToString()
            };
        }

        private static string GetArgsParamsAsString(MethodExecutionArgs args)
        {
            return string.Join(ParamsDelimiter, args.Arguments.AsEnumerable());
        }

        private string GetCacheKey(Arguments arguments)
        {
            return string.Join(ParamsDelimiter, arguments.AsEnumerable());
        }

        protected CacheInterceptorAspect(SerializationInfo info, StreamingContext context)
        {
            for (int index = CacheSize - 1; index > 0; CacheSize--)
            {
                var strCacheEntry = info.GetString(index.ToString());

                var data = strCacheEntry.Split(SerializationDelimiter);
                if (data.Length == 2 && !string.IsNullOrEmpty(data[0]) && !string.IsNullOrEmpty(data[1]))
                {
                    CustomCache.Add(data[0], data[1]);
                }
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

    public class LogInfo
    {
        public DateTime Date { get; set; }

        public string MethodName { get; set; }
        public string MethodAttributes { get; set; }
        public string MethodType { get; set; }
        public string MethodMemberType { get; set; }
        public string MethodModule { get; set; }
        public string ReturnValue { get; set; }
    }

    public static class LogInfoExtensions
    {
        public static string Serialize(this LogInfo logInfo)
        {
            var xmlSerializer = new XmlSerializer(typeof(LogInfo));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, logInfo);

                return textWriter.ToString();
            }
        }
    }
}
