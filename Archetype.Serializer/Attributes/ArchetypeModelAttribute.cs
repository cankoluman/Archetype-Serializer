using System;

namespace Archetype.Serializer.Attributes
{
    public class ArchetypeModelAttribute : Attribute
    {
        public string FieldsetName { get; private set; }
        public bool MultipleFieldsets { get; private set; }

        public ArchetypeModelAttribute(string fieldsetname, bool multipleFieldsets = false)
        {
            FieldsetName = fieldsetname;
            MultipleFieldsets = multipleFieldsets;
        }
    }
}
