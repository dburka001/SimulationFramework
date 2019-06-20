using System;
using System.Collections.Generic;

namespace MicroSim.DataSource.Core
{
    public interface IMsfExportableDataSource
    {
        string Title { get; }
        Type InputPartType { get; }
        IEnumerable<object> GetData();

    }
}