using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Archetype.Models;
using Archetype.Serializer.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Archetype.Serializer
{
    public static class Extensions
    {
        internal static string FieldsetName(this Type type)
        {
            var attributes = type.GetCustomAttributes(true);
            var archetypeDatatypeAttribute = (ArchetypeModelAttribute)attributes.FirstOrDefault(attr => attr is ArchetypeModelAttribute);

            return archetypeDatatypeAttribute != null ? archetypeDatatypeAttribute.FieldsetName : type.Name;
        }

        internal static string JsonPropertyName(this PropertyInfo propInfo)
        {
            var attributes = propInfo.GetCustomAttributes(true);
            var jsonPropAttribute = (JsonPropertyAttribute)attributes.FirstOrDefault(attr => attr is JsonPropertyAttribute);

            return jsonPropAttribute != null ? jsonPropAttribute.PropertyName : propInfo.Name;
        }

        internal static IEnumerable<PropertyInfo> SerialiazableProperties(this object obj)
        {
            if (obj == null)
                return new List<PropertyInfo>();

            return obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => !Attribute.IsDefined(prop, typeof(JsonIgnoreAttribute)));
        }

        internal static T JsonToModel<T>(this string json,
            bool returnInstanceIfNull = false, JsonConverter converter = null)
            where T : class, new()
        {
            var model = converter != null ?
                JsonConvert.DeserializeObject<T>(json, converter) :
                JsonConvert.DeserializeObject<T>(json);
            
            return  model ?? (returnInstanceIfNull ? Activator.CreateInstance<T>() : default(T));
        }

        public static T ToModel<T>(this string archetypeJson,
            bool returnInstanceIfNull = false)
            where T : class, new()
        {                      
            if (!Helpers.IsArchetypeJson(archetypeJson))
                throw new ArgumentException(String.Format("{0} is not Archetype Json", archetypeJson));
            
            var model = archetypeJson.JsonToModel<T>(returnInstanceIfNull, 
                new ArchetypeJsonConverter());

            return model ?? (returnInstanceIfNull ? Activator.CreateInstance<T>() : default(T));
        }

        public static T ToModel<T>(this ArchetypeModel archetype,
            bool returnInstanceIfNull = false)
            where T : class, new()
        {
            if (archetype == null || !archetype.ToList().Any())
                return null;

            var jsonConverter = new ArchetypeJsonConverter();
            return (T)jsonConverter.DeserializeFieldsets(typeof(T), archetype);
        }
    }
}
