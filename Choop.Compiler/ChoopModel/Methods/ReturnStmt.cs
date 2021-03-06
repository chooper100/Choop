﻿using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Choop.Compiler.BlockModel;
using Choop.Compiler.ChoopModel.Expressions;
using Choop.Compiler.Helpers;

namespace Choop.Compiler.ChoopModel.Methods
{
    /// <summary>
    /// Represents a return statement.
    /// </summary>
    public class ReturnStmt : IStatement
    {
        #region Properties

        /// <summary>
        /// Gets the expression for the value to be returned.
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

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="ReturnStmt"/> class.
        /// </summary>
        /// <param name="value">The value to be returned.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="errorToken">The token to report any compiler errors to.</param>
        public ReturnStmt(IExpression value, string fileName, IToken errorToken)
        {
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
        public IEnumerable<Block> Translate(TranslationContext context)
        {
            if (Value == null)
                return context.CurrentScope.CreateCleanUp().Concat(new[] {new Block(BlockSpecs.Stop, "this script")});

            return new BlockBuilder(BlockSpecs.SetVariableTo, context)
                .AddParam(((MethodDeclaration) context.CurrentScope.Method).GetReturnVariableName()).AddParam(Value).Create()
                .Concat(context.CurrentScope.CreateCleanUp())
                .Concat(new[] {new Block(BlockSpecs.Stop, "this script")});
        }

        #endregion
    }
}