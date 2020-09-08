using AspCore.BuiltInObjects.Abstractions;
using AspCore.BuiltInObjects;
using Dlrsoft.VBScript.Compiler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspCore.Mvc
{
    public class ClassicAspView : IView
    {
        private static Dictionary<string, CompiledPage> _scriptCache = new Dictionary<string, CompiledPage>();
        private static AspHost aspHost = new AspHost(null);

        public string Path { get; private set; }
        public string MasterPath { get; private set; }

        public ClassicAspView(string viewPath)
        {
            Path = viewPath;
        }
        public ClassicAspView(string viewPath, string masterPath)
        {
            Path = viewPath;
            MasterPath = masterPath;
        }

        public async Task RenderAsync(ViewContext context)
        {
            CompiledPage cpage = null;
            if(_scriptCache.ContainsKey(Path))
            {
                cpage = _scriptCache[Path];
                if (cpage.CompileTime < File.GetLastWriteTime(Path))
                    cpage = null;
            }
            if (cpage == null)
            {
                try
                {
                    cpage = aspHost.ProcessPageFromFile(Path);
                    _scriptCache[Path] = cpage;
                }
                catch (VBScriptCompilerException ex)
                {
                    AspHelper.RenderError(context, ex);
                    return;
                }
            }

            ScriptScope responseScope = aspHost.CreateScope();
            HttpContext httpContext = context.HttpContext;
            responseScope.SetVariable("request", httpContext.RequestServices.GetService<IAspRequest>());
            responseScope.SetVariable("session", httpContext.RequestServices.GetService<IAspSession>());
            responseScope.SetVariable("application", httpContext.RequestServices.GetService<IAspApplication>());
            responseScope.SetVariable("writer", context.Writer);
            IAspResponse aspResponse = httpContext.RequestServices.GetService<IAspResponse>();
            responseScope.SetVariable("response", aspResponse);

            responseScope.SetVariable("model", context.ViewData.Model);
            responseScope.SetVariable("viewdata", context.ViewData);
            responseScope.SetVariable("viewbag", context.ViewBag);

            responseScope.SetVariable("literals", cpage.Literals);

            object o = cpage.Code.Execute(responseScope);
            aspResponse.Flush();
        }
    }
}
