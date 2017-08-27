﻿using System;
using System.Linq;
using Antlr4.Runtime;
using Choop.Compiler.BlockModel;
using Choop.Compiler.ChoopModel.Declarations;
using Choop.Compiler.ChoopModel.Expressions;
using Choop.Compiler.ChoopModel.Methods;
using Choop.Compiler.Helpers;

namespace Choop.Compiler.ChoopModel.Assignments
{
    /// <summary>
    /// Represents an assignment statement.
    /// </summary>
    public class VarAssignStmt : IAssignStmt
    {
        #region Properties

        /// <summary>
        /// Gets the name of the variable being assigned.
        /// </summary>
        public string VariableName { get; }

        /// <summary>
        /// Gets the operator used for the assignment.
        /// </summary>
        public AssignOperator Operator { get; }

        /// <summary>
        /// Gets the input to the assignment operator. (Null for increment and decrement)
        /// </summary>
        public IExpression Value { get; }

        /// <summary>
        /// Gets the token to report any compiler errors to.
        /// </summary>
        public IToken ErrorToken { get; }

        /// <summary>
        /// Gets the file name where the grammar structure was found.
        /// </summary>
        public string FileName { get; }

        string IAssignStmt.ItemName => VariableName;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="VarAssignStmt"/> class.
        /// </summary>
        /// <param name="variableName">The name of the variable being assigned.</param>
        /// <param name="operator">The operator to use for the assignment.</param>
        /// <param name="value">The input to the assignment operator.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="errorToken">The token to report any compiler errors to.</param>
        public VarAssignStmt(string variableName, AssignOperator @operator, string fileName, IToken errorToken,
            IExpression value = null)
        {
            Operator = @operator;
            FileName = fileName;
            ErrorToken = errorToken;
            VariableName = variableName;
            Value = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the translated code for the grammar structure.
        /// </summary>
        /// <returns>The translated code for the grammar structure.</returns>
        public Block[] Translate(TranslationContext context)
        {
            // Get variable
            IDeclaration variable = context.GetDeclaration(VariableName);

            // Check variable was found
            if (variable == null)
            {
                context.ErrorList.Add(new CompilerError($"Variable '{VariableName}' was not defined",
                    ErrorType.NotDefined, ErrorToken, FileName));
                return new Block[0];
            }

            // Try as stack variable
            StackValue stackValue = variable as StackValue;
            if (stackValue != null && stackValue.StackSpace == 1)
            {
                CheckOperator(stackValue.Type, context);

                switch (Operator)
                {
                    case AssignOperator.Equals:
                        return stackValue.CreateVariableAssignment(context, Value);

                    case AssignOperator.AddEquals:
                        return stackValue.CreateVariableIncrement(context, Value);

                    case AssignOperator.MinusEquals:
                        return stackValue.CreateVariableIncrement(context, new UnaryExpression(Value, UnaryOperator.Minus, FileName,
                            ErrorToken));

                    case AssignOperator.DotEquals:
                        return stackValue.CreateVariableAssignment(context, new CompoundExpression(CompoundOperator.Concat,
                            new LookupExpression(stackValue, FileName, ErrorToken), Value, FileName, ErrorToken));

                    case AssignOperator.PlusPlus:
                        return new[] {stackValue.CreateVariableIncrement(1)};

                    case AssignOperator.MinusMinus:
                        return new[] {stackValue.CreateVariableIncrement(-1)};

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Try as global variable
            GlobalVarDeclaration globalVarDeclaration = variable as GlobalVarDeclaration;
            if (globalVarDeclaration != null)
            {
                CheckOperator(globalVarDeclaration.Type, context);

                switch (Operator)
                {
                    case AssignOperator.Equals:
                        return new BlockBuilder(BlockSpecs.SetVariableTo, context).AddParam(VariableName)
                            .AddParam(Value).Create().ToArray();

                    case AssignOperator.AddEquals:
                        return new BlockBuilder(BlockSpecs.ChangeVarBy, context).AddParam(VariableName).AddParam(Value)
                            .Create().ToArray();

                    case AssignOperator.MinusEquals:
                        return new BlockBuilder(BlockSpecs.ChangeVarBy, context).AddParam(VariableName)
                            .AddParam(new UnaryExpression(Value, UnaryOperator.Minus, FileName, ErrorToken)).Create()
                            .ToArray();

                    case AssignOperator.DotEquals:
                        return new BlockBuilder(BlockSpecs.SetVariableTo, context).AddParam(VariableName).AddParam(
                            new CompoundExpression(CompoundOperator.Concat,
                                new LookupExpression(globalVarDeclaration, FileName, ErrorToken), Value, FileName,
                                ErrorToken)).Create().ToArray();

                    case AssignOperator.PlusPlus:
                        return new BlockBuilder(BlockSpecs.ChangeVarBy, context).AddParam(VariableName).AddParam(1)
                            .Create().ToArray();

                    case AssignOperator.MinusMinus:
                        return new BlockBuilder(BlockSpecs.ChangeVarBy, context).AddParam(VariableName).AddParam(-1)
                            .Create().ToArray();

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Try as any readonly type
            if (variable is ParamDeclaration || variable is ConstDeclaration)
            {
                context.ErrorList.Add(new CompilerError($"Value '{VariableName}' is read-only", ErrorType.ValueIsReadonly,
                    ErrorToken, FileName));
                return new Block[0];
            }

            // Fail
            context.ErrorList.Add(new CompilerError($"'{VariableName}' is not a variable", ErrorType.ImproperUsage,
                ErrorToken, FileName));
            return new Block[0];
        }

        private void CheckOperator(DataType type, TranslationContext context)
        {
            switch (Operator)
            {
                case AssignOperator.Equals:
                    return;

                case AssignOperator.AddEquals:
                case AssignOperator.MinusEquals:
                case AssignOperator.PlusPlus:
                case AssignOperator.MinusMinus:

                    if (!DataType.Number.IsCompatible(type))
                        context.ErrorList.Add(new CompilerError(
                            $"Cannot use operator '{Operator}' on a value of type '{type}'",
                            ErrorType.TypeMismatch, ErrorToken, FileName));
                    return;

                case AssignOperator.DotEquals:

                    if (!DataType.String.IsCompatible(type))
                        context.ErrorList.Add(new CompilerError(
                            $"Cannot use operator '{Operator}' on a value of type '{type}'",
                            ErrorType.TypeMismatch, ErrorToken, FileName));
                    return;

                default:
                    throw new ArgumentOutOfRangeException(nameof(Operator), Operator, null);
            }
        }

        #endregion
    }
}