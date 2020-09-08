using Dlrsoft.VBScript.Compiler;
using Dlrsoft.VBScript.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using System.Reflection;

namespace AspCore
{
    public class AspHost
    {
        private ScriptRuntime _runtime;
        private ScriptEngine _engine;
        private AspHostConfiguration _config;

        public AspHost(AspHostConfiguration config)
        {
            _config = config;

            ScriptRuntimeSetup setup = new ScriptRuntimeSetup();

            if (config != null && config.Trace)
            {
                setup.Options["Trace"] = ScriptingRuntimeHelpers.True;
            }

            string qualifiedname = typeof(VBScriptContext).AssemblyQualifiedName;
            setup.LanguageSetups.Add(new LanguageSetup(
                qualifiedname, "vbscript", new[] { "vbscript" }, new[] { ".vbs" }));
            _runtime = new ScriptRuntime(setup);
            if (config != null && config.Assemblies != null)
            {
                foreach (Assembly a in config.Assemblies)
                {
                    _runtime.LoadAssembly(a);
                }
            }
            _engine = _runtime.GetEngine("vbscript");
        }

        public CompiledPage ProcessPageFromFile(string pagePath)
        {
            AspPageDom page = new AspPageDom();
            page.processPage(pagePath);
            return CompilePage(page);
        }

        public CompiledPage ProcessPageFromString(string includePath, string pageString)
        {
            AspPageDom page = new AspPageDom();
            page.processPage(includePath, pageString);
            return CompilePage(page);
        }

        public ScriptScope CreateScope()
        {
            ScriptScope scope = _engine.CreateScope();

            if (_config != null && _config.Trace)
            {
                scope.SetVariable(Dlrsoft.VBScript.VBScript.TRACE_PARAMETER, new Dlrsoft.VBScript.Runtime.TraceHelper());
            }

            return scope;
        }

        public AspHostConfiguration Configuration
        {
            get { return _config; }
        }

        private CompiledPage CompilePage(AspPageDom page)
        {
            ScriptSource src = _engine.CreateScriptSource(new VBScriptStringContentProvider(page.Code, page.Mapper), page.PagePath, SourceCodeKind.File);
            CompiledCode compiledCode = src.Compile();
            return new CompiledPage(compiledCode, page.Literals);
        }
    }
}
