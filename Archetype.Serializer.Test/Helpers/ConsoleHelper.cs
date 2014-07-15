using System;
using _7._1._4.ConsoleApp;

namespace Archetype.Serializer.Test.Helpers
{
    public sealed class ConsoleHelper
    {
        private readonly Commands _consoleCommands = new Commands();
        
        private static readonly Lazy<ConsoleHelper> _lazyLoader =
            new Lazy<ConsoleHelper>(() => new ConsoleHelper(), true);

        public static ConsoleHelper Instance { get { return _lazyLoader.Value; } }

        private ConsoleHelper()
        {
            
        }

        public Commands ConsoleCommands {get { return _consoleCommands; }}
    }
}
