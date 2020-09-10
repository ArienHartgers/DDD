using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Orders.Identities
{
    public class OrderLineIdentity : IdentityValueObject
    {
        private OrderLineIdentity(int id)
        {
            LineId = id;
        }

        public int LineId { get; }

        public override string Identity => $"{LineId}";

        public static OrderLineIdentity Create(int lineId)
        {
            if (lineId <= 0) throw new Exception("Invalid LineId");
            return new OrderLineIdentity(lineId);
        }

        public static OrderLineIdentity NextIdentity(OrderLineIdentity? lastOrderLineIdentity)
        {
            var lastLineId = lastOrderLineIdentity?.LineId ?? 0;
            return new OrderLineIdentity( lastLineId+ 1);
        }

        public static OrderLineIdentity Create(string id)
        {
            if (int.TryParse(id, out int value))
            {
                return new OrderLineIdentity(value);
            }

            throw new Exception($"Cannot parse id {id}");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LineId;
        }
    }
}