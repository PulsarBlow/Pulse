namespace Pulse.CodeAnalysis
{
    using System;
    using FrontEnd;
    using FrontEnd.Errors;
    using Helpers;

    public class Interpreter : IVisitor<object?>
    {
        private readonly IErrorReporter _errorReporter;

        public Interpreter(
            IErrorReporter errorReporter)
            => _errorReporter = errorReporter;

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

        /// <summary>
        /// Interpret a given Expression and returns
        /// </summary>
        /// <param name="expression">The expression to interpret</param>
        public void Interpret(
            Expression expression)
        {
            try
            {
                var value = Evaluate(expression);
                Console.WriteLine(ObjectFormatter.Stringify(value));
            }
            catch (RuntimeException e)
            {
                // We don't want the Interpreter to crash on runtime error.
                _errorReporter.ReportRuntimeError(e);
            }
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
