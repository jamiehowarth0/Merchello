﻿using NKart.Core.Configuration;
using NKart.Core.Models.Rdbms;

namespace NKart.Core.Persistence.Migrations.Upgrades.TargetVersionOneElevenZero
{
    using System;
    using System.Collections.Generic;

    using NKart.Core.Configuration;
    using NKart.Core.Models.Rdbms;

    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.Migrations;

    //using DatabaseSchemaHelper = Merchello.Core.Persistence.Migrations.DatabaseSchemaHelper;

    /// <summary>
    /// The create product collection table.
    /// </summary>
    [Migration("1.10.0", "1.11.0", 1, MerchelloConfiguration.MerchelloMigrationName)]
    public class CreateEntityCollectionTable : IMerchelloMigration
    {
        /// <summary>
        /// The schema helper.
        /// </summary>
        private readonly DatabaseSchemaHelper _schemaHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEntityCollectionTable"/> class.
        /// </summary>
        public CreateEntityCollectionTable()
        {
            var dbContext = ApplicationContext.Current.DatabaseContext;
            _schemaHelper = new DatabaseSchemaHelper(dbContext.Database, LoggerResolver.Current.Logger, dbContext.SqlSyntax);
        }

        /// <summary>
        /// Adds the merchProductCollection table to the database.
        /// </summary>
        public void Up()
        {
            if (!_schemaHelper.TableExist("merchEntityCollection"))
            {
                _schemaHelper.CreateTable(false, typeof(EntityCollectionDto));
            }
        }

        /// <summary>
        /// The down.
        /// </summary>
        /// <exception cref="DataLossException">
        /// Throws a data loss exception if a downgrade is attempted
        /// </exception>
        public void Down()
        {
            throw new DataLossException("Cannot downgrade from a version 1.11.0 database to a prior version, the database schema has already been modified");
        }
    }
}