namespace Bridge_React_Demo.API
{
    public class MessageId
    {
        public int Value { get; private set; }

        public static explicit operator MessageId(int value)
        {
            return new MessageId { Value = value };
        }

        public static implicit operator int(MessageId id)
        {
            return id.Value;
        }
    }
}
