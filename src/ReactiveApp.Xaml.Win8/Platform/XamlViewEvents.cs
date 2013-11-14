﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Platform;

#if WINDOWS_PHONE
using System.Windows;
#else
using Windows.UI.Xaml;
#endif

namespace ReactiveApp.Xaml
{
    internal class XamlViewEvents : IViewEvents
    {
        /// <summary>
        /// A singleton instance of the <see cref="IApplicationStorage"/>.
        /// </summary>
        private static Lazy<IViewEvents> instance = new Lazy<IViewEvents>(() => new XamlViewEvents());

        /// <summary>
        /// A singleton instance of the <see cref="IApplicationStorage"/>.
        /// </summary>
        internal static IViewEvents Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private static readonly DependencyProperty PreviouslyAttachedProperty = 
            DependencyProperty.RegisterAttached(
                "PreviouslyAttached",
                typeof(bool),
                typeof(XamlViewEvents),
                null
            );

        /// <summary>
        /// Executes the handler the fist time the view is loaded.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        public IObservable<object> OnFirstLoaded(object view)
        {
            var element = view as FrameworkElement;
            if (element != null && !(bool)element.GetValue(PreviouslyAttachedProperty))
            {
                element.SetValue(PreviouslyAttachedProperty, true);
                return Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(h => element.Loaded += h, h => element.Loaded -= h).FirstOrDefaultAsync().Select(ep => ep.Sender);
            }
            else
            {
                return Observable.Empty<object>();
            }
        }

        /// <summary>
        /// Executes the handler the next time the view's LayoutUpdated event fires.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        public IObservable<object> OnLayoutUpdated(object view)
        {
            var element = view as FrameworkElement;
            if (element != null)
            {
#if WINDOWS_PHONE
                return Observable.FromEventPattern(h => element.LayoutUpdated += h, h => element.LayoutUpdated -= h).FirstOrDefaultAsync().Select(ep => ep.Sender);
#else
                return Observable.FromEventPattern<object>(h => element.LayoutUpdated += h, h => element.LayoutUpdated -= h).FirstOrDefaultAsync().Select(ep => ep.Sender);
#endif
            }
            else
            {
                return Observable.Empty<object>();
            }
        }
    }
}
