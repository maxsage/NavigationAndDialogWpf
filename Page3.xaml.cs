using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using NavigationAndDialogWpf.Helpers;

namespace NavigationAndDialogWpf
{
    public partial class Page3 : INavigable
    {
        public Page3()
        {
            InitializeComponent();

            NavigateBackButton.Click += (s, e) =>
            {
                var nav = ServiceLocator.Current.GetInstance<INavigationService>();
                nav.GoBack();
            };
        }

        public void OnNavigatedTo()
        {
            Status.Text = "OnNavigatedTo was called";
        }

        public void OnNavigatingFrom()
        {
            // Do nothing
        }
    }
}
