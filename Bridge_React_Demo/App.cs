using System.Linq;
using Bridge;
using Bridge.Html5;
using Bridge.React;
using Bridge_React_Demo.Actions;
using Bridge_React_Demo.API;
using Bridge_React_Demo.Components;
using Bridge_React_Demo.Stores;

namespace Bridge_React_Demo
{
    public class App
    {
        [Ready]
        public static void Main()
        {
            var container = Document.GetElementById("main");
            container.ClassName = string.Join(" ", container.ClassName.Split().Where(c => c != "loading"));

            var dispatcher = new AppDispatcher();
            var messageApi = new MessageApi(dispatcher);
            var store = new MessageWriterStore(messageApi, dispatcher);

            React.Render(
                new AppContainer(dispatcher, store),
                container
            );

            // Need to initialise the store so the AppContainer begins rendering
            dispatcher.Dispatch(new StoreInitialised(store));
        }
    }
}
