using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;

namespace AspCore.Abstractions
{
    public interface IClassicAspRendering
    {
        Task<string> Render(FileInfo aspFile, object model, ViewDataDictionary viewData, ModelStateDictionary modelState);
    }
}
