using AspCore.BuiltInObjects.Abstractions;

namespace AspCore.BuiltInObjects
{
    /// <summary>
    /// The asperror object that is accessible from ASP code
    /// </summary>
    public class AspError : IAspError
	{
        public AspError()
		{
		}

		public AspError(string aspcode, int number, string source, string category, string file, int line, int column, string description, string aspdescription)
		{
			ASPCode=aspcode;
			Number=number;
			Source=source;
			Category=category;
			File=file;
			Line=line;
			Column=column;
			Description=description;
			ASPDescription=aspdescription;
		}

        public string ASPCode { get; } = "";

        public int Number { get; } = 0;

        public string Source { get; } = "";

        public string Category { get; } = "";

        public string File { get; } = "";

        public int Line { get; } = 0;

        public int Column { get; } = 0;

        public string Description { get; } = "";

        public string ASPDescription { get; } = "";
    }
}
