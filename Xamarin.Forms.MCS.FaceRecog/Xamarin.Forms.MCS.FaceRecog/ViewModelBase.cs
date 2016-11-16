using Prism.Mvvm;

namespace Xamarin.Forms.MCS.FaceRecog
{
    public class ViewModelBase : BindableBase
    {
        private string _busyMessage;
        private bool _isBusy;

        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        protected void NotBusy()
        {
            BusyMessage = null;
            IsBusy = false;
        }

        protected void ShowBusy(string message)
        {
            IsBusy = true;
            BusyMessage = message;
        }
    }
}