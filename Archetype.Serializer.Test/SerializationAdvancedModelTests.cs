using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archetype.Models;
using Archetype.Serializer.Test.Base;
using Archetype.Serializer.Test.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    public class SerializationAdvancedModelTests : TestBase
    {
        private ModelHelper _modelHelper;
        private const int _serializationTestsId = 1075;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _modelHelper = new ModelHelper();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _modelHelper = null;
        }

        [TestCase("MultiFieldsetModel")]
        [TestCase("MultiFieldsetModelList")]
        [TestCase("NullableSimpleModelNull")]
        [TestCase("NullableSimpleModelValues")]
        [TestCase("NullableSimpleModelAsFieldsets")]
        [TestCase("NullableSimpleModelAsFieldsetsList")]
        public void Model_Serializes_ToArchetypeJson(string modelAlias)
        {
            var model = _modelHelper.GetModel(modelAlias);

            Assert.IsNotNull(model);

            var json = JsonConvert.SerializeObject(model, Formatting.Indented, new ArchetypeJsonConverter());

            Assert.IsTrue(Serializer.Helpers.IsArchetypeJson(json));
        }

        [TestCase("MultiFieldsetModel")]
        [TestCase("MultiFieldsetModelList")]
        [TestCase("NullableSimpleModelNull")]
        [TestCase("NullableSimpleModelValues")]
        [TestCase("NullableSimpleModelAsFieldsets")]
        [TestCase("NullableSimpleModelAsFieldsetsList")]
        public void Model_Serializes_And_Deserializes_ToArchetype(string modelAlias)
        {
            var model = _modelHelper.GetModel(modelAlias);

            Assert.IsNotNull(model);

            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());

            Assert.IsInstanceOf<ArchetypeModel>(JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("MultiFieldsetModel", "MultiFieldsetModel")]
        [TestCase("NullableSimpleModelNull", "NullableSimpleModel")]
        [TestCase("NullableSimpleModelValues", "NullableSimpleModel")]
        [TestCase("NullableSimpleModelAsFieldsets", "NullableSimpleModelAsFieldsets")]
        public void Model_Serializes_And_Deserializes(string modelHelperAlias, string modelAlias)
        {
            var model = _modelHelper.GetModel(modelHelperAlias);

            Assert.IsNotNull(model);

            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actual = GetModelFromJson(modelAlias, json);

            AssertAreEqual(model, actual);
        }

        [TestCase("MultiFieldsetModelList")]
        public void ModelList_Serializes_And_Deserializes(string modelAlias)
        {
            var model = _modelHelper.GetMultiFieldsetModelList();

            Assert.IsNotNull(model);

            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actual = GetModelFromJson(modelAlias, json) as MultiFieldsetModelList;

            Assert.IsNotNull(actual);

            for (var i = 0; i < actual.SimpleModelList.Count; i++)
            {
                AssertAreEqual(model.SimpleModelList[i], actual.SimpleModelList[i]);
            }

            for (var i = 0; i < actual.NestedModelList.Count; i++)
            {
                AssertAreEqual(model.NestedModelList[i], actual.NestedModelList[i]);
            }
        }

        [TestCase("MultiFieldsetModel", "MultiFieldsetModel", null)]
        [TestCase("NullableSimpleModelNull", "NullableSimpleModel", null)]
        [TestCase("NullableSimpleModelValues", "NullableSimpleModel", null)]
        [TestCase("NullableSimpleModelAsFieldsets", "NullableSimpleModelAsFieldsets", "simpleModelAsFieldsets")]
        public void Model_SaveAndPublish_ReturnsCorrectModel(string modelHelperAlias, string modelAlias, string propertyAlias = null)
        {
            var model = _modelHelper.GetModel(modelHelperAlias);

            Assert.IsNotNull(model);

            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var propAlias = propertyAlias ?? ToPropertyAlias(modelHelperAlias);

            var result = ConsoleHelper.Instance.ConsoleCommands.SaveAndPublishArchetypeJson(propAlias,
                json, _serializationTestsId);

            Assert.AreEqual(true, result);

            var resultJson = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias, _serializationTestsId);

            var resultModel = GetModelFromJson(modelAlias, resultJson);
            AssertAreEqual(model, resultModel);
        }

        [TestCase("MultiFieldsetModelList")]
        public void ModelList_SaveAndPublish_ReturnsCorrectModel(string modelAlias)
        {
            var model = _modelHelper.GetMultiFieldsetModelList();

            Assert.IsNotNull(model);

            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var propAlias = ToPropertyAlias(modelAlias);

            var result = ConsoleHelper.Instance.ConsoleCommands.SaveAndPublishArchetypeJson(propAlias,
                json, _serializationTestsId);

            Assert.AreEqual(true, result);

            var resultJson = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias, _serializationTestsId);

            var resultModel = GetModelFromJson(modelAlias, resultJson) as MultiFieldsetModelList;

            Assert.IsNotNull(resultModel);

            for (var i = 0; i < resultModel.SimpleModelList.Count; i++)
            {
                AssertAreEqual(model.SimpleModelList[i], resultModel.SimpleModelList[i]);
            }

            for (var i = 0; i < resultModel.NestedModelList.Count; i++)
            {
                AssertAreEqual(model.NestedModelList[i], resultModel.NestedModelList[i]);
            }
        }
    }
}
