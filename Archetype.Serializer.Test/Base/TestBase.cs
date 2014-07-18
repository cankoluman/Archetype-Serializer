using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Archetype.Serializer.Test.Base
{
    public abstract class TestBase
    {
        protected void AssertAreEqual<T>(T model, T result)
        {
            foreach (var propInfo in model.SerialiazableProperties())
            {
                var expected = GetExpectedValue(model, propInfo);
                var actual = GetActualValue(result, propInfo);

                if (propInfo.PropertyType.Namespace.Equals("System"))
                {
                    Assert.AreEqual(expected, actual);
                    continue;
                }

                var list = expected as IList;
                if (list != null)
                {
                    foreach (var expectedItem in list)
                    {
                        var index = list.IndexOf(expectedItem);
                        AssertAreEqual(expectedItem, ((IList) actual)[index]);
                    }
                    continue;
                }

                AssertAreEqual(expected, actual);
            }
        }

        protected object GetModelFromJson(string modelAlias, string json)
        {
            var method = typeof(Extensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.IsGenericMethod && m.Name.Equals("ToModel")
                    && m.GetParameters().Any(p => p.Name.Equals("archetypeJson")));

            var typeAlias = String.Format("{0}.{1}", GetType().Namespace, modelAlias);
            var genericMethod = method.MakeGenericMethod(Type.GetType(typeAlias));
            return genericMethod.Invoke(json, new object[] { json, false });            
        }

        protected string ToPropertyAlias(string input)
        {
            return input.Substring(0, 1).ToLowerInvariant() + input.Substring(1);
        }

        private object GetExpectedValue<T>(T expected, PropertyInfo propInfo)
        {
            return propInfo.GetValue(expected, null);
        }

        private object GetActualValue<T>(T actual, PropertyInfo propInfo)
        {
            var actualProp = actual.SerialiazableProperties().SingleOrDefault(pinfo => pinfo.Name.Equals(propInfo.Name));
            return actualProp != null
                ? actualProp.GetValue(actual, null)
                : null;
        }
    }
}
