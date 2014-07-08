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
                return DeserializeFieldsets(objectType, 
                    jToken.ToString().GetModelFromJson<ArchetypeModel>());
            }
            catch (Exception ex)
            {
                return new {Exception = ex.Message};
            }
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


        #region private methods - deserialization

        private object DeserializeFieldsets(Type objectType, IEnumerable<ArchetypeFieldsetModel> fieldsets)
        {
            var fieldsetList = fieldsets.ToList();

            if (!fieldsetList.Any())
                return null;

            var model = Activator.CreateInstance(objectType);

            if (Helpers.ArePropertiesFieldsets(objectType))
                return DeserializeModels(model, fieldsetList);

            var modelFieldsets = fieldsetList
                .Where(fs => fs.Alias.Equals(objectType.GetFieldsetName()))
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

            foreach (var propInfo in obj.GetSerialiazableProperties())
            {
                IEnumerable<ArchetypePropertyModel> selectedProperties;

                var propertyAlias = propInfo.GetJsonPropertyName();
                var propertyType = propInfo.PropertyType;
                var archetypePropertyAlias = propertyType.GetFieldsetName();

                var selectedFieldsets = fieldsetList
                    .Where(fs => fs.Alias.Equals(propertyAlias))
                    .ToList();

                if (propInfo.PropertyType.Namespace.Equals("System"))
                {
                    archetypePropertyAlias = propInfo.GetJsonPropertyName();
                    selectedProperties = selectedFieldsets
                        .SelectMany(fs => fs.Properties)
                        .Where(prop => prop.Alias.Equals(archetypePropertyAlias))
                        .ToList();

                    propInfo.SetValue(obj, 
                        selectedProperties.Count() > 1 ?
                            selectedProperties.Select(p => 
                                GetArchetypePropValue(selectedFieldsets.First(),
                                propertyType, archetypePropertyAlias)) :
                            GetArchetypePropValue(selectedFieldsets.First(),
                                propertyType, archetypePropertyAlias)); 

                    continue;
                }

                selectedProperties = selectedFieldsets
                    .SelectMany(fs => fs.Properties)
                    .Where(prop => prop.Alias.Equals(archetypePropertyAlias));                    

                var selectedPropertyFieldsets = selectedProperties
                    .Select(prop => prop.GetValue<string>())
                    .SelectMany(json => json.GetModelFromJson<ArchetypeModel>() as IEnumerable<ArchetypeFieldsetModel>);
                
                propInfo.SetValue(obj, DeserializeFieldsets(propertyType, 
                    selectedPropertyFieldsets)); 
            }

            return obj;
        }

        private object DeserializeModel(object obj, ArchetypeFieldsetModel fieldset)
        {
            foreach (var propInfo in obj.GetSerialiazableProperties())
            {
                var propertyAlias = propInfo.GetJsonPropertyName();
                var propertyType = propInfo.PropertyType;

                if (!fieldset.HasProperty(propertyAlias) && !fieldset.HasValue(propertyAlias))
                    continue;

                if (Helpers.IsModelArchetype(propertyType))
                {
                    var archetypeModel = fieldset.GetValue(propertyAlias)
                        .GetModelFromJson<ArchetypeModel>();
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

        #endregion

    }
}