using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public class LogEntry
    {
        #region Fields and Properties
        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
                
        public string Message
        {
            get;
            set;
        }

        public LogEntryLevel Level
        {
            get;
            set;
        }

        private DateTime _dateTime = DateTime.MinValue;
        public DateTime DateTime
        {
            get{return _dateTime;}
            
        }

        public string ExceptionText
        {
            get;
            set;
        }

        public List<LogEntryAttribute> Attributes
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public LogEntry(string message)
            : this(message, LogEntryLevel.Informational)
        {
        }

        public LogEntry(string message, LogEntryLevel level)
            : this(message, level, null)
        { }

        public LogEntry(string message, LogEntryLevel level, System.Exception exception)
            : this(message, level, exception, null)
        {
        }
        public LogEntry(string message, LogEntryLevel level, System.Exception exception, params LogEntryAttribute[] logEntries)
        {
            Message = message;
            Level = level;
            if (exception != null)
                ExceptionText = String.Format("{0}\n\n\t {1}", exception.ToString(), exception.StackTrace);
            if (logEntries != null)
                Attributes = logEntries.ToList<LogEntryAttribute>();
        }
        #endregion

    }
}
