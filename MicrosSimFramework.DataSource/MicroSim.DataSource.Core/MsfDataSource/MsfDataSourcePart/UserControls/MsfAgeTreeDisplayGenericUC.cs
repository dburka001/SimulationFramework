using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSim.DataSource.Entities;

namespace MicroSim.DataSource.Core
{
    /// <summary>
    /// Default User Control for displaying an Age Tree based on Education level
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="MicroSim.DataSource.Core.IBaseUC{MicroSim.DataSource.Entities.IPopulationEduEntity}" />
    public partial class MsfAgeTreeDisplayGenericUC<Tenum, Tdata, TsubData> : UserControl, IBaseUC<Tdata> 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsfAgeTreeDisplayGenericUC"/> class.
        /// </summary>
        public MsfAgeTreeDisplayGenericUC()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            SetupEducationComboBox();
        }

        /// <summary>
        /// Setups the education ComboBox.
        /// </summary>
        private void SetupEducationComboBox()
        {            
            var edus = Enum.GetValues(typeof(Tenum)).Cast<Enum>()
                .Where(e => !e.IsHidden())
                .Select(e => new
                {
                    Title = e.GetLabel(),
                    Enum = e
                });
            cbxEducation.DataSource = edus.ToList();
            cbxEducation.DisplayMember = "Title";
            cbxEducation.ValueMember = "Enum";
        }

        /// <summary>
        /// Gets or sets a value indicating whether [create total].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [create total]; otherwise, <c>false</c>.
        /// </value>
        public bool CreateTotal { get; set; } = true;

        /// <summary>
        /// The data
        /// </summary>
        public List<Tdata> Data { get; } = new List<Tdata>();

        /// <summary>
        /// The Total Data.
        /// </summary>
        /// <value>
        /// The total data.
        /// </value>
        protected List<TsubData> TotalData { get; } = new List<TsubData>();

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public virtual UserControl View { get; } = new MsfAgeTreeDisplayUC();

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void LoadData(IEnumerable<Tdata> data)
        {
            Data.Clear();
            Data.AddRange(data);
            TotalData.Clear();
            if (CreateTotal)
                CreateTotalData();
            else if(View is MsfAgeTreeDisplayUC)
                ((MsfAgeTreeDisplayUC)View).MaxValue = 
                    (float)Data.Max(t =>((IValueEntity)t).Value);         
            displayPanel.Controls.Add(View);
            LoadView();
        }

        /// <summary>
        /// Creates the total data.
        /// </summary>
        protected virtual void CreateTotalData() { }

        /// <summary>
        /// Gets the sub data.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        protected virtual IEnumerable<TsubData> GetSubData(Enum e) { return null; }

        /// <summary>
        /// CBXs the education selected index changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void cbxEducationSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadView();
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        private void LoadView()
        {
            if (Data.Count == 0) return;
            var item = cbxEducation.SelectedItem;
            Enum e = (Enum)(item.GetType().GetProperty("Enum").GetValue(item));
            IEnumerable<TsubData> subData;            
            if (CreateTotal && e.IsFiltered())
                subData = TotalData;
            else
                subData = GetSubData(e);
            if (typeof(IBaseUC<TsubData>).IsAssignableFrom(View.GetType()))
                ((IBaseUC<TsubData>)View).LoadData(subData);
            View.Invalidate();
        }
    }
}
