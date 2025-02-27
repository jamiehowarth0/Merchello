﻿using NKart.Core.Configuration;
using NKart.Core.Configuration.Outline;

namespace NKart.Core.Models.TypeFields
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using NKart.Core.Configuration;
    using NKart.Core.Configuration.Outline;

    /// <summary>
    /// The type field mapper.
    /// </summary>
    /// <typeparam name="T">
    /// The type of TypeField
    /// </typeparam>
    public abstract class TypeFieldMapper<T> : TypeFieldMapperBase, ITypeFieldMapper<T>
    {

        /// <summary>
        /// The cached type fields.
        /// </summary>
        protected static readonly ConcurrentDictionary<T,ITypeField> CachedTypeFields = new ConcurrentDictionary<T, ITypeField>();

        /// <summary>
        /// Gets the custom type fields.
        /// </summary>
        public abstract IEnumerable<ITypeField> CustomTypeFields { get; }

        /// <summary>
        /// Builds the TypeField cache for the respective type
        /// </summary>
        internal abstract void BuildCache();

        /// <summary>
        /// Returns the respective enum value for a given <see cref="TypeField"/> TypeKey
        /// </summary>
        public T GetTypeField(Guid key)
        {
            return this.CustomTypeFields.Any(x => x.TypeKey == key) ? 
                CachedTypeFields.Keys.FirstOrDefault(x => CachedTypeFields[x].TypeKey == Guid.Empty) : 
                CachedTypeFields.Keys.FirstOrDefault(x => CachedTypeFields[x].TypeKey == key);
        }

        /// <summary>
        /// Returns a typefield for a given key
        /// </summary>
        public ITypeField GetTypeField(T key)
        {
            ITypeField typeField;
            return CachedTypeFields.TryGetValue(key, out typeField) ?
                typeField : NotFound;
        }

        /// <summary>
        /// Adds a key value pair to the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="typeField"></param>
        protected void AddUpdateCache(T key, ITypeField typeField)
        {
            CachedTypeFields.AddOrUpdate(key, typeField, (x, y) => typeField);
        }

        #region Custom
        
        /// <summary>
        /// A collection of custom typefields from the configuration file
        /// </summary>
        protected static TypeFieldDefinitionsElement Fields { get { return MerchelloConfiguration.Current.Section.TypeFields; } }

        /// <summary>
        /// Creates a <see cref="TypeField"/> from a configuration file element
        /// </summary>
        protected static ITypeField GetTypeField(TypeFieldElement element)
        {
            return element == null
                       ? NotFound
                       : new TypeField(element);
        }

        /// <summary>
        /// The get custom.
        /// </summary>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <returns>
        /// The <see cref="ITypeField"/>.
        /// </returns>
        protected abstract ITypeField GetCustom(string alias);

        /// <summary>
        /// Returns a custom <see cref="ITypeField"/> from the Merchello configuration section
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public ITypeField Custom(string alias)
        {
            return GetCustom(alias);
        }

        #endregion

    }
}
