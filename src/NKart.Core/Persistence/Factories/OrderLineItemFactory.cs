﻿using NKart.Core.Models;
using NKart.Core.Models.Rdbms;

namespace NKart.Core.Persistence.Factories
{
    /// <summary>
    /// Represents the OrderLineItemFactory
    /// </summary>
    internal class OrderLineItemFactory : IEntityFactory<IOrderLineItem, OrderItemDto>
    {
        public IOrderLineItem BuildEntity(OrderItemDto dto)
        {
            var lineItem = new OrderLineItem(dto.LineItemTfKey, dto.Name, dto.Sku, dto.Quantity, dto.Price,
                string.IsNullOrEmpty(dto.ExtendedData) ? new ExtendedDataCollection() : new ExtendedDataCollection(dto.ExtendedData))
            {
                Key = dto.Key,
                ShipmentKey = dto.ShipmentKey,
                ContainerKey = dto.ContainerKey,
                BackOrder = dto.BackOrder,
                Exported = dto.Exported,
                UpdateDate = dto.UpdateDate,
                CreateDate = dto.CreateDate
            };

            lineItem.ResetDirtyProperties();

            return lineItem;
        }

        public OrderItemDto BuildDto(IOrderLineItem entity)
        {
            var dto = new OrderItemDto()
            {
                Key = entity.Key,
                ShipmentKey = entity.ShipmentKey,
                ContainerKey = entity.ContainerKey,
                LineItemTfKey = entity.LineItemTfKey,
                Sku = entity.Sku,
                Name = entity.Name,
                Quantity = entity.Quantity,
                Price = entity.Price,
                ExtendedData = entity.ExtendedData.SerializeToXml(),
                BackOrder = entity.BackOrder,
                Exported = entity.Exported,
                UpdateDate = entity.UpdateDate,
                CreateDate = entity.CreateDate
            };

            return dto;
        }
    }
}