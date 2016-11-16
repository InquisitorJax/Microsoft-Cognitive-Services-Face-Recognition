using Microsoft.ProjectOxford.Face;

namespace Xamarin.Forms.MCS.FaceRecog.FaceApi
{
    public class FaceClient
    {
        public static void RegisterCommands()
        {
            //https://www.microsoft.com/cognitive-services/en-us/face-api/documentation/get-started-with-face-api/GettingStartedwithFaceAPIinCSharp
            //https://blog.xamarin.com/adding-facial-recognition-to-your-mobile-apps/

            //TODO: implement your own implementation of ApiKeyProvider that returns keys you register at https://www.microsoft.com/cognitive-services/en-us/sign-up
            DependencyService.Register<IApiKeyProvider, ApiKeyProvider>();

            DependencyService.Register<IRegisterFaceCommand, RegisterFaceCommand>();
            DependencyService.Register<IRecognizeFaceCommand, RecognizeFaceCommand>();
            DependencyService.Register<IDetectFaceCommand, DetectFaceCommand>();
            DependencyService.Register<IDeletePersonGroupCommand, DeletePersonGroupCommand>();
            DependencyService.Register<IFindGroupPeopleCommand, FindGroupPeopleCommand>();
        }

        public static IFaceServiceClient Resolve()
        {
            var keyProvider = DependencyService.Get<IApiKeyProvider>();
            string faceKey = keyProvider.GetApiKey(ApiKeyType.FaceApi);

            var faceServiceClient = new FaceServiceClient(faceKey);

            return faceServiceClient;
        }
    }
}