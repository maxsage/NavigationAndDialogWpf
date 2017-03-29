using System;

namespace NavigationAndDialogWpf.Helpers
{
    public class MessageInfo
    {
        public Action<bool> Callback
        {
            get;
            set;
        }

        public string CancelButtonText
        {
            get;
            set;
        }

        public string ConfirmButtonText
        {
            get;
            set;
        }

        public bool IsError
        {
            get;
        }

        public string Message
        {
            get;
        }

        public string Title
        {
            get;
        }

#if DEBUG
        public MessageInfo()
        {
            Title = "Dummy Title";
            Message = "This is a dummy message";
            ConfirmButtonText = "YES";
            IsError = true;
        }
#endif

        public MessageInfo(string message, string title, bool isError)
        {
            Message = message;
            Title = title;
            IsError = isError;
        }
    }
}