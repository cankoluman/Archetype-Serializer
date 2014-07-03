using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archetype.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using NUnit.Framework;

namespace Archetype.Serialization.Test
{
    [TestFixture]
    public class SerializationTests
    {
        private Helpers _testHelpers;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _testHelpers = new Helpers();   
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _testHelpers.ConsoleCommands.Exit();
        }

        [TestCase("simpleModel")]
        public void PropertyIsNotNullOrEmpty(string propAlias)
        {
            Assert.IsNotNullOrEmpty(_testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias));
        }

        [TestCase("simpleModel")]
        public void IsJsonValid(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject(json));
        }

        [TestCase("simpleModel")]
        public void IsJsonValidArchetype(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("simpleModel")]
        public void JsonDeserializesToArchetype(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            var model = JsonConvert.DeserializeObject<ArchetypeModel>(json);

            Assert.IsInstanceOf<ArchetypeModel>(model);
            Assert.IsNotNull(model);
        }
    }
}
