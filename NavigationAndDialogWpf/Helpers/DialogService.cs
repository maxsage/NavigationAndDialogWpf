using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;

namespace NavigationAndDialogWpf.Helpers
{
    public class DialogService : IDialogService
    {
        private IDialogWindow _window;

        public void Initialize(IDialogWindow window)
        {
            _window = window;
        }

        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            CheckWindow();
            var tcs = new TaskCompletionSource<bool>();
            var closing = false;

            _window.SetMessageInfo(
                new MessageInfo(message, title, true)
                {
                    ConfirmButtonText = buttonText,
                    Callback = r =>
                    {
                        if (closing)
                        {
                            return;
                        }

                        closing = true;
                        tcs.SetResult(true);
                        afterHideCallback?.Invoke();
                    }
                });

            return tcs.Task;
        }

        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            return ShowError(error.Message, title, buttonText, afterHideCallback);
        }

        public Task ShowMessage(string message, string title)
        {
            return ShowMessage(message, title, "OK", null);
        }

        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            CheckWindow();
            var tcs = new TaskCompletionSource<bool>();
            var closing = false;

            _window.SetMessageInfo(
                new MessageInfo(message, title, false)
                {
                    ConfirmButtonText = buttonText,
                    Callback = r =>
                    {
                        if (closing)
                        {
                            return;
                        }

                        closing = true;
                        tcs.SetResult(true);
                        afterHideCallback?.Invoke();
                    }
                });

            return tcs.Task;
        }

        public Task<bool> ShowMessage(
            string message,
            string title,
            string buttonConfirmText,
            string buttonCancelText,
            Action<bool> afterHideCallback)
        {
            CheckWindow();
            var tcs = new TaskCompletionSource<bool>();
            var closing = false;

            _window.SetMessageInfo(
                new MessageInfo(message, title, false)
                {
                    ConfirmButtonText = buttonConfirmText,
                    CancelButtonText = buttonCancelText,
                    Callback = r =>
                    {
                        if (closing)
                        {
                            return;
                        }

                        closing = true;
                        tcs.SetResult(r);
                        afterHideCallback?.Invoke(r);
                    }
                });

            return tcs.Task;
        }

        public Task ShowMessageBox(string message, string title)
        {
            return ShowMessage(message, title);
        }

        private void CheckWindow()
        {
            if (_window == null)
            {
                throw new InvalidOperationException(
                    "No Window to show message. Did you forget to call DialogService.Initialize?");
            }
        }
    }
}