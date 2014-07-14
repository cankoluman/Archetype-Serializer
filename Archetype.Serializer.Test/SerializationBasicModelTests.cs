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
        private int _serializationTestsId = 1074;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _testHelpers = new Helpers();
            //_testHelpers.ConsoleCommands.ClearDbLog();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            //_testHelpers.ConsoleCommands.ClearDbLog();
            _testHelpers.ConsoleCommands.Exit();
        }

        [TestCase("SimpleModel")]
        public void SimpleModel_Serializes_ToArchetypeJson(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, Formatting.Indented, new ArchetypeJsonConverter());

            Assert.IsTrue(Serializer.Helpers.IsArchetypeJson(json));
        }

        [TestCase("SimpleModel")]
        public void SimpleModel_Serializes_ToArchetype(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());

            Assert.IsInstanceOf<ArchetypeModel>(JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("SimpleModel")]
        public void SimpleModel_Serializes_And_Deserializes(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actual = GetModelFromJson(modelAlias, json);

            AssertAreEqual(model, actual);
        }

        
        [TestCase("SimpleModel")]
        public void SimpleModel_SaveAndPublish_ReturnsCorrectModel(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var propAlias = ToPropertyAlias(modelAlias);

            var result = _testHelpers.ConsoleCommands.SaveAndPublishArchetypeJson(propAlias, 
                json, _serializationTestsId);

            Assert.AreEqual(true, result);

            var resultModel = _testHelpers.ConsoleCommands.GetArchetypeFor(propAlias, _serializationTestsId);
            var resultJson = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias, _serializationTestsId);

            //Assert.AreEqual(json, resultJson);
            AssertAreEqual(model, resultModel);
        }
    }
}
