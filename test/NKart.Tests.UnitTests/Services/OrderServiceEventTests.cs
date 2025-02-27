﻿using System;
using System.Linq;
using NKart.Core.Events;
using NKart.Core.Models;
using NKart.Core.Persistence;
using NKart.Core.Services;
using NKart.Tests.Base.DataMakers;
using NKart.Tests.Base.Respositories;
using NKart.Tests.Base.Services;
using Moq;
using NUnit.Framework;

namespace NKart.Tests.UnitTests.Services
{
    using NKart.Core.Cache;

    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence.SqlSyntax;

    [TestFixture]
    [Category("Service Events")]
    public class OrderServiceEventTests : ServiceTestsBase<IOrder>
    {
        private IOrderService _orderService;


        [TestFixtureSetUp]
        public override void FixtureSetup()
        {
            base.FixtureSetup();

            var mockSettingService = new Mock<IStoreSettingService>();
            mockSettingService.Setup(x => x.GetNextOrderNumber(1)).Returns(111);
            var logger = Logger.CreateWithDefaultLog4NetConfiguration();

            var uow = new MockUnitOfWorkProvider();

            var repositoryFactory = new Mock<RepositoryFactory>(
                false,
                new NullCacheProvider(),
                new NullCacheProvider(),
                Logger.CreateWithDefaultLog4NetConfiguration(),
                SqlSyntaxProvider);

            // provider, repositoryFactory, logger, new TransientMessageFactory(), storeSettingService, shipmentService

            _orderService = new OrderService(uow, repositoryFactory.Object, new Mock<ILogger>().Object, mockSettingService.Object, new Mock<IShipmentService>().Object);
            

            OrderService.StatusChanging += OrderStatusChanging;
            OrderService.StatusChanged += OrderStatusChanged;
        }

        [TestFixtureTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            OrderService.StatusChanging -= OrderStatusChanging;
            OrderService.StatusChanged -= OrderStatusChanged;
        }

        private void OrderStatusChanging(Object sender, EventArgs args)
        {
            Console.Write(sender.GetType().Name + " " + typeof(StatusChangeEventArgs<>).Name);
            var beforeArgs = args as StatusChangeEventArgs<IOrder>;

            Before = beforeArgs.StatusChangedEntities.First();
        }

        private void OrderStatusChanged(IOrderService sender, StatusChangeEventArgs<IOrder> args)
        {
            After = args.StatusChangedEntities.First();
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            Before = null;
            After = null;

        }

        /// <summary>
        /// Test verifies that saving an order with an updated status does trigger events
        /// </summary>
        [Test]
        public void Changing_Order_Status_Triggers_StatusChange_Events()
        {
            //// Arrange
            var notFulfilled = MockOrderStatusMaker.OrderStatusNotFulfilledMock();
            var fulfulled = MockOrderStatusMaker.OrderStatusFulfilledMock();
            var order = new Order(notFulfilled, Guid.NewGuid()) {Key = Guid.NewGuid()};

            _orderService.Save(order);

            //// Act
            order.OrderStatus = fulfulled;
            _orderService.Save(order);

            //// Assert
            Assert.NotNull(Before, "Before was null");
            Assert.NotNull(After, "After was null");
            Assert.AreEqual(fulfulled.Key, Before.OrderStatusKey);
            Assert.AreEqual(fulfulled.Key, After.OrderStatusKey);
        }
    }
}