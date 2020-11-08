namespace Pulse
{
    internal static class ExitCode
    {
        /// <summary>
        /// The command was used incorrectly, e.g., with the wrong number of arguments,
        /// a bad flag, a bad syntax in a parameter, or whatever.
        /// </summary>
        /// <code>64</code>
        public const int ExUsage = 64;

        /// <summary>
        /// The input data was incorrect in some way.
        /// This should only be used for user's data and not system files.
        /// </summary>
        /// <code>65</code>
        public const int ExDataErr = 65;

        /// <summary>
        /// An internal software error has been detected.
        /// This should be limited to non-operating system related errors as possible.
        /// </summary>
        /// <code>70</code>
        public const int ExSoftware = 70;
    }
}
