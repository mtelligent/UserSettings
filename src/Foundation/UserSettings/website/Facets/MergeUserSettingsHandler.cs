using SF.Foundation.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.UserSettings.Facets
{
    /// <summary>
    /// Merges User Settings Dictionary when Contact becomes known.
    /// </summary>
    public class MergeUserSettingsHandler : MergingCalculatedFacetHandler<SF.Foundation.Facets.UserSettings>
    {
        public MergeUserSettingsHandler(): base(FacetNames.UserSettings, null)
        {

        }

        protected override bool Merge(Foundation.Facets.UserSettings source, Foundation.Facets.UserSettings target)
        {
            if (source == null || target == null)
            {
                // No contacts changed - return false
                return false;
            }

            foreach(var area in source.Settings.Keys)
            {
                if (target.Settings.ContainsKey(area))
                {
                    //merge settings
                    foreach(var key in source.Settings[area].Keys)
                    {
                        if (!target.Settings[area].ContainsKey(key))
                        {
                            target.Settings[area].Add(key, source.Settings[area][key]);
                        }
                        else
                        {
                            //uncomment if you want source to overwrite target
                            //target.Settings[area][key] = source.Settings[area][key];
                        }
                    }
                }
                else
                {
                    //add area
                    target.Settings.Add(area, source.Settings[area]);
                }
            }

            return true;
        }

        

        protected override bool UpdateFacet(Foundation.Facets.UserSettings currentFacet, Interaction interaction)
        {
            // For calculated facets only
            // Return false as contact not changed by this method
            return false;
        }
    }
}