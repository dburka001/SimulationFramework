using MicroSim.DataSource.Output;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MicroSim.DataSource.ExcelExport.Commands
{
    public abstract class ExportCommandBase : IExportCommand
    {
        #region protected class ExportState : Tuple<Stream, OutputDataSource>

        private class ExportState
        {
            public OutputDataSource DataSource { get; }

            public Stream Target { get; }

            public Type[] EnumTypes { get; }

            public ExportState(
                Stream target,
                OutputDataSource dataSource,
                Type[] enumTypes
                )
            {
                Target = target ?? throw new ArgumentNullException(nameof(target));
                DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
                EnumTypes = enumTypes ?? throw new ArgumentNullException(nameof(enumTypes));
            }
        }

        #endregion protected class ExportState : Tuple<Stream, OutputDataSource>

        public abstract void Export(Stream target, OutputDataSource datasource, Type[] enumTypes);

        public Task ExportAsync(Stream target, OutputDataSource datasource, Type[] enumTypes)
            => Task.Factory.StartNew(
                s => Export(
                    ((ExportState)s).Target,
                    ((ExportState)s).DataSource,
                    ((ExportState)s).EnumTypes
                    ),
                new ExportState(target, datasource, enumTypes));
    }
}