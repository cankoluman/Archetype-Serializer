using System.Collections;
using Archetype.Models;
using Archetype.Serializer.Test.Base;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    public class SerializationBasicModelTests : TestBase
    {
        private Helpers _testHelpers;
        private const int _serializationTestsId = 1074;

        [SetUp]
        public void FixtureSetUp()
        {
            _testHelpers = new Helpers();
            _testHelpers.Console.Start();
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _testHelpers.Console.ClearDbLog();
            _testHelpers.Console.Exit();
        }

        [TestCase("SimpleModel")]
        [TestCase("SimpleModelList")]
        public void Model_Serializes_ToArchetypeJson(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, Formatting.Indented, new ArchetypeJsonConverter());

            Assert.IsTrue(Serializer.Helpers.IsArchetypeJson(json));
        }

        [TestCase("SimpleModel")]
        [TestCase("SimpleModelList")]
        public void Model_Serializes_ToArchetype(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());

            Assert.IsInstanceOf<ArchetypeModel>(JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("SimpleModel")]
        public void Model_Serializes_And_Deserializes(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actual = GetModelFromJson(modelAlias, json);

            AssertAreEqual(model, actual);
        }

        [TestCase("SimpleModelList")]
        public void SimpleModelList_Serializes_And_Deserializes(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias) as IList;
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actual = GetModelFromJson(modelAlias, json) as IList;

            for (var i = 0; i < actual.Count; i++)
            {
                AssertAreEqual(model[i], actual[i]);
            }
        }

        
        [TestCase("SimpleModel")]
        public void Model_SaveAndPublish_ReturnsCorrectModel(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var propAlias = ToPropertyAlias(modelAlias);

            var result = _testHelpers.Console.SaveAndPublishArchetypeJson(propAlias, 
                json, _serializationTestsId);

            Assert.AreEqual(true, result);

            var resultJson = _testHelpers.Console.GetArchetypeJsonFor(propAlias, _serializationTestsId);

            var resultModel = GetModelFromJson(modelAlias, resultJson);
            AssertAreEqual(model, resultModel);
        }

        [TestCase("SimpleModelList")]
        public void ListModel_SaveAndPublish_ReturnsCorrectModel(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias) as IList;
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var propAlias = ToPropertyAlias(modelAlias);

            var result = _testHelpers.Console.SaveAndPublishArchetypeJson(propAlias,
                json, _serializationTestsId);

            Assert.AreEqual(true, result);

            var resultJson = _testHelpers.Console.GetArchetypeJsonFor(propAlias, _serializationTestsId);

            var resultModel = GetModelFromJson(modelAlias, resultJson) as IList;

            for (var i = 0; i < resultModel.Count; i++)
            {
                AssertAreEqual(model[i], resultModel[i]);
            }
        }
    }
}
