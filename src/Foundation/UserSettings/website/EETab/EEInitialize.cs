using EPExpressTab.Pipelines.Initialize;
using Sitecore.Data.Events;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.UserSettings.EETab
{
    /// <summary>
    /// Event was firing in creation of core items
    /// that was causing errors/failure. 
    /// Wrapping Initialize Pipeline in Event Disabler to allow this
    /// to work.
    /// </summary>
    public class EEInitialize
    {
        public void Process(PipelineArgs args)
        {
            using (new EventDisabler())
            {
                var init = new Initialize();
                init.Process(args);
            }
        }
    }
}