using Bridge.React;
using ProductiveRage.Immutable;

namespace Bridge_React_Demo.Actions
{
    public class SaveSucceeded : IDispatcherAction, IAmImmutable
    {
        public SaveSucceeded(API.RequestId requestId)
        {
            this.CtorSet(_ => _.RequestId, requestId);
        }
        public API.RequestId RequestId { get; }
    }
}