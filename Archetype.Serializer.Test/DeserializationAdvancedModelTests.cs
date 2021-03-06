﻿using System.Linq;
using Archetype.Models;
using Archetype.Serializer.Test.Base;
using Archetype.Serializer.Test.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    public class DeserializationAdvancedModelTests : TestBase
    {
        private ModelHelper _modelHelper;
        private int _pageId = 1070;

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

        [TestCase("multiFieldsetModel")]
        [TestCase("multiFieldsetModelList")]
        [TestCase("nullableSimpleModelNull")]
        [TestCase("nullableSimpleModelValues")]
        [TestCase("simpleModelAsFieldsets")]
        [TestCase("simpleModelAsFieldsetsList")]
        public void PropertyIsNotNullOrEmpty(string propAlias)
        {
            Assert.IsNotNullOrEmpty(ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias, _pageId));
        }

        [TestCase("multiFieldsetModel")]
        [TestCase("multiFieldsetModelList")]
        [TestCase("nullableSimpleModelNull")]
        [TestCase("nullableSimpleModelValues")]
        [TestCase("simpleModelAsFieldsets")]
        [TestCase("simpleModelAsFieldsetsList")]
        public void IsJsonValid(string propAlias)
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias, _pageId);
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
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias, _pageId);
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
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias, _pageId);
            var model = JsonConvert.DeserializeObject<ArchetypeModel>(json);

            Assert.IsInstanceOf<ArchetypeModel>(model);
            Assert.IsNotNull(model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModel_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModel", _pageId);
            var model = json.ToModel<MultiFieldsetModel>();

            Assert.IsInstanceOf<MultiFieldsetModel>(model);
            AssertAreEqual(_modelHelper.GetMultiFieldsetModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModel_FromArchetype()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModel", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<MultiFieldsetModel>();

            Assert.IsInstanceOf<MultiFieldsetModel>(model);
            AssertAreEqual(_modelHelper.GetMultiFieldsetModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModelList_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModelList", _pageId);
            var model = json.ToModel<MultiFieldsetModelList>();

            Assert.IsInstanceOf<MultiFieldsetModelList>(model);
            AssertAreEqual(_modelHelper.GetMultiFieldsetModelList(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesMultiFieldsetModelList_FromArchetype()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("multiFieldsetModelList", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<MultiFieldsetModelList>();            

            Assert.IsInstanceOf<MultiFieldsetModelList>(model);
            AssertAreEqual(_modelHelper.GetMultiFieldsetModelList(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithNulls_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelNull", _pageId);
            var model = json.ToModel<NullableSimpleModel>();

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_modelHelper.GetNullableSimpleModelNull(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithNulls_FromArchetype()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelNull", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NullableSimpleModel>();                 

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_modelHelper.GetNullableSimpleModelNull(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithValues_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelValues", _pageId);
            var model = json.ToModel<NullableSimpleModel>();

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_modelHelper.GetNullableSimpleModelValues(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_WithValues_FromArchetype()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("nullableSimpleModelValues", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NullableSimpleModel>();               

            Assert.IsInstanceOf<NullableSimpleModel>(model);
            AssertAreEqual(_modelHelper.GetNullableSimpleModelValues(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_AsFieldsets_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsets", _pageId);
            var model = json.ToModel<NullableSimpleModelAsFieldsets>();

            Assert.IsInstanceOf<NullableSimpleModelAsFieldsets>(model);
            AssertAreEqual(_modelHelper.GetNullableSimpleModelAsFieldsets(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_AsFieldsets_FromArchetype()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsets", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NullableSimpleModelAsFieldsets>();               

            Assert.IsInstanceOf<NullableSimpleModelAsFieldsets>(model);
            AssertAreEqual(_modelHelper.GetNullableSimpleModelAsFieldsets(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNullableSimpleModel_AsFieldsetsList_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsetsList", _pageId);
            var model = json.ToModel<NullableSimpleModelAsFieldsetsList>();
            var expected = _modelHelper.GetNullableSimpleModelAsFieldsetsList();

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
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("simpleModelAsFieldsetsList", _pageId);
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NullableSimpleModelAsFieldsetsList>();             
            var expected = _modelHelper.GetNullableSimpleModelAsFieldsetsList();

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
