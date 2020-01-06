using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System.Web;

namespace SF.Foundation.Facets.Models
{
    public class CollectionModel
    {
        public static XdbModel Model { get; } = BuildModel();

        private static XdbModel BuildModel()
        {
            XdbModelBuilder modelBuilder = new XdbModelBuilder("DocumentationModel", new XdbModelVersion(1, 0));

            // Facets and events here
            modelBuilder.DefineFacet<Contact, UserSettings>(FacetNames.UserSettings);

            return modelBuilder.BuildModel();
        }
    }
}