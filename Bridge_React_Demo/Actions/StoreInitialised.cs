using Bridge.React;
using ProductiveRage.Immutable;

namespace Bridge_React_Demo.Actions
{
    public class StoreInitialised : IDispatcherAction, IAmImmutable
    {
        public StoreInitialised(object store)
        {
            this.CtorSet(_ => _.Store, store);
        }
        public object Store { get; }
    }
}
