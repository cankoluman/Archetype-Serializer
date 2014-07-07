using System;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace _7._1._4.ConsoleApp
{
    public class ConsoleApp : IDisposable
    {
        public ConsoleApplicationBase ConsoleApplicationBase { get; private set; }
        public IContentService ContentService {get { return _serviceContext.ContentService; }}
        public UmbracoDatabase Database { get { return _dbContext.Database; } }

        private ApplicationContext _appContext;
        private DatabaseContext _dbContext;
        private ServiceContext _serviceContext;

        public ConsoleApp(ConsoleApplicationBase consoleApplicationBase)
        {
            ConsoleApplicationBase = consoleApplicationBase;
            ConsoleApplicationBase.Start(ConsoleApplicationBase, new EventArgs());

            Init();
        }

        public void Init()
        {
            _appContext = ApplicationContext.Current;
            _dbContext = _appContext.DatabaseContext;
            _serviceContext = _appContext.Services;            
        }


        public void Dispose()
        {
            _appContext.DisposeIfDisposable();
        }
    }
}
