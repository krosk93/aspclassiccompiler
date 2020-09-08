using AspCore.BuiltInObjects;
using Dlrsoft.VBScript;
using Dlrsoft.VBScript.Compiler;
using Dlrsoft.VBScript.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspCore
{
    public class AspHandler
    {
        private Dictionary<string, CompiledPage> _scriptCache = null;
        private AspHost _aspHost = null;

        // Must have constructor with this signature, otherwise exception at run time
        public AspHandler(RequestDelegate next)
        {
            _scriptCache = new Dictionary<string, CompiledPage>();
            AspHostConfiguration config = new AspHostConfiguration();

            _aspHost = new AspHost(config);
        }

        public async Task Invoke(HttpContext context)
        {
            string pagePath = context.Request.ServerVariables["PATH_TRANSLATED"];

            CompiledPage cpage = null;
            if (_scriptCache.ContainsKey(pagePath))
            {
                cpage = _scriptCache[pagePath];
                //don't use it if updated
                if (cpage.CompileTime < File.GetLastWriteTime(pagePath))
                    cpage = null;
            }

            if (cpage == null)
            {
                try
                {
                    cpage = _aspHost.ProcessPageFromFile(pagePath);
                    _scriptCache[pagePath] = cpage;
                }
                catch (VBScriptCompilerException ex)
                {
                    AspHelper.RenderError(context.Response, ex);
                    return;
                }
            }

            ScriptScope pageScope = _aspHost.CreateScope();
            pageScope.SetVariable("response", new AspResponse(context));
            pageScope.SetVariable("request", new AspRequest(context));
            pageScope.SetVariable("session", new AspSession(context));
            pageScope.SetVariable("server", new AspServer(context));
            pageScope.SetVariable("application", new AspApplication(context));
            AspObjectContext ctx = new AspObjectContext();
            pageScope.SetVariable("objectcontext", ctx);
            pageScope.SetVariable("getobjectcontext", ctx);
            //responseScope.SetVariable("err", new Microsoft.VisualBasic.ErrObject());
            //Used to get the literals
            pageScope.SetVariable("literals", cpage.Literals);

            try
            {
                object o = cpage.Code.Execute(pageScope);
            }
            catch (ThreadAbortException)
            {
                //Do nothing since we get this normally when calling response.redirect
            }
            catch (Exception ex)
            {
                if (_aspHost.Configuration.Trace)
                {
                    TraceHelper th = (TraceHelper)pageScope.GetVariable(VBScript.TRACE_PARAMETER);
                    string source = string.Format("{0} ({1},{2})-({3},{4})", th.Source, th.StartLine, th.StartColumn, th.EndLine, th.EndColumn);
                    throw new VBScriptRuntimeException(ex, source);
                }
                throw;
            }
        }

        // ...

        private string GenerateResponse(HttpContext context)
        {
            string title = context.Request.Query["title"];
            return string.Format("Title of the report: {0}", title);
        }

        private string GetContentType()
        {
            return "text/plain";
        }
    }

    public static class AspHandlerExtensions
    {
        public static IApplicationBuilder UseMyHandler(this IApplicationBuilder builder)
        {
            return builder.MapWhen(
                context => context.Request.Path.ToString().EndsWith(".asp", StringComparison.OrdinalIgnoreCase),
                appBranch => appBranch.UseMiddleware<AspHandler>()
            );
        }
    }
}
