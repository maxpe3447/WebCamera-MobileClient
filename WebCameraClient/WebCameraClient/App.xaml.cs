using Prism;
using Prism.Ioc;
using Prism.Unity;
using System;
using WebCameraClient.Services.IPEndPointFinder;
using WebCameraClient.Services.TCPClientManager;
using WebCameraClient.Services.UDPClientManager;
using WebCameraClient.ViewModel;
using WebCameraClient.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebCameraClient
{
    public partial class App

    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {

        }
        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync(nameof(MainPage));
            //await NavigationService.NavigateAsync(nameof(Views.MonitorPageView));
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigarion
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MonitorPageView, MonitorPageViewModel>();

            //Services
            containerRegistry.RegisterInstance<ITCPClientManager>(Container.Resolve<TCPClientManager>());
            containerRegistry.RegisterInstance<IIPEndPointFinder>(Container.Resolve<IPEndPointFinder>());
            containerRegistry.RegisterInstance<IUDPClientManager>(Container.Resolve<UDPClientManager>());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
