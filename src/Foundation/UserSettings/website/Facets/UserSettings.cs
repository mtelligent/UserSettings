using Sitecore.XConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets
{
    [Serializable]
    [FacetKey(FacetNames.UserSettings)]
    public class UserSettings : Sitecore.XConnect.Facet
    {
        public UserSettings()
        {
            
        }

        public Dictionary<string, Dictionary<string, string>> Settings
        {
            get; set;
        } = new Dictionary<string, Dictionary<string, string>>();
    }
}