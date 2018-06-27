using Bridge_React_Demo.ViewModels;

namespace Bridge_React_Demo.API
{
    public interface IReadAndWriteMessages
    {
        RequestId SaveMessage(MessageDetails message);
        RequestId GetMessages();
    }
}
