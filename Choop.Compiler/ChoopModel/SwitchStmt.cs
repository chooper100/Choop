﻿using System;
using System.Collections.ObjectModel;
using Antlr4.Runtime;
using Choop.Compiler.BlockModel;
using Choop.Compiler.TranslationUtils;

namespace Choop.Compiler.ChoopModel
{
    /// <summary>
    /// Represents a switch case statement.
    /// </summary>
    public class SwitchStmt : IStatement
    {
        #region Properties

        /// <summary>
        /// Gets the expression for the variable to switch on.
        /// </summary>
        public IExpression Variable { get; }

        /// <summary>
        /// Gets the collection of code blocks within the switch statement.
        /// </summary>
        /// <remarks>The code blocks should be in order, with the primary case first and the default case (if specified) last.</remarks>
        public Collection<ConditionalBlock> Blocks { get; } = new Collection<ConditionalBlock>();

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
        /// Creates a new instance of the <see cref="SwitchStmt"/> class.
        /// </summary>
        /// <param name="variable">The expression for the switch variable.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="errorToken">The token to report any compiler errors to.</param>
        public SwitchStmt(IExpression variable, string fileName, IToken errorToken)
        {
            Variable = variable;
            FileName = fileName;
            ErrorToken = errorToken;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the translated code for the grammar structure.
        /// </summary>
        /// <returns>The translated code for the grammar structure.</returns>
        public Block[] Translate(TranslationContext context)
        {
            return BuildIfElse(context, 0);
        }

        /// <summary>
        /// Recursively builds the tranlsated if-else statement.
        /// </summary>
        /// <param name="context">The context of the translation.</param>
        /// <param name="element">The current block.</param>
        /// <returns>The translated if-else statement.</returns>
        private Block[] BuildIfElse(TranslationContext context, int element)
        {
            if (element == Blocks.Count)
                throw new IndexOutOfRangeException("Element is out of range");

            if (element + 1 != Blocks.Count)
                return new[]
                {
                    new Block(
                        BlockSpecs.IfThenElse,
                        BuildCondition(Blocks[element].Conditions).Translate(context),
                        Blocks[element].Translate(context),
                        BuildIfElse(context, element + 1)
                    )
                };

            if (Blocks[element].IsDefault)
                return Blocks[element].Translate(context);

            return new[]
            {
                new Block(
                    BlockSpecs.IfThen,
                    BuildCondition(Blocks[element].Conditions).Translate(context),
                    Blocks[element].Translate(context)
                )
            };
        }

        /// <summary>
        /// Recursively builds the condition for a case block.
        /// </summary>
        /// <param name="conditions">The collection of input conditions.</param>
        /// <param name="index">The current index.</param>
        /// <returns>The expression for the condition combining all input conditions.</returns>
        private IExpression BuildCondition(Collection<IExpression> conditions, int index = 0) =>
            index == conditions.Count - 1
                ? conditions[index]
                : new CompoundExpression(CompoundOperator.And, conditions[index],
                    BuildCondition(conditions, index + 1), FileName, ErrorToken);

        #endregion
    }
}