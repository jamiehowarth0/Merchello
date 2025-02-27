﻿using NKart.Core.Models;
using NKart.Core.Models.Rdbms;
using NKart.Core.Models.TypeFields;

namespace NKart.Core.Persistence.Migrations.Initial
{
    using System;

    using NKart.Core.Models;
    using NKart.Core.Models.Rdbms;
    using NKart.Core.Models.TypeFields;

    using Newtonsoft.Json;

    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// Represents the initial data creation by running Insert for the base data.
    /// </summary>
    internal class BaseDataCreation
    {
        /// <summary>
        /// The database.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// The <see cref="ILogger"/>.
        /// </summary>
        private readonly ILogger _logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataCreation"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public BaseDataCreation(Database database, ILogger logger)
        {
            _database = database;
            _logger = logger;
        }

        /// <summary>
        /// Initialize the base data creation by inserting the data foundation for umbraco
        /// specific to a table
        /// </summary>
        /// <param name="tableName">Name of the table to create base data for</param>
        public void InitializeBaseData(string tableName)
        {
            _logger.Info<BaseDataCreation>(string.Format("Creating data in table {0}", tableName));

            if (tableName.Equals("merchTypeField")) CreateDbTypeFieldData();   

            if (tableName.Equals("merchInvoiceStatus")) CreateInvoiceStatusData();

            if (tableName.Equals("merchWarehouse")) CreateWarehouseData();

            if (tableName.Equals("merchOrderStatus")) CreateOrderStatusData();

            if (tableName.Equals("merchShipmentStatus")) this.CreateShipmentStatusData();
         
            if (tableName.Equals("merchGatewayProviderSettings")) CreateGatewayProviderSettingsData();

            if (tableName.Equals("merchStoreSetting")) CreateStoreSettingData();          
        }

        /// <summary>
        /// The create database type field data.
        /// </summary>
        private void CreateDbTypeFieldData()
        {
            // address
            var address = new AddressTypeField();
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = address.Shipping.TypeKey, Alias = address.Shipping.Alias, Name = address.Shipping.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now});
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = address.Billing.TypeKey, Alias = address.Billing.Alias, Name = address.Billing.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });

            // ItemCacheTypeField
            var itemcCache = new ItemCacheTypeField();
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = itemcCache.Basket.TypeKey, Alias = itemcCache.Basket.Alias, Name = itemcCache.Basket.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = itemcCache.Backoffice.TypeKey, Alias = itemcCache.Backoffice.Alias, Name = itemcCache.Backoffice.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = itemcCache.Wishlist.TypeKey, Alias = itemcCache.Wishlist.Alias, Name = itemcCache.Wishlist.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = itemcCache.Checkout.TypeKey, Alias = itemcCache.Checkout.Alias, Name = itemcCache.Checkout.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            
            var litf = new LineItemTypeField();
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = litf.Product.TypeKey, Alias = litf.Product.Alias, Name = litf.Product.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = litf.Shipping.TypeKey, Alias = litf.Shipping.Alias, Name = litf.Shipping.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = litf.Tax.TypeKey, Alias = litf.Tax.Alias, Name = litf.Tax.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = litf.Discount.TypeKey, Alias = litf.Discount.Alias, Name = litf.Discount.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });

            // PaymentMethodType
            var ptf = new PaymentMethodTypeField();
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = ptf.Cash.TypeKey, Alias = ptf.Cash.Alias, Name = ptf.Cash.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = ptf.CreditCard.TypeKey, Alias = ptf.CreditCard.Alias, Name = ptf.CreditCard.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = ptf.PurchaseOrder.TypeKey, Alias = ptf.PurchaseOrder.Alias, Name = ptf.PurchaseOrder.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = ptf.Redirect.TypeKey, Alias = ptf.Redirect.Alias, Name = ptf.Redirect.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });

            //// AppliedPaymentType
            var apf = new AppliedPaymentTypeField();
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = apf.Debit.TypeKey, Alias = apf.Debit.Alias, Name = apf.Debit.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = apf.Credit.TypeKey, Alias = apf.Credit.Alias, Name = apf.Credit.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = apf.Void.TypeKey, Alias = apf.Void.Alias, Name = apf.Void.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = apf.Denied.TypeKey, Alias = apf.Denied.Alias, Name = apf.Denied.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = apf.Refund.TypeKey, Alias = apf.Refund.Alias, Name = apf.Refund.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });

            // GatewayProviderType
            var gwp = new GatewayProviderTypeField();
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = gwp.Payment.TypeKey, Alias = gwp.Payment.Alias, Name = gwp.Payment.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = gwp.Shipping.TypeKey, Alias = gwp.Shipping.Alias, Name = gwp.Shipping.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = gwp.Taxation.TypeKey, Alias = gwp.Taxation.Alias, Name = gwp.Taxation.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = gwp.Notification.TypeKey, Alias = gwp.Notification.Alias, Name = gwp.Notification.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });

            var entity = new EntityTypeField();
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.Customer.TypeKey, Alias = entity.Customer.Alias, Name = entity.Customer.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.GatewayProvider.TypeKey, Alias = entity.GatewayProvider.Alias, Name = entity.GatewayProvider.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.Invoice.TypeKey, Alias = entity.Invoice.Alias, Name = entity.Invoice.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.ItemCache.TypeKey, Alias = entity.ItemCache.Alias, Name = entity.ItemCache.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.Order.TypeKey, Alias = entity.Order.Alias, Name = entity.Order.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.Payment.TypeKey, Alias = entity.Payment.Alias, Name = entity.Payment.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.Product.TypeKey, Alias = entity.Product.Alias, Name = entity.Product.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.ProductOption.TypeKey, Alias = entity.ProductOption.Alias, Name = entity.ProductOption.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.Shipment.TypeKey, Alias = entity.Shipment.Alias, Name = entity.Shipment.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.Warehouse.TypeKey, Alias = entity.Warehouse.Alias, Name = entity.Warehouse.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.WarehouseCatalog.TypeKey, Alias = entity.WarehouseCatalog.Alias, Name = entity.WarehouseCatalog.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
            _database.Insert("merchTypeField", "Key", new TypeFieldDto() { Key = entity.EntityCollection.TypeKey, Alias = entity.EntityCollection.Alias, Name = entity.EntityCollection.Name, UpdateDate = DateTime.Now, CreateDate = DateTime.Now });
        }

        /// <summary>
        /// Adds the invoice statuses.
        /// </summary>
        private void CreateInvoiceStatusData()
        {
            _database.Insert("merchInvoiceStatus", "Key", new InvoiceStatusDto() { Key = Constants.InvoiceStatus.Unpaid, Alias = "unpaid", Name = "Unpaid", Active = true, Reportable = true, SortOrder = 1, CreateDate = DateTime.Now, UpdateDate = DateTime.Now});
            _database.Insert("merchInvoiceStatus", "Key", new InvoiceStatusDto() { Key = Constants.InvoiceStatus.Paid, Alias = "paid", Name = "Paid", Active = true, Reportable = true, SortOrder = 2, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchInvoiceStatus", "Key", new InvoiceStatusDto() { Key = Constants.InvoiceStatus.Partial, Alias = "partial", Name = "Partial", Active = true, Reportable = true, SortOrder = 3, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchInvoiceStatus", "Key", new InvoiceStatusDto() { Key = Constants.InvoiceStatus.Cancelled, Alias = "cancelled", Name = "Cancelled", Active = true, Reportable = true, SortOrder = 4, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchInvoiceStatus", "Key", new InvoiceStatusDto() { Key = Constants.InvoiceStatus.Fraud, Alias = "fraud", Name = "Fraud", Active = true, Reportable = true, SortOrder = 5, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
        }

        /// <summary>
        /// Adds the order statuses
        /// </summary>
        private void CreateOrderStatusData()
        {
            _database.Insert("merchOrderStatus", "Key", new OrderStatusDto() { Key = Constants.OrderStatus.NotFulfilled, Alias = "notfulfilled", Name = "Not Fulfilled", Active = true, Reportable = true, SortOrder = 1, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchOrderStatus", "Key", new OrderStatusDto() { Key = Constants.OrderStatus.Open, Alias = "open", Name = "Open", Active = true, Reportable = true, SortOrder = 2, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchOrderStatus", "Key", new OrderStatusDto() { Key = Constants.OrderStatus.Fulfilled, Alias = "fulfilled", Name = "Fulfilled", Active = true, Reportable = true, SortOrder = 3, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchOrderStatus", "Key", new OrderStatusDto() { Key = Constants.OrderStatus.BackOrder, Alias = "backOrder", Name = "BackOrder", Active = true, Reportable = true, SortOrder = 4, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchOrderStatus", "Key", new OrderStatusDto() { Key = Constants.OrderStatus.Cancelled, Alias = "cancelled", Name = "Cancelled", Active = true, Reportable = true, SortOrder = 5, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });            
        }

        /// <summary>
        /// Adds the shipment statuses.
        /// </summary>
        private void CreateShipmentStatusData()
        {
            _database.Insert("merchShipmentStatus", "Key", new ShipmentStatusDto() { Key = Constants.ShipmentStatus.Quoted, Alias = "quoted", Name = "Quoted", Active = true, Reportable = true, SortOrder = 1, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchShipmentStatus", "Key", new ShipmentStatusDto() { Key = Constants.ShipmentStatus.Packaging, Alias = "packaging", Name = "Packaging", Active = true, Reportable = true, SortOrder = 2, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchShipmentStatus", "Key", new ShipmentStatusDto() { Key = Constants.ShipmentStatus.Ready, Alias = "ready", Name = "Ready", Active = true, Reportable = true, SortOrder = 3, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchShipmentStatus", "Key", new ShipmentStatusDto() { Key = Constants.ShipmentStatus.Shipped, Alias = "shipped", Name = "Shipped", Active = true, Reportable = true, SortOrder = 4, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchShipmentStatus", "Key", new ShipmentStatusDto() { Key = Constants.ShipmentStatus.Delivered, Alias = "delivered", Name = "Delivered", Active = true, Reportable = true, SortOrder = 5, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
        }

        /// <summary>
        /// Creates the warehouse data.
        /// </summary>
        private void CreateWarehouseData()
        {
            _database.Insert("merchWarehouse", "Key", new WarehouseDto() { Key = Constants.Warehouse.DefaultWarehouseKey, Name = "Default Warehouse", CountryCode = string.Empty, IsDefault = true, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchWarehouseCatalog", "Key", new WarehouseCatalogDto() { Key = Constants.Warehouse.DefaultWarehouseCatalogKey, WarehouseKey = Constants.Warehouse.DefaultWarehouseKey, Name = "Default Catalog", Description = null, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
        }

        /// <summary>
        /// The create gateway provider settings data.
        /// </summary>
        private void CreateGatewayProviderSettingsData()
        {
            var extended = new ExtendedDataCollection();

            // TODO - move this to a package action
            _database.Insert("merchGatewayProviderSettings", "Key", new GatewayProviderSettingsDto() { Key = Constants.ProviderKeys.Shipping.FixedRateShippingProviderKey, Name = "Fixed Rate Shipping Provider", ProviderTfKey = EnumTypeFieldConverter.GatewayProvider.GetTypeField(GatewayProviderType.Shipping).TypeKey, ExtendedData = new ExtendedDataCollection().SerializeToXml(), EncryptExtendedData = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });

            // add the everywhere else shipcountry
            _database.Insert(
                            "merchShipCountry", 
                            "Key",
                             new ShipCountryDto()
                                 {
                                     Key = Guid.NewGuid(),
                                     CatalogKey = Constants.Warehouse.DefaultWarehouseCatalogKey,
                                     CountryCode = Constants.CountryCodes.EverywhereElse,
                                     Name = "Everywhere Else",
                                     CreateDate = DateTime.Now,
                                     UpdateDate = DateTime.Now
                                 });

            // TODO - move this to a package action
            _database.Insert("merchGatewayProviderSettings", "Key", new GatewayProviderSettingsDto() { Key = Constants.ProviderKeys.Taxation.FixedRateTaxationProviderKey, Name = "Fixed Rate Tax Provider", ProviderTfKey = EnumTypeFieldConverter.GatewayProvider.GetTypeField(GatewayProviderType.Taxation).TypeKey, ExtendedData = new ExtendedDataCollection().SerializeToXml(), EncryptExtendedData = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });


            // TODO - move this to a package action
            _database.Insert("merchGatewayProviderSettings", "Key", new GatewayProviderSettingsDto() { Key = Constants.ProviderKeys.Payment.CashPaymentProviderKey, Name = "Cash Payment Provider", ProviderTfKey = EnumTypeFieldConverter.GatewayProvider.GetTypeField(GatewayProviderType.Payment).TypeKey, ExtendedData = new ExtendedDataCollection().SerializeToXml(), EncryptExtendedData = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });

            _database.Insert(
                "merchPaymentMethod", 
                "Key",
                new PaymentMethodDto()
                {
                    Key = Guid.NewGuid(),
                    Name = "Cash",
                    PaymentCode = "Cash",
                    Description = "Cash Payment",
                    ProviderKey = Constants.ProviderKeys.Payment.CashPaymentProviderKey,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
        }

        /// <summary>
        /// Adds the default store settings.
        /// </summary>
        private void CreateStoreSettingData()
        {
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.CurrencyCodeKey, Name = "currencyCode", Value = "USD", TypeName = "System.String", CreateDate = DateTime.Now, UpdateDate = DateTime.Now});
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.NextOrderNumberKey, Name = "nextOrderNumber", Value = "1", TypeName = "System.Int32", CreateDate = DateTime.Now, UpdateDate = DateTime.Now});
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.NextInvoiceNumberKey, Name = "nextInvoiceNumber", Value = "1", TypeName = "System.Int32", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.NextShipmentNumberKey, Name = "nextShipmentNumber", Value = "1", TypeName = "System.Int32", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.DateFormatKey, Name = "dateFormat", Value = "dd-MM-yyyy", TypeName = "System.String", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.TimeFormatKey, Name = "timeFormat", Value = "am-pm", TypeName = "System.String", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.UnitSystemKey, Name = "unitSystem", Value = "Imperial", TypeName = "System.String", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.GlobalShippableKey, Name = "globalShippable", Value = "true", TypeName = "System.Boolean", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.GlobalTaxableKey, Name = "globalTaxable", Value = "true", TypeName = "System.Boolean", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.GlobalTrackInventoryKey, Name = "globalTrackInventory", Value = "false", TypeName = "System.Boolean", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.GlobalShippingIsTaxableKey, Name = "globalShippingIsTaxable", Value = "false", TypeName = "System.Boolean", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.MigrationKey, Name = "migration", Value = Guid.NewGuid().ToString(), TypeName = "System.Guid", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Constants.StoreSetting.GlobalTaxationApplicationKey, Name = "globalTaxationApplication", Value = "Invoice", TypeName = "System.String", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Core.Constants.StoreSetting.DefaultExtendedContentCulture, Name = "defaultExtendedContentCulture", Value = "en-US", TypeName = "System.String", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            _database.Insert("merchStoreSetting", "Key", new StoreSettingDto() { Key = Core.Constants.StoreSetting.HasDomainRecordKey, Name = "hasDomainRecord", Value = false.ToString(), TypeName = "System.Boolean", CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
        }
    }
}
