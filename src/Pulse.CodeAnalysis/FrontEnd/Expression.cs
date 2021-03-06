﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Generator name    : pulse_ast
//     Generator version : 1.0.0-alpha
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Pulse.CodeAnalysis.FrontEnd
{
    public interface IExpressionVisitor<out T>
    {
        T VisitBinaryExpression(BinaryExpression expression);
        T VisitGroupingExpression(GroupingExpression expression);
        T VisitLiteralExpression(LiteralExpression expression);
        T VisitUnaryExpression(UnaryExpression expression);
    }
    public abstract class Expression
    {
        public abstract T Accept<T>(IExpressionVisitor<T> visitor);
    }
    public sealed class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Token Operator { get; }
        public Expression Right { get; }
        
        public BinaryExpression(Expression left,Token @operator,Expression right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
        
        public override T Accept<T>(IExpressionVisitor<T> visitor)
            => visitor.VisitBinaryExpression(this);
    }
    public sealed class GroupingExpression : Expression
    {
        public Expression Expression { get; }
        
        public GroupingExpression(Expression expression)
        {
            Expression = expression;
        }
        
        public override T Accept<T>(IExpressionVisitor<T> visitor)
            => visitor.VisitGroupingExpression(this);
    }
    public sealed class LiteralExpression : Expression
    {
        public object Value { get; }
        
        public LiteralExpression(object value)
        {
            Value = value;
        }
        
        public override T Accept<T>(IExpressionVisitor<T> visitor)
            => visitor.VisitLiteralExpression(this);
    }
    public sealed class UnaryExpression : Expression
    {
        public Token Operator { get; }
        public Expression Right { get; }
        
        public UnaryExpression(Token @operator,Expression right)
        {
            Operator = @operator;
            Right = right;
        }
        
        public override T Accept<T>(IExpressionVisitor<T> visitor)
            => visitor.VisitUnaryExpression(this);
    }
}
