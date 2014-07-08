using Archetype.Models;
using Archetype.Serializer.Test.Base;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    public class AdvancedDeserializationTests : TestBase
    {
        private Helpers _testHelpers;
        private int _pageId = 1070;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _testHelpers = new Helpers();
            _testHelpers.ConsoleCommands.ClearDbLog();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _testHelpers.ConsoleCommands.ClearDbLog();
            _testHelpers.ConsoleCommands.Exit();
        }

        [TestCase("multiFieldsetModel")]
        [TestCase("multiFieldsetModelList")]
        [TestCase("nullableSimpleModelNull")]
        [TestCase("nullableSimpleModelValues")]
        [TestCase("simpleModelAsFieldsets")]
        [TestCase("simpleModelAsFieldsetsList")]
        public void PropertyIsNotNullOrEmpty(string propAlias)
        {
            Assert.IsNotNullOrEmpty(_testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias, _pageId));
        }

        [TestCase("multiFieldsetModel")]
        [TestCase("multiFieldsetModelList")]
        [TestCase("nullableSimpleModelNull")]
        [TestCase("nullableSimpleModelValues")]
        [TestCase("simpleModelAsFieldsets")]
        [TestCase("simpleModelAsFieldsetsList")]
        public void IsJsonValid(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias, _pageId);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject(json));
        }

        [TestCase("multiFieldsetModel")]
        [TestCase("multiFieldsetModelList")]
        [TestCase("nullableSimpleModelNull")]
        [TestCase("nullableSimpleModelValues")]
        [TestCase("simpleModelAsFieldsets")]
        [TestCase("simpleModelAsFieldsetsList")]
        public void IsJsonValidArchetype(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias, _pageId);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("multiFieldsetModel")]
        [TestCase("multiFieldsetModelList")]
        [TestCase("nullableSimpleModelNull")]
        [TestCase("nullableSimpleModelValues")]
        [TestCase("simpleModelAsFieldsets")]
        [TestCase("simpleModelAsFieldsetsList")]
        public void JsonDeserializesToArchetype(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias, _pageId);
            var model = JsonConvert.DeserializeObject<ArchetypeModel>(json);

            Assert.IsInstanceOf<ArchetypeModel>(model);
            Assert.IsNotNull(model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModel()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModel", _pageId);
            var model = json.GetModelFromArchetypeJson<MultiFieldsetModel>();

            Assert.IsInstanceOf<MultiFieldsetModel>(model);
            AssertAreEqual(_testHelpers.GetMultiFieldsetModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModelList()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModelList", _pageId);
            var model = json.GetModelFromArchetypeJson<MultiFieldsetModelList>();

            Assert.IsInstanceOf<MultiFieldsetModelList>(model);
            AssertAreEqual(_testHelpers.GetMultiFieldsetModelList(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithNulls()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelNull", _pageId);
            var model = json.GetModelFromArchetypeJson<NullableSimpleModel>();

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelNulled(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithValues()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelValues", _pageId);
            var model = json.GetModelFromArchetypeJson<NullableSimpleModel>();

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelPopulated(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_AsFieldsets()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsets", _pageId);
            var model = json.GetModelFromArchetypeJson<NullableSimpleModelAsFieldsets>();

            Assert.IsInstanceOf<NullableSimpleModelAsFieldsets>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelAsFieldsets(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_AsFieldsetsList()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsetsList", _pageId);
            var model = json.GetModelFromArchetypeJson<NullableSimpleModelAsFieldsetsList>();

            Assert.IsInstanceOf<NullableSimpleModelAsFieldsetsList>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelAsFieldsetsList(), model);
        }
    }
}
