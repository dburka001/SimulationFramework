using RDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Rtools
{
    /// <summary>
    /// Extension methods for REngine
    /// </summary>
    public static class REngineExtensions
    {
        /// <summary>
        /// Creates the numeric variable.
        /// </summary>
        /// <param name="engine">The engine.</param>
        public static void CreateVariable(this REngine engine, string variable, object value)
        {
            SymbolicExpression exp = null;
            if (value is string) exp = engine.CreateCharacter((string)value);
            else if (value is double) exp = engine.CreateNumeric((double)value);
            else if (value is int) exp = engine.CreateNumeric((int)value);
            else if (value is double[]) exp = engine.CreateNumericVector((double[])value);
            else if (value is int[]) exp = engine.CreateNumericVector(Array.ConvertAll((int[])value, val => (double)val));            
            else if (value is double[,]) exp = engine.CreateNumericMatrix((double[,])value);
            else throw new NotImplementedException();
            engine.SetSymbol(variable, exp);
        }
    }
}
