using System;
using System.Windows;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace NavigationAndDialogWpf
{
    public partial class Page1
    {
        public Page1()
        {
            InitializeComponent();

            NavigateButton.Click += (s, e) =>
            {
                var nav = ServiceLocator.Current.GetInstance<INavigationService>();
                nav.NavigateTo(MainWindow.Page2Key, $"Navigated at {DateTime.Now.ToLongTimeString()}");
            };

            MessageButton.Click += (s, e) =>
            {
                var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                dialog.ShowMessage(
                    "This is a dialog",
                    "Dialog title",
                    "Got it!",
                    null);
            };

            ConfirmButton.Click += (s, e) =>
            {
                var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                dialog.ShowMessage(
                    "Confirm that you want this!",
                    "Please conform",
                    "Yes please!",
                    "No way!",
                    result =>
                    {
                        MessageBox.Show("The result is " + result);
                    });
            };

            ErrorButton.Click += (s, e) =>
            {
                var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                dialog.ShowError(
                    "This is an error",
                    "Error title",
                    "Too bad...",
                    null);
            };
        }
    }
}
