using System;
using System.Runtime.Serialization;

namespace MicroSim.DataSource.ExcelExport.Documents
{
    [Serializable]
    internal class ExcelException : Exception
    {
        public ExcelException()
        {
        }

        public ExcelException(string message) : base(message)
        {
        }

        public ExcelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}