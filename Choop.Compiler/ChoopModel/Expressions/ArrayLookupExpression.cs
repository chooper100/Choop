﻿using Antlr4.Runtime;
using Choop.Compiler.BlockModel;
using Choop.Compiler.Helpers;

namespace Choop.Compiler.ChoopModel.Expressions
{
    /// <summary>
    /// Represents an array value lookup expression.
    /// </summary>
    public class ArrayLookupExpression : LookupExpression
    {
        #region Properties

        /// <summary>
        /// Gets the expression for the index of the item being looked up.
        /// </summary>
        public IExpression Index { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="ArrayLookupExpression"/> class.
        /// </summary>
        /// <param name="identifierName">The name of the array being looked up.</param>
        /// <param name="index">The expression for the index of the item being looked up.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="errorToken">The token to report any compiler errors to.</param>
        public ArrayLookupExpression(string identifierName, IExpression index, string fileName, IToken errorToken) : base(identifierName, fileName, errorToken)
        {
            Index = index;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the translated code for the grammar structure.
        /// </summary>
        /// <returns>The translated code for the grammar structure.</returns>
        public override object Translate(TranslationContext context)
        {
            return new Block(BlockSpecs.GetItemOfList, Index.Translate(context), IdentifierName);
        }

        #endregion
    }
}