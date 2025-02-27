﻿namespace NKart.Tests.PaymentProviders.PayPal.TestHelpers
{
    using NKart.Providers.Payment.PayPal.Services;
    using NKart.Tests.Base.TestHelpers;

    using NUnit.Framework;

    /// <summary>
    /// A base class for testing PayPal payments.
    /// </summary>
    public abstract class PayPalTestBase : MerchelloAllInTestBase
    {
        /// <summary>
        /// Gets the PayPal API service.
        /// </summary>
        protected IPayPalApiService PayPalApiService
        {
            get; private set;
        }

        /// <summary>
        /// The test fixture setup.
        /// </summary>
        [TestFixtureSetUp]
        public virtual void TestFixtureSetup()
        {
            var settings = TestHelper.GetPayPalProviderSettings();

            this.PayPalApiService = new PayPalApiService(settings);
        }
    }
}