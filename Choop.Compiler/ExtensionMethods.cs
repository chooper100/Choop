﻿using System;
using Choop.Compiler.ChoopModel;

namespace Choop.Compiler
{
    /// <summary>
    /// Provides various extension methods for the Choop compiler unit.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Converts a <see cref="ChoopParser.TypeSpecifierContext"/> instance into a <see cref="DataType"/> value.
        /// </summary>
        /// <param name="typeSpecifierAst">The <see cref="ChoopParser.TypeSpecifierContext"/> instance to convert. Can be null.</param>
        /// <returns>The equivalent <see cref="DataType"/> for the <see cref="ChoopParser.TypeSpecifierContext"/> instance.</returns>
        internal static DataType ToDataType(this ChoopParser.TypeSpecifierContext typeSpecifierAst)
        {
            if (typeSpecifierAst == null)
                return DataType.Object;
            
            switch (typeSpecifierAst.start.Type)
            {
                case ChoopParser.TypeNum:
                    return DataType.Number;
                case ChoopParser.TypeString:
                    return DataType.String;
                case ChoopParser.TypeBool:
                    return DataType.Boolean;
                default:
                    throw new ArgumentException("Unknown type", nameof(typeSpecifierAst));
            }
        }

        /// <summary>
        /// Converts a <see cref="ChoopParser.AssignOpContext"/> instance into a <see cref="AssignOperator"/> value.
        /// </summary>
        /// <param name="assignOpAst">The <see cref="ChoopParser.AssignOpContext"/> instance to convert.</param>
        /// <returns>The equivalent <see cref="AssignOperator"/> for the <see cref="ChoopParser.AssignOpContext"/> instance.</returns>
        internal static AssignOperator ToAssignOperator(this ChoopParser.AssignOpContext assignOpAst)
        {
            if (assignOpAst == null) throw new ArgumentNullException(nameof(assignOpAst));

            switch (assignOpAst.start.Type)
            {
                case ChoopParser.Assign:
                    return AssignOperator.Equals;
                case ChoopParser.AssignAdd:
                    return AssignOperator.AddEquals;
                case ChoopParser.AssignSub:
                    return AssignOperator.MinusEquals;
                case ChoopParser.AssignConcat:
                    return AssignOperator.DotEquals;
                default:
                    throw new ArgumentException("Unknown type", nameof(assignOpAst));
            }
        }
    }
}