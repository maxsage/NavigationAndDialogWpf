using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using NavigationAndDialogWpf.Helpers;

namespace NavigationAndDialogWpf
{
    public partial class MainWindow : IDialogWindow
    {
        public static readonly string Page2Key = typeof(Page2).FullName;
        public static readonly string Page3Key = typeof(Page3).FullName;

        public MainWindow()
        {
            InitializeComponent();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var nav = new NavigationService();
            nav.Initialize((FrameworkElement)LayoutRoot.Content, LayoutRoot);
            nav.Configure(Page2Key, typeof(Page2));
            nav.Configure(Page3Key, typeof(Page3));
            SimpleIoc.Default.Register<INavigationService>(() => nav);

            var dialog = new DialogService();
            dialog.Initialize(this);
            SimpleIoc.Default.Register<IDialogService>(() => dialog);
        }

        public void SetMessageInfo(MessageInfo messageInfo)
        {
            MessageWindow.DataContext = messageInfo;
            MessageWindow.Show();
        }
    }
}