﻿using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Helixbase.Foundation.ORM.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISitecoreService>(provider => new SitecoreService(Sitecore.Context.Database));
            serviceCollection.AddScoped<IRequestContext, RequestContext>();
            serviceCollection.AddScoped<IMvcContext, MvcContext>();
            serviceCollection.AddScoped<IGlassHtml, GlassHtml>();
        }
    }
}