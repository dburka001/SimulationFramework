using MicroSim.DataSource.DataAccess;
using MicroSim.DataSource.Entities;
using MicroSim.DataSource.ProcessLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Core
{
    /// <summary>
    /// Class representing a MicroSimFrameWork DataSource
    /// </summary>
    public abstract class MsfDataSource
    {
        /// <summary>
        /// The button height
        /// </summary>
        private const int _buttonHeight = 60;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsfDataSource"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public MsfDataSource(params MsfDataSource[] inputs)
        {
            InputDataSources.AddRange(inputs);
        }

        /// <summary>
        /// Gets the input data sources.
        /// </summary>
        /// <value>
        /// The input data sources.
        /// </value>
        protected List<MsfDataSource> InputDataSources { get; } = new List<MsfDataSource>();

        /// <summary>
        /// Gets the parts.
        /// </summary>
        /// <value>
        /// The parts.
        /// </value>
        public List<MsfDataSourcePart> Parts { get; } = new List<MsfDataSourcePart>();

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public MsfDataSourceUC View { get; } = new MsfDataSourceUC();

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public abstract string Title { get; set; }

        /// <summary>
        /// Gets the source path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public string GetSourcePath(string fileName)
            => Path.Combine(Resources.SourceFolder, fileName);

        /// <summary>
        /// Gets the type of the input data of.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public MsfDataSource GetInputDataOfType<T>()
            => InputDataSources.Where(d => d.GetType() == typeof(T)).FirstOrDefault();

        /// <summary>
        /// Gets the type of the part of.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public MsfDataSourcePart GetPartOfType<T>()
            => Parts.Where(p => p.GetType() == typeof(T)).FirstOrDefault();

        /// <summary>
        /// Adds the part.
        /// </summary>
        /// <param name="part">The part.</param>
        public void AddPart(MsfDataSourcePart part)
        {
            part.Parent = this;
            Parts.Add(part);
        }

        /// <summary>
        /// Initalizes this instance.
        /// </summary>
        public void Initalize()
        {
            int currentTop = 0;
            foreach (var part in Parts)
            {
                var partButton = CreateButton(part);
                partButton.Top = currentTop;
                currentTop += partButton.Height;                
            }
            DataHandler.Instance.CreateDirectory(Title);
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load(bool forceGeneration = false)
        {                        
            Log.WriteLine(String.Format(Resources.LoadDataSourceMessage, Title));
            Log.IncreaseIndent();
            foreach (var part in Parts)
            {
                try
                {                    
                    part.Load(forceGeneration);
                }
                catch (Exception ex)
                {
                    Log.WriteLine(String.Format(Resources.LoadError, part.Title), LogLevel.Error);
                    Log.WriteLineWithIndent(ex.Message);
                }                
            }
            Log.DecreaseIndent();
            if (View.MainPanel.Controls.Count == 0)
                DisplayPart(Parts.FirstOrDefault());
        }

        /// <summary>
        /// Creates the button.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <returns></returns>
        private Button CreateButton(MsfDataSourcePart part)
        {
            var partButton = new Button();
            partButton.Text = part.Title;
            partButton.Left = 0;
            partButton.Width = View.SidePanel.Width - 20;            
            partButton.Height = _buttonHeight;            
            partButton.Tag = part;
            partButton.Click += PartButtonClick;
            View.SidePanel.Controls.Add(partButton);
            return partButton;
        }

        /// <summary>
        /// Parts the button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PartButtonClick(object sender, EventArgs e)
        {
            DisplayPart((MsfDataSourcePart)((Button)sender).Tag);
        }

        /// <summary>
        /// Displays the part.
        /// </summary>
        /// <param name="part">The part.</param>
        private void DisplayPart(MsfDataSourcePart part)
        {
            if (part == null) return;
            View.MainPanel.Controls.Clear();
            InvokeExtensions.InvokeAction(View, () =>
            {
                View.MainPanel.Controls.Add(part.View);
                part.View.Invalidate();
            });
        }
    }
}
