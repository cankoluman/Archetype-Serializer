using System.Linq;
using Archetype.Models;
using Archetype.Serializer.Test.Base;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    [Ignore]
    public class DeserializationBasicModelTests : TestBase
    {
        private Helpers _testHelpers;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _testHelpers = new Helpers();
            _testHelpers.Console.Start();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {            
            _testHelpers.Console.ClearDbLog();
            _testHelpers.Console.Exit();
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        [TestCase("simpleModelList")]
        [TestCase("nestedModelList")]
        public void PropertyIsNotNullOrEmpty(string propAlias)
        {
            Assert.IsNotNullOrEmpty(_testHelpers.Console.GetArchetypeJsonFor(propAlias));
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        [TestCase("simpleModelList")]
        [TestCase("nestedModelList")]
        public void IsJsonValid(string propAlias)
        {
            var json = _testHelpers.Console.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject(json));
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        [TestCase("simpleModelList")]
        [TestCase("nestedModelList")]
        public void IsJsonValidArchetype(string propAlias)
        {
            var json = _testHelpers.Console.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        [TestCase("simpleModelList")]
        [TestCase("nestedModelList")]
        public void JsonDeserializesToArchetype(string propAlias)
        {
            var json = _testHelpers.Console.GetArchetypeJsonFor(propAlias);
            var model = JsonConvert.DeserializeObject<ArchetypeModel>(json);

            Assert.IsInstanceOf<ArchetypeModel>(model);
            Assert.IsNotNull(model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesSimpleModel_FromJson()
        {
            var json = _testHelpers.Console.GetArchetypeJsonFor("simpleModel");
            var model = json.ToModel<SimpleModel>();

            Assert.IsInstanceOf<SimpleModel>(model);
            AssertAreEqual(_testHelpers.GetSimpleModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesSimpleModel_FromArchetype()
        {
            var json = _testHelpers.Console.GetArchetypeJsonFor("simpleModel");
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<SimpleModel>();

            Assert.IsInstanceOf<SimpleModel>(model);
            AssertAreEqual(_testHelpers.GetSimpleModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesSimpleModelList_FromJson()
        {
            var json = _testHelpers.Console.GetArchetypeJsonFor("simpleModelList");
            var model = json.ToModel<SimpleModelList>();

            var expected = _testHelpers.GetSimpleModelList();

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
            var json = _testHelpers.Console.GetArchetypeJsonFor("simpleModelList");
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<SimpleModelList>();

            var expected = _testHelpers.GetSimpleModelList();

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
            var json = _testHelpers.Console.GetArchetypeJsonFor("nestedModel");
            var model = json.ToModel<NestedModel>();

            Assert.IsInstanceOf<NestedModel>(model);
            AssertAreEqual(_testHelpers.GetNestedModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNestedModel_FromArchetype()
        {
            var json = _testHelpers.Console.GetArchetypeJsonFor("nestedModel");
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NestedModel>();

            Assert.IsInstanceOf<NestedModel>(model);
            AssertAreEqual(_testHelpers.GetNestedModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNestedModelList_FromJson()
        {
            var json = _testHelpers.Console.GetArchetypeJsonFor("nestedModelList");
            var model = json.ToModel<NestedModelList>();

            var expected = _testHelpers.GetNestedModelList();

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
            var json = _testHelpers.Console.GetArchetypeJsonFor("nestedModelList");
            var archetype = json.JsonToModel<ArchetypeModel>();
            var model = archetype.ToModel<NestedModelList>();

            var expected = _testHelpers.GetNestedModelList();

            Assert.IsInstanceOf<NestedModelList>(model);
            Assert.AreEqual(expected.Count, model.Count);

            foreach (var item in model)
            {
                AssertAreEqual(expected.ElementAt(model.IndexOf(item)), item);
            }
        }
    }
}
