﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Archetype.Serializer.Attributes;
using Newtonsoft.Json;

namespace Archetype.Serializer
{
    public static class Extensions
    {
        public static string GetFieldsetName(this Type type)
        {
            var attributes = type.GetCustomAttributes(true);
            var archetypeDatatypeAttribute = (AsArchetypeAttribute)attributes.FirstOrDefault(attr => attr is AsArchetypeAttribute);

            return archetypeDatatypeAttribute != null ? archetypeDatatypeAttribute.FieldsetName : type.Name;
        }

        public static string GetJsonPropertyName(this PropertyInfo propInfo)
        {
            var attributes = propInfo.GetCustomAttributes(true);
            var jsonPropAttribute = (JsonPropertyAttribute)attributes.FirstOrDefault(attr => attr is JsonPropertyAttribute);

            return jsonPropAttribute != null ? jsonPropAttribute.PropertyName : propInfo.Name;
        }

        public static IEnumerable<PropertyInfo> GetSerialiazableProperties(this object obj)
        {
            if (obj == null)
                return new List<PropertyInfo>();

            return obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => !Attribute.IsDefined(prop, typeof(JsonIgnoreAttribute)));
        }

    }
}