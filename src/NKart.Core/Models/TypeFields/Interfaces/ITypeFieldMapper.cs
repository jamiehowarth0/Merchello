﻿using System;

namespace NKart.Core.Models.TypeFields
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a TypeFieldMapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITypeFieldMapper<T> : ICustomTypeField
    {
        /// <summary>
        /// Gets the collection of custom type fields.
        /// </summary>
        IEnumerable<ITypeField> CustomTypeFields { get; }

        /// <summary>
        /// Returns the enumerated value from the TypeKey
        /// </summary>
        /// <param name="key"><see cref="Guid"/></param>        
        T GetTypeField(Guid key);

        /// <summary>
        /// Returns a <see cref="ITypeField"/> from an enumerated value
        /// </summary>        
        ITypeField GetTypeField(T key);

    }
}