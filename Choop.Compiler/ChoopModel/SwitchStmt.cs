﻿using System;
using System.Collections.ObjectModel;
using Choop.Compiler.BlockModel;

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
        #endregion
        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="SwitchStmt"/> class.
        /// </summary>
        /// <param name="variable">The expression for the switch variable.</param>
        public SwitchStmt(IExpression variable)
        {
            Variable = variable;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Gets the translated code for the grammar structure.
        /// </summary>
        /// <returns>The translated code for the grammar structure.</returns>
        public Block[] Translate()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}