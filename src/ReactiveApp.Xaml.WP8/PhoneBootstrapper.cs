﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ReactiveApp.Exceptions;
using ReactiveApp.Services;
using ReactiveApp.Xaml.Adapters;
using ReactiveApp.Xaml.Services;
using Splat;

namespace ReactiveApp.Xaml
{
    public abstract class PhoneBootstrapper : ReactiveBootstrapper
    {
        private readonly IArgumentsProvider arguments;
        private readonly PhoneApplicationFrame frame;

        protected PhoneBootstrapper(PhoneApplicationFrame frame, IArgumentsProvider arguments)
        {
            Contract.Requires<ArgumentNullException>(frame != null, "frame");
            Contract.Requires<ArgumentNullException>(arguments != null, "arguments");

            this.frame = frame;
            this.arguments = arguments;
        }

        public override void Run()
        {
            base.Run();

            IExceptionHandler handler = Locator.Current.GetService<IExceptionHandler>();
            if (handler != null)
            {
                handler.SetupErrorHandling();
            }
            ISuspensionService suspension = Locator.Current.GetService<ISuspensionService>();
            if (suspension != null)
            {
                suspension.SetupStartup();
            }
        }

        protected override void InitializePlatformServices()
        {
            base.InitializePlatformServices();

            InitializeSuspensionService();
            InitializeOrientationManager();
            InitializeNavigationSerializer();
        }

        protected virtual ISuspensionService CreateSuspensionService()
        {
            var phoneApplicationService = new PhoneApplicationService();
            Application.Current.ApplicationLifetimeObjects.Add(phoneApplicationService);
            return new SuspensionService(Application.Current, this.arguments);
        }

        protected virtual void InitializeSuspensionService()
        {
            var suspensionService = CreateSuspensionService();
            Locator.CurrentMutable.RegisterConstant<ISuspensionService>(suspensionService);
        }
        protected virtual IOrientationManager CreateOrientationManager()
        {
            var orientationManager = OrientationManager.Instance;
            OrientationManager.InternalInstance.Initialize(this.frame);
            return orientationManager;
        }

        protected virtual void InitializeOrientationManager()
        {
            var orientationManager = CreateOrientationManager();
            Locator.CurrentMutable.RegisterConstant<IOrientationManager>(orientationManager);
        }

        protected virtual INavigationSerializer CreateNavigationSerializer()
        {
            //TODO: default implementation
            return null;
        }

        protected virtual void InitializeNavigationSerializer()
        {
            var serializer = CreateNavigationSerializer();
            Locator.CurrentMutable.RegisterConstant<INavigationSerializer>(serializer);
        }

        protected override IMainThreadDispatcher CreateMainThreadDispatcher()
        {
            return new PhoneMainThreadDispatcher();
        }

        protected override IViewDispatcher CreateViewDispatcher()
        {
            var dispatcher = Locator.Current.GetService<IMainThreadDispatcher>();
            var viewPresenter = this.CreateViewPresenter(this.frame);
            return new PhoneViewDispatcher(dispatcher, viewPresenter);
        }

        protected virtual IViewPresenter CreateViewPresenter(PhoneApplicationFrame frame)
        {
            var requestTranslator = this.CreateViewModelRequestTranslator();
            return new PhoneViewPresenter(frame, requestTranslator);
        }

        protected virtual IPhoneReactiveViewModelRequestTranslator CreateViewModelRequestTranslator()
        {
            var viewLocator = Locator.Current.GetService<IViewLocator>();
            var navigationSerializer = Locator.Current.GetService<INavigationSerializer>();
            return new PhoneReactiveViewModelRequestTranslator(viewLocator, navigationSerializer);
        }
    }
}