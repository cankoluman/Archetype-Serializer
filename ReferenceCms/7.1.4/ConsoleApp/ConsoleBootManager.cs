using System;
using Archetype.Models;
using Lucene.Net.Documents;
using Umbraco.Core;
using Umbraco.Core.Services;
using umbraco.editorControls;
using umbraco.interfaces;

namespace _7._1._4.ConsoleApp
{
    /// <summary>
    /// Extends the CoreBootManager for use in this Console app.
    /// </summary>
    public class ConsoleBootManager : CoreBootManager
    {
        public ConsoleBootManager(UmbracoApplicationBase umbracoApplication, string baseDirectory)
            : base(umbracoApplication)
        {
            //This is only here to ensure references to the assemblies needed for the DataTypesResolver
            //otherwise they won't be loaded into the AppDomain.
            var interfacesAssemblyName = typeof(IDataType).Assembly.FullName;
            var editorControlsAssemblyName = typeof(uploadField).Assembly.FullName;
            var archetypeAssemblyName = typeof (ArchetypeModel).Assembly.FullName;

            base.InitializeApplicationRootPath(baseDirectory);
        }

        protected override void CreateApplicationContext(DatabaseContext dbContext, ServiceContext serviceContext)
        {
            base.CreateApplicationContext(dbContext, serviceContext);
        }

        /// <summary>
        /// Can be used to initialize our own Application Events
        /// </summary>
        protected override void InitializeApplicationEventsResolver()
        {
            base.InitializeApplicationEventsResolver();
        }

        /// <summary>
        /// Can be used to add custom resolvers or overwrite existing resolvers once they are made public
        /// </summary>
        protected override void InitializeResolvers()
        {
            base.InitializeResolvers();
        }
    }
}