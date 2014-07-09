using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Archetype.Serializer.Attributes;


namespace Archetype.Serializer
{
    public class Helpers
    {
        public static bool IsModelArchetype(Type type)
        {
            return type.GetCustomAttributes(typeof(ArchetypeModelAttribute), true).Length > 0;
        }

        public static bool IsModelArchetype(object value)
        {
            return value != null &&
                IsModelArchetype(value.GetType());
        }

        public static bool ArePropertiesFieldsets(Type type)
        {
            var archetypeAttribute = type
                .GetCustomAttributes(typeof(ArchetypeModelAttribute), true);

            return archetypeAttribute.Length > 0 &&
                (archetypeAttribute.First() as ArchetypeModelAttribute).ArePropertiesFieldsets;
        }

        public static bool IsArchetypeJson(string input)
        {
            var archetypeRegex = new Regex(@"^\{\s*?\\*?""fieldsets\\*?""\s*?:\s*?\[[\s\S]*?\]\s*?}$", 
                RegexOptions.Multiline);
            return !String.IsNullOrWhiteSpace(input) && archetypeRegex.IsMatch(input);
        }

        public static bool IsIEnumerableType(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static bool IsNonStringIEnumerableType(Type type)
        {
            return IsIEnumerableType(type) && typeof(String) != type;
        }

        public static bool IsSystemType(Type type)
        {
            return type.Namespace != null && type.Namespace.Equals("System");
        }

        public static Type GetIEnumerableType(Type type)
        {
            return type.GetGenericArguments().FirstOrDefault() ??
                type.BaseType.GetGenericArguments().First();
        }
    }
}
