﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.App;
using ReactiveApp.Exceptions;
using ReactiveApp.Interfaces;
using ReactiveApp.Settings;
using ReactiveUI;
using Splat;

namespace ReactiveApp
{
    public abstract class ReactiveBootstrapper : IEnableLogger
    {
        protected abstract IReactiveApplication CreateApplication();

        protected abstract IMainThreadDispatcher CreateMainThreadDispatcher();

        protected abstract IViewLocator CreateViewLocator();

        protected abstract IViewPresenter CreateViewPresenter();

        public virtual void Run()
        {
            this.InitializeDependencyResolver();
            this.Log().Info("Dependency Resolver initialized.");
            this.Log().Info("Before Bootstrapping");
            this.BeforeBootstrapping();
            this.Log().Info("Initializing Debug Services.");
            this.InitializeDebugServices();
            this.Log().Info("Initializing Platform Services.");
            this.InitializePlatformServices();
            this.Log().Info("Initializing Settings");
            this.InitializeSettings();
            this.Log().Info("Initializing MainThread Dispatcher");
            this.InitializeMainThreadDispatcher();
            this.Log().Info("Initializing Application");
            this.InitializeApplication();
            this.Log().Info("Initializing View Locator");
            this.InitializeViewLocator();
            this.Log().Info("Initializing View Presenter");
            this.InitializeViewPresenter();
            this.Log().Info("AFter Bootstrapping");
            this.AfterBootstrapping();
        }

        /// <summary>
        /// Befores the bootstrapping.
        /// </summary>
        protected virtual void BeforeBootstrapping()
        { }

        protected virtual void AfterBootstrapping()
        { }

        protected virtual void InitializeDebugServices()
        {
            
        }

        protected virtual void InitializePlatformServices()
        {
            // do nothing by default
        }

        protected virtual IReactiveAppSettings CreateSettings()
        {
            return new ReactiveAppSettings();
        }

        protected virtual void InitializeSettings()
        {
            var settings = this.CreateSettings();
            Locator.CurrentMutable.RegisterConstant<IReactiveAppSettings>(settings);
        }

        protected virtual IExceptionHandler CreateExceptionHandler()
        {
            return new DefaultExceptionHandler();
        }

        protected virtual void InitializeExceptionHandler()
        {
            var exceptionHandler = this.CreateExceptionHandler();
            Locator.CurrentMutable.RegisterConstant<IExceptionHandler>(exceptionHandler);
        }

        protected virtual void InitializeApplication()
        {
            var app = this.CreateApplication();
            app.Initialize();
            Locator.CurrentMutable.RegisterConstant<IReactiveApplication>(app);
        }

        protected virtual void InitializeMainThreadDispatcher()
        {
            var dispatcher = this.CreateMainThreadDispatcher();
            Locator.CurrentMutable.RegisterConstant<IMainThreadDispatcher>(dispatcher);
        }

        protected virtual void InitializeViewLocator()
        {
            var viewLocator = this.CreateViewLocator();
            Locator.CurrentMutable.RegisterConstant<IViewLocator>(viewLocator);
        }

        protected virtual void InitializeViewPresenter()
        {
            var viewPresenter = this.CreateViewPresenter();
            Locator.CurrentMutable.RegisterConstant<IViewPresenter>(viewPresenter);
        }

        protected virtual INavigationSerializer CreateNavigationSerializer()
        {
            return null;
        }

        protected virtual void InitializeNavigationSerializer()
        {
            var serializer = CreateNavigationSerializer();
            Locator.CurrentMutable.RegisterConstant<INavigationSerializer>(serializer);
        }

        protected virtual IDependencyResolver CreateDependencyResolver()
        {
            return Locator.Current;
        }

        protected virtual void InitializeDependencyResolver()
        {
            var resolver = this.CreateDependencyResolver();
            if(Locator.Current != resolver)
            {
                Locator.Current = resolver;
            }
            Locator.CurrentMutable.RegisterConstant<IDependencyResolver>(Locator.Current);
        }
    }
}
