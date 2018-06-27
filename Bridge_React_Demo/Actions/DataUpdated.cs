using Bridge.React;
using ProductiveRage.Immutable;

namespace Bridge_React_Demo.Actions
{
    public class DataUpdated<T> : IDispatcherAction, IAmImmutable
    {
        public DataUpdated(API.RequestId requestId, T data)
        {
            this.CtorSet(_ => _.RequestId, requestId);
            this.CtorSet(_ => _.Data, data);
        }

        public API.RequestId RequestId { get; }
        public T Data { get; }
    }

    public static class DataUpdated
    {
        public static DataUpdated<T> For<T>(API.RequestId requestId, T data)
        {
            return new DataUpdated<T>(requestId, data);
        }
    }
}
