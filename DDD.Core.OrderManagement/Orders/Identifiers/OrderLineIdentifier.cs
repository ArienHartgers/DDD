using System;
using System.Collections.Generic;
using DDD.SharedKernel.Identifiers;

namespace DDD.Core.OrderManagement.Orders.Identifiers
{
    public class OrderLineIdentifier : IdentifierValueObject
    {
        public static readonly Prefix Prefix = new Prefix("oln");

        private OrderLineIdentifier(uint id)
        {
            LineId = id;
        }

        public uint LineId { get; }

        public override string Identifier => LineId.ToSolidCode(Prefix);

        public static OrderLineIdentifier Create(uint lineId)
        {
            return new OrderLineIdentifier(lineId);
        }

        public static OrderLineIdentifier NextIdentifier(OrderLineIdentifier? lastOrderLineIdentity)
        {
            var lastLineId = lastOrderLineIdentity?.LineId ?? 0;
            return new OrderLineIdentifier( lastLineId+ 1);
        }

        public static OrderLineIdentifier Parse(string id)
        {
            if (SolidCode.TryParseUInt(id, Prefix, out var value))
            {
                return new OrderLineIdentifier(value);
            }

            throw new Exception($"Cannot parse id {id}");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LineId;
        }
    }
}