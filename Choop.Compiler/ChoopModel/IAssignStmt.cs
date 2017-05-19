﻿using Choop.Compiler.BlockModel;

namespace Choop.Compiler.ChoopModel
{
    /// <summary>
    /// Represents a generic assignment.
    /// </summary>
    public interface IAssignStmt : ICompilable<Block[]>
    {
        #region Properties
        /// <summary>
        /// Gets the name of the item being assigned.
        /// </summary>
        string ItemName { get; }

        /// <summary>
        /// Gets the operator used for the assignment.
        /// </summary>
        AssignOperator Operator { get; }
        
        /// <summary>
        /// Gets the input to the assignment operator. (Null for increment and decrement)
        /// </summary>
        IExpression Value { get; }
        #endregion
    }
}
