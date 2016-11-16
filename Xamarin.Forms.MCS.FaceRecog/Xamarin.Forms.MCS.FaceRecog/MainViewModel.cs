using Prism.Commands;
using System.Windows.Input;
using Xamarin.Forms.MCS.FaceRecog.Commands;
using Xamarin.Forms.MCS.FaceRecog.FaceApi;
using Xamarin.Forms.MCS.FaceRecog.Model;

namespace Xamarin.Forms.MCS.FaceRecog
{
    public class MainViewModel : ViewModelBase
    {
        private string _message;
        private Mutant _searchResult;

        private bool _useCamera;

        private bool _useFrontCamera;

        public MainViewModel()
        {
            AddNewPersonCommand = new DelegateCommand(AddNewPerson);
            RecognizePersonCommand = new DelegateCommand(RecognizePerson);
            DeleteAllPeopleCommand = new DelegateCommand(DeleteAllPeople);
        }

        public ICommand AddNewPersonCommand { get; }

        public ICommand DeleteAllPeopleCommand { get; }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ICommand RecognizePersonCommand { get; }

        public Mutant SearchResult
        {
            get { return _searchResult; }
            set { SetProperty(ref _searchResult, value); }
        }

        public bool UseCamera
        {
            get { return _useCamera; }
            set { SetProperty(ref _useCamera, value); }
        }

        public bool UseFrontCamera
        {
            get { return _useFrontCamera; }
            set { SetProperty(ref _useFrontCamera, value); }
        }

        private IDeletePersonGroupCommand DeleteGroup
        {
            get { return DependencyService.Get<IDeletePersonGroupCommand>(); }
        }

        private IFindGroupPeopleCommand FindPeople
        {
            get { return DependencyService.Get<IFindGroupPeopleCommand>(); }
        }

        private IRecognizePersonCommand RecognizeCommand
        {
            get { return DependencyService.Get<IRecognizePersonCommand>(); }
        }

        private async void AddNewPerson()
        {
            await App.Current.MainPage.Navigation.PushAsync(new RegisterPersonPage());
        }

        private async void DeleteAllPeople()
        {
            ShowBusy("deleting...");

            try
            {
                var peopleResult = await DeleteGroup.ExecuteAsync(new DeletePersonGroupContext { GroupId = Constants.GroupId });

                if (peopleResult.IsValid())
                {
                    Message = "People Deleted!";
                }
                else
                {
                    Message = peopleResult.Notification.ToString();
                }
            }
            finally
            {
                NotBusy();
            }
        }

        private async void FindAllPeople()
        {
            ShowBusy("finding...");

            try
            {
                var peopleResult = await FindPeople.ExecuteAsync(new FindGroupPeopleContext { GroupId = Constants.GroupId });

                if (peopleResult.IsValid())
                {
                    Message = $"Found {peopleResult.People.Count} people already in the group!";
                }
                else
                {
                    Message = peopleResult.Notification.ToString();
                }
            }
            finally
            {
                NotBusy();
            }
        }

        private async void RecognizePerson()
        {
            ShowBusy("recognizing...");

            Message = null;

            try
            {
                var context = new RecognizePersonContext { UseCamera = UseCamera };
                if (UseCamera)
                {
                    context.CameraOption = UseFrontCamera ? CameraOption.Front : CameraOption.Back;
                }
                var result = await RecognizeCommand.ExecuteAsync(context);

                if (result.IsValid())
                {
                    SearchResult = result.Person;
                    Message = $"Hello {result.Person.Name}!";
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
    }
}