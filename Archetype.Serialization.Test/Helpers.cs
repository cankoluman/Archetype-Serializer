using System;
using System.IO;
using _7._1._4.ConsoleApp;

namespace Archetype.Serialization.Test
{
    public class Helpers
    {
        public Commands ConsoleCommands { get; private set; }

        public Helpers()
        {
            ConsoleCommands = new Commands();
        }

    }
}
