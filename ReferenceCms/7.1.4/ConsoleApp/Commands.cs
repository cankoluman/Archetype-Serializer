using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace _7._1._4.ConsoleApp
{
    public class Commands
    {
        public ConsoleApp ConsoleApp { get; private set; }
        public UmbracoHelper UmbracoHelper { get; private set; }

        public Commands()
        {
            ConsoleApp = new ConsoleApp(new ConsoleApplicationBase()); 
        }

        public string GetArchetypeJsonFor(string propertyAlias)
        {
            return GetArchetypeJsonFor(propertyAlias, null);
        }

        public string GetArchetypeJsonFor(string propertyAlias, int? pageId)
        {
            var content = GetContent(pageId);

            if (content == null || !content.HasProperty(propertyAlias))
                return null;

            return content.GetValue<string>(propertyAlias);
        }

        public bool SaveAndPublishArchetypeJson(string propertyAlias, string json, int pageId)
        {
            var content = ConsoleApp.ContentService.GetPublishedVersion(pageId);
            content.SetValue(propertyAlias, json);
            var status =  ConsoleApp.ContentService.SaveAndPublishWithStatus(content, 0, false);

            return status.Success;
        }

        public void ClearDbLog()
        {
            ConsoleApp.Database.Execute("DELETE FROM umbracoLog;");
        }

        public void Start()
        {           
            ConsoleApp.Start();
        }

        public void Exit()
        {
            ConsoleApp.Dispose();
        }

        private IContent GetContent(int? pageId)
        {
            return pageId.HasValue ?
                ConsoleApp.ContentService.GetRootContent()
                    .FirstOrDefault(c => c.Published && c.Id == pageId.Value) :
                ConsoleApp.ContentService.GetRootContent()                    
                    .Where(c => c.Published)
                    .OrderBy(c => c.SortOrder)
                    .FirstOrDefault();
        }
    }
}
