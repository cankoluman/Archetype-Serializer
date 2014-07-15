using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archetype.Serializer.Test.Helpers;
using NUnit.Framework;

namespace Archetype.Serializer.Test
{
    [SetUpFixture]
    public class GlobalSetup
    {
        [SetUp]
        public void SuiteSetUp()
        {
            ConsoleHelper.Instance.ConsoleCommands.Start();
        }

        [TearDown]
        public void SuiteTeardown()
        {
            ConsoleHelper.Instance.ConsoleCommands.ClearDbLog();
            ConsoleHelper.Instance.ConsoleCommands.Exit();
        }
    }
}
