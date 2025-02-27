﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NKart.Core.Models;
using NKart.Core.Models.Rdbms;
using NKart.Tests.Base.SqlSyntax;
using NUnit.Framework;
using Umbraco.Core.Persistence;

namespace NKart.Tests.UnitTests.Querying
{
    [TestFixture]
    [Category("SqlSyntax")]
    public class ShipMethodSqlClausesTest : BaseUsingSqlServerSyntax<IShipMethod>
    {
        /// <summary>
        /// Test to verify that the typed <see cref="ShipmentDto"/> query matches generic "select * ..." query 
        /// </summary>
        [Test]
        public void Can_Verify_ShipMethod_Base_Sql_Clause()
        {
            //// Arrange
            var key = Guid.NewGuid();

            var expected = new Sql();
            expected.Select("*")
                .From("[merchShipMethod]")
                .Where("[merchShipMethod].[pk] = @0", new { key });

            //// Act
            var sql = new Sql();
            sql.Select("*")
                .From<ShipMethodDto>()
                .Where<ShipMethodDto>(x => x.Key == key);

            //// Assert
            Assert.That(sql.SQL, Is.EqualTo(expected.SQL));
        }


        //public void Can_Verify_ShipMethod_ShipRat
    }
}
