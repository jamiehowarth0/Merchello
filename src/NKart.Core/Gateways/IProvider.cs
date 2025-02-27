﻿using NKart.Core.Models;

namespace NKart.Core.Gateways
{
    using System;
    using System.Collections.Generic;

    using NKart.Core.Models;

    /// <summary>
    /// Marker interface for Providers 
    /// </summary>
    public interface IProvider : IHasExtendedData
    {
        /// <summary>
        /// Gets the unique key for the gateway.  
        /// Used by Merchello in the GatewayProvider's installation/configuration
        /// </summary>
        Guid Key { get; }

        /// <summary>
        /// Returns a collection of all possible gateway methods associated with this provider
        /// </summary>
        /// <returns>
        /// A collection of <see cref="IGatewayResource"/>
        /// </returns>
        IEnumerable<IGatewayResource> ListResourcesOffered();
    }
}