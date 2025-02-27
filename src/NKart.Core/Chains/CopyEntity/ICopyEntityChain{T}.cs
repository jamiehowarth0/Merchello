﻿using NKart.Core.Models.EntityBase;

namespace NKart.Core.Chains.CopyEntity
{
    using NKart.Core.Models.EntityBase;

    using Umbraco.Core;

    /// <summary>
    /// The CopyEntityChain interface.
    /// </summary>
    /// <typeparam name="T">
    /// The type of <see cref="Entity"/>
    /// </typeparam>
    public interface ICopyEntityChain<T>
    {
        /// <summary>
        /// The copy.
        /// </summary>
        /// <returns>
        /// The <see cref="Attempt"/>.
        /// </returns>
        Attempt<T> Copy();
    }
}