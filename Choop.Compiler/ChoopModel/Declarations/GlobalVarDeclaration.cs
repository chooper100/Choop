﻿using Antlr4.Runtime;
using Choop.Compiler.BlockModel;
using Choop.Compiler.ChoopModel.Expressions;
using Choop.Compiler.Helpers;

namespace Choop.Compiler.ChoopModel.Declarations
{
    /// <summary>
    /// Represents a global variable declaration.
    /// </summary>
    public class GlobalVarDeclaration : IVarDeclaration<TerminalExpression>, ICompilable<Variable>
    {
        #region Properties

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the data stored in the variable.
        /// </summary>
        public DataType Type { get; }

        /// <summary>
        /// Gets the initial value stored in the variable.
        /// </summary>
        public TerminalExpression Value { get; }

        /// <summary>
        /// Gets the token to report any compiler errors to.
        /// </summary>
        public IToken ErrorToken { get; }

        /// <summary>
        /// Gets the file name where the grammar structure was found.
        /// </summary>
        public string FileName { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="GlobalVarDeclaration"/> class.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="type">The data type of the variable.</param>
        /// <param name="value">The initial value of the variable.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="errorToken">The token to report any compiler errors to.</param>
        public GlobalVarDeclaration(string name, DataType type, TerminalExpression value, string fileName,
            IToken errorToken)
        {
            Name = name;
            Type = type;
            Value = value;
            FileName = fileName;
            ErrorToken = errorToken;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the translated code for the grammar structure.
        /// </summary>
        /// <returns>The translated code for the grammar structure.</returns>
        public Variable Translate(TranslationContext context)
        {
            if (Value == null)
                return new Variable(Name, Type.GetDefault());

            if (!Type.IsCompatible(Value.Type))
                context.ErrorList.Add(new CompilerError(
                    $"Expected value of type '{Type}' but instead found value of type '{Value.Type}'",
                    ErrorType.TypeMismatch, Value.ErrorToken, Value.FileName));

            return new Variable(Name, Value.Value);
        }

        #endregion
    }
}