using System;

namespace Archetype.Serialization.Attributes
{
    public class ArchetypeAttribute : Attribute
    {
        public string FieldsetName { get; set; }

        public ArchetypeAttribute(string fieldsetname)
        {
            FieldsetName = fieldsetname;
        }
    }
}
