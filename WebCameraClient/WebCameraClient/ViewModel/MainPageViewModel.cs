using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Input;
using WebCameraClient.Services.TCPClientManager;
using WebCameraClient.Views;

namespace WebCameraClient.ViewModel
{
    internal class MainPageViewModel : ViewModelBase
    {
        
        public MainPageViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            
        }

        private ICommand _connectionCommand;
        public ICommand ConnectionCommand => _connectionCommand ??= new DelegateCommand(ConnectionCommandRelease);

        private ICommand _scanningCommand;
        public ICommand ScanningCommand => _scanningCommand ??= new DelegateCommand(ScanningCommandRelease);

        private async void ConnectionCommandRelease()
        {
            if(!IPAddress.TryParse(IpAdress, out IPAddress ip))
            {
                return;
            }
            if(!int.TryParse(Port, out int port)){
                return;
            }
            NavigationParameters parametrs = new NavigationParameters();
            parametrs.Add(nameof(IpAdress), IpAdress);
            parametrs.Add(nameof(Port), Port);

            await NavigationService.NavigateAsync(nameof(MonitorPageView), parametrs);
        }

        private async void ScanningCommandRelease()
        {

            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            if (result != null)
            {
                //await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(result.Text);
                var res = result.Text.Split(':');
                IpAdress = res[0];
                Port = res[1];
            }
            //if (!IPAddress.TryParse(IpAdress, out IPAddress ip))
            //{
            //    return;
            //}
            //if (!int.TryParse(Port, out int port))
            //{
            //    return;
            //}


            //NavigationParameters parametrs = new NavigationParameters();
            //parametrs.Add(nameof(IpAdress), IpAdress);
            //parametrs.Add(nameof(Port), Port);

            //await NavigationService.NavigateAsync(nameof(MonitorPageView), parametrs);
        }

        private string _ipAdress;
        public string IpAdress
        {
            get => _ipAdress;
            set => SetProperty(ref _ipAdress, value);
        }

        private string _port;
        public string Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

    }
}
