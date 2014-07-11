using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Archetype.Models;
using Archetype.Serializer.Test.Base;
using Newtonsoft.Json;
using NUnit.Framework;
using Archetype.Serializer;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    public class SerializationBasicModelTests : TestBase
    {
        private Helpers _testHelpers;

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
        public void SimpleModel_Serializes_ToArchetype(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var converter = new ArchetypeJsonConverter();

            Assert.IsInstanceOf<ArchetypeModel>(converter.GenerateArchetype(model));
        }

        [TestCase("SimpleModel")]
        public void SimpleModel_Serializes_ToArchetypeJson(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());

            Assert.IsTrue(Serializer.Helpers.IsArchetypeJson(json));
        }

        [TestCase("SimpleModel")]
        public void SimpleModel_Serializes_And_Deserializes(string modelAlias)
        {
            var model = _testHelpers.GetModel(modelAlias);
            var json = JsonConvert.SerializeObject(model, new ArchetypeJsonConverter());
            var actual = GetModelFromJson(modelAlias, json);

            AssertAreEqual(model, actual);
        }
    }
}
