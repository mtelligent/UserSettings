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
                
                if (field is Sitecore.ExperienceForms.Mvc.Models.Fields.ListViewModel)
                {
                    var listField = field as Sitecore.ExperienceForms.Mvc.Models.Fields.ListViewModel;
                    settings[field.Name, area] = string.Join(",", listField.Value);
                    continue;
                }

                if (field is Sitecore.ExperienceForms.Mvc.Models.Fields.CheckBoxListViewModel)
                {
                    var listField = field as Sitecore.ExperienceForms.Mvc.Models.Fields.CheckBoxListViewModel;
                    settings[field.Name, area] = string.Join(",", listField.Value);
                    continue;
                }

                if (field is Sitecore.ExperienceForms.Mvc.Models.Fields.DropDownListViewModel)
                {
                    var listField = field as Sitecore.ExperienceForms.Mvc.Models.Fields.DropDownListViewModel;
                    settings[field.Name, area] = string.Join(",", listField.Value);
                    continue;
                }

                if (field is Sitecore.ExperienceForms.Mvc.Models.Fields.ListBoxViewModel)
                {
                    var listField = field as Sitecore.ExperienceForms.Mvc.Models.Fields.ListBoxViewModel;
                    settings[field.Name, area] = string.Join(",", listField.Value);
                    continue;
                }

                if (field is Sitecore.ExperienceForms.Mvc.Models.Fields.ListViewModel)
                {
                    var listField = field as Sitecore.ExperienceForms.Mvc.Models.Fields.ListViewModel;
                    settings[field.Name, area] = string.Join(",", listField.Value);
                    continue;
                }

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