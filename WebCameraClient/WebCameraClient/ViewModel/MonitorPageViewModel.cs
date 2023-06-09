using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WebCameraClient.Services.TCPClientManager;
using WebCameraClient.Services.UDPClientManager;
using Xamarin.Forms;

namespace WebCameraClient.ViewModel
{
    internal class MonitorPageViewModel : ViewModelBase
    {
        ITCPClientManager _tCPClientManager;
        IUDPClientManager _uDPClientManager;
        public MonitorPageViewModel(INavigationService navigationService,
                                    ITCPClientManager tCPClientManager,
                                    IUDPClientManager uDPClientManager
            ) 
            : base(navigationService)
        {
            _tCPClientManager = tCPClientManager;
            _uDPClientManager = uDPClientManager;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                IpAdress = parameters.GetValue<string>(nameof(IpAdress));
                Port = parameters.GetValue<string>(nameof(Port));

                //_tCPClientManager.Connected(IpAdress, int.Parse(Port));
                //_tCPClientManager.StartGetting(CaptureImage);
                _uDPClientManager.Connecting(IpAdress, int.Parse(Port));
                _uDPClientManager.StartReciving(CaptureImageHandler, InitCameraListHandler);
            }catch(Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(ex.Message);
            }
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            _uDPClientManager.SendDisconnectMe();
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

        private string _lText;
        public string LText
        {
            get => _lText;
            set => SetProperty(ref _lText, value);
        }
        
        private ImageSource _mainImg;
        public ImageSource MainImg
        {
            get => _mainImg;
            set => SetProperty(ref _mainImg, value);
        }
        private List<string> _camerasList;
        public List<string> CamerasList
        {
            get => _camerasList ??= new();
            set => SetProperty(ref _camerasList, value);
        }

        private string _selectedWebCamera;
        public string SelectedWebCamera
        {
            get => _selectedWebCamera;
            set => SetProperty(ref _selectedWebCamera, value, OnSelectedCameraChanged);
        }

        private ICommand _startRecordCommand;
        public ICommand StartRecordCommand
        {
            get => _startRecordCommand ??= new DelegateCommand(StartRecordCommandRelease);
        }
        private void StartRecordCommandRelease()
        {
            _uDPClientManager.SendStartRecording();
        }

        private ICommand _stopRecordCommand;
        public ICommand StopRecordCommand
        {
            get => _stopRecordCommand ??= new DelegateCommand(StopRecordCommandRelease);
        }
        private void StopRecordCommandRelease()
        {
            _uDPClientManager.SendStopRecording();
        }

        private ICommand _startShowCommand;
        public ICommand StartShowCommand
        {
            get => _startShowCommand ??= new DelegateCommand(StartShowCommandRelease);
        }
        private void StartShowCommandRelease()
        {
            _uDPClientManager.SendStartShow();
        }

        private ICommand _stopShowCommand;
        public ICommand StopShowCommand
        {
            get => _stopShowCommand ??= new DelegateCommand(StopShowCommandRelease);
        }
        private void StopShowCommandRelease()
        {
            _uDPClientManager.SendStopShow();
        }

        private ICommand _peopleSearchCommand;
        public ICommand PeopleSearchCommand
        {
            get => _peopleSearchCommand ??= new DelegateCommand(PeopleSearchCommandRelease);
        }
        private void PeopleSearchCommandRelease()
        {
            _uDPClientManager.SendSearchPeople();
        }
        private void CaptureImageHandler(MemoryStream stream, string text)
        {
            try
            {
                var img = ImageSource.FromStream(() => stream);
                MainImg = img;

                LText = $"Speed: {int.Parse(text)/1024}Mb/s";

            }catch(Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(ex.Message);
            }
        }

        private void InitCameraListHandler(List<string> cameras)
        {
            //Acr.UserDialogs.UserDialogs.Instance.Alert(String.Join("\n", cameras));
            CamerasList = new List<string>(cameras);
        }

        private void OnSelectedCameraChanged()
        {
            _uDPClientManager.SendCameraChange(SelectedWebCamera);
        }
    }
}
