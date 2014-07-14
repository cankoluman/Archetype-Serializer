using System;
using System.Collections.Generic;
using _7._1._4.ConsoleApp;

namespace Archetype.Serializer.Test
{
    public class Helpers
    {
        public Commands Console { get; private set; }

        public Helpers()
        {
            Console = new Commands();
        }

        #region basic models

        public object GetModel(string modelAlias)
        {
            try
            {
                return GetType().GetMethod("Get" + modelAlias).Invoke(this, null);
            }
            catch
            {
                return null;
            }
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

        public MultiFieldsetModelList GetMultiFieldsetModelList()
        {
            return new MultiFieldsetModelList
            {
                NestedModelList = new NestedModelList
                {
                    new NestedModel
                    {
                        SimpleModel = new SimpleModel
                        {
                            TextField   = "MF NM1 SM 1",
                            DateField = Convert.ToDateTime("2014-07-08T00:00:00")
                        },
                        TextField = "MF NM 1"
                    },
                    new NestedModel
                    {
                        SimpleModel = new SimpleModel
                        {
                            TextField   = "MF NM 2 SM 1",
                            DateField = Convert.ToDateTime("2014-07-08T00:00:00")
                        },
                        TextField = "MF NM 2"
                    }
                },
                SimpleModelList = new SimpleModelList
                {
                    new SimpleModel
                    {
                        TextField = "MF SM 1",
                        DateField = Convert.ToDateTime("2014-07-08T00:00:00")
                    },
                    new SimpleModel
                    {
                        TextField = "MF SM 2",
                        DateField = Convert.ToDateTime("2014-07-08T00:00:00")
                    }
                }
            };
        }

        public NullableSimpleModel GetNullableSimpleModelNulled()
        {
            return new NullableSimpleModel();
        }

        public NullableSimpleModel GetNullableSimpleModelPopulated()
        {
            return new NullableSimpleModel
            {
                DateField = Convert.ToDateTime("2014-07-08"),
                DateWithTimeField = Convert.ToDateTime("2014-07-15 10:48:44"),
                NodePicker = 1070,
                TextField = "Nullable With Text",
                TrueFalse = true
            };
        }

        public NullableSimpleModelAsFieldsets GetNullableSimpleModelAsFieldsets()
        {
            return new NullableSimpleModelAsFieldsets
            {
                DateWithTimeField = Convert.ToDateTime("2014-07-15T11:09:18"),
                TextField = "Sample Text",
                TrueFalse = true
            };
        }

        public NullableSimpleModelAsFieldsetsList GetNullableSimpleModelAsFieldsetsList()
        {
            return new NullableSimpleModelAsFieldsetsList
            {
                DateWithTimeField = new List<DateTime>
                {
                    Convert.ToDateTime("2014-07-15T11:09:18")
                },
                TextField = new List<string>
                {
                    "Test String 1",
                    "Test String 2",
                },
                TrueFalse = true
            };
        }

        #endregion
    }
}
