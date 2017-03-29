using System;
using System.Windows;
using System.Windows.Media.Animation;
using NavigationAndDialogWpf.Helpers;

namespace NavigationAndDialogWpf
{
    public partial class MessageWindow
    {
        private MessageInfo Info => DataContext as MessageInfo;

        public MessageWindow()
        {
            InitializeComponent();
        }

        public void Close()
        {
            var sbd1 = (Storyboard)Resources["OpenBoxStoryboard"];
            sbd1.Stop();
            var sbd2 = (Storyboard)Resources["CloseBoxStoryboard"];
            sbd2.Completed += CloseStoryboardCompleted;
            sbd2.Begin();
        }

        public void Show()
        {
            MessageHost.Visibility = Visibility.Visible;

            var sbd1 = (Storyboard)Resources["CloseBoxStoryboard"];
            sbd1.Stop();
            var sbd2 = (Storyboard)Resources["OpenBoxStoryboard"];
            sbd2.Begin();
        }

        private void Cancel()
        {
            Close();
            Info?.Callback?.Invoke(false);
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        private void CloseStoryboardCompleted(object sender, EventArgs e)
        {
            var sbd2 = (Storyboard)Resources["CloseBoxStoryboard"];
            sbd2.Completed -= CloseStoryboardCompleted;
            MessageHost.Visibility = Visibility.Collapsed;
        }

        private void Confirm()
        {
            Close();
            Info?.Callback?.Invoke(true);
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            Confirm();
        }
    }
}