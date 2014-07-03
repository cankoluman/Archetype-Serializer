using System;

namespace Archetype.Serializer.Attributes
{
    public class ArchetypeModelAttribute : Attribute
    {
        public string FieldsetName { get; set; }

        public ArchetypeModelAttribute(string fieldsetname)
        {
            FieldsetName = fieldsetname;
        }
    }
}
