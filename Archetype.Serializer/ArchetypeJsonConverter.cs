using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Archetype.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Archetype.Serializer
{
    public class ArchetypeJsonConverter : JsonConverter
    {        
        #region public methods

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.ReadFrom(reader);

            if (jToken == null)
                return null;

            return DeserializeFieldsets(objectType, 
                jToken.ToString().JsonToModel<ArchetypeModel>());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var models = GenerateModels(value);

            if (models.Count < 1)
                return;

            if (models.Count == 1 && models[0] == null)
                return;

            var archetype = SerializeModelToFieldset(models);

            writer.WriteRaw(archetype.ToString(writer.Formatting));
        }

        public override bool CanConvert(Type objectType)
        {
            return Helpers.IsArchetype(objectType);
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        #endregion

        #region internal methods

        internal object DeserializeFieldsets(Type objectType, IEnumerable<ArchetypeFieldsetModel> fieldsets)
        {
            var fieldsetList = fieldsets.ToList();

            if (!fieldsetList.Any())
                return null;

            var model = Activator.CreateInstance(objectType);

            if (Helpers.ArePropertiesFieldsets(objectType))
                return DeserializeModels(model, fieldsetList);

            var modelFieldsets = fieldsetList
                .Where(fs => fs.Alias.Equals(objectType.FieldsetName()))
                .ToList();

            if (!modelFieldsets.Any())
                return null;

            if (null != model as IEnumerable<object>)
            {
                return DeserializeEnumerableModel(model as IEnumerable<object>,
                    modelFieldsets);
            }

            return DeserializeModel(model, modelFieldsets.Single());
        }

        #endregion

        #region private methods - deserialization

        private object DeserializeEnumerableModel(object obj, IEnumerable<ArchetypeFieldsetModel> fieldsets)
        {
            var itemType = obj.GetType().GetGenericArguments().FirstOrDefault() ??
                obj.GetType().BaseType.GetGenericArguments().First();

            foreach (var item in fieldsets.Select(fs => DeserializeFieldsets(itemType, new List<ArchetypeFieldsetModel> { fs })))
            {
                obj.GetType().GetMethod("Add").Invoke(obj, new[] { item });
            }

            return obj;
        }

        private object DeserializeModels(object obj,  IEnumerable<ArchetypeFieldsetModel> fieldsets)
        {
            var fieldsetList = fieldsets.ToList();

            foreach (var propInfo in obj.SerialiazableProperties())
            {
                var propertyAlias = propInfo.JsonPropertyName();
                var propertyType = propInfo.PropertyType;

                var selectedFieldsets = fieldsetList
                    .Where(fs => fs.Alias.Equals(propertyAlias))
                    .ToList();

                if (!selectedFieldsets.Any())
                    continue;

                if (Helpers.IsSystemType(propertyType))
                {
                    propInfo.SetValue(obj, GetSytemTypeValue(selectedFieldsets,
                        propertyType, propInfo.JsonPropertyName())); 
                    continue;
                }

                if (Helpers.IsNonStringIEnumerableType(propertyType) && 
                    Helpers.IsSystemType(Helpers.GetIEnumerableType(propertyType)))
                {
                    var method = GetType()
                        .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .First(m => m.IsGenericMethod && m.Name.Equals("GetIEnumerableSytemTypeValue"));
                    
                    var genericMethod = method.MakeGenericMethod(Helpers.GetIEnumerableType(propertyType));
                    var propValue = genericMethod.Invoke(this, new object[] { selectedFieldsets, propertyAlias });

                    propInfo.SetValue(obj, propValue);
                    continue;
                }

                var selectedProperties = selectedFieldsets
                    .SelectMany(fs => fs.Properties)
                    .Where(prop => prop.Alias.Equals(propInfo.JsonPropertyName()));                    

                var selectedPropertyFieldsets = selectedProperties
                    .Select(prop => prop.Value.ToString())
                    .SelectMany(json => json.JsonToModel<ArchetypeModel>() as IEnumerable<ArchetypeFieldsetModel>);
                
                propInfo.SetValue(obj, DeserializeFieldsets(propertyType, 
                    selectedPropertyFieldsets)); 
            }

            return obj;
        }

        private object DeserializeModel(object obj, ArchetypeFieldsetModel fieldset)
        {
            foreach (var propInfo in obj.SerialiazableProperties())
            {
                var propertyAlias = propInfo.JsonPropertyName();
                var propertyType = propInfo.PropertyType;

                if (!fieldset.HasProperty(propertyAlias) && !fieldset.HasValue(propertyAlias))
                    continue;

                if (Helpers.IsArchetype(propertyType))
                {
                    var archetypeModel = fieldset.GetValue(propertyAlias)
                        .JsonToModel<ArchetypeModel>();
                    propInfo.SetValue(obj, DeserializeFieldsets(propertyType, archetypeModel));
                    continue;                    
                }

                propInfo.SetValue(obj, 
                    GetArchetypePropValue(fieldset, propertyType, propertyAlias));
            }

            return obj;
        }

        private object GetArchetypePropValue(ArchetypeFieldsetModel fieldset, 
            Type propertyType, string propertyAlias)
        {
            var method = fieldset.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .First(m => m.IsGenericMethod && m.Name.Equals("GetValue"));
            var genericMethod = method.MakeGenericMethod(propertyType);
            return genericMethod.Invoke(fieldset, new object[] { propertyAlias });            
        }

        private object GetSytemTypeValue(IEnumerable<ArchetypeFieldsetModel> fieldsets,
            Type propertyType, string propertyAlias)
        {
            return GetArchetypePropValue(fieldsets.First(), propertyType, propertyAlias);
        }

        private IEnumerable<T> GetIEnumerableSytemTypeValue<T>(IEnumerable<ArchetypeFieldsetModel> fieldsets,
            string propertyAlias)
        {
            var fieldsetList = fieldsets.ToList();
            var selectedProperties = new Dictionary<int, ArchetypePropertyModel>();

            foreach (var fs in fieldsetList)
            {
                foreach (var prop in fs.Properties.Where(p => p.Alias.Equals(propertyAlias)))
                {
                    selectedProperties.Add(fieldsetList.IndexOf(fs), prop);
                }
            }

            return selectedProperties
                .Select(p => (T) GetArchetypePropValue(fieldsetList.ElementAt(p.Key),
                    typeof (T), propertyAlias))
                .ToList();
        }

        #endregion

        #region private methods - serialization


        private IList GenerateModels(object value)
        {
            var models = value as IList;

            if (null != models)
                return models;

            return new List<object>() { value };
        }

        private JObject SerializeModelToFieldset(IEnumerable models)
        {
            var jObj = new JObject
            {
                {
                    "fieldsets",
                    new JArray(new JRaw(SerializeModels(models)))
                }
            };
            return jObj;
        }

        private IEnumerable SerializeModels(IEnumerable models)
        {
            var fieldsetJson = (from object model in models 
                                where null != model 
                                select SerializeModel(model)).ToList();

            return String.Join(",", fieldsetJson);
        }

        private string SerializeModel(object value)
        {
            return value == null ? null : GetJObject(value).ToString(Formatting.None);
        }

        private JObject GetJObject(object obj)
        {
            var jObj = new JObject
                {
                    {
                        "alias",
                        new JValue(obj.GetType().FieldsetName())
                    },
                    {
                        "disabled",
                        false
                    }
                };

            var properties = GetProperties(obj);

            var fsProperties = new List<JObject>();

            foreach (var propertyInfo in properties)
            {
                var fsProperty = new JObject();
                var jProperty = new JProperty("alias", propertyInfo.JsonPropertyName());
                fsProperty.Add(jProperty);

                var propValue = propertyInfo.GetIndexParameters().Length == 0
                                    ? propertyInfo.GetValue(obj, null)
                                    : obj as string;

                fsProperty.Add(
                    new JProperty("value", GetJPropertyValue(propValue)));

                fsProperties.Add(fsProperty);
            }

            jObj.Add("properties", new JRaw(JsonConvert.SerializeObject(fsProperties)));

            return jObj;
        }

        private IEnumerable<PropertyInfo> GetProperties(object obj)
        {
            if (obj.GetType() != typeof(string))
                return obj.SerialiazableProperties();

            var indexer = ((DefaultMemberAttribute)obj.GetType()
                .GetCustomAttributes(typeof(DefaultMemberAttribute), true)[0]).MemberName;

            return new List<PropertyInfo> { obj.GetType().GetProperty(indexer) };
        }

        private object GetJPropertyValue(object value)
        {
            return Helpers.IsArchetype(value)
                ? new JRaw(JsonConvert.SerializeObject(value, this))
                : Helpers.IsNonStringIEnumerableType(value.GetType()) &&
                    Helpers.IsArchetype(Helpers.GetIEnumerableType(value.GetType()))
                        ? new JRaw(SerializeModelToFieldset(value as IEnumerable))
                        : new JValue(GetSerializedPropertyValue(value));
        }

        private string GetSerializedPropertyValue(object propValue)
        {
            if (propValue == null)
                return String.Empty;

            if (propValue is bool)
                return (bool)propValue ? GetSerializedPropertyValue(1) : GetSerializedPropertyValue(0);

            return String.Format("{0}", propValue);
        }

        #endregion

    }
}