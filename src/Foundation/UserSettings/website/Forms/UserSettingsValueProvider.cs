using Sitecore;
using Sitecore.ExperienceForms.ValueProviders;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets.Forms
{
    public class UserSettingsValueProvider : IFieldValueProvider
    {
        public FieldValueProviderContext ValueProviderContext { get; set; }

        public object GetValue(string parameters)
        {
            var settings = SF.Foundation.Facets.Facades.UserSettings.Settings;
            return settings[ValueProviderContext.FieldItem.Name, parameters];
        }
    }
}