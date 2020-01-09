using SF.Feature.Preference.Models;
using SF.Feature.Preference.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SF.Feature.Preference.Controllers
{
    public class PreferencesListController : StandardController
    {
        protected readonly IPreferencesListRepository PreferencesListRepository;

        public PreferencesListController(IPreferencesListRepository repository)
        {
            this.PreferencesListRepository = repository;
        }

        protected override object GetModel()
        {
            return PreferencesListRepository.GetModel();
        }

        public override ActionResult Index()
        {
            var model = this.GetModel() as PreferencesListModel;

            //Always show in Experience Editor Mode
            if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                model.Show = true;
            }

            return View(model);
        }
    }
}