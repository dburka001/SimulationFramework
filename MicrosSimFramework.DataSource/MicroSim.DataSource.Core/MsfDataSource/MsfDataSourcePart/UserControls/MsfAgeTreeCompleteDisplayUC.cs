﻿using System;
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
    public partial class MsfAgeTreeCompleteDisplayUC : 
        MsfAgeTreeDisplayGenericUC<BirthOrder, IPopulationCompleteEntity, IPopulationEduEntity>
    {
        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public override UserControl View { get; } = new MsfAgeTreeEduDisplayUC();

        /// <summary>
        /// Creates the total data.
        /// </summary>
        protected override void CreateTotalData()
        {
            TotalData.AddRange(Data
                .GroupBy(d => new { d.Age, d.Year, d.Gender, d.Education })
                .Select(g => new AgeTreeEduEntity()
                {
                    Age = g.Key.Age,
                    Year = g.Key.Year,
                    Gender = g.Key.Gender,
                    Education = g.Key.Education,
                    Value = g.Count(d => d.Value != null) == 0 ? null : g.Sum(d => d.Value)
                }));
        }

        /// <summary>
        /// Gets the sub data.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        protected override IEnumerable<IPopulationEduEntity> GetSubData(Enum e)
            => Data.Where(d => d.BirthOrder == (BirthOrder)e);
    }
}
