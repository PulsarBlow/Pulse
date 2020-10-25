namespace Pulse.Interpreter.FrontEnd
{
    using System;
    using System.Runtime.Serialization;

    public class ParseException : Exception
    {
        public ParseException(
            string? message)
            : base(message)
        {
        }

        public ParseException(
            string? message,
            Exception? innerException)
            : base(
                message,
                innerException)
        {
        }

        protected ParseException(
            SerializationInfo info,
            StreamingContext context)
            : base(
                info,
                context)
        {
        }
    }
}
