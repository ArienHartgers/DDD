using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Orders.Identitfiers
{
    public class OrderLineIdentifier : IdentifierValueObject
    {
        private OrderLineIdentifier(int id)
        {
            LineId = id;
        }

        public int LineId { get; }

        public override string Identifier => $"{LineId}";

        public static OrderLineIdentifier Create(int lineId)
        {
            if (lineId <= 0) throw new Exception("Invalid LineId");
            return new OrderLineIdentifier(lineId);
        }

        public static OrderLineIdentifier NextIdentifier(OrderLineIdentifier? lastOrderLineIdentity)
        {
            var lastLineId = lastOrderLineIdentity?.LineId ?? 0;
            return new OrderLineIdentifier( lastLineId+ 1);
        }

        public static OrderLineIdentifier Create(string id)
        {
            if (int.TryParse(id, out int value))
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