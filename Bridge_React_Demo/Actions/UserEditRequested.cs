using Bridge.React;
using ProductiveRage.Immutable;

namespace Bridge_React_Demo.Actions
{
    public class UserEditRequested<T> : IDispatcherAction, IAmImmutable
    {
        public UserEditRequested(T newState)
        {
            this.CtorSet(_ => _.NewState, newState);
        }
        public T NewState { get; }
    }
    public static class UserEditRequested
    {
        public static UserEditRequested<T> For<T>(T newState)
        {
            return new UserEditRequested<T>(newState);
        }
    }
}
