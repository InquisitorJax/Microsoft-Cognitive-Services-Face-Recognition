using Prism.Mvvm;

namespace Xamarin.Forms.MCS.FaceRecog.Model
{
    public class UserData : BindableBase
    {
        private string _abilities;
        private string _alias;
        private string _email;
        private byte[] _mug;
        private string _phone;

        public string Abilities
        {
            get { return _abilities; }
            set { SetProperty(ref _abilities, value); }
        }

        public string Alias
        {
            get { return _alias; }
            set { SetProperty(ref _alias, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public byte[] Mug
        {
            get { return _mug; }
            set { SetProperty(ref _mug, value); }
        }

        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }
    }
}