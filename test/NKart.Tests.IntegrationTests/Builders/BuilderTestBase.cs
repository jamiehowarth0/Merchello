﻿using NKart.Core;
using NKart.Core.Cache;
using NKart.Core.Gateways.Shipping.FixedRate;
using NKart.Core.Models;
using NKart.Core.Persistence.UnitOfWork;
using NKart.Core.Sales;
using NKart.Core.Services;
using NKart.Tests.Base.DataMakers;
using NKart.Web.Workflow;
using NUnit.Framework;
using Umbraco.Core;
using System.Linq;
using NKart.Web;
using NKart.Tests.IntegrationTests.TestHelpers;

namespace NKart.Tests.IntegrationTests.Builders
{
    using NKart.Core.Logging;

    public class BuilderTestBase : DatabaseIntegrationTestBase
    {
        protected IItemCache ItemCache;
        protected ICustomerBase Customer;
        protected SalePreparationBase SalePreparationMock;
        protected IAddress BillingAddress;
        protected IBasket Basket;
        protected const int ProductCount = 5;
        protected const decimal WeightPerProduct = 3;
        protected const decimal PricePerProduct = 5;
        
        [TestFixtureSetUp]
        public override void FixtureSetup()
        {
            base.FixtureSetup();

       
            PreTestDataWorker.DeleteAllShipCountries();

            var defaultCatalog = PreTestDataWorker.WarehouseService.GetDefaultWarehouse().WarehouseCatalogs.FirstOrDefault();
            if (defaultCatalog == null) Assert.Ignore("Default WarehouseCatalog is null");

            var us = MerchelloContext.Current.Services.StoreSettingService.GetCountryByCode("US");
            var usCountry = new ShipCountry(defaultCatalog.Key, us);
            ((ServiceContext)MerchelloContext.Current.Services).ShipCountryService.Save(usCountry);

            var key = Core.Constants.ProviderKeys.Shipping.FixedRateShippingProviderKey;
            var rateTableProvider = (FixedRateShippingGatewayProvider)MerchelloContext.Current.Gateways.Shipping.GetProviderByKey(key);
            rateTableProvider.DeleteAllActiveShipMethods(usCountry);

            #region Add and configure 3 rate table shipmethods

            var gwshipMethod1 = (FixedRateShippingGatewayMethod)rateTableProvider.CreateShipMethod(FixedRateShippingGatewayMethod.QuoteType.VaryByPrice, usCountry, "Ground (Vary by Price)");
            gwshipMethod1.RateTable.AddRow(0, 10, 25);
            gwshipMethod1.RateTable.AddRow(10, 15, 30);
            gwshipMethod1.RateTable.AddRow(15, 25, 35);
            gwshipMethod1.RateTable.AddRow(25, 60, 40); // total price should be 50M so we should hit this tier
            gwshipMethod1.RateTable.AddRow(25, 10000, 50);
            rateTableProvider.SaveShippingGatewayMethod(gwshipMethod1);

            #endregion
        }

        [SetUp]
        public virtual void Init()
        {
            Customer = PreTestDataWorker.MakeExistingAnonymousCustomer();
            Basket = Web.Workflow.Basket.GetBasket(MerchelloContext.Current, Customer);

            var odd = true;
            for (var i = 0; i < ProductCount; i++)
            {
                
                var product = PreTestDataWorker.MakeExistingProduct(true, WeightPerProduct, PricePerProduct);
                product.AddToCatalogInventory(PreTestDataWorker.WarehouseCatalog);
                product.CatalogInventories.First().Count = 10;
                product.TrackInventory = true;
                PreTestDataWorker.ProductService.Save(product);
                Basket.AddItem(product, 2);

                odd = !odd;
            }

            BillingAddress = new Address()
            {
                Name = "Out there",
                Address1 = "some street",
                Locality = "some city",
                Region = "ST",
                PostalCode = "98225",
                CountryCode = "US"
            };

            var origin = new Address()
            {
                Name = "Somewhere",
                Address1 = "origin street",
                Locality = "origin city",
                Region = "ST",
                PostalCode = "98225",
                CountryCode = "US"
            };



            PreTestDataWorker.DeleteAllItemCaches();
            PreTestDataWorker.DeleteAllInvoices();


            Customer.ExtendedData.AddAddress(BillingAddress, AddressType.Billing);
            ItemCache = new Core.Models.ItemCache(Customer.Key, ItemCacheType.Checkout);

            PreTestDataWorker.ItemCacheService.Save(ItemCache);

            foreach (var item in Basket.Items)
            {
                ItemCache.AddItem(item.AsLineItemOf<ItemCacheLineItem>());
            }


            // setup the checkout
            SalePreparationMock = new SalePreparationMock(MerchelloContext.Current, ItemCache, Customer);

            // add the shipment rate quote
            var shipment = Basket.PackageBasket(MerchelloContext.Current, BillingAddress).First();
            var shipRateQuote = shipment.ShipmentRateQuotes(MerchelloContext.Current).FirstOrDefault();
            
            //_checkoutMock.ItemCache.Items.Add(shipRateQuote.AsLineItemOf<InvoiceLineItem>());
            SalePreparationMock.SaveShipmentRateQuote(shipRateQuote);
        }

    }
}