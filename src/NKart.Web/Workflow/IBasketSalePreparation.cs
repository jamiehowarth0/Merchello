﻿using NKart.Web.Discounts.Coupons;
using NKart.Web.Models.ContentEditing;

namespace NKart.Web.Workflow
{
    using NKart.Core.Models;
    using NKart.Core.Sales;
    using NKart.Web.Discounts.Coupons;
    using NKart.Web.Models.ContentEditing;

    /// <summary>
    /// Marker interface for <see cref="IBasket"/> based checkouts
    /// </summary>
    public interface IBasketSalePreparation : ISalePreparationBase
    {

        /// <summary>
        /// Saves the sale note
        /// </summary>
        /// <param name="note">The shipping <see cref="INote"/></param>
        void SaveNote(NoteDisplay note);

        /// <summary>
        /// Gets the sale note
        /// </summary>
        /// <remarks>Returns the <see cref="INote"/></remarks>
        /// <returns>A <see cref="INote"/></returns>
        NoteDisplay GetNote();

        /// <summary>
        /// Attempts to add a coupon offer to the sale.
        /// </summary>
        /// <param name="offerCode">
        /// The offer code.
        /// </param>
        /// <returns>
        /// The <see cref="ICouponRedemptionResult"/>.
        /// </returns>
        ICouponRedemptionResult RedeemCouponOffer(string offerCode);
    }
}