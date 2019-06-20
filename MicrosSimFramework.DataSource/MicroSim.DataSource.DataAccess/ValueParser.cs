using MicroSim.DataSource.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.DataAccess
{
    /// <summary>
    /// ValueParser class
    /// </summary>
    public class ValueParser
    {
        /// <summary>
        /// The english culture information
        /// </summary>
        private static CultureInfo _englishCultureInfo = new CultureInfo("en-US");

        private static CultureInfo _hungarianCultureInfo = new CultureInfo("hu-HU");

        /// <summary>
        /// The instance
        /// </summary>
        private static ValueParser _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ValueParser Instance
            => _instance ?? (_instance = new ValueParser());

        /// <summary>
        /// Prevents a default instance of the <see cref="ValueParser"/> class from being created.
        /// </summary>
        private ValueParser() { }

        /// <summary>
        /// Converts from string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object Parse(string value, Type type)
        {
            value = value.Trim();

            if (type != typeof(string) && value == String.Empty)
                return null;
            if (type == typeof(string))
                return value;
            if (type == typeof(int) || type == typeof(int?))
                return ParseInt(value);
            if (type == typeof(decimal) || type == typeof(decimal?))
                return ParseDecimal(value);
            if (type == typeof(Gender) || type == typeof(Gender?))
                return GenderExtensions.Parse(value);
            if (type == typeof(Education) || type == typeof(Education?))
                return EducationExtensions.Parse(value);
            if (type == typeof(DetEducationType) || type == typeof(DetEducationType?))
                return DetEducationTypeExtensions.Parse(value);
            if (type == typeof(BirthOrder) || type == typeof(BirthOrder?))
                return BirthOrderExtensions.Parse(value);
            if (type == typeof(WorkStatus) || type == typeof(WorkStatus?))
                return WorkStatusExtensions.Parse(value);
            if (type == typeof(SocialGroup) || type == typeof(SocialGroup?))
                return SocialGroupExtensions.Parse(value);
            if (type == typeof(PensionType) || type == typeof(PensionType?))
                return PensionTypeExtensions.Parse(value);

            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// Parses the decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private decimal? ParseDecimal(string value)
        {
            string[] values = value.Split(' ');
            value = values[0];

            if (value == ":")
                return null;

            if(value.Contains("."))
                return decimal.Parse(value, _englishCultureInfo);
            else
                return decimal.Parse(value, _hungarianCultureInfo);                      
        }

        /// <summary>
        /// Parses the int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private int? ParseInt(string value)
        {
            string[] values = value.Split(' ');
            value = values[0];

            if (value == ":")
                return null;
            
            return int.Parse(value);
        }
    }
}
