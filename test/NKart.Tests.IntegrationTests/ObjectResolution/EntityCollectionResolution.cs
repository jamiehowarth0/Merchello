﻿namespace NKart.Tests.IntegrationTests.ObjectResolution
{
    using System.Linq;

    using NKart.Core.EntityCollections;
    using NKart.Core.EntityCollections.Providers;
    using NKart.Tests.Base.TestHelpers;
    using NKart.Web.Models.ContentEditing.Collections;

    using NUnit.Framework;

    [TestFixture]
    public class EntityCollectionResolution : MerchelloAllInTestBase
    {
        /// <summary>
        /// Test shows a key can be resolved from the attribute based on the type of provider
        /// </summary>
        [Test]
        public void Can_Resolve_ProductFilterGroupProvider_Key()
        {
            //// Arrange
            var expected = Core.Constants.ProviderKeys.EntityCollection.EntityFilterGroupProviderKey;
            var resolver = EntityCollectionProviderResolver.Current;

            //// Act
            var key = resolver.GetProviderKey<ProductFilterGroupProvider>();

            //// Assert
            Assert.AreEqual(expected, key);
        }

        [Test]
        public void Can_Resolve_IEntityFilterGroupProvider_Keys()
        {
            //// Arrange
            var expected = 1;
            var resolver = EntityCollectionProviderResolver.Current;

            //// Act
            var keys = resolver.GetProviderKeys<IEntityFilterGroupProvider>();
            if (!keys.Any()) Assert.Fail("No keys returned");
            var key = keys.First();

            //// Assert
            Assert.AreEqual(expected, keys.Count());
            Assert.AreEqual(key, Core.Constants.ProviderKeys.EntityCollection.EntityFilterGroupProviderKey);
        }

        [Test]
        public void Can_Resolve_IProductCollectionProvider_Keys()
        {
            //// Arrange
            var expected = 1;
            var resolver = EntityCollectionProviderResolver.Current;

            //// Act
            var keys = resolver.GetProviderKeys<IProductEntityCollectionProvider>();
            if (!keys.Any()) Assert.Fail("No keys returned");
            var key = keys.First();

            //// Assert
            Assert.AreEqual(expected, keys.Count());
            Assert.AreEqual(key, Core.Constants.ProviderKeys.EntityCollection.StaticProductCollectionProviderKey);
        }

        /// <summary>
        /// Test shows a key can be resolved from the attribute based on the type of provider
        /// </summary>
        [Test]
        public void Can_Resolve_StaticProductCollectionProviders_Key()
        {
            //// Arrange
            var expected = Core.Constants.ProviderKeys.EntityCollection.StaticProductCollectionProviderKey;
            var resolver = EntityCollectionProviderResolver.Current;

            //// Act
            var key = resolver.GetProviderKey<StaticProductCollectionProvider>();

            //// Assert
            Assert.AreEqual(expected, key);
        }

        [Test]
        public void Can_Resolve_StaticInvoiceCollectionProvider_Key()
        {
            //// Arrange
            var expected = Core.Constants.ProviderKeys.EntityCollection.StaticInvoiceCollectionProviderKey;
            var resolver = EntityCollectionProviderResolver.Current;

            //// Act
            var key = resolver.GetProviderKey<StaticInvoiceCollectionProvider>();

            //// Assert
            Assert.AreEqual(expected, key);
        }

        [Test]
        public void Can_Resolve_StaticCustomerCollectionProvider_Key()
        {
            //// Arrange
            var expected = Core.Constants.ProviderKeys.EntityCollection.StaticCustomerCollectionProviderKey;
            var resolver = EntityCollectionProviderResolver.Current;

            //// Act
            var key = resolver.GetProviderKey<StaticCustomerCollectionProvider>();

            //// Assert
            Assert.AreEqual(expected, key);
        }

        [Test]
        public void Can_Resolve_StaticEntityCollectionCollectionProvider_Key()
        {
            //// Arrange
            var expected = Core.Constants.ProviderKeys.EntityCollection.StaticEntityCollectionCollectionProvider;
            var resolver = EntityCollectionProviderResolver.Current;

            //// Act
            var key = resolver.GetProviderKey<StaticEntityCollectionCollectionProvider>();

            //// Assert
            Assert.AreEqual(expected, key);
        }

        [Test]
        public void Can_Map_EntityCollectionProviderAttribute_To_EntityCollectionProviderDisplay()
        {
            //// Arrange
            var att = EntityCollectionProviderResolver.Current.GetProviderAttribute<StaticProductCollectionProvider>();

            //// Act
            var display = att.ToEntityCollectionProviderDisplay();

            //// Assert
            Assert.NotNull(display);
        }
    }
}