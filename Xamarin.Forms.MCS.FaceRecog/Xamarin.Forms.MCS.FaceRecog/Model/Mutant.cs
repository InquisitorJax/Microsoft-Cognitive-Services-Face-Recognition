using Prism.Mvvm;

namespace Xamarin.Forms.MCS.FaceRecog.Model
{
    public class Mutant : BindableBase
    {
        private UserData _data;
        private string _id;

        private string _name;

        public UserData Data
        {
            get
            {
                if (_data == null)
                    Data = new UserData();
                return _data;
            }
            set { SetProperty(ref _data, value); }
        }

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}