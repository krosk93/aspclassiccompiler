using AspCore.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace AspCore.Mvc
{
    public class ClassicAspMvcViewOptionsSetup : IConfigureOptions<MvcViewOptions>
    {
        private readonly IClassicAspViewEngine classicAspViewEngine;

        public ClassicAspMvcViewOptionsSetup(IClassicAspViewEngine classicAspViewEngine)
        {
            this.classicAspViewEngine = classicAspViewEngine ?? throw new ArgumentNullException(nameof(classicAspViewEngine));
        }

        public void Configure(MvcViewOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.ViewEngines.Add(classicAspViewEngine);
        }
    }
}
