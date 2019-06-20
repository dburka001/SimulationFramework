using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSim.DataSource.Entities
{
    /// <summary>
    /// Extension methods used in multiple different Enums
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Determines whether this instance is filtered.
        /// </summary>
        /// <param name="enum">The enum.</param>
        /// <returns>
        ///   <c>true</c> if the specified enum is filtered; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool IsFiltered(this Enum e)
        {
            if(e is BirthOrder)
                switch (e)
                {
                    case BirthOrder.Total: return true;
                    default: return false;
                }

            if (e is Gender)
                switch (e)
                {
                    case Gender.Total: return true;
                    default: return false;
                }

            if (e is Education)
                switch (e)
                {
                    case Education.Total: return true;
                    default: return false;
                }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether this instance is hidden.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>
        ///   <c>true</c> if the specified e is hidden; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool IsHidden(this Enum e)
        {
            if (e is Gender)
                switch (e)
                {
                    case Gender.Total: return true;
                    default: return false;
                }

            if (e is BirthOrder)
                switch (e)
                {
                    case BirthOrder.B0: return true;
                    default: return false;
                }

            if (e is Education)
                switch (e)
                {
                    case Education.NAP: return true;
                    case Education.UNK: return true;                    
                    default: return false;
                }

            if (e is DetEducationType)
                switch (e)
                {
                    case DetEducationType.Total: return true;
                    default: return false;
                }

            if (e is PensionType)
                switch (e)
                {
                    case PensionType.Total: return true;
                    default: return false;
                }

            return false;
        }

        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Enum GetTotal(this Enum e)
        {
            if (e is BirthOrder) return BirthOrder.Total;

            if (e is Education) return Education.Total;             

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="birthOrder">The birth order.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static string GetLabel(this Enum e)
        {
            if (e is Gender)
                switch (e)
                {
                    case Gender.Total: return Resources.Total;
                    case Gender.Male: return Resources.Male;
                    case Gender.Female: return Resources.Female;
                    default: throw new NotImplementedException();
                }

            if (e is BirthOrder)
                switch (e)
                {
                    case BirthOrder.Total: return Resources.Total;
                    case BirthOrder.B0:
                    case BirthOrder.B1: 
                    case BirthOrder.B2: 
                    case BirthOrder.B3: 
                    case BirthOrder.B4: return String.Format(Resources.BirthOrderLabel, (byte)(BirthOrder)e - 1);
                    case BirthOrder.B5more: return String.Format(Resources.BirthOrderMoreLabel, (byte)(BirthOrder)e - 1);
                    default: throw new NotImplementedException();
                }

            if (e is Education)
                switch (e)
                {
                    case Education.Total: return Resources.Total;
                    case Education.ED0_2: return Resources.EducationED02;
                    case Education.ED3_4: return Resources.EducationED34;
                    case Education.ED5_8: return Resources.EducationED58;
                    case Education.UNK: return Resources.Unknown;
                    case Education.NAP: return Resources.NAP;
                    default: throw new NotImplementedException();
                }

            if (e is DetEducationType)
                switch (e)
                {
                    case DetEducationType.Normal: return Resources.Normal;
                    case DetEducationType.Pessimistic: return Resources.Pessimistic;
                    case DetEducationType.Optimistic: return Resources.Optimistic;                    
                    default: throw new NotImplementedException();
                }

            if (e is WorkStatus)
                switch (e)
                {
                    case WorkStatus.A: return Resources.WorkStatusA;
                    case WorkStatus.B1: return Resources.WorkStatusB1;
                    case WorkStatus.B2: return Resources.WorkStatusB2;
                    case WorkStatus.B3: return Resources.WorkStatusB3;
                    case WorkStatus.B4: return Resources.WorkStatusB4;
                    case WorkStatus.G: return Resources.WorkStatusG;
                    default: throw new NotImplementedException();
                }

            if (e is SocialGroup)
                switch (e)
                {
                    case SocialGroup.Majority: return Resources.Majority;
                    case SocialGroup.Minority: return Resources.Minority;
                    case SocialGroup.Immigrant: return Resources.Immigrant;
                    default: throw new NotImplementedException();
                }

            if (e is PensionType)
                switch (e)
                {
                    case PensionType.Base: return Resources.Base;
                    case PensionType.Variable: return Resources.Variable;
                    case PensionType.Hybrid: return Resources.Hybrid;
                    default: throw new NotImplementedException();
                }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetDisplayName(Type t)
        {
            if (t == typeof(Gender)) return Resources.Gender;
            if (t == typeof(Education)) return Resources.Education;
            if (t == typeof(DetEducationType)) return Resources.DetEducationType;
            if (t == typeof(WorkStatus)) return Resources.WorkStatus;
            if (t == typeof(SocialGroup)) return Resources.SocialGroup;
            if (t == typeof(PensionType)) return Resources.PensionType;

            throw new NotImplementedException();
        }
    }
}
