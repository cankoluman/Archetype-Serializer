using System.Linq;
using Archetype.Models;
using Umbraco.Core.Models;

namespace _7._1._4.ConsoleApp
{
    public class Commands
    {
        public ConsoleApp ConsoleApp { get; private set; }

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

        public ArchetypeModel GetArchetypeFor(string propertyAlias)
        {
            return GetArchetypeFor(propertyAlias, null);
        }

        public ArchetypeModel GetArchetypeFor(string propertyAlias, int? pageId)
        {
            var content = GetContent(pageId);

            if (content == null || !content.HasProperty(propertyAlias))
                return null;

            return content.GetValue<ArchetypeModel>(propertyAlias);
        }

        public void ClearDbLog()
        {
            ConsoleApp.Database.Execute("DELETE FROM umbracoLog;");
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
