using Bridge.React;
using Bridge_React_Demo.API;
using Bridge_React_Demo.ViewModels;
using ProductiveRage.Immutable;
using System;
using System.Linq;

namespace Bridge_React_Demo.Components
{
    public class MessageHistory : StatelessComponent<MessageHistory.Props>
    {
        public MessageHistory(
            NonNullList<Saved<MessageId, MessageDetails>> messages,
            Optional<NonBlankTrimmedString> className = new Optional<NonBlankTrimmedString>())
            : base(new Props(className, messages)) { }

        public override ReactElement Render()
        {
            Console.WriteLine("MessageHistory.Render");

            var className = this.props.ClassName;
            if (!this.props.Messages.Any())
            {
                className = className.Add(" ", new NonBlankTrimmedString("zero-messages"));
            }

            // Any time a set of child components is dynamically-created (meaning that the
            // numbers of items may vary from one render to another), each must have a unique
            // "Key" property set (this may be a int or a string). Here, this is simple as
            // each message tuple is a unique id and the contents of that message (and the
            // unique id is ideal for use as a unique "Key" property).
            var messageElements = props.Messages
              .Select(savedMessage => DOM.Div(new Attributes { Key = (int)savedMessage.Key },
                DOM.Span(new Attributes { ClassName = "title" }, savedMessage.Value.Title),
                DOM.Span(new Attributes { ClassName = "content" }, savedMessage.Value.Content)
              ));

            return DOM.FieldSet(new FieldSetAttributes { ClassName = className.ToStringIfDefined() },
              DOM.Legend(null, "Message History"),
              DOM.Div(null, messageElements)
            );
        }

        public class Props : IAmImmutable
        {
            public Props(Optional<NonBlankTrimmedString> className, NonNullList<Saved<MessageId, MessageDetails>> messages)
            {
                this.CtorSet(_ => _.ClassName, className);
                this.CtorSet(_ => _.Messages, messages);
            }

            public Optional<NonBlankTrimmedString> ClassName { get; }
            public NonNullList<Saved<MessageId, MessageDetails>> Messages { get; }
        }
    }
}
