﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MicroSim.DataSource.Rtools {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MicroSim.DataSource.Rtools.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ExtrapolatePopulation &lt;- function(startValue, steps, totalValue) {
        ///    Gompertz &lt;- function(x, a, c) {
        ///        (exp(-c * (exp(a * x) - 1)))
        ///    }
        ///
        ///    TargetFunction &lt;- function(p) {
        ///        g &lt;- startValue * Gompertz(x, p[1], p[2])
        ///        abs(sum(round(g, digits = 0)) - totalValue)
        ///    }
        ///
        ///    x &lt;- 0:steps
        ///    a = 0.5
        ///    c = 0.5
        ///
        ///    par = c(a, c)
        ///
        ///    result &lt;- optim(par, TargetFunction)$par
        ///    
        ///    return(startValue * Gompertz(x, result[1], result[2]))
        ///}
        ///
        ///ExtrapolatePopulation(star [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExtrapolatePopulation {
            get {
                return ResourceManager.GetString("ExtrapolatePopulation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to if (!&quot;demography&quot; %in% installed.packages()) { install.packages(&quot;demography&quot;) }
        ///library(&quot;demography&quot;)
        ///
        ///ForecastMortality &lt;- function(mortality, pop, ages, years, forecastYears) {
        ///    data &lt;- demogdata(mortality, pop, ages, years, &quot;mortality&quot;, &quot;Forecast&quot;, &quot;Total&quot;)
        ///    lcaModel &lt;- lca(data, series = &quot;Total&quot;, max.age = max(ages), interpolate=TRUE)
        ///    lcaResult &lt;- forecast(lcaModel, h = forecastYears)
        ///    output &lt;- lcaResult$rate$Total
        ///    return(list(&quot;output&quot; = output, &quot;kt_lower&quot; = lcaResult$kt.f$lowe [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ForecastDemography {
            get {
                return ResourceManager.GetString("ForecastDemography", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to InterpolateMortality &lt;- function(inputArray) {
        ///    dat &lt;- inputArray
        ///    #dat[length(dat)] = 1
        ///    output &lt;- dat
        ///
        ///    non.na.pos &lt;- which(!is.na(dat))
        ///    na.pos &lt;- which(is.na(dat))
        ///
        ///    for (i in na.pos) {
        ///        minId = non.na.pos[max(which((non.na.pos &lt; i) == TRUE))]
        ///        maxId = non.na.pos[min(which((non.na.pos &lt; i) == FALSE))]
        ///        multiplier = (i - minId) / (maxId - minId)
        ///        output[i] = dat[minId] + (dat[maxId] - dat[minId]) * multiplier
        ///    }
        ///
        ///    return(output)
        ///}
        ///
        ///Int [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InterpolateMortality {
            get {
                return ResourceManager.GetString("InterpolateMortality", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SmoothValues &lt;- function(values) {    
        ///    return(smooth(values))
        ///}
        ///
        ///SmoothValues(values).
        /// </summary>
        internal static string SmoothValues {
            get {
                return ResourceManager.GetString("SmoothValues", resourceCulture);
            }
        }
    }
}
