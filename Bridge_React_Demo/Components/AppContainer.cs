using Bridge.React;
using Bridge_React_Demo.Actions;
using Bridge_React_Demo.API;
using Bridge_React_Demo.Stores;
using Bridge_React_Demo.ViewModels;
using ProductiveRage.Immutable;

namespace Bridge_React_Demo.Components
{
    public class AppContainer : Component<AppContainer.Props, Optional<AppContainer.State>>
    {
        public AppContainer(AppDispatcher dispatcher, MessageWriterStore store) 
            : base(new Props(dispatcher, store)) { }

        protected override void ComponentDidMount()
        {
            this.props.Store.Change += StoreChanged;
        }

        protected override void ComponentWillUnmount()
        {
            this.props.Store.Change -= StoreChanged;
        }

        private void StoreChanged()
        {
            this.SetState(new State(this.props.Store.Message, this.props.Store.MessageHistory));
        }

        public override ReactElement Render()
        {
            if (!state.IsDefined)
            {
                return null;
            }

            return DOM.Div(null,
                new MessageEditor(                
                    className: new NonBlankTrimmedString("message"),
                    message: this.state.Value.Message,
                    onChange: newState => this.props.Dispatcher.Dispatch(
                        UserEditRequested.For(newState)
                    ),
                    onSave: () => this.props.Dispatcher.Dispatch(
                        SaveRequested.For(
                            new MessageDetails(                          
                                new NonBlankTrimmedString(this.state.Value.Message.Title.Text),
                                new NonBlankTrimmedString(this.state.Value.Message.Content.Text)
                            )
                        )
                    )
                ),
                new MessageHistory(this.state.Value.MessageHistory, new NonBlankTrimmedString("history"))           
            );
        }

        public class Props : IAmImmutable
        {
            public Props(AppDispatcher dispatcher, MessageWriterStore store)
            {
                this.CtorSet(_ => _.Dispatcher, dispatcher);
                this.CtorSet(_ => _.Store, store);
            }

            public AppDispatcher Dispatcher { get; }
            public MessageWriterStore Store { get; }
        }

        public class State : IAmImmutable
        {
            public State(MessageEditState message, NonNullList<Saved<MessageId, MessageDetails>> messageHistory)
            {
                this.CtorSet(_ => _.Message, message);
                this.CtorSet(_ => _.MessageHistory, messageHistory);
            }

            public MessageEditState Message { get; }
            public NonNullList<Saved<MessageId, MessageDetails>> MessageHistory { get; }
        }
    }
}
