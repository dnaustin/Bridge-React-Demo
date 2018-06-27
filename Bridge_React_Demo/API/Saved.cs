using ProductiveRage.Immutable;

namespace Bridge_React_Demo.API
{
    public class Saved<TKey, TValue> : IAmImmutable
    {
        public Saved(TKey key, TValue value)
        {
            this.CtorSet(_ => _.Key, key);
            this.CtorSet(_ => _.Value, value);
        }

        public TKey Key { get; }
        public TValue Value { get; }
    }

    public static class Saved
    {
        /// <summary>
        /// This generic method makes code that creates generic Saved instances more succinct
        /// by relying upon type inference (based upon the key and value argument types), so
        /// that the calling code does not have to explicitly declare TKey and TValue
        /// </summary>
        public static Saved<TKey, TValue> For<TKey, TValue>(TKey key, TValue value)
        {
            return new Saved<TKey, TValue>(key, value);
        }
    }
}
