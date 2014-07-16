using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Archetype.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Archetype.Serializer
{
    public class ArchetypeJsonConverter : JsonConverter
    {
        private const string _ROOT_FS_ALIAS = "rootFs";
        
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

        private object GetArchetypePropValue(ArchetypePropertyModel property,
            Type propertyType, string propertyAlias)
        {
            var method = property.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .First(m => m.IsGenericMethod && m.Name.Equals("GetValue"));
            var genericMethod = method.MakeGenericMethod(propertyType);

            return genericMethod.Invoke(property, null);
        }

        private object GetSytemTypeValue(IEnumerable<ArchetypeFieldsetModel> fieldsets,
            Type propertyType, string propertyAlias)
        {
            return GetArchetypePropValue(fieldsets.First(), propertyType, propertyAlias);
        }

        private IEnumerable<T> GetIEnumerableSytemTypeValue<T>(IEnumerable<ArchetypeFieldsetModel> fieldsets,
            string propertyAlias)
        {
            return  fieldsets
                        .SelectMany(fs => fs.Properties.Where(p => p.Alias.Equals(propertyAlias)))            
                        .Select(p => GetArchetypePropValue(p, typeof (T), propertyAlias))
                        .Cast<T>().ToList();
        }

        #endregion

        #region private methods - serialization


        private IList GenerateModels(object value)
        {
            var models = value as IList;

            if (null != models)
                return models;

            if (Helpers.ArePropertiesFieldsets(value.GetType()))
                return value.SerialiazableProperties()
                .Select(p =>
                {
                    var fsModel = GetDynamicModel(p.JsonPropertyName());
                    fsModel.Add(p.JsonPropertyName(), p.GetValue(value));
                    return fsModel;
                })
                .ToList();

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
            if (value == null)
                return null;

            var jObj = Helpers.IsExpandoObject(value)
                ? GetJObjectFromExpandoObject(value as IDictionary<string, object>)
                : GetJObject(value);

            return jObj.ToString(Formatting.None);
        }

        private JObject GetJObjectFromExpandoObject(IDictionary<string, object> expandoObj)
        {
            var jObj = InitFieldset((string)expandoObj[_ROOT_FS_ALIAS]);

            var fsProperties = new List<JObject>();

            foreach (var item in expandoObj.SkipWhile(i => i.Key.Equals(_ROOT_FS_ALIAS)))
            {
                var property = item;
                var alias = property.Key;
                var value = property.Value;

                if (value != null 
                    && Helpers.IsNonStringIEnumerableType(value.GetType()))
                {
                    foreach (var elt in value as IEnumerable)
                    {
                        fsProperties.Add(new JObject 
                        {
                            new JProperty("alias", alias), 
                            new JProperty("value", GetJPropertyValue(elt))
                        });                        
                    }
                    continue;
                }

                fsProperties.Add(new JObject 
                {
                    new JProperty("alias", alias), 
                    new JProperty("value", GetJPropertyValue(value))
                });
            }

            jObj.Add("properties", new JRaw(JsonConvert.SerializeObject(fsProperties, this)));
            return jObj;
        }

        private JObject GetJObject(object obj)
        {
            var jObj = InitFieldset(obj.GetType().FieldsetName());

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
            if (value == null)
                return new JValue(String.Empty);

            if (Helpers.IsArchetype(value))
                return new JRaw(JsonConvert.SerializeObject(value, this));

            if (Helpers.IsNonStringIEnumerableType(value.GetType()) &&
                Helpers.IsArchetype(Helpers.GetIEnumerableType(value.GetType())))
                return new JRaw(SerializeModelToFieldset(value as IEnumerable));

            return new JValue(GetSerializedPropertyValue(value));
        }

        private string GetSerializedPropertyValue(object propValue)
        {
            if (propValue == null)
                return String.Empty;

            if (propValue is bool)
                return (bool)propValue ? GetSerializedPropertyValue(1) : GetSerializedPropertyValue(0);

            return String.Format("{0}", propValue);
        }

        private IDictionary<string, object> GetDynamicModel(string rootFsAlias)
        {
            var dynamicModel = new ExpandoObject() as IDictionary<string, object>;
            dynamicModel.Add(_ROOT_FS_ALIAS, rootFsAlias);
            return dynamicModel;
        }

        private JObject InitFieldset(string alias)
        {
            return new JObject
            {
                {
                    "alias",
                    new JValue(alias)
                },
                {
                    "disabled",
                    false
                }
            };
        }

        #endregion

    }
}