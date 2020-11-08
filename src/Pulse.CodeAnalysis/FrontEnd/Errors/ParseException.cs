namespace Pulse.CodeAnalysis.FrontEnd.Errors
{
    using System;

    public class ParseException : Exception
    {
        public Token Token { get; }

        public ParseException(
            Token token,
            string message)
            : base(message)
            => Token = token;
    }
}
