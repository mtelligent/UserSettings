using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SF.Feature.Preference.Models;
using SF.Foundation.Facets.Facades;
using Sitecore.XA.Feature.Composites.Repositories;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;

namespace SF.Feature.Preference.Repositories
{
    public class PrefrencesListRepository : CompositeComponentRepository, IPreferencesListRepository
    {
        public IRenderingModelBase GetModel()
        {
            var model = new PreferencesListModel();

            FillBaseProperties(model);

            model.Area = model.Item.Fields["Area"].Value;
            model.Key = model.Item.Fields["Key"].Value;

            var hideIfSet = ((Sitecore.Data.Fields.CheckboxField)model.Item.Fields["HideIfSet"]).Checked;
            var isSet = !string.IsNullOrEmpty(UserSettings.Settings[model.Key, model.Area]);

            model.Show = !hideIfSet || !isSet;

            return model;
        }
    }
}