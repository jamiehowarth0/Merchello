﻿using System;
using System.Collections.Generic;
using NKart.Core.Models;
using Umbraco.Core.Persistence.Repositories;

namespace NKart.Core.Persistence.Repositories
{
    /// <summary>
    /// Defines a GatewayProviderRepository
    /// </summary>
    internal interface IGatewayProviderRepository : IRepositoryQueryable<Guid, IGatewayProviderSettings>
    {
        /// <summary>
        /// Returns a list of GatewayProviders associated with a ship country
        /// </summary>
        /// <param name="shipCountryKey"></param>
        /// <returns></returns>
        IEnumerable<IGatewayProviderSettings> GetGatewayProvidersByShipCountryKey(Guid shipCountryKey);
    }
}