﻿using System.Collections.ObjectModel;
using Antlr4.Runtime;

namespace Choop.Compiler.Antlr
{
    sealed partial class ChoopParser
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="ChoopParser"/> class. 
        /// </summary>
        /// <param name="input">The token stream to parse.</param>
        /// <param name="errorCollection">The collection to record compiler errors to.</param>
        /// <param name="fileName">The name of the file being parsed.</param>
        public ChoopParser(ITokenStream input, Collection<CompilerError> errorCollection, string fileName) : this(input)
        {
            // Set error listener
            RemoveErrorListeners();
            AddErrorListener(new ChoopParserErrorListener(errorCollection, fileName));
        }

        #endregion
    }
}