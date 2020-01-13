using System.IO;
using System.Numerics;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;

namespace AOP.CacheLib
{
    class LogConverter : PatternLayout
    {
        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var message = loggingEvent.MessageObject.GetType().IsPrimitive || loggingEvent.MessageObject is string || loggingEvent.MessageObject is decimal || loggingEvent.MessageObject is BigInteger
                ? new { message = loggingEvent.MessageObject }
                : loggingEvent.MessageObject;

            writer.WriteLine(JsonConvert.SerializeObject(new
            {
                timestamp = loggingEvent.TimeStampUtc,
                threadId = loggingEvent.ThreadName,
                details = message,
                logger = loggingEvent.LoggerName,
                level = loggingEvent.Level.DisplayName,
                user = loggingEvent.UserName
            }));
        }

    }
}
