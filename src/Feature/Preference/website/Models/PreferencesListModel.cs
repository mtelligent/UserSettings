using Sitecore.XA.Feature.Composites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Preference.Models
{
    public class PreferencesListModel : CompositeComponentRenderingModel
    {
        public string Area { get; set; }

        public string Key { get; set; }


        public bool Show { get; set; }
    }
}