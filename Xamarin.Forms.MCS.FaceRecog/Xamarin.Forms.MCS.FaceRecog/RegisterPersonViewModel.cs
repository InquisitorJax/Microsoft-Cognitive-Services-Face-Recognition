using Prism.Commands;
using System.Windows.Input;
using Xamarin.Forms.MCS.FaceRecog.Commands;
using Xamarin.Forms.MCS.FaceRecog.FaceApi;
using Xamarin.Forms.MCS.FaceRecog.Model;

namespace Xamarin.Forms.MCS.FaceRecog
{
    public class RegisterPersonViewModel : ViewModelBase
    {
        private string _message;
        private Mutant _model;
        private bool _useChoosePicture;

        public RegisterPersonViewModel()
        {
            RegisterPersonCommand = new DelegateCommand(RegisterPerson);
            SelectPictureCommand = new DelegateCommand(SelectPicture);
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public Mutant Model
        {
            get
            {
                if (_model == null)
                    Model = new Mutant();
                return _model;
            }
            set { SetProperty(ref _model, value); }
        }

        public ICommand RegisterPersonCommand { get; }

        public ICommand SelectPictureCommand { get; }

        public bool UseChoosePicture
        {
            get { return _useChoosePicture; }
            set { SetProperty(ref _useChoosePicture, value); }
        }

        private IChoosePictureCommand ChoosePicture
        {
            get { return DependencyService.Get<IChoosePictureCommand>(); }
        }

        private IRegisterFaceCommand RegisterFace
        {
            get { return DependencyService.Get<IRegisterFaceCommand>(); }
        }

        private ITakePictureCommand TakePicture
        {
            get { return DependencyService.Get<ITakePictureCommand>(); }
        }

        private async void RegisterPerson()
        {
            Message = null;
            ShowBusy("commencing mutant registration...");

            try
            {
                if (Model.Data.Mug == null)
                {
                    Message = "Please add a picture of the mutant first";
                    return;
                }

                if (string.IsNullOrEmpty(Model.Name))
                {
                    Message = "Please add a name for the mutant";
                    return;
                }

                var context = new RegisterFaceContext
                {
                    GroupId = Constants.GroupId,
                    GroupName = Constants.GroupName,
                    PersonName = Model.Name,
                    Data = Model.Data
                };
                var result = await RegisterFace.ExecuteAsync(context);

                if (result.IsValid())
                {
                    Message = "Register Success!";
                }
                else
                {
                    Message = result.Notification.ToString();
                }
            }
            finally
            {
                NotBusy();
            }
        }

        private async void SelectPicture()
        {
            SelectPictureResult pictureResult = null;

            if (UseChoosePicture)
            {
                var request = new ChoosePictureRequest { MaxPixelDimension = 500 };
                pictureResult = await ChoosePicture.ExecuteAsync(request);
            }
            else
            {
                var request = new TakePictureRequest { MaxPixelDimension = 500, CameraOption = CameraOption.Back };
                pictureResult = await TakePicture.ExecuteAsync(request);
            }

            if (pictureResult.IsValid())
            {
                Model.Data.Mug = pictureResult.Image;
            }
            else
            {
                Message = pictureResult.Notification.ToString();
            }
        }
    }
}