# User Settings

Welcome to User Settings for Sitecore, the easy way to manage custom user data in Sitecore's Experience Database.

Backed by a custom facet that stores it's values in xDB, User Settings provides a facade that abstracts you from needing to manage the contact and xConnect APi's.

It handles making sure contacts are known, handling contact merging and leveraging xConnect API's so you can focus on your business logic.

# Areas, Keys and Values

User Settings provides a dictionary of string dictionaries for saving and retreiving user data as key value pairs. 

This allows you to organize user data into "Areas", giving you the ability to segment types of settings as you see fit.

# Facade API

The User Settings  module includes a Facade Singleton class called UserSettings which can be used to access areas or individual settings without worrying about xConnect.

* Settings[key] - Fetch or update User Setting in the default Area (configured as a Sitecore setting)
* Settings[area,key] - Fetch or update User Setting in specified Area
* Settings.GetArea(area) - Get all the settings for an area as a String Dictionary
* Settings.SaveArea(area, areaSettings) - Replaces all the settings in an area with the provided string dictionary. This is more efficient than updating settings using the indexer, as each use of the indexer makes a call to xConnect.
* Settings.ContainsArea(area) - Checks whether an area has been defined for the current user.
* Settings.ContainsKey(area, key) - Checks whether a key has been defined in a given area for the current user.
* Settings.GetAreas() - returns all the areas defined for the user.
* Settings.GetKeys(area) - returns all the keys for a given area for the user.

# REST API

The User Settings module includes a rest API that can be used to manage User Settings client side.

To get individual settings, issue a get request to this route:
/api/sf/1.0/userSettings/{area}/{key}

To update or add a setting you can post to the same url.

If you want to get or update all the settings for a given area, you can use this route:
/api/sf/1.0/userSettings/{area}

# Sitecore Forms Submit Action and Value Provider

The User Settings module include a Sitecore Forms Submit Action that will use the form's field names as the key's for the configured area.
When you add the User Settings Submit Action to your form, it will prompt you to specify which area to save the submitted values for.

If you want the default value of a field to come from User Settings, you can configure the User Settings Value Provider for that field. Set the Value Provider Parameters to the name of the area you want to retrieve the value for, and it will use the name of the field as a key to retrieve it.

# Experience Profile Tab

The User Settings module adds a tab called "User Settings" to the Experience Profile. This will display all settings for a contact by area.

# Personalization Rule

The User Settings module add a custom User Settings personalization rule that allow you to personalize based on values saved for a given user.

where the current user has a User Setting for Area "Area Name" and "Key" that "compares to" "Specific Value", 
allowing content authors to specify the area, key, comparison type and value they wish to use for the condition.

# User Preferences List Module

An optional module for SXA is also part of this project. It gives content authors an SXA component that displays a list of options that when clicked will set a User Setting that can be used for personlization. This can be used to quickly capture visitor intent and personalize based on that.

The component itself is built as a composite and the Item template is based on the Carousel slide, though you can customize the look and feel with variants the same way you can work with the SXA carousel. It uses the User Settings API to save the setting asynchronously, though it can be configured to redirect the page when it returns.


# Installation

See the Releases tab to see the latest release of a Sitecore package and xConnect Zip file. As this implementation leverages xConnect for storage and retreival of the facets, you need to unzip this file to the root of the xConnect service. This has been arranged for development envrionments, so may need to be tailored for scaled envrionments.

There is a separate package for User Preferences for SXA. After installing, the Preference feature can be activated on your site. Note that it should automatically add the Preferences List Base Theme to your site theme, though sometimes I have seen needing to manually associate your theme with this base theme.

This has been built and tested for Sitecore 9.3 and SXA 9.3.

