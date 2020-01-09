using Sitecore.Analytics;
using Sitecore.Analytics.Model;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets.Facades
{
    public class UserSettings
    {
        private static UserSettings _Settings = new UserSettings();

        public static UserSettings Settings
        {
            get
            {
                return _Settings;
            }
        }

        public string this[string key]
        {
            get
            {
                return this[key, UserSettingsConfiguration.DefaultArea];
            }
            set
            {
                this[key, UserSettingsConfiguration.DefaultArea] = value;
            }
        }

        protected Foundation.Facets.UserSettings GetUserSettings()
        {
            using (XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                var contact = client.Get<Contact>(GetIdentifier(), new Sitecore.XConnect.ContactExpandOptions(FacetNames.UserSettings));
                if (contact != null)
                {
                    return contact.GetFacet<Foundation.Facets.UserSettings>();
                }
            }
            return null;
        }

        private static Sitecore.Analytics.Model.Entities.ContactIdentifier GetContactId()
        {
            if (Tracker.Current?.Contact == null)
            {
                return null;
            }
            if (Tracker.Current.Contact.IsNew)
            {
                // write the contact to xConnect so we can work with it
                SaveContact();
            }

            return Tracker.Current.Contact.Identifiers.FirstOrDefault();
        }

        private static IdentifiedContactReference GetIdentifier()
        {
            // get the contact id from the current contact
            var id = GetContactId();

            // if the contact is new or has no identifiers
            var anon = Tracker.Current.Contact.IsNew || Tracker.Current.Contact.Identifiers.Count == 0;

            // if the user is anon, get the xD.Tracker identifier, else get the one we found
            return anon
                ? new IdentifiedContactReference(Sitecore.Analytics.XConnect.DataAccess.Constants.IdentifierSource, Tracker.Current.Contact.ContactId.ToString("N"))
                : new IdentifiedContactReference(id.Source, id.Identifier);
        }

        public static void SaveContact()
        {
            try
            {
                if (Tracker.Current.Contact.IsNew || Tracker.Current.Contact.Identifiers.Count == 0)
                {
                    Tracker.Current.Session.IdentifyAs("Anon", Tracker.Current.Contact.ContactId.ToString("N"));
                }

                // we need the contract to be saved to xConnect. It is only in session now
                var manager = Sitecore.Configuration.Factory.CreateObject("tracking/contactManager", true) as Sitecore.Analytics.Tracking.ContactManager;
                if (manager != null)
                {
                    // Save contact to xConnect; at this point, a contact has an anonymous
                    // TRACKER IDENTIFIER, which follows a specific format. Do not use the contactId overload
                    // and make sure you set the ContactSaveMode as demonstrated
                    Sitecore.Analytics.Tracker.Current.Contact.ContactSaveMode = ContactSaveMode.AlwaysSave;
                    manager.SaveContactToCollectionDb(Sitecore.Analytics.Tracker.Current.Contact);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error Saving Contact", ex, Settings);
            }
        }

        public string this[string key, string area]
        {
            get
            {
                if (Tracker.Current == null)
                {
                    Tracker.StartTracking();
                }

                try
                {

                    var userSettings = GetUserSettings();

                    if (userSettings == null ||
                        !userSettings.Settings.ContainsKey(area))
                    {
                        return string.Empty;
                    }
                    var areaSettings = userSettings.Settings[area];
                    if (areaSettings != null)
                    {
                        if (!areaSettings.ContainsKey(key))
                        {
                            return string.Empty;
                        }
                        return areaSettings[key];
                    }
                }
                catch (XdbExecutionException ex)
                {
                    // Manage conflicts / exceptions
                    Sitecore.Diagnostics.Log.Error("Error Fetching Custom UserSettings Facet", ex, this);
                }

                return string.Empty;
            }
            set
            {
                if (Tracker.Current == null)
                {
                    Tracker.StartTracking();
                }

                try
                {
                    using (XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
                    {
                        Contact contact = EnsureContact(client);
                        if (contact != null)
                        {
                            var userSettings = contact.GetFacet<Foundation.Facets.UserSettings>();
                            if (userSettings == null)
                            {
                                userSettings = new Facets.UserSettings();
                            }

                            if (!userSettings.Settings.ContainsKey(area))
                            {
                                userSettings.Settings.Add(area, new Dictionary<string, string>());
                            }

                            var areaSettings = userSettings.Settings[area];
                            if (areaSettings != null)
                            {
                                if (!areaSettings.ContainsKey(key))
                                {
                                    areaSettings.Add(key, value);
                                }
                                else
                                {
                                    areaSettings[key] = value;
                                }

                                client.SetFacet(contact, FacetNames.UserSettings, userSettings);
                                client.Submit();

                            }
                        }
                    }
                }
                catch (XdbExecutionException ex)
                {
                    // Manage conflicts / exceptions
                    Sitecore.Diagnostics.Log.Error("Error SAving Custom UserSettings Facet", ex, this);
                }


            }
        }

        private static Contact EnsureContact(XConnectClient client)
        {
            var contact = client.Get<Contact>(GetIdentifier(), new Sitecore.XConnect.ContactExpandOptions(FacetNames.UserSettings));
            if (contact == null)
            {
                SaveContact();

                contact = client.Get<Contact>(GetIdentifier(), new Sitecore.XConnect.ContactExpandOptions(FacetNames.UserSettings));

            }

            return contact;
        }


        /// <summary>
        /// Get the Entire Dictionary if you'll be working with multiple
        /// keys or updates.
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetArea(string area)
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }

            try
            {
                var userSettings = GetUserSettings();
                if (userSettings != null && userSettings.Settings.ContainsKey(area))
                {
                    return userSettings.Settings[area];
                }

            }
            catch (XdbExecutionException ex)
            {
                // Manage conflicts / exceptions
                Sitecore.Diagnostics.Log.Error("Error Fetching Custom UserSettings Facet", ex, this);
            }
            return null;
        }

        /// <summary>
        /// Use for Batch Updates of Settings within an Area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaSettings"></param>
        public void UpdateArea(string area, Dictionary<string, string> areaSettings)
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }

            try
            {
                using (XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
                {
                    var contact = EnsureContact(client);
                    if (contact != null)
                    {
                        var userSettings = contact.GetFacet<Foundation.Facets.UserSettings>();
                        if (userSettings == null)
                        {
                            userSettings = new Facets.UserSettings();
                        }

                        if (userSettings.Settings.ContainsKey(area))
                        {
                            userSettings.Settings.Remove(area);
                        }

                        userSettings.Settings.Add(area, areaSettings);

                        client.SetFacet(contact, FacetNames.UserSettings, userSettings);
                        client.Submit();
                    }
                }
            }
            catch (XdbExecutionException ex)
            {
                // Manage conflicts / exceptions
                Sitecore.Diagnostics.Log.Error("Error SAving Custom UserSettings Facet", ex, this);
            }
        }

        public bool ContainsArea(string area)
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }

            try
            {
                var userSettings = GetUserSettings();
                if (userSettings == null ||
                            !userSettings.Settings.ContainsKey(area))
                {
                    return false;
                }
                var areaSettings = userSettings.Settings[area];
                if (areaSettings != null)
                {
                    return true;
                }
            }
            catch (XdbExecutionException ex)
            {
                // Manage conflicts / exceptions
                Sitecore.Diagnostics.Log.Error("Error Fetching Custom UserSettings Facet", ex, this);
            }

            return false;
        }

        public bool ContainsKey(string key, string area)
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }

            try
            {
                var userSettings = GetUserSettings();

                if (userSettings == null ||
                            !userSettings.Settings.ContainsKey(area))
                {
                    return false;
                }
                var areaSettings = userSettings.Settings[area];
                if (areaSettings != null)
                {
                    if (!areaSettings.ContainsKey(key))
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch (XdbExecutionException ex)
            {
                // Manage conflicts / exceptions
                Sitecore.Diagnostics.Log.Error("Error Fetching Custom UserSettings Facet", ex, this);
            }

            return false;
        }

        public List<string> GetAreas()
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }

            try
            {
                var userSettings = GetUserSettings();
                if (userSettings != null && userSettings.Settings.Keys.Count > 0)
                {
                    return userSettings.Settings.Keys.ToList();
                }
                
            }
            catch (XdbExecutionException ex)
            {
                // Manage conflicts / exceptions
                Sitecore.Diagnostics.Log.Error("Error Fetching Custom UserSettings Facet", ex, this);
            }

            return new List<string>();
        }

        public List<string> GetKeys(string area)
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }

            try
            {
                var userSettings = GetUserSettings();
                if (userSettings != null)
                {
                    var areaSettings = userSettings.Settings[area];
                    if (areaSettings != null && areaSettings.Keys.Count > 0)
                    {
                        return areaSettings.Keys.ToList();
                    }
                }

            }
            catch (XdbExecutionException ex)
            {
                // Manage conflicts / exceptions
                Sitecore.Diagnostics.Log.Error("Error Fetching Custom UserSettings Facet", ex, this);
            }

            return new List<string>();
        }

        public bool ContainsKey(string key)
        {
            return ContainsKey(key, UserSettingsConfiguration.DefaultArea);
        }


    }
}