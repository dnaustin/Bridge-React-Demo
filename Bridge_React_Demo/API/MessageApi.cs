using Bridge;
using Bridge.Html5;
using Bridge.React;
using Bridge_React_Demo.Actions;
using Bridge_React_Demo.ViewModels;
using ProductiveRage.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bridge_React_Demo.API
{
    public class MessageApi : IReadAndWriteMessages
    {
        private readonly AppDispatcher dispatcher;
        private readonly NonNullList<Saved<MessageId, MessageDetails>> messages;

        public MessageApi(AppDispatcher dispatcher)
        {
            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            this.messages = NonNullList<Saved<MessageId, MessageDetails>>.Empty;

            // To further mimic a server-based API (where other people may be recording messages
            // of their own), after a 10s delay a periodic task will be executed to retrieve a
            // new message
            Window.SetTimeout(
              () => Window.SetInterval(GetChuckNorrisFact, 5000),
              10000
            );
        }

        public RequestId GetMessages()
        {
            var requestId = new RequestId();

            Window.SetTimeout(
              () => this.dispatcher.Dispatch(DataUpdated.For(requestId, messages)),
              1000 // Simulate a roundtrip to the server
            );

            return requestId;
        }

        public RequestId SaveMessage(MessageDetails message)
        {
            return SaveMessage(message, optionalSaveCompletedCallBack: null);
        }

        private RequestId SaveMessage(MessageDetails message, Action optionalSaveCompletedCallBack)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var requestId = new RequestId();
            Window.SetTimeout(
                () =>
                {
                    this.messages.Add(Saved.For((MessageId)(int)messages.Count, message));
                    this.dispatcher.Dispatch(new SaveSucceeded(requestId));
                    optionalSaveCompletedCallBack?.Invoke();
                },
                1000 // Simulate a roundtrip to the server
                );

            return requestId;
        }

        private void GetChuckNorrisFact()
        {
            var request = new XMLHttpRequest
            {
                ResponseType = XMLHttpRequestResponseType.Json
            };
            request.OnReadyStateChange = () =>
            {
                if (request.ReadyState != AjaxReadyState.Done)
                    return;

                if ((request.Status == 200) || (request.Status == 304))
                {
                    try
                    {
                        var apiResponse = (ChuckNorrisFactApiResponse)request.Response;
                        if ((apiResponse.Type == "success")
                        && (apiResponse.Value != null)
                        && !string.IsNullOrWhiteSpace(apiResponse.Value.Joke))
                        {
                            // The Chuck Norris Facts API (http://www.icndb.com/api/) returns strings
                            // html-encoded, so they need decoding before be wrapped up in a
                            // MessageDetails instance
                            SaveMessage(new MessageDetails(
                                title: new NonBlankTrimmedString("Fact"),
                                content: new NonBlankTrimmedString(HtmlDecode(apiResponse.Value.Joke))
                            ),
                            () => GetMessages()
                            );
                            return;
                        }
                    }
                    catch
                    {
                        // Ignore any error and drop through to the fallback message-generator below
                    }
                }

                SaveMessage(new MessageDetails(
                    title: new NonBlankTrimmedString("Fact"),
                    content: new NonBlankTrimmedString("API call failed when polling for server content :(")
                ));
            };
            request.Open("GET", "http://api.icndb.com/jokes/random");
            request.Send();
        }

        private string HtmlDecode(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
                
            var wrapper = Document.CreateElement("div");
            wrapper.InnerHTML = value;
            return wrapper.TextContent;
        }

        [IgnoreCast]
        private class ChuckNorrisFactApiResponse
        {
            public extern string Type { [Template("type")] get; }
            public extern FactDetails Value { [Template("value")] get; }

            [IgnoreCast]
            public class FactDetails
            {
                public extern int Id { [Template("id")] get; }
                public extern string Joke { [Template("joke")]get; }
            }
        }
    }
}
