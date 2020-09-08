using Microsoft.Scripting;
using Microsoft.Scripting.Utils;
using System.IO;

namespace Dlrsoft.VBScript.Compiler
{
    public class VBScriptStringContentProvider : TextContentProvider {
        private readonly string _code;
        private ISourceMapper _mapper;

        public VBScriptStringContentProvider(string code, ISourceMapper mapper)
        {
            ContractUtils.RequiresNotNull(code, "code");

            _code = code;
            _mapper = mapper;
        }

        public override SourceCodeReader GetReader() {
            return new VBScriptSourceCodeReader(new StringReader(_code), null, _mapper);
        }
    }
}
