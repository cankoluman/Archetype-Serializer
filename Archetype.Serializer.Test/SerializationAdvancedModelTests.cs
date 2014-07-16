using System.Collections;
using System.Linq;
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
        private const int _referenceJsonPageId = 1070;

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

        [TestCase("NullableSimpleModelAsFieldsetsList")]
        public void SimpleModelAsFieldsetsList_Serializes_And_Deserializes(string modelAlias)
        {
            var model = _modelHelper.GetNullableSimpleModelAsFieldsetsList();

            Assert.IsNotNull(model);

            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actual = GetModelFromJson(modelAlias, json) as NullableSimpleModelAsFieldsetsList;

            Assert.IsNotNull(actual);

            var dateWithTime = (IList) actual.DateWithTimeField;
            var textfield = (IList)actual.TextField;

            for (var i = 0; i < dateWithTime.Count; i++)
            {
                Assert.AreEqual(((IList)model.DateWithTimeField)[i], dateWithTime[i]);
            }

            for (var i = 0; i < textfield.Count; i++)
            {
                Assert.AreEqual(((IList)model.TextField)[i], textfield[i]);
            }

            Assert.AreEqual(model.DateField, actual.DateField);
            Assert.AreEqual(model.NodePicker, actual.NodePicker);
            Assert.AreEqual(model.TrueFalse, actual.TrueFalse);
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

        [TestCase("NullableSimpleModelAsFieldsetsList", "simpleModelAsFieldsetsList")]
        public void SimpleModelAsFieldsetsList_SaveAndPublish_ReturnsCorrectModel(string modelAlias, string propertyAlias)
        {
            var model = _modelHelper.GetNullableSimpleModelAsFieldsetsList();

            Assert.IsNotNull(model);

            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());;

            var result = ConsoleHelper.Instance.ConsoleCommands.SaveAndPublishArchetypeJson(propertyAlias,
                json, _serializationTestsId);

            Assert.AreEqual(true, result);

            var resultJson = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propertyAlias, _serializationTestsId);

            var resultModel = GetModelFromJson(modelAlias, resultJson) as NullableSimpleModelAsFieldsetsList;

            Assert.IsNotNull(resultModel);

            var dateWithTime = (IList)resultModel.DateWithTimeField;
            var textfield = (IList)resultModel.TextField;

            for (var i = 0; i < dateWithTime.Count; i++)
            {
                Assert.AreEqual(((IList)model.DateWithTimeField)[i], dateWithTime[i]);
            }

            for (var i = 0; i < textfield.Count; i++)
            {
                Assert.AreEqual(((IList)model.TextField)[i], textfield[i]);
            }

            Assert.AreEqual(model.DateField, resultModel.DateField);
            Assert.AreEqual(model.NodePicker, resultModel.NodePicker);
            Assert.AreEqual(model.TrueFalse, resultModel.TrueFalse);
        }

        [TestCase("NullableSimpleModelAsFieldsetsList", "simpleModelAsFieldsetsList")]
        public void Model_Serializes_And_Preserves_Fieldsets(string modelAlias, string propAlias)
        {
            var referenceJson = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias, _referenceJsonPageId);
            var referenceArchetype = JsonConvert.DeserializeObject<ArchetypeModel>(referenceJson);

            var model = _modelHelper.GetModel(modelAlias);
            Assert.IsNotNull(model);

            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actualArchetype = JsonConvert.DeserializeObject<ArchetypeModel>(json);

            Assert.AreEqual(GetFieldsetCount(referenceArchetype, "dateTime"),
                GetFieldsetCount(actualArchetype, "dateTime"));

            Assert.AreEqual(GetFieldsetCount(referenceArchetype, "textField"),
                GetFieldsetCount(actualArchetype, "textField"));
        }

        #region private methods

        private int GetFieldsetCount(ArchetypeModel referenceArchetype, string fsAlias)
        {
            return referenceArchetype.Count(fs => fs.Alias.Equals(fsAlias));
        }

        #endregion


    }
}
