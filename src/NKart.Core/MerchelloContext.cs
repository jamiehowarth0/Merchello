﻿using NKart.Core.Logging;

namespace NKart.Core
{
    using System;
    using System.Threading;
    using Configuration;
    using Gateways;
    using NKart.Core.Logging;

    using Services;
    using Umbraco.Core;

    /// <summary>
    /// The MerchelloContext singleton
    /// </summary>
    public class MerchelloContext : IMerchelloContext
    {
        #region Fields

        /// <summary>
        /// A disposal thread locker.
        /// </summary>
        private readonly ReaderWriterLockSlim _disposalLocker = new ReaderWriterLockSlim();

        /// <summary>
        /// The <see cref="IServiceContext"/>
        /// </summary>
        private IServiceContext _services;

        /// <summary>
        /// The <see cref="IGatewayContext"/>
        /// </summary>
        private IGatewayContext _gateways;

        /// <summary>
        /// The disposed value
        /// </summary>
        private volatile bool _disposed;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchelloContext"/> class.
        /// </summary>
        /// <param name="serviceContext">
        /// The service context.
        /// </param>
        /// <param name="gatewayContext">
        /// The gateway context.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        internal MerchelloContext(IServiceContext serviceContext, IGatewayContext gatewayContext, CacheHelper cache)
        {
            Ensure.ParameterNotNull(serviceContext, "serviceContext");
            Ensure.ParameterNotNull(gatewayContext, "gatewayContext");
            Ensure.ParameterNotNull(cache, "cache");
            
            _services = serviceContext;
            _gateways = gatewayContext;
            Cache = cache;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MerchelloContext"/> class. 
        /// Creates a basic basic context
        /// </summary>
        /// <param name="cache">
        /// The <see cref="CacheHelper"/>
        /// </param>
        /// <remarks>
        /// Used for testing
        /// </remarks>
        internal MerchelloContext(CacheHelper cache)
        {
            Cache = cache;
        }

        /// <summary>
        /// Gets the singleton accessor
        /// </summary>
        public static MerchelloContext Current { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether has current.
        /// </summary>
        public static bool HasCurrent
        {
            get
            {
                return Current != null;
            }
        }

        /// <summary>
        /// Gets the current ServiceContext
        /// </summary>
        /// <remarks>
        /// Internal set is generally only used for unit tests
        /// </remarks>
        public IServiceContext Services
        {
            get
            {
                if (_services == null)
                    throw new InvalidOperationException("The ServiceContext has not been set on the MerchelloContext");
                return _services;
            }

            internal set
            {
                _services = value;
            }
        }

        /// <summary>
        /// Gets the current GatewayContext
        /// </summary>
        public IGatewayContext Gateways
        {
            get
            {
                if (_gateways == null)
                    throw new InvalidOperationException("The GatewayContext has not been set on the MerchelloContext");
                return _gateways;
            }

            internal set
            {
                _gateways = value;
            }
        }               

        /// <summary>
        /// Gets the application wide cache accessor
        /// </summary>
        /// <remarks>
        /// This is generally a short cut to the ApplicationContext.ApplicationCache
        /// </remarks>
        public CacheHelper Cache { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the Merchello needs to be upgraded
        /// </summary>
        /// <remarks>
        /// Compares the binary version to that listed in the Merchello configuration to determine if the 
        /// package was upgraded
        /// </remarks>
        public bool IsConfigured
        {
            get
            {
                try
                {
                    var configVersion = ConfigurationVersion;
                    var currentVersion = MerchelloVersion.Current.ToString();


                    if (currentVersion != configVersion)
                    {
                        MultiLogHelper.Info<ApplicationContext>("CurrentVersion different from configurationStatus: '" + currentVersion + "','" + configVersion + "'");
                    }

                    return configVersion == currentVersion;
                }
                catch
                {                    
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the configuration version.
        /// </summary>
        private static string ConfigurationVersion
        {
            get
            {
                try
                {
                    return MerchelloConfiguration.ConfigurationStatus;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// This will dispose and reset all resources used to run the Merchello
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            using (new WriteLock(_disposalLocker))
            {                
                _disposed = true;
            }
        }
    }
}
