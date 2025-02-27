﻿using NKart.Core.Events;

namespace NKart.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;

    using NKart.Core.Events;

    using Models;
    using Persistence;
    using Persistence.Querying;
    using Persistence.UnitOfWork;
    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Logging;

    /// <summary>
    /// Defines the ShipMethodService
    /// </summary>
    internal class ShipMethodService : MerchelloRepositoryService, IShipMethodService
    {
        /// <summary>
        /// The locker.
        /// </summary>
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipMethodService"/> class.
        /// </summary>
        internal ShipMethodService()
            : this(LoggerResolver.Current.Logger)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipMethodService"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        internal ShipMethodService(ILogger logger)
            : this(new RepositoryFactory(), logger)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipMethodService"/> class.
        /// </summary>
        /// <param name="repositoryFactory">
        /// The repository factory.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        internal ShipMethodService(RepositoryFactory repositoryFactory, ILogger logger)
            : this(new PetaPocoUnitOfWorkProvider(logger), repositoryFactory, logger)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipMethodService"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="repositoryFactory">
        /// The repository factory.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        internal ShipMethodService(IDatabaseUnitOfWorkProvider provider, RepositoryFactory repositoryFactory, ILogger logger)
            : this(provider, repositoryFactory, logger, new TransientMessageFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipMethodService"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="repositoryFactory">
        /// The repository factory.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="eventMessagesFactory">
        /// The event messages factory.
        /// </param>
        internal ShipMethodService(IDatabaseUnitOfWorkProvider provider, RepositoryFactory repositoryFactory, ILogger logger, IEventMessagesFactory eventMessagesFactory)
            : base(provider, repositoryFactory, logger, eventMessagesFactory)
        {
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Occurs after Create
        /// </summary>
        public static event TypedEventHandler<IShipMethodService, Events.NewEventArgs<IShipMethod>> Creating;


        /// <summary>
        /// Occurs after Create
        /// </summary>
        public static event TypedEventHandler<IShipMethodService, Events.NewEventArgs<IShipMethod>> Created;

        /// <summary>
        /// Occurs before Save
        /// </summary>
        public static event TypedEventHandler<IShipMethodService, SaveEventArgs<IShipMethod>> Saving;

        /// <summary>
        /// Occurs after Save
        /// </summary>
        public static event TypedEventHandler<IShipMethodService, SaveEventArgs<IShipMethod>> Saved;

        /// <summary>
        /// Occurs before Delete
        /// </summary>		
        public static event TypedEventHandler<IShipMethodService, DeleteEventArgs<IShipMethod>> Deleting;

        /// <summary>
        /// Occurs after Delete
        /// </summary>
        public static event TypedEventHandler<IShipMethodService, DeleteEventArgs<IShipMethod>> Deleted;

        #endregion

        /// <summary>
        /// Saves a single <see cref="IShipMethod"/>
        /// </summary>
        /// <param name="shipMethod">The <see cref="IShipMethod"/> to save</param>
        /// <param name="raiseEvents">Optional boolean indicating whether or not to raise events.</param>
        public void Save(IShipMethod shipMethod, bool raiseEvents = true)
        {
            if(raiseEvents)
            if (Saving.IsRaisedEventCancelled(new SaveEventArgs<IShipMethod>(shipMethod), this))
            {
                ((ShipMethod) shipMethod).WasCancelled = true;
                return;
            }

            using (new WriteLock(Locker))
            {
                var uow = UowProvider.GetUnitOfWork();
                using (var repository = RepositoryFactory.CreateShipMethodRepository(uow))
                {
                    repository.AddOrUpdate(shipMethod);
                    uow.Commit();
                }
            }

            if(raiseEvents) Saved.RaiseEvent(new SaveEventArgs<IShipMethod>(shipMethod), this);
        }

        /// <summary>
        /// Saves a collection of <see cref="IShipMethod"/>
        /// </summary>
        /// <param name="shipMethodList">Collection of <see cref="IShipMethod"/> to save</param>
        /// <param name="raiseEvents">Optional boolean indicating whether or not to raise events</param>
        public void Save(IEnumerable<IShipMethod> shipMethodList, bool raiseEvents = true)
        {
            var shipMethodsArray = shipMethodList as IShipMethod[] ?? shipMethodList.ToArray();
            if(raiseEvents) Saving.RaiseEvent(new SaveEventArgs<IShipMethod>(shipMethodsArray), this);

            using (new WriteLock(Locker))
            {
                var uow = UowProvider.GetUnitOfWork();
                using (var repository = RepositoryFactory.CreateShipMethodRepository(uow))
                {
                    foreach (var shipMethod in shipMethodsArray)
                    {
                        repository.AddOrUpdate(shipMethod);
                    }
                    uow.Commit();
                }
            }

            if(raiseEvents) Saved.RaiseEvent(new SaveEventArgs<IShipMethod>(shipMethodsArray), this);
        }

        /// <summary>
        /// Deletes a <see cref="IShipMethod"/>
        /// </summary>
        /// <param name="shipMethod">The <see cref="IShipMethod"/> to delete</param>
        /// <param name="raiseEvents">Optional boolean indicating whether or not to raise events</param>
        public void Delete(IShipMethod shipMethod, bool raiseEvents = true)
        {
            if(raiseEvents)
            if (Deleting.IsRaisedEventCancelled(new DeleteEventArgs<IShipMethod>(shipMethod), this))
            {
                ((ShipMethod) shipMethod).WasCancelled = true;
                return;
            }

            using (new WriteLock(Locker))
            {
                var uow = UowProvider.GetUnitOfWork();
                using (var repository = RepositoryFactory.CreateShipMethodRepository(uow))
                {
                    repository.Delete(shipMethod);
                    uow.Commit();
                }
            }

            if(raiseEvents) Deleted.RaiseEvent(new DeleteEventArgs<IShipMethod>(shipMethod), this);
        }

        /// <summary>
        /// Deletes a collection of <see cref="IShipMethod"/>
        /// </summary>
        /// <param name="shipMethods">The collection of <see cref="IShipMethod"/> to be deleted</param>
        /// <param name="raiseEvents">Optional boolean indicating whether or not to raise events</param>
        public void Delete(IEnumerable<IShipMethod> shipMethods, bool raiseEvents = true)
        {
            var methods = shipMethods as IShipMethod[] ?? shipMethods.ToArray();

            if (raiseEvents)
            Deleting.RaiseEvent(new DeleteEventArgs<IShipMethod>(methods), this);

            using (new WriteLock(Locker))
            {
                var uow = UowProvider.GetUnitOfWork();
                using (var repository = RepositoryFactory.CreateShipMethodRepository(uow))
                {
                    foreach (var method in methods)
                    {
                        repository.Delete(method);
                    }
                    uow.Commit();
                }
            }

            if (raiseEvents)
            Deleted.RaiseEvent(new DeleteEventArgs<IShipMethod>(methods), this);

        }

        /// <summary>
        /// Gets a <see cref="IShipMethod"/> given it's unique 'key' (GUID)
        /// </summary>
        /// <param name="key">The <see cref="IShipMethod"/>'s unique 'key' (GUID)</param>
        /// <returns>The <see cref="IShipMethod"/></returns>
        public IShipMethod GetByKey(Guid key)
        {
            using (var repository = RepositoryFactory.CreateShipMethodRepository(UowProvider.GetUnitOfWork()))
            {
                return repository.Get(key);
            }
        }

        /// <summary>
        /// Gets a list of <see cref="IShipMethod"/> objects given a <see cref="IGatewayProviderSettings"/> key and a <see cref="IShipCountry"/> key
        /// </summary>
        /// <param name="providerKey">
        /// The provider Key.
        /// </param>
        /// <param name="shipCountryKey">
        /// The ship Country Key.
        /// </param>
        /// <returns>
        /// A collection of <see cref="IShipMethod"/>
        /// </returns>
        public IEnumerable<IShipMethod> GetShipMethodsByProviderKey(Guid providerKey, Guid shipCountryKey)
        {
            using (var repository = RepositoryFactory.CreateShipMethodRepository(UowProvider.GetUnitOfWork()))
            {
                var query =
                    Query<IShipMethod>.Builder.Where(
                        x => x.ProviderKey == providerKey && x.ShipCountryKey == shipCountryKey);

                return repository.GetByQuery(query);
            }
        }

        /// <summary>
        /// Gets a list of all <see cref="IShipMethod"/> objects given a <see cref="IGatewayProviderSettings"/> key
        /// </summary>
        /// <param name="providerKey">
        /// The provider Key.
        /// </param>
        /// <returns>
        /// A collection of <see cref="IShipMethod"/>
        /// </returns>
        public IEnumerable<IShipMethod> GetShipMethodsByProviderKey(Guid providerKey)
        {
            using (var repository = RepositoryFactory.CreateShipMethodRepository(UowProvider.GetUnitOfWork()))
            {
                var query =
                    Query<IShipMethod>.Builder.Where(
                        x => x.ProviderKey == providerKey);

                return repository.GetByQuery(query);
            }
        }

        /// <summary>
        /// Gets all <see cref="IShipMethod"/>s.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IShipMethod}"/>.
        /// </returns>
        public IEnumerable<IShipMethod> GetAll()
        {
            using (var repository = RepositoryFactory.CreateShipMethodRepository(UowProvider.GetUnitOfWork()))
            {
                return repository.GetAll();
            }
        }

        /// <summary>
        /// Creates a <see cref="IShipMethod"/>.  This is useful due to the data constraint
        /// preventing two ShipMethods being created with the same ShipCountry and ServiceCode for any provider.
        /// </summary>
        /// <param name="providerKey">
        /// The unique gateway provider key (GUID)
        /// </param>
        /// <param name="shipCountry">
        /// The <see cref="IShipCountry"/> this ship method is to be associated with
        /// </param>
        /// <param name="name">
        /// The required name of the <see cref="IShipMethod"/>
        /// </param>
        /// <param name="serviceCode">
        /// The ShipMethods service code
        /// </param>
        /// <param name="raiseEvents">
        /// Optional boolean indicating whether or not to raise events
        /// </param>
        /// <returns>
        /// The <see cref="Attempt"/>.
        /// </returns>
        internal Attempt<IShipMethod> CreateShipMethodWithKey(Guid providerKey, IShipCountry shipCountry, string name, string serviceCode, bool raiseEvents = true)
        {
            Ensure.ParameterCondition(providerKey != Guid.Empty, "providerKey");
            Ensure.ParameterNotNull(shipCountry, "shipCountry");
            Ensure.ParameterNotNullOrEmpty(name, "name");
            Ensure.ParameterNotNullOrEmpty(serviceCode, "serviceCode");

            if (ShipMethodExists(providerKey, shipCountry.Key, serviceCode))
                return Attempt<IShipMethod>.Fail(new ConstraintException("A Shipmethod already exists for this ShipCountry with this ServiceCode"));

            var shipMethod = new ShipMethod(providerKey, shipCountry.Key)
            {
                Name = name,
                ServiceCode = serviceCode,
                Provinces = shipCountry.Provinces.ToShipProvinceCollection()
            };

            if (raiseEvents)
                if (Creating.IsRaisedEventCancelled(new Events.NewEventArgs<IShipMethod>(shipMethod), this))
                {
                    shipMethod.WasCancelled = true;
                    return Attempt<IShipMethod>.Fail(shipMethod);
                }

            using (new WriteLock(Locker))
            {
                var uow = UowProvider.GetUnitOfWork();
                using (var repository = RepositoryFactory.CreateShipMethodRepository(uow))
                {
                    repository.AddOrUpdate(shipMethod);
                    uow.Commit();
                }
            }

            if (raiseEvents) Created.RaiseEvent(new Events.NewEventArgs<IShipMethod>(shipMethod), this);

            return Attempt<IShipMethod>.Succeed(shipMethod);
        }

        /// <summary>
        /// The ship method exists.
        /// </summary>
        /// <param name="providerKey">
        /// The provider key.
        /// </param>
        /// <param name="shipCountryKey">
        /// The ship country key.
        /// </param>
        /// <param name="serviceCode">
        /// The service code.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool ShipMethodExists(Guid providerKey, Guid shipCountryKey, string serviceCode)
        {
            using (var repository = RepositoryFactory.CreateShipMethodRepository(UowProvider.GetUnitOfWork()))
            {
                var query =
                   Query<IShipMethod>.Builder.Where(
                       x => x.ShipCountryKey == shipCountryKey && x.ServiceCode == serviceCode && x.ProviderKey == providerKey);

                return repository.GetByQuery(query).Any();

            }
        }
    }
}