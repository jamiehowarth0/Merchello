﻿using NKart.Core.Models.EntityBase;
using NKart.Core.Models.Interfaces;
using NKart.Core.Models.Rdbms;
using NKart.Core.Persistence.Factories;
using NKart.Core.Persistence.Querying;
using NKart.Core.Persistence.UnitOfWork;

namespace NKart.Core.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NKart.Core.Models;
    using NKart.Core.Models.EntityBase;
    using NKart.Core.Models.Interfaces;
    using NKart.Core.Models.Rdbms;
    using NKart.Core.Persistence.Factories;
    using NKart.Core.Persistence.Querying;
    using NKart.Core.Persistence.UnitOfWork;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.Querying;
    using Umbraco.Core.Persistence.SqlSyntax;

    /// <summary>
    /// Represents the audit log repository.
    /// </summary>
    internal class AuditLogRepository : PagedRepositoryBase<IAuditLog, AuditLogDto>, IAuditLogRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogRepository"/> class.
        /// </summary>
        /// <param name="work">
        /// The work.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="sqlSyntax">
        /// The SQL Syntax.
        /// </param>
        public AuditLogRepository(IDatabaseUnitOfWork work, ILogger logger, ISqlSyntaxProvider sqlSyntax)
            : base(work, logger, sqlSyntax)
        {
        }

        /// <summary>
        /// Searches the audit log
        /// </summary>
        /// <param name="searchTerm">
        /// The search term.
        /// </param>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="itemsPerPage">
        /// The items per page.
        /// </param>
        /// <param name="orderExpression">
        /// The order expression.
        /// </param>
        /// <param name="sortDirection">
        /// The sort direction.
        /// </param>
        /// <returns>
        /// The <see cref="Page{Guid}"/>.
        /// </returns>
        public override Page<Guid> SearchKeys(
            string searchTerm,
            long page,
            long itemsPerPage,
            string orderExpression,
            SortDirection sortDirection = SortDirection.Descending)
        {
            var terms = searchTerm.Split(' ');

            var sql = new Sql();
            sql.Select("*").From<AuditLogDto>(SqlSyntax);

            if (terms.Any())
            {
                var preparedTerms = string.Format("%{0}%", string.Join("%", terms));

                sql.Where("message LIKE @msg", new { @msg = preparedTerms });
            }

            return GetPagedKeys(page, itemsPerPage, sql, orderExpression, sortDirection);
        }

        /// <summary>
        /// Gets a <see cref="IAuditLog"/>
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="IAuditLog"/>.
        /// </returns>
        protected override IAuditLog PerformGet(Guid key)
        {
            var sql = GetBaseQuery(false)
            .Where(GetBaseWhereClause(), new { Key = key });


            var dto = Database.Fetch<AuditLogDto>(sql).FirstOrDefault();

            if (dto == null)
                return null;

            var factory = new AuditLogFactory();

            var customer = factory.BuildEntity(dto);

            return customer;
        }

        /// <summary>
        /// Gets the collection of all <see cref="IAuditLog"/>
        /// </summary>
        /// <param name="keys">
        /// The keys.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{IAuditLog}"/>.
        /// </returns>
        protected override IEnumerable<IAuditLog> PerformGetAll(params Guid[] keys)
        {

            var dtos = new List<AuditLogDto>();

            if (keys.Any())
            {
                // This is to get around the WhereIn max limit of 2100 parameters and to help with performance of each WhereIn query
                var keyLists = keys.Split(400).ToList();

                // Loop the split keys and get them
                foreach (var keyList in keyLists)
                {
                    dtos.AddRange(Database.Fetch<AuditLogDto>(GetBaseQuery(false).WhereIn<AuditLogDto>(x => x.Key, keyList, SqlSyntax)));
                }
            }
            else
            {
                dtos = Database.Fetch<AuditLogDto>(GetBaseQuery(false));
            }

            var factory = new AuditLogFactory();
            foreach (var dto in dtos)
            {
                yield return factory.BuildEntity(dto);
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="IAuditLog"/> by query
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{IAuditLog}"/>.
        /// </returns>
        protected override IEnumerable<IAuditLog> PerformGetByQuery(IQuery<IAuditLog> query)
        {
            var sqlClause = GetBaseQuery(false);
            var translator = new SqlTranslator<IAuditLog>(sqlClause, query);
            var sql = translator.Translate();

            var dtos = Database.Fetch<AuditLogDto>(sql);

            return dtos.DistinctBy(x => x.Key).Select(dto => Get(dto.Key));
        }

        /// <summary>
        /// Gets the base query.
        /// </summary>
        /// <param name="isCount">
        /// The is count.
        /// </param>
        /// <returns>
        /// The <see cref="Sql"/>.
        /// </returns>
        protected override Sql GetBaseQuery(bool isCount)
        {
            var sql = new Sql();
            sql.Select(isCount ? "COUNT(*)" : "*")
               .From<AuditLogDto>(SqlSyntax);

            return sql;
        }

        /// <summary>
        /// Gets the base where clause.
        /// </summary>
        /// <returns>
        /// The base where clause.
        /// </returns>
        protected override string GetBaseWhereClause()
        {
            return "merchAuditLog.pk = @Key";
        }

        /// <summary>
        /// Gets a collection of delete clauses
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{String}"/>.
        /// </returns>
        protected override IEnumerable<string> GetDeleteClauses()
        {
            var list = new List<string>
                {
                    "DELETE FROM merchAuditLog WHERE merchAuditLog.pk = @Key"
                };

            return list;
        }

        /// <summary>
        /// The persist new audit log.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void PersistNewItem(IAuditLog entity)
        {
            ((Entity)entity).AddingEntity();

            var factory = new AuditLogFactory();
            var dto = factory.BuildDto(entity);
            Database.Insert(dto);
            entity.Key = dto.Key;

            entity.ResetDirtyProperties();

        }

        /// <summary>
        /// Updates an audit log
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void PersistUpdatedItem(IAuditLog entity)
        {
            ((Entity)entity).UpdatingEntity();

            var factory = new AuditLogFactory();
            var dto = factory.BuildDto(entity);
            Database.Update(dto);
            
            entity.ResetDirtyProperties();
        }

    }
}