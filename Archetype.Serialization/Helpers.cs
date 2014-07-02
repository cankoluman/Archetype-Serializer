using System.Reflection;
using Archetype.Serialization.Attributes;

namespace Archetype.Serialization
{
    public class Helpers
    {
        public static bool HasFieldsetAttribute(PropertyInfo pInfo)
        {
            return pInfo
                .GetCustomAttributes(typeof(FieldsetAttribute), true).Length > 0;
        }
    }
}
