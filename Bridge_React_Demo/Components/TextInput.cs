using System;
using Bridge.Html5;
using Bridge.React;
using ProductiveRage.Immutable;

namespace Bridge_React_Demo.Components
{
    public class TextInput : StatelessComponent<TextInput.Props>
    {
        public TextInput(
            string content,
            bool disabled,
            Action<string> onChange,
            Optional<NonBlankTrimmedString> className = new Optional<NonBlankTrimmedString>()) 
            : base(new Props(content, disabled, onChange, className)) { }

        public override ReactElement Render()
        {
            return DOM.Input(new InputAttributes
            {
                Type = InputType.Text,
                ClassName = this.props.ClassName.IsDefined ? this.props.ClassName.Value : null,
                Disabled = this.props.Disabled,
                Value = this.props.Content,
                OnChange = e => this.props.OnChange(e.CurrentTarget.Value)
            });
        }

        public class Props : IAmImmutable
        {
            public Props(
                string content,
                bool disabled,
                Action<string> onChange,
                Optional<NonBlankTrimmedString> className)
            {
                this.CtorSet(_ => _.Content, content);
                this.CtorSet(_ => _.Disabled, disabled);
                this.CtorSet(_ => _.OnChange, onChange);
                this.CtorSet(_ => _.ClassName, className);
            }

            public Optional<NonBlankTrimmedString> ClassName { get; }
            public string Content { get; }
            public bool Disabled { get; }
            public Action<string> OnChange { get; }            
        }
    }
}
