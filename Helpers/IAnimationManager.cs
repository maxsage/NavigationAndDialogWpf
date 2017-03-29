using System;
using System.Windows;

namespace NavigationAndDialogWpf.Helpers
{
    public interface IAnimationManager
    {
        void Animate(
            FrameworkElement newContent,
            Action<FrameworkElement> callbackWithOldContent);
    }
}