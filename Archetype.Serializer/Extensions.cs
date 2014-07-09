using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Archetype.Models;
using Archetype.Serializer.Attributes;
using Newtonsoft.Json;

namespace Archetype.Serializer
{
    public static class Extensions
    {
        internal static string GetFieldsetName(this Type type)
        {
            var attributes = type.GetCustomAttributes(true);
            var archetypeDatatypeAttribute = (ArchetypeModelAttribute)attributes.FirstOrDefault(attr => attr is ArchetypeModelAttribute);

            return archetypeDatatypeAttribute != null ? archetypeDatatypeAttribute.FieldsetName : type.Name;
        }

        internal static string GetJsonPropertyName(this PropertyInfo propInfo)
        {
            var attributes = propInfo.GetCustomAttributes(true);
            var jsonPropAttribute = (JsonPropertyAttribute)attributes.FirstOrDefault(attr => attr is JsonPropertyAttribute);

            return jsonPropAttribute != null ? jsonPropAttribute.PropertyName : propInfo.Name;
        }

        internal static IEnumerable<PropertyInfo> GetSerialiazableProperties(this object obj)
        {
            if (obj == null)
                return new List<PropertyInfo>();

            return obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => !Attribute.IsDefined(prop, typeof(JsonIgnoreAttribute)));
        }

        internal static T GetModelFromJson<T>(this string json,
            bool returnInstanceIfNull = false, JsonConverter converter = null)
            where T : class, new()
        {
            var model = converter != null ?
                JsonConvert.DeserializeObject<T>(json, converter) :
                JsonConvert.DeserializeObject<T>(json);
            
            return  model ?? (returnInstanceIfNull ? Activator.CreateInstance<T>() : default(T));
        }

        public static T GetModelFromArchetypeJson<T>(this string json,
            bool returnInstanceIfNull = false)
            where T : class, new()
        {
            var model = json.GetModelFromJson<T>(returnInstanceIfNull, 
                new ArchetypeJsonConverter());

            return model ?? (returnInstanceIfNull ? Activator.CreateInstance<T>() : default(T));
        }

        public static T MapArchetypeToModel<T>(this ArchetypeModel archetype,
            bool returnInstanceIfNull = false)
            where T : class, new()
        {
            var json = JsonConvert.SerializeObject(archetype);
            return json.GetModelFromArchetypeJson<T>(returnInstanceIfNull);
        }
    }
}
