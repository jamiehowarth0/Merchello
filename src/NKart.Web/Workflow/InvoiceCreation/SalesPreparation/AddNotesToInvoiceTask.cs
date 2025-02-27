﻿namespace NKart.Web.Workflow.InvoiceCreation.SalesPreparation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NKart.Core;
    using NKart.Core.Chains.InvoiceCreation.SalesPreparation;
    using NKart.Core.Models;
    using NKart.Core.Models.TypeFields;
    using NKart.Core.Sales;
    using NKart.Web.Models.ContentEditing;

    using Umbraco.Core;

    /// <summary>
    /// Represents a task responsible for adding a note collected during a checkout process to the
    /// invoice.
    /// </summary>
    [Obsolete("Superseded by CheckoutManager.AddNotesToInvoiceTask")]
    internal class AddNotesToInvoiceTask : InvoiceCreationAttemptChainTaskBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddNotesToInvoiceTask"/> class. 
        /// </summary>
        /// <param name="salePreparation">
        /// The sale preparation.
        /// </param>
        public AddNotesToInvoiceTask(SalePreparationBase salePreparation)
            : base(salePreparation)
        {            
        }

        /// <summary>
        /// Adds billing information to the invoice
        /// </summary>
        /// <param name="value">
        /// The <see cref="IInvoice"/>
        /// </param>
        /// <returns>
        /// The <see cref="Attempt"/>.
        /// </returns>
        public override Attempt<IInvoice> PerformTask(IInvoice value)
        {
            var noteDisplay = this.SalePreparation.Customer.ExtendedData.GetNote();

            if (noteDisplay == null) return Attempt<IInvoice>.Succeed(value);

            value.Notes = new List<INote>()
                              {
                                 noteDisplay.ToNote()
                              };

            return Attempt<IInvoice>.Succeed(value);            
        }
    }
}