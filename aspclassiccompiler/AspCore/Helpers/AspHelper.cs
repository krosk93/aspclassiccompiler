using Dlrsoft.VBScript.Compiler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace AspCore
{
    public static class AspHelper
    {
        public static void RenderError(ViewContext context, VBScriptCompilerException exception)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.StatusCode = 500;

            RenderError(context.Writer, exception);
        }

        public static void RenderError(TextWriter output, VBScriptCompilerException exception)
        {
            output.Write("<h1>VBScript Compiler Error</h1>");
            output.Write("<table>");
            output.Write("<tr>");
            output.Write(string.Format("<td>{0}</td>", "FileName"));
            output.Write(string.Format("<td>{0}</td>", "Line"));
            output.Write(string.Format("<td>{0}</td>", "Column"));
            output.Write(string.Format("<td>{0}</td>", "Error Code"));
            output.Write(string.Format("<td>{0}</td>", "Error Description"));
            output.Write("</tr>");
            foreach (VBScriptSyntaxError error in exception.SyntaxErrors)
            {
                output.Write("<tr>");
                output.Write(string.Format("<td>{0}</td>", error.FileName));
                output.Write(string.Format("<td>{0}</td>", error.Span.Start.Line));
                output.Write(string.Format("<td>{0}</td>", error.Span.Start.Column));
                output.Write(string.Format("<td>{0}</td>", error.ErrorCode));
                output.Write(string.Format("<td>{0}</td>", error.ErrorDescription));
                output.Write("</tr>");
            }
            output.Write("</table>");
        }
    }
}
