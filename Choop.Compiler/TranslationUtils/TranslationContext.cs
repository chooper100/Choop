﻿using System.Collections.ObjectModel;
using Choop.Compiler.ChoopModel;

namespace Choop.Compiler.TranslationUtils
{
    /// <summary>
    /// Provides properties and methods to use whilst translating ChoopModel code.
    /// </summary>
    public class TranslationContext
    {
        #region Properties

        /// <summary>
        /// Gets the current active scope.
        /// </summary>
        public Scope CurrentScope { get; }

        /// <summary>
        /// Gets the current sprite being translated.
        /// </summary>
        public SpriteDeclaration CurrentSprite { get; }

        /// <summary>
        /// Gets the project being translated.
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// Gets the collection of compiler errors.
        /// </summary>
        public Collection<CompilerError> ErrorList { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="TranslationContext"/> class.
        /// </summary>
        /// <param name="errorList">The collection of compiler errors.</param>
        public TranslationContext(Collection<CompilerError> errorList)
        {
            CurrentScope = null;
            CurrentSprite = null;
            Project = null;
            ErrorList = errorList;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TranslationContext"/> class.
        /// </summary>
        /// <param name="project">The project being translated.</param>
        /// <param name="oldContext">The previous translation context.</param>
        public TranslationContext(Project project, TranslationContext oldContext)
        {
            CurrentScope = null;
            CurrentSprite = null;
            Project = project;
            ErrorList = oldContext.ErrorList;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TranslationContext"/> class.
        /// </summary>
        /// <param name="sprite">The current sprite being translated.</param>
        /// <param name="oldContext">The previous translation context.</param>
        public TranslationContext(SpriteDeclaration sprite, TranslationContext oldContext)
        {
            CurrentScope = null;
            CurrentSprite = sprite;
            Project = oldContext.Project;
            ErrorList = oldContext.ErrorList;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TranslationContext"/> class.
        /// </summary>
        /// <param name="scope">The current scope.</param>
        /// <param name="oldContext">The previous translation context.</param>
        public TranslationContext(Scope scope, TranslationContext oldContext)
        {
            CurrentScope = scope;
            CurrentSprite = oldContext.CurrentSprite;
            Project = oldContext.Project;
            ErrorList = oldContext.ErrorList;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds a declaration (excluding methods) with the specified name
        /// </summary>
        /// <param name="name">The name of the declaration to search for.</param>
        /// <returns>The declaration with the specified name or null if not found.</returns>
        public IDeclaration GetDeclaration(string name)
        {
            // Project
            if (Project == null) return null;

            IDeclaration superGlobalDeclaration = Project.GetDeclaration(name);
            if (superGlobalDeclaration != null) return superGlobalDeclaration;

            // Sprite
            if (CurrentSprite == null) return null;

            ITypedDeclaration globalDeclaration = CurrentSprite.GetDeclaration(name);
            if (globalDeclaration != null) return globalDeclaration;

            // Methods
            if (CurrentScope == null) return null;

            // Method params
            ParamDeclaration methodParam = (CurrentScope.Method as MethodDeclaration)?.FindParam(name);
            if (methodParam != null) return methodParam;

            // Scoped variables
            return CurrentScope.Search(name);
        }

        #endregion
    }
}