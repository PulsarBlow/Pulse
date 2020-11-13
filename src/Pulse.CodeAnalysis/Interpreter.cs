namespace Pulse.CodeAnalysis
{
    using System;
    using System.Collections.Generic;
    using FrontEnd;
    using FrontEnd.Errors;
    using Helpers;

    public class Interpreter : IExpressionVisitor<object?>, IStatementVisitor
    {
        private readonly IErrorReporter _errorReporter;

        public Interpreter(
            IErrorReporter errorReporter)
            => _errorReporter = errorReporter;

        /// <summary>
        /// Interpret a given Statements and Expression
        /// </summary>
        /// <param name="statements">A collection of <see cref="Statement"/> to interpret</param>
        public void Interpret(
            IEnumerable<Statement> statements)
        {
            try
            {
                foreach (var stmt in statements) { Execute(stmt); }
            }
            catch (RuntimeException e)
            {
                // We don't want the Interpreter to crash on runtime error.
                _errorReporter.ReportRuntimeError(e);
            }
        }

        public void VisitExpressionStatement(
            ExpressionStatement statement)
            => Evaluate(statement.Expression);

        public void VisitPrintStatement(
            PrintStatement statement)
        {
            var value = Evaluate(statement.Expression);
            Console.WriteLine(ObjectFormatter.Stringify(value));
        }

        public object? VisitBinaryExpression(
            BinaryExpression expression)
        {
            var left = Evaluate(expression.Left);
            var right = Evaluate(expression.Right);

            switch (expression.Operator.Type)
            {
                case TokenType.Greater:
                    GuardNumberOperands(
                        expression.Operator,
                        left,
                        right);
                    return (double) left! > (double) right!;

                case TokenType.GreaterEqual:
                    GuardNumberOperands(
                        expression.Operator,
                        left,
                        right);
                    return (double) left! >= (double) right!;

                case TokenType.Less:
                    GuardNumberOperands(
                        expression.Operator,
                        left,
                        right);
                    return (double) left! < (double) right!;

                case TokenType.LessEqual:
                    GuardNumberOperands(
                        expression.Operator,
                        left,
                        right);
                    return (double) left! <= (double) right!;

                case TokenType.Minus:
                    GuardNumberOperands(
                        expression.Operator,
                        left,
                        right);
                    return (double) left! - (double) right!;

                case TokenType.Plus:
                    return left switch
                    {
                        double dl when right is double dr => dl + dr,
                        string _ when right is string => string.Concat(
                            left,
                            right),
                        _ => throw new RuntimeException(
                            expression.Operator,
                            "Operation not supported"),
                    };

                case TokenType.Slash:
                    GuardNumberOperands(
                        expression.Operator,
                        left,
                        right);
                    return (double) left! / (double) right!;

                case TokenType.Star:
                    GuardNumberOperands(
                        expression.Operator,
                        left,
                        right);
                    return (double) left! * (double) right!;

                case TokenType.BangEqual:
                    return !ObjectComparer.AreEqual(
                        left,
                        right);

                case TokenType.EqualEqual:
                    return ObjectComparer.AreEqual(
                        left,
                        right);
                default:
                    return null;
            }
        }

        public object? VisitGroupingExpression(
            GroupingExpression expression)
            => Evaluate(expression.Expression);

        public object? VisitLiteralExpression(
            LiteralExpression expression)
            => expression.Value;

        public object? VisitUnaryExpression(
            UnaryExpression expression)
        {
            var right = Evaluate(expression.Right);
            if (expression.Operator.Type == TokenType.Minus)
            {
                GuardNumberOperand(
                    expression.Operator,
                    right);
                return -(double) right!;
            }

            if (expression.Operator.Type == TokenType.Bang)
            {
                return !ObjectComparer.IsTruthy(right);
            }

            return null;
        }

        private void Execute(
            Statement statement)
        {
            statement.Accept(this);
        }

        private object? Evaluate(
            Expression expression)
            => expression.Accept(this);

        private static void GuardNumberOperand(
            Token op,
            object? operand)
        {
            if (operand is double) { return; }

            throw new RuntimeException(
                op,
                "Operand must be a number.");
        }

        private static void GuardNumberOperands(
            Token op,
            object? left,
            object? right)
        {
            if (left is double && right is double) { return; }

            throw new RuntimeException(
                op,
                "Operands must be numbers.");
        }
    }
}
