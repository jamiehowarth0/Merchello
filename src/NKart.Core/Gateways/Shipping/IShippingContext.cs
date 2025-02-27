﻿using NKart.Core.Models;

namespace NKart.Core.Gateways.Shipping
{
    using System.Collections.Generic;
    using NKart.Core.Models;

    /// <summary>
    /// Defines a shipping context
    /// </summary>
    public interface IShippingContext : IGatewayProviderTypedContextBase<ShippingGatewayProviderBase>
    {
        /// <summary>
        /// Returns a collection of all <see cref="IShipmentRateQuote"/> that are available for the <see cref="IShipment"/>
        /// </summary>
        /// <param name="shipment">The <see cref="IShipment"/> to quote</param>
        /// <param name="tryGetCached">
        /// If set true the strategy will try to get a quote from cache
        /// </param>
        /// <returns>A collection of <see cref="IShipmentRateQuote"/></returns>
        IEnumerable<IShipmentRateQuote> GetShipRateQuotesForShipment(IShipment shipment, bool tryGetCached = true);

        /// <summary>
        /// Returns a list of all countries that can be assigned to a shipment
        /// </summary>
        /// <returns>A collection of all <see cref="ICountry"/> that have been identified as allowable shipping destinations</returns>
        IEnumerable<ICountry> GetAllowedShipmentDestinationCountries();

        /// <summary>
        /// Gets a collection of <see cref="ShippingGatewayProviderBase"/> by ship country
        /// </summary>
        /// <param name="shipCountry">The <see cref="IShipCountry"/></param>
        /// <returns>A collection of <see cref="IShippingGatewayProvider"/>s associated with the ship country</returns>
        IEnumerable<ShippingGatewayProviderBase> GetGatewayProvidersByShipCountry(IShipCountry shipCountry);
    }
}