using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace NavigationAndDialogWpf
{
   public partial class Page2
    {
        public Page2(string parameter)
        {
            InitializeComponent();
            NavigationParameter.Text = parameter;

            var nav = ServiceLocator.Current.GetInstance<INavigationService>();

            NavigateButton.Click += (s, e) =>
            {
                nav.NavigateTo(MainWindow.Page3Key);
            };

            NavigateBackButton.Click += (s, e) =>
            {
                nav.GoBack();
            };
        }
    }
}
