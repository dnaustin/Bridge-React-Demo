using ProductiveRage.Immutable;

namespace Bridge_React_Demo.ViewModels
{
    public class MessageDetails : IAmImmutable
    {
        public MessageDetails(NonBlankTrimmedString title, NonBlankTrimmedString content)
        {
            this.CtorSet(_ => _.Title, title);
            this.CtorSet(_ => _.Content, content);
        }

        public NonBlankTrimmedString Title { get; }
        public NonBlankTrimmedString Content { get; }
    }
}
