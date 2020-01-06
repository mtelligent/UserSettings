using EPExpressTab.Data;
using EPExpressTab.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.UserSettings.EETab
{
    public class UserSettingsTab : EpExpressViewModel
    {
        public override string Heading => "User Settings by Area";
        public override string TabLabel => "User Settings";

        public override object GetModel(Guid contactId)
        {
            Sitecore.XConnect.Contact contact = EPRepository.GetContact(contactId, Facets.FacetNames.UserSettings);
            var settings = contact.GetFacet<SF.Foundation.Facets.UserSettings>().Settings;
            return new UserSettingsTabModel
            {
                UserSettings = settings
            };
        }

        public override string GetFullViewPath(object model)
        {
            return "/views/UserSettings/UserSettingsTab.cshtml";
        }
    }
}