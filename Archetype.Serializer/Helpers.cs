using System;
using System.Reflection;
using Archetype.Serializer.Attributes;


namespace Archetype.Serializer
{
    public class Helpers
    {
        public static bool HasFieldsetAttribute(PropertyInfo pInfo)
        {
            return pInfo
                .GetCustomAttributes(typeof(AsFieldsetAttribute), true).Length > 0;
        }

        public static bool IsModelArchetype(Type type)
        {
            return type.GetCustomAttributes(typeof(AsArchetypeAttribute), true).Length > 0;
        }

        public static bool IsModelArchetype(object value)
        {
            return value != null &&
                IsModelArchetype(value.GetType());
        }
    }
}
