using System;

namespace Archetype.Serializer.Attributes
{
    public class ArchetypeModelAttribute : Attribute
    {
        public string FieldsetName { get; private set; }
        public bool ArePropertiesFieldsets { get; private set; }

        public ArchetypeModelAttribute(string fieldsetname, bool arePropertiesFieldsets = false)
        {
            FieldsetName = fieldsetname;
            ArePropertiesFieldsets = arePropertiesFieldsets;
        }
    }
}
