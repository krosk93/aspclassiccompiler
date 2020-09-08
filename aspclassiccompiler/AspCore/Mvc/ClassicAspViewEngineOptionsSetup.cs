using Microsoft.Extensions.Options;

using static AspCore.Constants;

namespace AspCore
{
    public class ClassicAspViewEngineOptionsSetup : ConfigureOptions<ClassicAspViewEngineOptions>
    {
        public ClassicAspViewEngineOptionsSetup(): base(Configure) { }

        private new static void Configure(ClassicAspViewEngineOptions options)
        {
            options.ViewLocationFormats.Add("Views/{1}/{0}" + VIEW_EXTENSION);
            options.ViewLocationFormats.Add("Views/Shared/{0}" + VIEW_EXTENSION);
        }
    }
}
