using System;
using Newtonsoft.Json;

namespace Archetype.Serializer.Test
{
    [Serializer.Attributes.AsArchetype("simpleModel")]
    public class SimpleModel
    {
        [JsonProperty("date")]
        public DateTime DateField { get; set; }
        [JsonProperty("dateTime")]
        public DateTime DateWithTimeField { get; set; }
        [JsonProperty("textField")]
        public string TextField { get; set; }
        [JsonProperty("trueFalse")]
        public bool TrueFalse { get; set; }
        [JsonProperty("nodePicker")]
        public int NodePicker { get; set; }
    }

    [Serializer.Attributes.AsArchetype("nestedModel")]
    public class NestedModel
    {
        [JsonProperty("textString")]
        public string TextField { get; set; }
        [JsonProperty("simpleModel")]
        public SimpleModel SimpleModel { get; set; }
    }
}
