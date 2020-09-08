using AspCore.Abstractions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;
using System.Threading.Tasks;

namespace AspCore.TagHelpers
{
    [HtmlTargetElement("classicasp")]
    public class ClassicAspTagHelper : TagHelper
    {
        public object Model { get; set; }
        public string View { get; set; }

        private readonly IClassicAspRendering classicAspRendering;

        public ClassicAspTagHelper(IClassicAspRendering classicAspRendering)
        {
            this.classicAspRendering = classicAspRendering;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var result = await classicAspRendering.Render(new FileInfo(View), Model, null, null).ConfigureAwait(false);
            output.TagName = null;
            output.Content.AppendHtml(result);
        }
    }
}
