using System;
using System.Collections.Generic;
using Bridge.React;
using Bridge_React_Demo.Actions;
using Bridge_React_Demo.API;
using Bridge_React_Demo.ViewModels;
using ProductiveRage.Immutable;

namespace Bridge_React_Demo.Stores
{
    public class MessageWriterStore
    {
        private Optional<API.RequestId> saveActionRequestId, lastDataUpdatedRequestId;

        public MessageWriterStore(IReadAndWriteMessages messageApi, AppDispatcher dispatcher)
        {
            if (messageApi == null)
            {
                throw new ArgumentNullException(nameof(messageApi));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            this.Message = GetInitialMessageEditState();
            this.MessageHistory = NonNullList<Saved<MessageId, MessageDetails>>.Empty;

            dispatcher.Receive(a => a
                .If<StoreInitialised>(
                  condition: action => (action.Store == this),
                  work: action => { }
                )
                .Else<UserEditRequested<MessageEditState>>(action =>
                {
                    Message = ValidateMessage(action.NewState);
                })
                .Else<SaveRequested<MessageDetails>>(action =>
                {
                    saveActionRequestId = messageApi.SaveMessage(action.Data);
                    Message = Message.With(_ => _.IsSaveInProgress, true);
                })
                .Else<SaveSucceeded>(
                  condition: action => (action.RequestId == this.saveActionRequestId),
                  work: action =>
                  {
                      this.saveActionRequestId = null;
                      this.Message = GetInitialMessageEditState();
                      this.lastDataUpdatedRequestId = messageApi.GetMessages();
                  }
                )
                .Else<DataUpdated<NonNullList<Saved<MessageId, MessageDetails>>>>(
                  condition: action => action.RequestId.IsEqualToOrComesAfter(this.lastDataUpdatedRequestId),
                  work: action =>
                  {
                      this.lastDataUpdatedRequestId = action.RequestId;
                      this.MessageHistory = action.Data;
                  }
                )
                .IfAnyMatched(() => Change?.Invoke())
              );
        }

        public event Action Change;
        public MessageEditState Message { get; private set; }
        public NonNullList<Saved<MessageId, MessageDetails>> MessageHistory { get; private set; }

        private readonly static NonBlankTrimmedString defaultCaption = new NonBlankTrimmedString("Untitled");
        private readonly static NonBlankTrimmedString noTitleWarning = new NonBlankTrimmedString("Must enter a title");
        private readonly static NonBlankTrimmedString noContentWarning = new NonBlankTrimmedString("Must enter message content");

        private static MessageEditState GetInitialMessageEditState()
        {
            return new MessageEditState(
              caption: defaultCaption,
              title: new TextEditState(string.Empty, noTitleWarning),
              content: new TextEditState(string.Empty, noContentWarning),
              isSaveInProgress: false
            );
        }

        private static MessageEditState ValidateMessage(MessageEditState message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }               

            return message
              .With(_ => _.Caption, ToNonBlankTrimmedString(message.Title, defaultCaption))
              .With(_ => _.Title, Validate(message.Title, MustHaveValue, noTitleWarning))
              .With(_ => _.Content, Validate(message.Content, MustHaveValue, noContentWarning));
        }

        private static NonBlankTrimmedString ToNonBlankTrimmedString(
          TextEditState textEditState,
          NonBlankTrimmedString fallback)
        {
            if (textEditState == null)
            {
                throw new ArgumentNullException(nameof(textEditState));
            }
                
            if (fallback == null)
            {
                throw new ArgumentNullException(nameof(fallback));
            }             

            return (textEditState.Text.Trim() == string.Empty)
              ? fallback
              : new NonBlankTrimmedString(textEditState.Text);
        }

        private static TextEditState Validate(
          TextEditState textEditState,
          Predicate<TextEditState> validIf,
          NonBlankTrimmedString messageIfInvalid)
        {
            if (textEditState == null)
            {
                throw new ArgumentNullException(nameof(textEditState));
            }
                
            if (validIf == null)
            {
                throw new ArgumentNullException(nameof(validIf));
            }
                
            if (messageIfInvalid == null)
            {
                throw new ArgumentNullException(nameof(messageIfInvalid));
            }                

            return textEditState.With(_ => _.ValidationError, validIf(textEditState)
              ? null
              : messageIfInvalid);
        }

        private static bool MustHaveValue(TextEditState textEditState)
        {
            if (textEditState == null)
            {
                throw new ArgumentNullException(nameof(textEditState));
            }
                
            return !string.IsNullOrWhiteSpace(textEditState.Text);
        }

    }
}