﻿using NKart.Core.Events;
using NKart.Core.Models;
using NKart.Core.Persistence;
using NKart.Core.Persistence.Querying;
using NKart.Core.Persistence.UnitOfWork;

namespace NKart.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using NKart.Core.Events;
    using NKart.Core.Models;
    using NKart.Core.Persistence;
    using NKart.Core.Persistence.Querying;
    using NKart.Core.Persistence.UnitOfWork;

    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Logging;

    /// <summary>
    /// Represents the Warehouse Service 
    /// </summary>
    internal class WarehouseService : MerchelloRepositoryService, IWarehouseService
    {
        /// <summary>
        /// The locker.
        /// </summary>
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// The warehouse catalog service.
        /// </summary>
        private readonly IWarehouseCatalogService _warehouseCatalogService;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehouseService"/> class.
        /// </summary>
        public WarehouseService()
            : this(LoggerResolver.Current.Logger)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehouseService"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public WarehouseService(ILogger logger)
            : this(new RepositoryFactory(), logger, new WarehouseCatalogService(logger))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehouseService"/> class.
        /// </summary>
        /// <param name="repositoryFactory">
        /// The repository factory.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="warehouseCatalogService">
        /// The warehouse Catalog Service.
        /// </param>
        public WarehouseService(RepositoryFactory repositoryFactory, ILogger logger, IWarehouseCatalogService warehouseCatalogService)
            : this(new PetaPocoUnitOfWorkProvider(logger), repositoryFactory, logger, warehouseCatalogService)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehouseService"/> class.
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
        /// <param name="warehouseCatalogService">
        /// The warehouse Catalog Service.
        /// </param>
        public WarehouseService(IDatabaseUnitOfWorkProvider provider, RepositoryFactory repositoryFactory, ILogger logger, IWarehouseCatalogService warehouseCatalogService)
            : this(provider, repositoryFactory, logger, new TransientMessageFactory(), warehouseCatalogService)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehouseService"/> class.
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
        /// <param name="warehouseCatalogService">
        /// The warehouse catalog service.
        /// </param>
        public WarehouseService(IDatabaseUnitOfWorkProvider provider, RepositoryFactory repositoryFactory, ILogger logger, IEventMessagesFactory eventMessagesFactory, IWarehouseCatalogService warehouseCatalogService)
            : base(provider, repositoryFactory, logger, eventMessagesFactory)
        {
            Ensure.ParameterNotNull(warehouseCatalogService, "warehouseCatalogService");
            _warehouseCatalogService = warehouseCatalogService;
        }


        #endregion

        #region Event Handlers

        /// <summary>
        /// Occurs before Delete
        /// </summary>		
        public static event TypedEventHandler<IWarehouseService, DeleteEventArgs<IWarehouse>> Deleting;

        /// <summary>
        /// Occurs after Delete
        /// </summary>
        public static event TypedEventHandler<IWarehouseService, DeleteEventArgs<IWarehouse>> Deleted;

        /// <summary>
        /// Occurs before Save
        /// </summary>
        public static event TypedEventHandler<IWarehouseService, SaveEventArgs<IWarehouse>> Saving;

        /// <summary>
        /// Occurs after Save
        /// </summary>
        public static event TypedEventHandler<IWarehouseService, SaveEventArgs<IWarehouse>> Saved;

        ///// <summary>
        ///// Occurs before Create
        ///// </summary>
        //public static event TypedEventHandler<IWarehouseService, Events.NewEventArgs<IWarehouse>> Creating;

        /// <summary>
        /// Occurs after Create
        /// </summary>
        public static event TypedEventHandler<IWarehouseService, Events.NewEventArgs<IWarehouse>> Created;

        #endregion

        /// <summary>
        /// Saves a single <see cref="IWarehouse"/> object
        /// </summary>
        /// <param name="warehouse">The <see cref="IWarehouse"/> to save</param>
        /// <param name="raiseEvents">Optional boolean indicating whether or not to raise events.</param>
        public void Save(IWarehouse warehouse, bool raiseEvents = true)
        {
            if (raiseEvents) Saving.RaiseEvent(new SaveEventArgs<IWarehouse>(warehouse), this);

            using (new WriteLock(Locker))
            {
                var uow = UowProvider.GetUnitOfWork();
                using (var repository = RepositoryFactory.CreateWarehouseRepository(uow))
                {
                    repository.AddOrUpdate(warehouse);
                    uow.Commit();
                }

                if (raiseEvents) Saved.RaiseEvent(new SaveEventArgs<IWarehouse>(warehouse), this);
            }
        }

        /// <summary>
        /// Saves a collection of <see cref="IWarehouse"/> objects.
        /// </summary>
        /// <param name="warehouseList">Collection of <see cref="Warehouse"/> to save</param>
        /// <param name="raiseEvents">Optional boolean indicating whether or not to raise events</param>
        public void Save(IEnumerable<IWarehouse> warehouseList, bool raiseEvents = true)
        {
            var warehouseArray = warehouseList as IWarehouse[] ?? warehouseList.ToArray();

            if (raiseEvents) Saving.RaiseEvent(new SaveEventArgs<IWarehouse>(warehouseArray), this);

            using (new WriteLock(Locker))
            {
                var uow = UowProvider.GetUnitOfWork();
                using (var repository = RepositoryFactory.CreateWarehouseRepository(uow))
                {
                    foreach (var warehouse in warehouseArray)
                    {
                        repository.AddOrUpdate(warehouse);
                    }
                    uow.Commit();
                }
            }

            if (raiseEvents) Saved.RaiseEvent(new SaveEventArgs<IWarehouse>(warehouseArray), this);
        }


        /// <summary>
        /// Gets the default <see cref="IWarehouse"/>
        /// </summary>
        /// <returns>The default <see cref="IWarehouse"/></returns>
        public IWarehouse GetDefaultWarehouse()
        {
            using (var repository = RepositoryFactory.CreateWarehouseRepository(UowProvider.GetUnitOfWork()))
            {
                var query = Query<IWarehouse>.Builder.Where(x => x.IsDefault);
                return repository.GetByQuery(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a Warehouse by its unique key
        /// </summary>
        /// <param name="key">The key for the Warehouse</param>
        /// <returns>The <see cref="IWarehouse"/></returns>
        public IWarehouse GetByKey(Guid key)
        {
            using (var repository = RepositoryFactory.CreateWarehouseRepository(UowProvider.GetUnitOfWork()))
            {
                return repository.Get(key);
            }
        }

        /// <summary>
        /// Gets a list of Warehouse give a list of unique keys
        /// </summary>
        /// <param name="keys">List of unique keys</param>
        /// <returns>A collection of <see cref="IWarehouse"/></returns>
        public IEnumerable<IWarehouse> GetByKeys(IEnumerable<Guid> keys)
        {
            using (var repository = RepositoryFactory.CreateWarehouseRepository(UowProvider.GetUnitOfWork()))
            {
                return repository.GetAll(keys.ToArray());
            }
        }

        #region Warehouse Catalog

        /// <summary>
        /// Creates warehouse catalog and persists it to the database.
        /// </summary>
        /// <param name="warehouseKey">
        /// The warehouse key.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <returns>
        /// The <see cref="IWarehouseCatalog"/>.
        /// </returns>
        public IWarehouseCatalog CreateWarehouseCatalogWithKey(Guid warehouseKey, string name, string description = "")
        {
            return _warehouseCatalogService.CreateWarehouseCatalogWithKey(warehouseKey, name, description);
        }

        /// <summary>
        /// Saves a single <see cref="IWarehouseCatalog"/>.
        /// </summary>
        /// <param name="warehouseCatalog">
        /// The warehouse catalog.
        /// </param>
        public void Save(IWarehouseCatalog warehouseCatalog)
        {
            _warehouseCatalogService.Save(warehouseCatalog);
        }

        /// <summary>
        /// Saves a collection of <see cref="IWarehouseCatalog"/>.
        /// </summary>
        /// <param name="warehouseCatalogs">
        /// The warehouse catalogs.
        /// </param>
        public void Save(IEnumerable<IWarehouseCatalog> warehouseCatalogs)
        {
            _warehouseCatalogService.Save(warehouseCatalogs);
        }

        /// <summary>
        /// Deletes a single <see cref="IWarehouseCatalog"/>.
        /// </summary>
        /// <param name="warehouseCatalog">
        /// The warehouse catalog.
        /// </param>
        /// <remarks>
        /// Cannot delete the default catalog in the default warehouse
        /// </remarks>
        public void Delete(IWarehouseCatalog warehouseCatalog)
        {
            _warehouseCatalogService.Delete(warehouseCatalog);
        }

        /// <summary>
        /// Deletes a collection of <see cref="IWarehouseCatalog"/>.
        /// </summary>
        /// <param name="warehouseCatalogs">
        /// The warehouse catalogs.
        /// </param>
        /// <remarks>
        /// Cannot delete the default catalog in the default warehouse
        /// </remarks>
        public void Delete(IEnumerable<IWarehouseCatalog> warehouseCatalogs)
        {
            _warehouseCatalogService.Delete(warehouseCatalogs);
        }

        /// <summary>
        /// Gets a <see cref="IWarehouseCatalog"/> by it's unique key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="IWarehouseCatalog"/>.
        /// </returns>
        public IWarehouseCatalog GetWarehouseCatalogByKey(Guid key)
        {
            return _warehouseCatalogService.GetByKey(key);
        }

        /// <summary>
        /// Gets a collection of all <see cref="IWarehouseCatalog"/>.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="IWarehouseCatalog"/>.
        /// </returns>
        public IEnumerable<IWarehouseCatalog> GetAllWarehouseCatalogs()
        {
            return _warehouseCatalogService.GetAll();
        }

        /// <summary>
        /// Get a collection of <see cref="IWarehouseCatalog"/> by warehouse key.
        /// </summary>
        /// <param name="warehouseKey">
        /// The warehouse key.
        /// </param>
        /// <returns>
        /// A collection of <see cref="IWarehouseCatalog"/>.
        /// </returns>
        public IEnumerable<IWarehouseCatalog> GetWarhouseCatalogByWarehouseKey(Guid warehouseKey)
        {
            return _warehouseCatalogService.GetByWarehouseKey(warehouseKey);
        }

        #endregion

        /// <summary>
        /// Creates an <see cref="IWarehouse"/> object
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="IWarehouse"/>.
        /// </returns>
        internal IWarehouse CreateWarehouse(string name)
        {
            Ensure.ParameterNotNull(name, "name");

            var warehouse = new Warehouse()
            {
                Name = name
            };

            Created.RaiseEvent(new Events.NewEventArgs<IWarehouse>(warehouse), this);

            return warehouse;
        }


        /// <summary>
        /// Deletes a single <see cref="IWarehouse"/> object
        /// </summary>
        /// <param name="warehouse">The <see cref="IWarehouse"/> to delete</param>
        /// <param name="raiseEvents">Optional boolean indicating whether or not to raise events</param>
        internal void Delete(IWarehouse warehouse, bool raiseEvents = true)
        {
            if (raiseEvents) Deleting.RaiseEvent(new DeleteEventArgs<IWarehouse>(warehouse), this);

            using (new WriteLock(Locker))
            {
                var uow = UowProvider.GetUnitOfWork();
                using (var repository = RepositoryFactory.CreateWarehouseRepository(uow))
                {
                    repository.Delete(warehouse);
                    uow.Commit();
                }
            }
            if (raiseEvents) Deleted.RaiseEvent(new DeleteEventArgs<IWarehouse>(warehouse), this);
        }

        /// <summary>
        /// Deletes a collection <see cref="IWarehouse"/> objects
        /// </summary>
        /// <param name="warehouseList">Collection of <see cref="IWarehouse"/> to delete</param>
        /// <param name="raiseEvents">Optional boolean indicating whether or not to raise events</param>
        internal void Delete(IEnumerable<IWarehouse> warehouseList, bool raiseEvents = true)
        {
            var warehouseArray = warehouseList as IWarehouse[] ?? warehouseList.ToArray();

            if (raiseEvents) Deleting.RaiseEvent(new DeleteEventArgs<IWarehouse>(warehouseArray), this);

            using (new WriteLock(Locker))
            {
                var uow = UowProvider.GetUnitOfWork();
                using (var repository = RepositoryFactory.CreateWarehouseRepository(uow))
                {
                    foreach (var warehouse in warehouseArray)
                    {
                        repository.Delete(warehouse);
                    }
                    uow.Commit();
                }
            }

            if (raiseEvents) Deleted.RaiseEvent(new DeleteEventArgs<IWarehouse>(warehouseArray), this);
        }

    }
}