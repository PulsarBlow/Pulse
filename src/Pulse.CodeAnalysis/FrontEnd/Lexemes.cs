namespace Pulse.CodeAnalysis.FrontEnd
{
    internal static class Lexemes
    {
        public const char LeftParen = '(';
        public const char RightParen = ')';
        public const char LeftBrace = '{';
        public const char RightBrace = '}';
        public const char Comma = ',';
        public const char Dot = '.';
        public const char Minus = '-';
        public const char Plus = '+';
        public const char Semicolon = ';';
        public const char Star = '*';
        public const char Bang = '!';
        public const string BangEqual = "!=";
        public const char Equal = '=';
        public const string EqualEqual = "==";
        public const char Less = '<';
        public const string LessEqual = "<=";
        public const char Greater = '>';
        public const string GreaterEqual = ">=";
        public const char Slash = '/';
        public const char StringDelimiter = '"';
        public const char Whitespace = ' ';
        public const char CarriageReturn = '\r';
        public const char Tabulation = '\t';
        public const char NewLine = '\n';

        // Keywords
        public const string And = "and";
        public const string Class = "class";
        public const string Else = "else";
        public const string False = "false";
        public const string For = "for";
        public const string Fun = "fun";
        public const string If = "if";
        public const string Nil = "nil";
        public const string Or = "or";
        public const string Return = "return";
        public const string Super = "super";
        public const string This = "this";
        public const string True = "true";
        public const string Var = "var";
        public const string While = "while";

        // Standard library
        public const string Print = "print";
    }
}
