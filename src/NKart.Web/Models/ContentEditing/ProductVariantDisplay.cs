﻿namespace NKart.Web.Models.ContentEditing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The product variant display.
    /// </summary>
    public class ProductVariantDisplay : ProductDisplayBase
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        public Guid ProductKey { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        public IEnumerable<ProductAttributeDisplay> Attributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether master.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}