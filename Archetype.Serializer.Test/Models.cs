using System;
using System.Collections.Generic;
using Archetype.Serializer.Attributes;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [ArchetypeModel("simpleModel")]
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

    [ArchetypeModel("simpleModel")]
    public class SimpleModelList : List<SimpleModel>
    {
    }

    [ArchetypeModel("nestedModel")]
    public class NestedModel
    {
        [JsonProperty("textString")]
        public string TextField { get; set; }
        [JsonProperty("simpleModel")]
        public SimpleModel SimpleModel { get; set; }
    }

    [ArchetypeModel("nestedModel")]
    public class NestedModelList : List<NestedModel>
    {
    }
}
