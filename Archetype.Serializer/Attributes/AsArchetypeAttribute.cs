using System;

namespace Archetype.Serializer.Attributes
{
    public class AsArchetypeAttribute : Attribute
    {
        public string FieldsetName { get; set; }

        public AsArchetypeAttribute(string fieldsetname)
        {
            FieldsetName = fieldsetname;
        }
    }
}
