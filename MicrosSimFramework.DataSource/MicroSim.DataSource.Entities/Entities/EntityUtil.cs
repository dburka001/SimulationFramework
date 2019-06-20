using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MicroSim.DataSource.Entities
{
    public static class EntityUtil
    {
        private static DisplayNameAttribute GetDisplayNameAttr(PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttributes()
                .OfType<DisplayNameAttribute>()
                .FirstOrDefault();

        public static string GetPropertyCaption(PropertyInfo property)
        {
            var displayNameAttr = property.DeclaringType.GetInterfaces()
                .SelectMany(i => i.GetProperties())
                .Select(p => new { name = p.Name, attr = GetDisplayNameAttr(p) })
                .FirstOrDefault(p => p.attr != null && string.Equals(p.name, property.Name))?.attr;

            return displayNameAttr?.DisplayName 
                ?? GetDisplayNameAttr(property)?.DisplayName
                ?? property.Name;
        }
    }
}