using System;
using System.IO;
using System.Threading.Tasks;
using MicroSim.DataSource.Output;

namespace MicroSim.DataSource.ExcelExport.Commands
{
    public interface IExportCommand
    {
        void Export(Stream target, OutputDataSource datasource, Type[] enumTypes);
        Task ExportAsync(Stream target, OutputDataSource datasource, Type[] enumTypes);
    }
}