using System;
using System.Diagnostics;
using NLog;

namespace SdiEmailLib.Logging
{
    internal static class EventLogger
    {
        private const string EventViewerSource = "SdiCommService";

        public static void Info(int appid, string msg, Logger logger)
        {
            try
            {
                EventLog eventLog = new EventLog { Source = EventViewerSource };
                eventLog.WriteEntry(msg, EventLogEntryType.Information);
            }
            catch (Exception) { /* */ }
            logger?.Info(msg);
        }

        public static void Error(int appid, string msg, Logger logger)
        {
            try
            {
                EventLog eventLog = new EventLog { Source = EventViewerSource };
                eventLog.WriteEntry(msg, EventLogEntryType.Error);
            }
            catch (Exception) { /* */ }
            logger?.Error(msg);
        }
    }
}
