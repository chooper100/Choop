﻿using System;
using System.Collections.ObjectModel;
using Choop.Compiler.BlockModel;

namespace Choop.Compiler.ChoopModel
{
    /// <summary>
    /// Represents a repeat loop.
    /// </summary>
    public class RepeatLoop : IHasBody, IStatement
    {
        #region Properties
        /// <summary>
        /// Gets whether the repeat loop should be inlined.
        /// </summary>
        public bool Inline { get; }

        /// <summary>
        /// Gets the expression for the number of iterations to perform.
        /// </summary>
        public IExpression Iterations { get; }

        /// <summary>
        /// Gets the collection of statements within the loop.
        /// </summary>
        public Collection<IStatement> Statements { get; } = new Collection<IStatement>();
        #endregion
        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="RepeatLoop"/> class.
        /// </summary>
        /// <param name="inline">Whether to inline the repeat loop.</param>
        /// <param name="iterations">The expression for the number of iterations to be run.</param>
        public RepeatLoop(bool inline, IExpression iterations)
        {
            Inline = inline;
            Iterations = iterations;
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
