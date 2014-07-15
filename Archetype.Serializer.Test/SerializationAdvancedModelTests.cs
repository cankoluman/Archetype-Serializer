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
        public void Model_Serializes_ToArchetypeJson(string modelAlias)
        {
            var model = _modelHelper.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, Formatting.Indented, new ArchetypeJsonConverter());

            Assert.IsTrue(Serializer.Helpers.IsArchetypeJson(json));
        }

        [TestCase("MultiFieldsetModel")]
        [TestCase("MultiFieldsetModelList")]
        public void Model_Serializes_And_Deserializes_ToArchetype(string modelAlias)
        {
            var model = _modelHelper.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());

            Assert.IsInstanceOf<ArchetypeModel>(JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("MultiFieldsetModel")]
        public void Model_Serializes_And_Deserializes(string modelAlias)
        {
            var model = _modelHelper.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actual = GetModelFromJson(modelAlias, json);

            AssertAreEqual(model, actual);
        }

        [TestCase("MultiFieldsetModelList")]
        public void ModelList_Serializes_And_Deserializes(string modelAlias)
        {
            var model = _modelHelper.GetModel(modelAlias) as IList;
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actual = GetModelFromJson(modelAlias, json) as IList;

            for (var i = 0; i < actual.Count; i++)
            {
                AssertAreEqual(model[i], actual[i]);
            }
        }

        [TestCase("MultiFieldsetModel")]
        public void Model_SaveAndPublish_ReturnsCorrectModel(string modelAlias)
        {
            var model = _modelHelper.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var propAlias = ToPropertyAlias(modelAlias);

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
            var model = _modelHelper.GetModel(modelAlias) as IList;
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var propAlias = ToPropertyAlias(modelAlias);

            var result = ConsoleHelper.Instance.ConsoleCommands.SaveAndPublishArchetypeJson(propAlias,
                json, _serializationTestsId);

            Assert.AreEqual(true, result);

            var resultJson = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias, _serializationTestsId);

            var resultModel = GetModelFromJson(modelAlias, resultJson) as IList;

            for (var i = 0; i < resultModel.Count; i++)
            {
                AssertAreEqual(model[i], resultModel[i]);
            }
        }
    }
}
