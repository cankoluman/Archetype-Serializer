using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Archetype.Serializer.Attributes;


namespace Archetype.Serializer
{
    public class Helpers
    {
        public static bool HasFieldsetAttribute(PropertyInfo pInfo)
        {
            return pInfo
                .GetCustomAttributes(typeof(FieldsetModelAttribute), true).Length > 0;
        }

        public static bool IsModelArchetype(Type type)
        {
            return type.GetCustomAttributes(typeof(ArchetypeModelAttribute), true).Length > 0;
        }

        public static bool IsModelArchetype(object value)
        {
            return value != null &&
                IsModelArchetype(value.GetType());
        }

        public static bool IsArchetypeJson(string input)
        {
            var archetypeRegex = new Regex(@"^\{\s*?\\*?""fieldsets\\*?""\s*?:\s*?\[[\s\S]*?\]\s*?}$", 
                RegexOptions.Multiline);
            return !String.IsNullOrWhiteSpace(input) && archetypeRegex.IsMatch(input);
        }
    }
}
