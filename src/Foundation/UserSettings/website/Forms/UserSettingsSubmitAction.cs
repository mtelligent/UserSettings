using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SF.Foundation.Facets.Facades;

namespace SF.Foundation.Facets.Forms
{
    public class UserSettingsSubmitAction : SubmitActionBase<UserSettingsParameters>
    {
        public UserSettingsSubmitAction(ISubmitActionData submitActionData) : base(submitActionData)
        {

        }

        protected override bool Execute(UserSettingsParameters data, FormSubmitContext formSubmitContext)
        {
            var settings = SF.Foundation.Facets.Facades.UserSettings.Settings;

            var area = data == null || string.IsNullOrEmpty(data.Area) ? UserSettingsConfiguration.DefaultArea : data.Area;

            foreach (var field in formSubmitContext.Fields)
            {
                settings[field.Name, area] = GetValue(field);
            }

            return true;
        }

        private static string GetValue(object field)
        {
            return field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
        }
    }
}