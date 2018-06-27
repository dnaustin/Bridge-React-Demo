using Bridge.React;
using Bridge_React_Demo.API;
using Bridge_React_Demo.ViewModels;
using ProductiveRage.Immutable;
using System;

namespace Bridge_React_Demo.Components
{
    public class MessageEditor : StatelessComponent<MessageEditor.Props>
    {
        public MessageEditor(
            MessageEditState message,
            Action<MessageEditState> onChange,
            Action onSave,
            Optional<NonBlankTrimmedString> className = new Optional<NonBlankTrimmedString>()) 
            : base(new Props(className, message, onChange, onSave)) { }

        public override ReactElement Render()
        {
            var formIsInvalid = this.props.Message.Title.ValidationError.IsDefined || this.props.Message.Content.ValidationError.IsDefined;

            return DOM.FieldSet(new FieldSetAttributes { ClassName = this.props.ClassName.ToStringIfDefined() },
                DOM.Legend(null, this.props.Message.Caption),
                DOM.Span(new Attributes { ClassName = "label" }, "Title"),
                new ValidatedTextInput(                
                    className: new NonBlankTrimmedString("title"),
                    content: this.props.Message.Title.Text,
                    disabled: this.props.Message.IsSaveInProgress,
                    onChange: newTitle => this.props.OnChange(this.props.Message.With(_ => _.Title, new TextEditState(newTitle))),
                    validationMessage: this.props.Message.Title.ValidationError
                ),
                DOM.Span(new Attributes { ClassName = "label" }, "Content"),
                new ValidatedTextInput(
                    className: new NonBlankTrimmedString("content"),
                    content: this.props.Message.Content.Text,
                    disabled: this.props.Message.IsSaveInProgress,
                    onChange: newContent => this.props.OnChange(this.props.Message.With(_ => _.Content, new TextEditState(newContent))),
                    validationMessage: this.props.Message.Content.ValidationError
                ),
                DOM.Button(
                    new ButtonAttributes
                    {
                        Disabled = formIsInvalid || this.props.Message.IsSaveInProgress
                        , OnClick = e => this.props.OnSave()
                    }, 
                    "Save"
                )
            );
        }

        public class Props : IAmImmutable
        {
            public Props(Optional<NonBlankTrimmedString> className, MessageEditState message, Action<MessageEditState> onChange, Action onSave)
            {
                this.CtorSet(_ => _.ClassName, className);
                this.CtorSet(_ => _.Message, message);
                this.CtorSet(_ => _.OnChange, onChange);
                this.CtorSet(_ => _.OnSave, onSave);
            }

            public Optional<NonBlankTrimmedString> ClassName { get; }
            public MessageEditState Message { get; }
            public Action<MessageEditState> OnChange { get; }
            public Action OnSave { get; }
        }
    }
}
