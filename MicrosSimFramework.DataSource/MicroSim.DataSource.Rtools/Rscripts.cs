using RDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Rtools
{
    /// <summary>
    /// Allows the use of built in R scripts
    /// </summary>
    public sealed class Rscripts
    {
        /// <summary>
        /// Creates the engine.
        /// </summary>
        /// <returns></returns>
        private static REngine CreateEngine()
        {
            var engine = REngine.GetInstance();
            engine.Initialize();
            return engine;
        }

        /// <summary>
        /// Extrapolates the population.
        /// </summary>
        /// <param name="startValue">The start value.</param>
        /// <param name="steps">The steps.</param>
        /// <param name="totalValue">The total value.</param>
        /// <returns></returns>
        public static double[] ExtrapolatePopulation(double startValue, int steps, double totalValue)
            => RunScript(nameof(ExtrapolatePopulation), startValue, steps, totalValue)
            .AsNumeric().ToArray<double>();

        /// <summary>
        /// Interpolates the mortality.
        /// </summary>
        /// <param name="inputArray">The input array.</param>
        /// <returns></returns>
        public static double[] InterpolateMortality(double[] inputArray)
            => RunScript(nameof(InterpolateMortality), inputArray)
            .AsNumeric().ToArray<double>();

        /// <summary>
        /// Smoothes the values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double[] SmoothValues(double[] values)
            => RunScript(nameof(SmoothValues), values)
            .AsNumeric().ToArray<double>();

        /// <summary>
        /// Forecasts the mortality.
        /// </summary>
        /// <param name="inputMatrix">The input matrix.</param>
        /// <param name="forecastYears">The forecast years.</param>
        public static MortalityForecastEntity ForecastDemography(string type, double[,] mortality, double[,] pop, int[] ages, int[] years, int forecastYears)
        {
            var result = RunScript(nameof(ForecastDemography), type, mortality, pop, ages, years, forecastYears).AsList();

            var resultMatrix = result[0].AsNumeric();
            var outputMatrix = new double[ages.Length, forecastYears];

            int counter = 0;
            for (int y = 0; y < outputMatrix.GetLength(1); y++)
            {
                for (int a = 0; a < outputMatrix.GetLength(0); a++)
                {
                
                    outputMatrix[a, y] = resultMatrix[counter];
                    counter++;
                }
            }

            return new MortalityForecastEntity()
            {
                Matrix = outputMatrix,
                KtLower = result[1].AsNumeric().ToArray<double>(),
                Kt = result[1].AsNumeric().ToArray<double>(),
                KtUpper = result[1].AsNumeric().ToArray<double>()
            };
        }

        /// <summary>
        /// Runs the script.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="variables">The variables.</param>
        /// <returns></returns>
        private static SymbolicExpression RunScript(string methodName, params object[] variables)
        {
            var engine = CreateEngine();

            var m = typeof(Rscripts).GetMethod(methodName);
            int pCounter = 0;
            foreach (var p in m.GetParameters())
            {
                engine.CreateVariable(p.Name, variables[pCounter]);
                pCounter++;
            }

            return engine.Evaluate(LoadRScript(methodName));
        }

        /// <summary>
        /// Loads the r script.
        /// </summary>
        /// <param name="scriptId">The script identifier.</param>
        /// <returns></returns>
        private static string LoadRScript(string scriptId)
        {
            Assembly Assembly = typeof(Rscripts).Assembly;
            string ResourceName = String.Format("{0}.Resources", Assembly.GetName().Name);
            return new ResourceManager(ResourceName, Assembly).GetString(scriptId);
        }
    }
}
