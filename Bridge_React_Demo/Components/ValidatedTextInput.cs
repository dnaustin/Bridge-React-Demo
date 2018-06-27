using Bridge.React;
using Bridge_React_Demo.API;
using ProductiveRage.Immutable;
using System;

namespace Bridge_React_Demo.Components
{
    public class ValidatedTextInput : StatelessComponent<ValidatedTextInput.Props>
    {
        public ValidatedTextInput(
            bool disabled,
            string content,
            Action<string> onChange,
            Optional<NonBlankTrimmedString> validationMessage,
            Optional<NonBlankTrimmedString> className = new Optional<NonBlankTrimmedString>()) 
            : base(new Props(className, content, disabled, onChange, validationMessage)) { }

        public override ReactElement Render()
        {
            var className = this.props.ClassName;
            if (this.props.ValidationMessage.IsDefined)
            {
                className = className.Add(" ", new NonBlankTrimmedString("invalid"));
            }

            return DOM.Span(new Attributes { ClassName = className.ToStringIfDefined() },
                new TextInput(
                    className: this.props.ClassName,                   
                    content: this.props.Content,
                    disabled: this.props.Disabled,
                    onChange: this.props.OnChange
                ),
                this.props.ValidationMessage.IsDefined
                    ? DOM.Span(
                        new Attributes { ClassName = "validation-message"},
                        this.props.ValidationMessage.ToStringIfDefined()
                    )
                    : null
            );
        }

        public class Props : IAmImmutable
        {
            public Props(
                Optional<NonBlankTrimmedString> className,
                string content,
                bool disabled,
                Action<string> onChange,
                Optional<NonBlankTrimmedString> validationMessage)
            {
                this.CtorSet(_ => _.ClassName, className);
                this.CtorSet(_ => _.Content, content);
                this.CtorSet(_ => _.Disabled, disabled);
                this.CtorSet(_ => _.OnChange, onChange);
                this.CtorSet(_ => _.ValidationMessage, validationMessage);
            }

            public Optional<NonBlankTrimmedString> ClassName { get; }
            public string Content { get; }
            public bool Disabled { get; }
            public Action<string> OnChange { get; }
            public Optional<NonBlankTrimmedString> ValidationMessage { get; }
        }
    }
}