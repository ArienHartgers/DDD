namespace DDD.Core.OrderManagement.Orders
{
    public class OrderLineIdentity
    {
        public OrderLineIdentity(int id)
        {
            LineId = id;
        }

        public int LineId { get; }

        public bool Equals(OrderLineIdentity other)
        {
            return LineId == other.LineId;
        }

        public override bool Equals(object obj)
        {
            return obj is OrderLineIdentity other && Equals(other);
        }

        public override int GetHashCode()
        {
            return LineId;
        }

        public static bool operator ==(OrderLineIdentity left, OrderLineIdentity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OrderLineIdentity left, OrderLineIdentity right)
        {
            return !left.Equals(right);
        }
    }
}