using MicroSim.DataSource.Core;
using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim.DataSource.Other
{
    /// <summary>
    /// DspIncomeIncreaseGraphs class
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfDataSourcePart" />
    public class DspIncomeIncreaseGraphs : MsfDataSourcePart<IncomeIncreaseDisplayEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DspIncomeIncreaseGraphs"/> class.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public DspIncomeIncreaseGraphs(params MsfDataSourcePart[] inputs)
            : base(inputs) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title { get; set; } = Resources.DspIncomeIncreaseGraphs;

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public override UserControl View { get; } = new IncomeIncreaseUC();

        /// <summary>
        /// Generates the data.
        /// </summary>
        protected override void GenerateData()
        {
            var output = new List<IncomeIncreaseDisplayEntity>();
            var baseData = GetInputDataOfType<IncomeIncreaseEntity>();
            var eduVars = GetInputDataOfType<EduBasedVarEntity>();

            var data = baseData
                .Join(eduVars,
                b => b.Education,
                e => e.Education,
                (b, e) => new
                {
                    b.WorkYears,
                    b.Education,
                    b.Value,
                    e.StartingIncome,
                    e.StartYearOfWork
                });

            foreach (var d in data)
            {
                output.Add(new IncomeIncreaseDisplayEntity()
                {
                    WorkYears = d.WorkYears,
                    Education = d.Education,
                    StartingIncome = (decimal)d.StartingIncome,
                    StartYearOfWork = Convert.ToInt32(d.StartYearOfWork),
                    Value = d.Value
                });
            }

            Data = output;
        }
    }
}
