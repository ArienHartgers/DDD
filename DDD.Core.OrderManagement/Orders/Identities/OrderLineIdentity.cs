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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LineId;
        }

        public static OrderLineIdentity NextIdentity(OrderLineIdentity? lastOrderLineIdentity)
        {
            return new OrderLineIdentity(lastOrderLineIdentity?.LineId ?? 0 + 1);
        }

        public static OrderLineIdentity Parse(string id)
        {
            if (int.TryParse(id, out int value))
            {
                return new OrderLineIdentity(value);
            }

            throw new Exception($"Cannot parse id {id}");
        }
    }
}