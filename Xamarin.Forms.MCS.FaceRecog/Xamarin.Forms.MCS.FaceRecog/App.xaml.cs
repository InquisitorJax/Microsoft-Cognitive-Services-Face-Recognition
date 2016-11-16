using Xamarin.Forms.MCS.FaceRecog.Commands;
using Xamarin.Forms.MCS.FaceRecog.FaceApi;
using Xamarin.Forms.MCS.FaceRecog.Repo;

namespace Xamarin.Forms.MCS.FaceRecog
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            FaceClient.RegisterCommands();

            DependencyService.Register<IRecognizePersonCommand, RecognizePersonCommand>();
            DependencyService.Register<ITakePictureCommand, TakePictureCommand>();
            DependencyService.Register<IChoosePictureCommand, ChoosePictureCommand>();
            DependencyService.Register<IPersonStore, InMemoryPersonStore>();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}