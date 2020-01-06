using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SF.Foundation.Facets.Controllers
{
    public class UserSettingsController : ServicesApiController
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [ActionName("GetUserSetting")]
        public string GetUserSetting(string key, string area)
        {
            var userSettings = SF.Foundation.Facets.Facades.UserSettings.Settings;
            return userSettings[key, area];
        }

        [HttpPost]
        [ActionName("GetUserSetting")]
        public HttpResponseMessage GetUserSetting(string key, string area, string value)
        {
            var userSettings = SF.Foundation.Facets.Facades.UserSettings.Settings;
            userSettings[key, area] = value;
            return Request.CreateResponse(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [ActionName("GetAreaSettings")]
        public Dictionary<string, string> GetAreaSettings(string area)
        {
            var userSettings = SF.Foundation.Facets.Facades.UserSettings.Settings;
            return userSettings.GetArea(area);
        }

        [HttpPost]
        [ActionName("GetAreaSettings")]
        public HttpResponseMessage SaveAreaSettings(string area, Dictionary<string, string> settings)
        {
            var userSettings = SF.Foundation.Facets.Facades.UserSettings.Settings;
            userSettings.UpdateArea(area, settings);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK);
        }
    }
}