﻿using NKart.FastTrack.Models;

namespace NKart.FastTrack.Controllers
{
    using System.Web.Mvc;

    using NKart.FastTrack.Models;
    using NKart.Web.Controllers;
    using NKart.Web.Models.Ui;

    using Umbraco.Core;
    using Umbraco.Web.Mvc;

    /// <summary>
    /// A controller responsible quoting shipment rate quotes.
    /// </summary>
    [PluginController("FastTrack")]
    public class CheckoutShipRateQuoteController : CheckoutShipRateQuoteControllerBase<FastTrackShipRateQuoteModel>
    {
        /// <summary>
        /// The handle ship rate quote save success.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        protected override ActionResult HandleShipRateQuoteSaveSuccess(FastTrackShipRateQuoteModel model)
        {
            return !model.SuccessRedirectUrl.IsNullOrWhiteSpace() ? 
                this.Redirect(model.SuccessRedirectUrl) 
                : base.HandleShipRateQuoteSaveSuccess(model);
        }
    }
}
