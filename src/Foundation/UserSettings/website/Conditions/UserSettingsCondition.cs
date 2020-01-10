using Sitecore.Analytics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets
{
    public class UserSettingsCondition<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string Area { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        protected override bool Execute(T ruleContext) 
        {
            if (Tracker.Current != null &&
                Tracker.Current.Sampling != null && 
                Tracker.Current.Session != null &&
                Tracker.Current.Session.Interaction != null)
            {
                var userSettings = SF.Foundation.Facets.Facades.UserSettings.Settings;
                if (userSettings != null)
                {
                    if (userSettings.ContainsKey(Key, Area))
                    {
                        var result = Compare(userSettings[Key, Area], Value);
                        return result;
                    }                    
                }
            }
            return false;
        }
    }
}