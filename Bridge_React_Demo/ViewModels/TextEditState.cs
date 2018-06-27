using ProductiveRage.Immutable;

namespace Bridge_React_Demo.ViewModels
{
    public class TextEditState : IAmImmutable
    {
        public TextEditState(
            string text,
            Optional<NonBlankTrimmedString> validationError = new Optional<NonBlankTrimmedString>())
        {
            this.CtorSet(_ => _.Text, text);
            this.CtorSet(_ => _.ValidationError, validationError);
        }

        public string Text { get; }
        public Optional<NonBlankTrimmedString> ValidationError { get; }
    }
}
