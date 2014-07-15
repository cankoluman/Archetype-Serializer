using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [TestFixture]
    public class SerializerHelperTests
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(@"{""fieldsets"":[{""properties"":[{""alias"":""textField"",""value"":""Simple Model Text String""},{""alias"":""dateTime"",""value"":null},{""alias"":""date"",""value"":""2014-07-07T00:00:00""},{""alias"":""trueFalse"",""value"":0},{""alias"":""nodePicker"",""value"":null}],""alias"":""simpleModel""}]}", true)]
        [TestCase(@"{""fieldsets"":[{""properties"":[{""alias"":""textField"",""value"":""Simple Model Text String""},{""alias"":""dateTime"",""value"":null},

{""alias"":""date"",""value"":""2014-07-07T00:00:00""},{""alias"":""trueFalse"",""value"":0},{""alias"":""nodePicker"",""value"":null}],""alias"":""simpleModel""}]}", true)]
        [TestCase(@"{""fieldsets"":[{""pr", false)]
        public void IsArchetypeJson_ReturnsValidResult(string input, bool expected)
        {
            Assert.AreEqual(expected, Serializer.Helpers.IsArchetypeJson(input));
        }

    }
}
