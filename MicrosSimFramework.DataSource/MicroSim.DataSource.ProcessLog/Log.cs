using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Localizations = MicroSim.DataSource.ProcessLog.Properties.Resources;

namespace MicroSim.DataSource.ProcessLog
{
    /// <summary>
    /// LogUpdated event delegate
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="message">The new text.</param>
    /// <param name="color">The color.</param>
    public delegate void LogUpdatedEventHandler(string message, Color color);

    /// <summary>
    /// Log class
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Occurs when the log is updated.
        /// </summary>
        public static event LogUpdatedEventHandler LogUpdated;

        /// <summary>
        /// The indentation
        /// </summary>
        private static int _indentation = 0;

        /// <summary>
        /// Prevents a default instance of the <see cref="Log"/> class from being created.
        /// </summary>
        private Log() { }

        /// <summary>
        /// Increases the indent.
        /// </summary>
        public static void IncreaseIndent()
        {
            _indentation++;
        }

        /// <summary>
        /// Decreases the indent.
        /// </summary>
        public static void DecreaseIndent()
        {
            if (_indentation > 0)
                _indentation--;
        }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The log level.</param>
        public static void Write(string message, LogLevel logLevel = LogLevel.Default)
        {
            if (LogUpdated != null)
                LogUpdated(message, logLevel.GetColor());
        }

        /// <summary>
        /// Writes the specified message and starts a new line.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The log level.</param>
        public static void WriteLine(string message, LogLevel logLevel = LogLevel.Default)
        {
            message = new String('\t', _indentation) + message;
            Write(message + Environment.NewLine, logLevel);
        }

        /// <summary>
        /// Writes an empty line.
        /// </summary>
        public static void WriteLine()
            => Write(Environment.NewLine);

        /// <summary>
        /// Writes the line with indent.
        /// </summary>
        public static void WriteLineWithIndent(string message, LogLevel logLevel = LogLevel.Default)
        {
            IncreaseIndent();
            WriteLine(message, logLevel);
            DecreaseIndent();
        }

        /// <summary>
        /// Writes the exception to the log.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public static void Exception(string message, Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine(message);

            while (ex != null)
            {
                sb.AppendLine($"** {ex.Message} ({ex.GetType().AssemblyQualifiedName})\n{ex.StackTrace}");
                ex = ex.InnerException;

                if (ex != null)
                    sb.AppendLine(Localizations.MsgInnerException);

            }

            WriteLine(sb.ToString(), LogLevel.Error);
        }
    }
}
