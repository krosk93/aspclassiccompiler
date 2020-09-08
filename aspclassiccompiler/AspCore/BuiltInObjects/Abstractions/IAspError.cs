using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.BuiltInObjects.Abstractions
{
    public interface IAspError
    {
        string ASPCode { get; }
        int Number { get; }
        string Category { get; }
        string File { get; }
        int Line { get; }
        string Description { get; }
        string ASPDescription { get; }
        int Column { get; }
        string Source { get; }
    }
}
