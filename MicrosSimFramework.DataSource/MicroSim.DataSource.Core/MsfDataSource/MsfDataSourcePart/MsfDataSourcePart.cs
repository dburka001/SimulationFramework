using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MicroSim.DataSource.Core
{

    /// <summary>
    /// Handler class for an iteration of the processing of a DataSource
    /// </summary>
    public abstract class MsfDataSourcePart: IMsfExportableDataSource
    {
        /// <summary>
        /// Gets or sets the input parts.
        /// </summary>
        /// <value>
        /// The input parts.
        /// </value>
        public List<MsfDataSourcePart> InputParts { get; } = new List<MsfDataSourcePart>();

        /// <summary>
        /// Gets the type of the input part.
        /// </summary>
        /// <value>
        /// The type of the input part.
        /// </value>
        public abstract Type InputPartType { get; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public virtual MsfDataSource Parent { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public virtual UserControl View { get; } = new MsfDataDisplayUC();

        /// <summary>
        /// Sources the file path.
        /// </summary>
        /// <returns></returns>
        protected string SourceFilePath()
            => Path.ChangeExtension(Path.Combine(Parent.Title, Title), DataFileType.CSV.GetExtension());

        /// <summary>
        /// Loads the specified force generation.
        /// </summary>
        /// <param name="forceGeneration">if set to <c>true</c> [force generation].</param>
        public abstract void Load(bool forceGeneration = false);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>The data content as an array.</returns>
        public abstract IEnumerable<object> GetData();
    }
}