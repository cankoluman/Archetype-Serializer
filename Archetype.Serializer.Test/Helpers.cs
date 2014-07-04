using System;
using System.Collections.Generic;
using _7._1._4.ConsoleApp;

namespace Archetype.Serializer.Test
{
    public class Helpers
    {
        public Commands ConsoleCommands { get; private set; }

        public Helpers()
        {
            ConsoleCommands = new Commands();
        }

        public SimpleModel GetSimpleModel()
        {
            return new SimpleModel
            {
                DateField = Convert.ToDateTime("2014-07-10T00:00:00"),
                DateWithTimeField = Convert.ToDateTime("2014-07-16T14:18:50"),
                NodePicker = 1061,
                TextField = "Testing text field",
                TrueFalse = true
            };
        }

        public NestedModel GetNestedModel()
        {
            return new NestedModel
            {
                SimpleModel = new SimpleModel
                {
                    DateField = Convert.ToDateTime("2014-07-07T00:00:00"),
                    NodePicker = 1061,
                    TextField = "Simple Model Text String",
                    TrueFalse = false
                },
                TextField = "Nested Model Text String"
            };
        }

        public List<SimpleModel> GetSimpleModelList()
        {
            return new SimpleModelList
            {
                GetSimpleModel(),
                GetSimpleModel()
            };
        }
    }
}
