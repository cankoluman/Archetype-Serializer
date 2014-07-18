using System.Linq;
using Archetype.Models;
using Archetype.Serializer.Test.Base;
using Archetype.Serializer.Test.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    public class DeserializationBasicModelTests : TestBase
    {
        private ModelHelper _modelHelper;

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

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        [TestCase("simpleModelList")]
        [TestCase("nestedModelList")]
        public void PropertyIsNotNullOrEmpty(string propAlias)
        {
            Assert.IsNotNullOrEmpty(ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias));
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        [TestCase("simpleModelList")]
        [TestCase("nestedModelList")]
        public void IsJsonValid(string propAlias)
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject(json));
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        [TestCase("simpleModelList")]
        [TestCase("nestedModelList")]
        public void IsJsonValidArchetype(string propAlias)
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        [TestCase("simpleModelList")]
        [TestCase("nestedModelList")]
        public void JsonDeserializesToArchetype(string propAlias)
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            var model = JsonConvert.DeserializeObject<ArchetypeModel>(json);

            Assert.IsInstanceOf<ArchetypeModel>(model);
            Assert.IsNotNull(model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesSimpleModel_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("simpleModel");
            var model = json.ToModel<SimpleModel>();

            Assert.IsInstanceOf<SimpleModel>(model);
            AssertAreEqual(_modelHelper.GetSimpleModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesSimpleModel_FromArchetype()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("simpleModel");
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<SimpleModel>();

            Assert.IsInstanceOf<SimpleModel>(model);
            AssertAreEqual(_modelHelper.GetSimpleModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesSimpleModelList_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("simpleModelList");
            var model = json.ToModel<SimpleModelList>();

            var expected = _modelHelper.GetSimpleModelList();

            Assert.IsInstanceOf<SimpleModelList>(model);
            Assert.AreEqual(expected.Count, model.Count);

            foreach (var item in model)
            {
                AssertAreEqual(expected.ElementAt(model.IndexOf(item)) , item);
            }
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesSimpleModelList_FromArchetype()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("simpleModelList");
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<SimpleModelList>();

            var expected = _modelHelper.GetSimpleModelList();

            Assert.IsInstanceOf<SimpleModelList>(model);
            Assert.AreEqual(expected.Count, model.Count);

            foreach (var item in model)
            {
                AssertAreEqual(expected.ElementAt(model.IndexOf(item)), item);
            }
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNestedModel_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("nestedModel");
            var model = json.ToModel<NestedModel>();

            Assert.IsInstanceOf<NestedModel>(model);
            AssertAreEqual(_modelHelper.GetNestedModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNestedModel_FromArchetype()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("nestedModel");
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NestedModel>();

            Assert.IsInstanceOf<NestedModel>(model);
            AssertAreEqual(_modelHelper.GetNestedModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNestedModelList_FromJson()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("nestedModelList");
            var model = json.ToModel<NestedModelList>();

            var expected = _modelHelper.GetNestedModelList();

            Assert.IsInstanceOf<NestedModelList>(model);
            Assert.AreEqual(expected.Count, model.Count);

            foreach (var item in model)
            {
                AssertAreEqual(expected.ElementAt(model.IndexOf(item)), item);
            }
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNestedModelList_FromArchetype()
        {
            var json = ConsoleHelper.Instance.ConsoleCommands.GetArchetypeJsonFor("nestedModelList");
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NestedModelList>();

            var expected = _modelHelper.GetNestedModelList();

            Assert.IsInstanceOf<NestedModelList>(model);
            Assert.AreEqual(expected.Count, model.Count);

            foreach (var item in model)
            {
                AssertAreEqual(expected.ElementAt(model.IndexOf(item)), item);
            }
        }
    }
}
