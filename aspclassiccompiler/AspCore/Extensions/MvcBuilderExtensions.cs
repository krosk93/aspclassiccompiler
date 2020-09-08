using AspCore.Abstractions;
using AspCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace AspCore.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddClassicAsp(this IMvcBuilder builder, Action<ClassicAspViewEngineOptions> setupAction = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddOptions()
                .AddTransient<IConfigureOptions<ClassicAspViewEngineOptions>, ClassicAspViewEngineOptionsSetup>();

            if (setupAction != null)
            {
                builder.Services.Configure(setupAction);
            }

            builder.Services
                .AddTransient<IConfigureOptions<MvcViewOptions>, ClassicAspMvcViewOptionsSetup>()
                .AddSingleton<IClassicAspRendering, ClassicAspRendering>()
                .AddSingleton<IClassicAspViewEngine, ClassicAspViewEngine>();

            return builder;
        }
    }
}
