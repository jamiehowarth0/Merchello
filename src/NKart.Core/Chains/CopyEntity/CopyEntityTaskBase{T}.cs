﻿using NKart.Core.Models.EntityBase;
using NKart.Core.Services;

namespace NKart.Core.Chains.CopyEntity
{
    using NKart.Core.Models.EntityBase;
    using NKart.Core.Services;

    using Umbraco.Core;

    /// <summary>
    /// The copy entity task base.
    /// </summary>
    /// <typeparam name="T">
    /// The type of <see cref="IEntity"/>
    /// </typeparam>
    public abstract class CopyEntityTaskBase<T> : AttemptChainTaskBase<T>
    {
        /// <summary>
        /// The _merchello context.
        /// </summary>
        private readonly IMerchelloContext _merchelloContext;

        /// <summary>
        /// The original entity
        /// </summary>
        private readonly T _original;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyEntityTaskBase{T}"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        /// <param name="original">
        /// The original.
        /// </param>
        protected CopyEntityTaskBase(IMerchelloContext merchelloContext, T original)
        {
            Ensure.ParameterNotNull(merchelloContext, "merchelloContext");
            Ensure.ParameterCondition(original is IEntity, "orginal");
            _merchelloContext = merchelloContext;
            _original = original;
        }

        /// <summary>
        /// Gets the original entity
        /// </summary>
        protected T Original
        {
            get
            {
                return _original;
            }
        }

        /// <summary>
        /// Gets the <see cref="IServiceContext"/>.
        /// </summary>
        protected IServiceContext Services
        {
            get
            {
                return _merchelloContext.Services;
            }
        }
    }
}