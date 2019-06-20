using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Settings class
    /// </summary>
    public static class Settings
    {
        private static NameValueCollection appSettings = ConfigurationManager.AppSettings;

        public static string WorkingDirectory => appSettings.Get("WorkingDirectory");
        public static string GeoFilter => appSettings.Get("GeoFilter");
        public static int StartYearOfValidation => int.Parse(appSettings.Get("StartYearOfValidation"));
        public static int StartYear => int.Parse(appSettings.Get("StartYear"));
        public static int ForecastYears => int.Parse(appSettings.Get("ForecastYears"));
        public static int AgeLimit => int.Parse(appSettings.Get("AgeLimit"));
        public static int DetEducationAgeStart => int.Parse(appSettings.Get("DetEducationAgeStart"));
        public static int FertilityForecastFromYear => int.Parse(appSettings.Get("FertilityForecastFromYear"));
        public static int MortalityForecastFromYear => int.Parse(appSettings.Get("MortalityForecastFromYear"));
        public static bool SmoothProbabilities => bool.Parse(appSettings.Get("SmoothProbabilities"));        
    }
}
