using System;
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

            try
            {
                var archetypeModel = JsonConvert.DeserializeObject<ArchetypeModel>(jToken.ToString());
                return DeserializeFieldsets(objectType, archetypeModel);
            }
            catch (Exception ex)
            {
                return new {Exception = ex.Message};
            }
        }

        private object DeserializeFieldsets(Type objectType, IEnumerable<ArchetypeFieldsetModel> fieldsets)
        {
            var model = Activator.CreateInstance(objectType);
            var fieldsetList = fieldsets.ToList();

            if (fieldsetList.Count() == 1)
            {
                foreach (var propInfo in model.GetSerialiazableProperties())
                {
                    var propertyAlias = propInfo.GetJsonPropertyName();
                    var propertyType = propInfo.PropertyType;

                    if (!fieldsetList.Any(fs => fs.HasProperty(propertyAlias) && 
                        fs.HasValue(propertyAlias)))
                            continue;

                    var propValue = fieldsetList.Select(fs =>
                    {
                        var method = fs.GetType()
                            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                            .First(m => m.IsGenericMethod && m.Name.Equals("GetValue"));
                        var genericMethod = method.MakeGenericMethod(propertyType);
                        return genericMethod.Invoke(fs, new object[] { propertyAlias });
                    }).Single();

                    propInfo.SetValue(model, propValue);
                }
            }

            return model;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return Helpers.IsModelArchetype(objectType);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        #endregion

        //#region private methods - deserialization

        //private object DeserializeJson(Type objectType, IEnumerable<ArchetypePropertyModel> properties)
        //{

        //    var obj = Activator.CreateInstance(objectType);

        //    return null;
        //}

        //private object DeserializeEnumerableObject(object obj, JToken jToken)
        //{
        //    var model = obj as IEnumerable<object>;
        //    var fsToken = jToken;

        //    var itemType = model.GetType().GetGenericArguments().FirstOrDefault();

        //    if (itemType == null)
        //    {
        //        itemType = model.GetType().BaseType.GetGenericArguments().First();
        //        fsToken = fsToken["fieldsets"];
        //    }

        //    foreach (var fs in fsToken.Where(fs => fs["alias"].ToString().Equals(itemType.GetFieldsetName())))
        //    {
        //        var item = DeserializeJson(itemType, fs["properties"]);

        //        obj.GetType().GetMethod("Add").Invoke(obj, new[] { item });
        //    }

        //    return obj;
        //}

        //private object DeserializeObject(object obj, JToken jToken)
        //{
        //    if (jToken.SelectToken("value") != null
        //        && String.IsNullOrEmpty(jToken.SelectToken("value").ToString()))
        //        return obj;
            
        //    var properties = obj.GetSerialiazableProperties().ToList();
        //    var asFieldset = properties.Where(Helpers.HasFieldsetAttribute).ToList();

        //    foreach (var propInfo in asFieldset)
        //    {
        //        var propAlias = propInfo.GetJsonPropertyName();
        //        var fsJToken = propInfo.PropertyType.Namespace.Equals("System") 
        //                        ? GetFieldsetJTokenFromAlias(propAlias, jToken)
        //                        : GetFieldsetJTokenFromPropertyAlias(propInfo, jToken);

        //        var propJToken = GetPropertyJToken(fsJToken, propAlias);

        //        if (propJToken == null)
        //            continue;

        //        var propValue = GetPropertyValue(propInfo, propJToken);

        //        propInfo.SetValue(obj, propValue);
        //    }

        //    if (properties.All(Helpers.HasFieldsetAttribute))
        //        return obj;

        //    var objToken = GetFieldsetJTokenFromTypeAlias(obj.GetType(), jToken);

        //    if (objToken == null)
        //        return obj;

        //    if (objToken.SelectToken("properties") != null)
        //        return PopulateProperties(obj, objToken["properties"]);

        //    if ((objToken is JArray) && (objToken as JArray).Count <= 0)
        //        return null;

        //    if (!(objToken is JArray))
        //        return PopulateProperties(obj, new JArray(objToken));

        //    var defaultFsProperties = properties.Where(pInfo => !Helpers.HasFieldsetAttribute(pInfo)).ToList();                

        //    foreach (var property in defaultFsProperties)
        //    {
        //        var propJToken = ParseJTokenFromItems(objToken, property.GetJsonPropertyName());
        //        PopulateProperty(obj, propJToken["properties"], property);
        //    }

        //    return PopulateProperties(obj, new JArray(objToken));
        //}

        //private JToken GetPropertyJToken(JToken fsJToken, string propAlias)
        //{
        //    if (fsJToken == null)
        //        return null;
            
        //    //make recursive
        //    if (fsJToken.SelectToken("properties") != null)
        //        return GetPropertyAliasJToken(propAlias, fsJToken["properties"]);

        //    var nestedPropJToken = fsJToken
        //        .SingleOrDefault(p => p.SelectToken("alias").ToString().Equals(propAlias));

        //    return nestedPropJToken != null ? GetPropertyAliasJToken(propAlias, nestedPropJToken["properties"]) : null;
        //}

        //private JToken GetFieldsetJTokenFromTypeAlias(Type objType, JToken jToken)
        //{
        //    var objAlias = objType.GetFieldsetName();
        //    return GetFieldsetJTokenFromAlias(objAlias, jToken);
        //}

        //private JToken GetFieldsetJTokenFromPropertyAlias(PropertyInfo propInfo, JToken jToken)
        //{
        //    var objAlias = propInfo.GetJsonPropertyName();
        //    return GetFieldsetJTokenFromAlias(objAlias, jToken);
        //}

        //private JToken GetFieldsetJTokenFromAlias(string objAlias, JToken jToken)
        //{
        //    JToken propJToken;

        //    if (TryParseJTokenFromNamedFieldset(jToken, objAlias, out propJToken))
        //        return propJToken;

        //    if (TryParseJTokenFromNestedFieldset(jToken, objAlias, out propJToken))
        //        return propJToken;

        //    if (TryParseJTokenFromDefaultFieldset(jToken, out propJToken))
        //        return propJToken;

        //    return propJToken
        //                ?? ParseJTokenFromItem(jToken, objAlias);
        //}

        //private object PopulateProperties(object obj, JToken jToken)
        //{
        //    var properties = obj.GetSerialiazableProperties();

        //    foreach (var propertyInfo in properties)
        //    {
        //        PopulateProperty(obj, jToken, propertyInfo);
        //    }

        //    return obj;
        //}

        //private void PopulateProperty(object obj, JToken jToken, PropertyInfo propertyInfo)
        //{
        //    var propAlias = propertyInfo.GetJsonPropertyName();
        //    var propJToken = GetPropertyAliasJToken(propAlias, jToken);

        //    if (propJToken == null)
        //        return;

        //    var propValue = GetPropertyValue(propertyInfo, propJToken);
        //    propertyInfo.SetValue(obj, propValue);
        //}

        //private JToken GetPropertyAliasJToken(string propAlias, JToken jToken)
        //{
        //    return jToken.SingleOrDefault(p => p.SelectToken("alias").ToString().Equals(propAlias));
        //}

        //private object GetPropertyValue(PropertyInfo propertyInfo, JToken propJToken)
        //{
        //    return Helpers.IsModelArchetype(propertyInfo.PropertyType)
        //        ? DeserializeJson(propertyInfo.PropertyType, propJToken)
        //            : IsTypeIEnumerableArchetypeDatatype(propertyInfo.PropertyType)
        //            ? DeserializeJson(propertyInfo.PropertyType, propJToken["value"].SelectToken("fieldsets"))
        //        : GetDeserializedPropertyValue(propJToken, propertyInfo.PropertyType);
        //}

        //private object GetDeserializedPropertyValue(JToken jToken, Type type)
        //{
        //    return String.IsNullOrEmpty(jToken.ToString())
        //                ? GetDefault(type)
        //                : GetTypedValue(jToken, type);
        //}

        //private object GetTypedValue(JToken jToken, Type type)
        //{
        //    var localType = Nullable.GetUnderlyingType(type) ?? type;

        //    if (localType == typeof(bool))
        //        return jToken.ToString() == "1";

        //    return localType == typeof(DateTime)
        //        ? Convert.ToDateTime(jToken.ToString())
        //        : jToken.ToObject(localType);
        //}

        //private object GetDefault(Type type)
        //{
        //    return type.IsValueType ? Activator.CreateInstance(type) : null;
        //}

        //private JToken ParseJTokenFromItem(JToken jToken, string itemAlias)
        //{
        //    return (jToken.SelectToken("alias") != null && jToken["alias"].ToString().Equals(itemAlias))
        //            ? jToken
        //            : null;
        //}

        //private JToken ParseJTokenFromItems(JToken jToken, string itemAlias)
        //{
        //    return jToken.SingleOrDefault(jT => ParseJTokenFromItem(jT, itemAlias) != null);
        //}

        //private bool TryParseJTokenFromNamedFieldset(JToken jToken, string objAlias, out JToken resultToken)
        //{
        //    resultToken = null;

        //    if (jToken.SelectToken("fieldsets") == null)
        //        return false;

        //    resultToken = jToken["fieldsets"].SingleOrDefault(p => p.SelectToken("alias").ToString().Equals(objAlias));

        //    return resultToken != null;
        //}

        //private bool TryParseJTokenFromNestedFieldset(JToken jToken, string objAlias, out JToken resultToken)
        //{
        //    resultToken = null;

        //    if (jToken.SelectToken("value") == null || !jToken["value"].HasValues
        //                            || jToken["value"]["fieldsets"] == null)
        //        return false;

        //    resultToken = jToken["value"]["fieldsets"].SingleOrDefault(p => p.SelectToken("alias").ToString().Equals(objAlias));

        //    return resultToken != null;
        //}

        //private bool TryParseJTokenFromDefaultFieldset(JToken jToken, out JToken resultToken)
        //{
        //    resultToken = jToken.SelectToken("fieldsets");

        //    return resultToken != null;
        //}

        //private bool TryParseJTokenAsEnumerable(JToken jToken, out JToken resultToken)
        //{
        //    resultToken = null;
            
        //    //To Do: Strange newtonsoft behaviour
        //    var fsToken = jToken.Parent != null
        //        ? jToken.Parent.Children().First() is JArray
        //            ? jToken.Parent.Children().First()
        //            : jToken.Parent.Children().First()["fieldsets"] 
        //        : jToken.SelectToken("fieldsets");

        //    var jTokenEnumerable = jToken != null
        //        && fsToken != null && fsToken is JArray;

        //    if (jTokenEnumerable)
        //        resultToken = jToken;

        //    return jTokenEnumerable;
        //}

        //#endregion

    }
}