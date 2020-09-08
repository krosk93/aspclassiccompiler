using AspCore.Abstractions;
using AspCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace AspCore
{
    public class ClassicAspRendering : IClassicAspRendering
    {
        private readonly ClassicAspViewEngineOptions options;

        public ClassicAspRendering(IOptions<ClassicAspViewEngineOptions> options)
        {
            this.options = options.Value;
        }

        public async Task<string> Render(FileInfo aspFile, object model, ViewDataDictionary viewData, ModelStateDictionary modelState)
        {
            return "";
        }
    }
}
