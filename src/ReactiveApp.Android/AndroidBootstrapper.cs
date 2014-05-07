using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveApp.Android.Services;
using ReactiveApp.App;
using ReactiveApp.Exceptions;
using ReactiveApp.Services;
using ReactiveUI;
using Splat;

namespace ReactiveApp.Android
{
    public abstract class AndroidBootstrapper : ReactiveBootstrapper
    {
        private readonly Application application;

        protected AndroidBootstrapper(Application application)
        {
            Contract.Requires<ArgumentNullException>(application != null, "application");

            this.application = application;
        }

        public override void Run()
        {
            base.Run();

            IExceptionHandler handler = Locator.Current.GetService<IExceptionHandler>();
            if (handler != null)
            {
                handler.SetupErrorHandling(this.application);
            }
            ISuspensionService suspension = Locator.Current.GetService<ISuspensionService>();
            if (suspension != null)
            {
                suspension.SetupStartup();
            }
        }

        protected override void InitializePlatformServices()
        {
            this.InitializeSuspensionService();
            base.InitializePlatformServices();
        }

        protected virtual SuspensionService CreateSuspensionService()
        {
            return new SuspensionService(application);
        }

        protected virtual void InitializeSuspensionService()
        {
            var suspensionService = CreateSuspensionService();
            Locator.CurrentMutable.RegisterConstant<IAndroidCurrentActivity>(suspensionService);
            Locator.CurrentMutable.RegisterConstant<ISuspensionService>(suspensionService);
        }

        protected override IMainThreadDispatcher CreateMainThreadDispatcher()
        {
            return new AndroidMainThreadDispatcher();
        }

        protected override IViewDispatcher CreateViewDispatcher()
        {
            var dispatcher = Locator.Current.GetService<IMainThreadDispatcher>();
            var viewPresenter = this.CreateViewPresenter();
            return new AndroidViewDispatcher(dispatcher, viewPresenter);
        }

        protected virtual IViewPresenter CreateViewPresenter()
        {
            var currentActivity = Locator.Current.GetService<IAndroidCurrentActivity>();
            var requestTranslator = this.CreateViewModelRequestTranslator();
            return new AndroidViewPresenter(currentActivity, requestTranslator);
        }

        protected virtual IAndroidReactiveViewModelRequestTranslator CreateViewModelRequestTranslator()
        {
            var viewLocator = Locator.Current.GetService<ReactiveApp.Services.IViewLocator>();
            var navigationSerializer = Locator.Current.GetService<INavigationSerializer>();
            return new AndroidReactiveViewModelRequestTranslator(this.application, viewLocator, navigationSerializer);
        }
    }
}