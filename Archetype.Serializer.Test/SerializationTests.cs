﻿using System.Collections.Generic;
using System.Linq;
using Archetype.Models;
using Archetype.Serializer.Test.Base;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    public class SerializationTests : TestBase
    {
        private Helpers _testHelpers;

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

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        public void PropertyIsNotNullOrEmpty(string propAlias)
        {
            Assert.IsNotNullOrEmpty(_testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias));
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        public void IsJsonValid(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject(json));
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        public void IsJsonValidArchetype(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("simpleModel")]
        [TestCase("nestedModel")]
        public void JsonDeserializesToArchetype(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            var model = JsonConvert.DeserializeObject<ArchetypeModel>(json);

            Assert.IsInstanceOf<ArchetypeModel>(model);
            Assert.IsNotNull(model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesSimpleModel()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("simpleModel");
            var model = json.GetModelFromArchetypeJson<SimpleModel>();

            Assert.IsInstanceOf<SimpleModel>(model);
            AssertAreEqual(_testHelpers.GetSimpleModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesSimpleModelList()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("simpleModelList");
            var model = json.GetModelFromArchetypeJson<SimpleModelList>();

            var expected = _testHelpers.GetSimpleModelList();

            Assert.IsInstanceOf<SimpleModelList>(model);
            Assert.AreEqual(expected.Count, model.Count);

            foreach (var item in model)
            {
                AssertAreEqual(expected.ElementAt(model.IndexOf(item)) , item);
            }
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNestedModel()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("nestedModel");
            var model = json.GetModelFromArchetypeJson<NestedModel>();

            Assert.IsInstanceOf<NestedModel>(model);
            AssertAreEqual(_testHelpers.GetNestedModel(), model);
        }

        [Test]
        public void ArchetypeJsonConverter_DeserializesNestedModelList()
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor("nestedModelList");
            var model = json.GetModelFromArchetypeJson<NestedModelList>();

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
