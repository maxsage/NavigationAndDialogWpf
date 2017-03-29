namespace NavigationAndDialogWpf.Helpers
{
    public interface INavigable
    {
        void OnNavigatedTo();
        void OnNavigatingFrom();
    }
}