using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Views;

namespace NavigationAndDialogWpf.Helpers
{
    public class NavigationService : INavigationService
    {
        /// <summary>
        /// The key that is returned by the <see cref="CurrentPageKey"/> property
        /// when the current Page is the root page.
        /// </summary>
        public const string RootPageKey = "-- ROOT --";

        /// <summary>
        /// The key that is returned by the <see cref="CurrentPageKey"/> property
        /// when the current Page is not found.
        /// This can be the case when the navigation wasn't managed by this NavigationService,
        /// for example when it is directly triggered in the code behind, and the
        /// NavigationService was not configured for this page type.
        /// </summary>
        public const string UnknownPageKey = "-- UNKNOWN --";

        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private readonly Stack<FrameworkElement> _stack = new Stack<FrameworkElement>();
        private IAnimationManager _animationManager;
        private ContentControl _frame;

        /// <summary>
        /// The key corresponding to the currently displayed page.
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_stack.Count == 1)
                    {
                        return RootPageKey;
                    }

                    if (_frame.Content == null)
                    {
                        return UnknownPageKey;
                    }

                    var currentType = _frame.Content.GetType();

                    if (_pagesByKey.All(p => p.Value != currentType))
                    {
                        return UnknownPageKey;
                    }

                    var item = _pagesByKey.FirstOrDefault(
                        i => i.Value == currentType);

                    return item.Key;
                }
            }
        }

        /// <summary>
        /// Adds a key/page pair to the navigation service.
        /// </summary>
        /// <param name="key">The key that will be used later
        /// in the <see cref="NavigateTo(string)"/> or <see cref="NavigateTo(string, object)"/> methods.</param>
        /// <param name="pageType">The type of the page corresponding to the key.</param>
        public void Configure(string key, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                {
                    throw new ArgumentException("This key is already used: " + key);
                }

                if (_pagesByKey.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException(
                        "This type is already configured with key " + _pagesByKey.First(p => p.Value == pageType).Key);
                }

                _pagesByKey.Add(
                    key,
                    pageType);
            }
        }

        /// <summary>
        /// If possible, discards the current page and displays the previous page
        /// on the navigation stack.
        /// </summary>
        public void GoBack()
        {
            CheckFrame();

            if (_stack.Count >= 2)
            {
                // Remove current content
                var oldContent = _stack.Pop();
                (oldContent as INavigable)?.OnNavigatingFrom();

                if (_animationManager == null)
                {
                    // Add the last entry in the stack to the UI
                    _frame.Content = _stack.Peek();
                    (_frame.Content as INavigable)?.OnNavigatedTo();
                }
                else
                {
                    var newContent = _stack.Peek();

                    _animationManager.Animate(
                        newContent,
                        old =>
                        {
                            (newContent as INavigable)?.OnNavigatedTo();
                        });
                }
            }
        }

        public void Initialize(
            FrameworkElement initialContent,
            ContentControl frame = null,
            IAnimationManager animationManager = null)
        {
            _frame = frame;
            _animationManager = animationManager;

            var content = initialContent;
            _stack.Push(content);
        }

        /// <summary>
        /// Displays a new page corresponding to the given key. 
        /// Make sure to call the <see cref="Configure"/>
        /// method first.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        /// <exception cref="ArgumentException">When this method is called for 
        /// a key that has not been configured earlier.</exception>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <summary>
        /// Displays a new page corresponding to the given key,
        /// and passes a parameter to the new page.
        /// Make sure to call the <see cref="Configure"/>
        /// method first.
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        /// that should be displayed.</param>
        /// <param name="parameter">The parameter that should be passed
        /// to the new page.</param>
        /// <exception cref="ArgumentException">When this method is called for 
        /// a key that has not been configured earlier.</exception>
        public virtual void NavigateTo(string pageKey, object parameter)
        {
            CheckFrame();

            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(
                        $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?",
                        nameof(pageKey));
                }

                // Create new UserControl
                var newContent = MakeControl(_pagesByKey[pageKey], parameter);
                _stack.Push(newContent);

                if (_animationManager == null)
                {
                    (_frame.Content as INavigable)?.OnNavigatingFrom();
                    _frame.Content = newContent;
                    (newContent as INavigable)?.OnNavigatedTo();
                }
                else
                {
                    _animationManager.Animate(
                        newContent,
                        oldContent =>
                        {
                            (oldContent as INavigable)?.OnNavigatingFrom();
                            (newContent as INavigable)?.OnNavigatedTo();
                        });
                }
            }
        }

        private void CheckFrame()
        {
            if (_frame == null
                && _animationManager == null)
            {
                throw new InvalidOperationException(
                    "No frame or animation manager found. Did you forget to call NavigationService.Initialize?");
            }
        }

        private FrameworkElement MakeControl(Type controlType, object parameter)
        {
            ConstructorInfo constructor;
            object[] parameters;

            if (parameter == null)
            {
                constructor = controlType.GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(c => !c.GetParameters().Any());

                parameters = new object[]
                {
                };
            }
            else
            {
                constructor = controlType.GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(
                        c =>
                        {
                            var p = c.GetParameters();
                            return p.Length == 1
                                   && p[0].ParameterType == parameter.GetType();
                        });

                parameters = new[]
                {
                    parameter
                };
            }

            var control = constructor?.Invoke(parameters) as FrameworkElement;
            return control;
        }
    }
}