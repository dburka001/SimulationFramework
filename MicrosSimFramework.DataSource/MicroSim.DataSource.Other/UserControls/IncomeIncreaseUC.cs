using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;

namespace MicroSim.DataSource.Other
{
    /// <summary>
    /// IncomeIncrease UserControl
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="IBaseUC{System.Object}" />
    public partial class IncomeIncreaseUC : UserControl, IBaseUC<IIncomeIncreaseDisplayEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeIncreaseUC"/> class.
        /// </summary>
        public IncomeIncreaseUC()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            btnPercentages.Text = Resources.Percentage;
            btnIncrements.Text = Resources.Increment;
            btnIncomes.Text = Resources.Income;
            btnIncomeTotals.Text = Resources.IncomeTotal;
        }

        /// <summary>
        /// The percentages user control
        /// </summary>
        public MsfLineGraphUC _percentagesUC { get; } = new MsfLineGraphUC()
        { xLabel = Resources.WorkYears, yLabel = Resources.Percentage };

        /// <summary>
        /// The increments user control
        /// </summary>
        public MsfLineGraphUC _incrementsUC { get; } = new MsfLineGraphUC()
        { xLabel = Resources.WorkYears, yLabel = Resources.Increment };

        /// <summary>
        /// The increments user control
        /// </summary>
        public MsfLineGraphUC _incomesUC { get; } = new MsfLineGraphUC()
        { xLabel = Resources.Age, yLabel = Resources.Income };

        /// <summary>
        /// The income totals user control
        /// </summary>
        public MsfLineGraphUC _incomeTotalsUC { get; } = new MsfLineGraphUC()
        { xLabel = Resources.Age, yLabel = Resources.IncomeTotal };

        /// <summary>
        /// The data
        /// </summary>
        private IEnumerable<IIncomeIncreaseDisplayEntity> _data;

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void LoadData(IEnumerable<IIncomeIncreaseDisplayEntity> data)
        {
            _data = data;
            LoadUserControls();
            DisplayLineGraphUC(_percentagesUC);
        }

        /// <summary>
        /// Loads the user controls.
        /// </summary>
        private void LoadUserControls()
        {
            _percentagesUC.Reset();
            _incrementsUC.Reset();
            _incomesUC.Reset();
            _incomeTotalsUC.Reset();

            var educations = _data
                .Select(d => new { d.Education, d.StartingIncome, d.StartYearOfWork })
                .Distinct()
                .OrderBy(d => d.Education);

            var minAge = educations.Min(e => e.StartYearOfWork);

            var workYears = _data
                .Select(d => d.WorkYears)
                .Distinct()
                .OrderBy(d => d)
                .ToArray();

            int[] ages = new int[workYears.Length];            
            for (int i = 0; i < ages.Length; i++)
            {
                ages[i] = minAge + i;
            }

            _percentagesUC.SetXValues(workYears);
            _incrementsUC.SetXValues(workYears);
            _incomesUC.SetXValues(ages);
            _incomeTotalsUC.SetXValues(ages);

            foreach (var e in educations)
            {
                var data = _data
                    .Where(d => d.Education == e.Education)
                    .OrderBy(d => d.WorkYears)
                    .Select(d => d.Value)
                    .ToArray();

                var increments = new decimal?[data.Length];
                increments[0] = data[0] + 1;
                for (int i = 1; i < data.Length; i++)
                {
                    increments[i] = increments[i - 1] * (1 + data[i]);
                }

                var incomes = new decimal?[ages.Length];
                for (int i = 0; i < incomes.Length; i++)
                {
                    if (i < (e.StartYearOfWork - minAge))
                        incomes[i] = null;
                    else if (i == (e.StartYearOfWork - minAge))
                        incomes[i] = e.StartingIncome;
                    else
                        incomes[i] = incomes[i - 1] * (1 + data[i - (e.StartYearOfWork - minAge + 1)]);
                }

                var incomeTotals = new decimal?[incomes.Length];
                incomeTotals[0] = incomes[0] ?? 0;
                for (int i = 1; i < incomeTotals.Length; i++)
                {
                    incomeTotals[i] = incomeTotals[i - 1] + (incomes[i] ?? 0);
                }

                _percentagesUC.AddSeries(e.Education.GetLabel(), data);
                _incrementsUC.AddSeries(e.Education.GetLabel(), increments);
                _incomesUC.AddSeries(e.Education.GetLabel(), incomes);
                _incomeTotalsUC.AddSeries(e.Education.GetLabel(), incomeTotals);
            }
        }

        /// <summary>
        /// Displays the line graph uc.
        /// </summary>
        /// <param name="uc">The uc.</param>
        private void DisplayLineGraphUC(MsfLineGraphUC uc)
        {
            mainPanel.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(uc);
        }

        /// <summary>
        /// BTNs the percentages click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnPercentagesClick(object sender, EventArgs e)
        {
            DisplayLineGraphUC(_percentagesUC);
        }

        /// <summary>
        /// BTNs the increments click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnIncrementsClick(object sender, EventArgs e)
        {
            DisplayLineGraphUC(_incrementsUC);
        }

        /// <summary>
        /// BTNs the incomes click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnIncomesClick(object sender, EventArgs e)
        {
            DisplayLineGraphUC(_incomesUC);
        }

        /// <summary>
        /// BTNs the income totals click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnIncomeTotalsClick(object sender, EventArgs e)
        {
            DisplayLineGraphUC(_incomeTotalsUC);
        }
    }
}
