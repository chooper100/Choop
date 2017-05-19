﻿namespace Choop.Compiler.ChoopModel
{
    /// <summary>
    /// Represents an array or list declaration.
    /// </summary>
    public interface IArrayDeclaration : IVarDeclaration<IExpression[]>
    {
        #region Properties
        /// <summary>
        /// Gets the length of the array.
        /// </summary>
        int Length { get; }
        #endregion
    }
}
