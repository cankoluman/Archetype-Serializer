using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private int _pageId = 1068;

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
        public void PropertyIsNotNullOrEmpty(string propAlias)
        {
            Assert.IsNotNullOrEmpty(_testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias));
        }

        [TestCase("multiFieldsetModel")]    
        public void IsJsonValid(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject(json));
        }

        [TestCase("multiFieldsetModel")]
        public void IsJsonValidArchetype(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<ArchetypeModel>(json));
        }

        [TestCase("multiFieldsetModel")]
        public void JsonDeserializesToArchetype(string propAlias)
        {
            var json = _testHelpers.ConsoleCommands.GetArchetypeJsonFor(propAlias);
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
    }
}
