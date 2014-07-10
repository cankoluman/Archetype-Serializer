using System.Linq;
using Archetype.Models;
using Archetype.Serializer.Test.Base;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    public class DeserializationAdvancedModelTests : TestBase
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
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModel_FromJson()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModel", _pageId);
            var model = json.ToModel<MultiFieldsetModel>();

            Assert.IsInstanceOf<MultiFieldsetModel>(model);
            AssertAreEqual(_testHelpers.GetMultiFieldsetModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModel_FromArchetype()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModel", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<MultiFieldsetModel>();

            Assert.IsInstanceOf<MultiFieldsetModel>(model);
            AssertAreEqual(_testHelpers.GetMultiFieldsetModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModelList_FromJson()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModelList", _pageId);
            var model = json.ToModel<MultiFieldsetModelList>();

            Assert.IsInstanceOf<MultiFieldsetModelList>(model);
            AssertAreEqual(_testHelpers.GetMultiFieldsetModelList(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModelList_FromArchetype()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModelList", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<MultiFieldsetModelList>();            

            Assert.IsInstanceOf<MultiFieldsetModelList>(model);
            AssertAreEqual(_testHelpers.GetMultiFieldsetModelList(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithNulls_FromJson()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelNull", _pageId);
            var model = json.ToModel<NullableSimpleModel>();

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelNulled(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithNulls_FromArchetype()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelNull", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NullableSimpleModel>();                 

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelNulled(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithValues_FromJson()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelValues", _pageId);
            var model = json.ToModel<NullableSimpleModel>();

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelPopulated(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithValues_FromArchetype()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelValues", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NullableSimpleModel>();               

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelPopulated(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_AsFieldsets_FromJson()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsets", _pageId);
            var model = json.ToModel<NullableSimpleModelAsFieldsets>();

            Assert.IsInstanceOf<NullableSimpleModelAsFieldsets>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelAsFieldsets(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_AsFieldsets_FromArchetype()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsets", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NullableSimpleModelAsFieldsets>();               

            Assert.IsInstanceOf<NullableSimpleModelAsFieldsets>(model);
            AssertAreEqual(_testHelpers.GetNullableSimpleModelAsFieldsets(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_AsFieldsetsList_FromJson()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsetsList", _pageId);
            var model = json.ToModel<NullableSimpleModelAsFieldsetsList>();
            var expected = _testHelpers.GetNullableSimpleModelAsFieldsetsList();

            Assert.IsInstanceOf<NullableSimpleModelAsFieldsetsList>(model);

            Assert.AreEqual(expected.DateField, model.DateField);
            Assert.AreEqual(expected.NodePicker, model.NodePicker);
            Assert.AreEqual(expected.TrueFalse, model.TrueFalse);

            Assert.AreEqual(expected.DateWithTimeField.ElementAt(0), 
                model.DateWithTimeField.ElementAt(0));

            Assert.AreEqual(expected.TextField.ElementAt(0),
                model.TextField.ElementAt(0));
            Assert.AreEqual(expected.TextField.ElementAt(1),
                model.TextField.ElementAt(1));
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_AsFieldsetsList_FromArchetype()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsetsList", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NullableSimpleModelAsFieldsetsList>();             
            var expected = _testHelpers.GetNullableSimpleModelAsFieldsetsList();

            Assert.IsInstanceOf<NullableSimpleModelAsFieldsetsList>(model);

            Assert.AreEqual(expected.DateField, model.DateField);
            Assert.AreEqual(expected.NodePicker, model.NodePicker);
            Assert.AreEqual(expected.TrueFalse, model.TrueFalse);

            Assert.AreEqual(expected.DateWithTimeField.ElementAt(0),
                model.DateWithTimeField.ElementAt(0));

            Assert.AreEqual(expected.TextField.ElementAt(0),
                model.TextField.ElementAt(0));
            Assert.AreEqual(expected.TextField.ElementAt(1),
                model.TextField.ElementAt(1));
        }
    }
}
