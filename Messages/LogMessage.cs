using System;

namespace BES.MVVM.Core.Messages {
    public class LogMessage {
        #region Constructors

        public LogMessage(LogType type, string message) {
            this.Type = type;
            this.Message = message;
        }
        public LogMessage( LogType type, Exception exception ) {
            if ( type!=LogType.FATAL_EXCEPTION && type!=LogType.ERROR_EXCEPTION ) {
                throw new ArgumentException($"[LogMessage] LogMessage({type}, {exception}): Exceptions may only be passed if LogType is 'FATAL' or 'ERROR'.");
            }
            this.Type = type;
            this.ExceptionContent = exception;
        }

        #endregion

        #region Properties

        public LogType Type { get; set; }
        public string Message { get; set; }
        public Exception ExceptionContent { get; set; }

        #endregion
        
    }

    public enum LogType {
        INVALID,
        DEBUG,
        ERROR_STRING_MESSAGE,
        ERROR_EXCEPTION,
        FATAL_STRING_MESSAGE,
        FATAL_EXCEPTION,
        INFO,
        WARN
    }
}
