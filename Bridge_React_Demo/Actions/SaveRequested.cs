using Bridge.React;
using ProductiveRage.Immutable;

namespace Bridge_React_Demo.Actions
{
    public class SaveRequested<T> : IDispatcherAction, IAmImmutable
    {
        public SaveRequested(T data)
        {
            this.CtorSet(_ => _.Data, data);
        }

        public T Data { get; }
    }

    public static class SaveRequested
    {
        public static SaveRequested<T> For<T>(T data)
        {
            return new SaveRequested<T>(data);
        }
    }
}
