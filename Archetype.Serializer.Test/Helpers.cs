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

        #region basic models

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

        public List<SimpleModel> GetSimpleModelList()
        {
            return new SimpleModelList
            {
                GetSimpleModel(),
                GetSimpleModel()
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

        public NestedModelList GetNestedModelList()
        {
            return new NestedModelList
            {
                new NestedModel
                {
                    SimpleModel = new SimpleModel
                    {
                        DateField = Convert.ToDateTime("2014-07-07T00:00:00"),
                        NodePicker = 1061,
                        TextField = "Simple Model Text String 1",
                        TrueFalse = false
                    },
                    TextField = "Nested Model Text String 1"
                },
                new NestedModel
                {
                    SimpleModel = new SimpleModel
                    {
                        DateField = Convert.ToDateTime("2014-07-07T00:00:00"),
                        NodePicker = 1061,
                        TextField = "Simple Model Text String 2",
                        TrueFalse = false
                    },
                    TextField = "Nested Model Text String 2"
                },
                new NestedModel
                {
                    SimpleModel = new SimpleModel
                    {
                        DateField = Convert.ToDateTime("2014-07-07T00:00:00"),
                        DateWithTimeField = Convert.ToDateTime("2014-07-16T14:18:50"),
                        NodePicker = 1061,
                        TextField = "Simple Model Text String 3",
                        TrueFalse = true
                    },
                    TextField = "Nested Model Text String 3"
                }
            };
        }

        #endregion

        #region advanced models

        public MultiFieldsetModel GetMultiFieldsetModel()
        {
            return new MultiFieldsetModel
            {
                NestedModel = new NestedModel
                {
                    SimpleModel = new SimpleModel
                    {
                        DateField = Convert.ToDateTime("2014-07-08T00:00:00"),
                        NodePicker = 1070,
                        TextField = "MF NM Simple Model Text"
                    },
                    TextField = "MF Nested Model Text"
                },
                SimpleModel = new SimpleModel
                {   
                    DateField = Convert.ToDateTime("2014-07-07T00:00:00"),
                    NodePicker = 1070,
                    TextField = "MF Simple Model Text"
                }
            };
        }

        #endregion
    }
}
