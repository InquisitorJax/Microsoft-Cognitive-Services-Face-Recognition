using Newtonsoft.Json;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms.MCS.FaceRecog.FaceApi;
using Xamarin.Forms.MCS.FaceRecog.Model;
using Xamarin.Forms.MCS.FaceRecog.Repo;

namespace Xamarin.Forms.MCS.FaceRecog.Commands
{
    public interface IRecognizePersonCommand : IAsyncLogicCommand<RecognizePersonContext, RecognizePersonResult>
    {
    }

    public class RecognizePersonCommand : AsyncLogicCommand<RecognizePersonContext, RecognizePersonResult>, IRecognizePersonCommand
    {
        private IChoosePictureCommand ChoosePicture
        {
            get { return DependencyService.Get<IChoosePictureCommand>(); }
        }

        private IPersonStore PersonStore
        {
            get { return DependencyService.Get<IPersonStore>(DependencyFetchTarget.GlobalInstance); }
        }

        private IRecognizeFaceCommand RecognizeCommand
        {
            get { return DependencyService.Get<IRecognizeFaceCommand>(); }
        }

        private ITakePictureCommand TakePicture
        {
            get { return DependencyService.Get<ITakePictureCommand>(); }
        }

        public override async Task<RecognizePersonResult> ExecuteAsync(RecognizePersonContext request)
        {
            var retResult = new RecognizePersonResult();

            SelectPictureResult pictureResult = null;
            if (request.UseCamera)
            {
                var pictureRequest = new TakePictureRequest { CameraOption = request.CameraOption, MaxPixelDimension = 500 };
                pictureResult = await TakePicture.ExecuteAsync(pictureRequest);
            }
            else
            {
                var choosePictureRequest = new ChoosePictureRequest { MaxPixelDimension = 500 };
                pictureResult = await ChoosePicture.ExecuteAsync(choosePictureRequest);
            }

            retResult.Notification.AddRange(retResult.Notification);

            if (retResult.IsValid() && pictureResult.TaskResult == TaskResult.Success)
            {
                var faceContext = new RecognizeFaceContext { FaceImage = pictureResult.Image, GroupId = Constants.GroupId };
                var recogResult = await RecognizeCommand.ExecuteAsync(faceContext);

                retResult.Notification.AddRange(recogResult.Notification);

                if (retResult.IsValid())
                {
                    var personResult = await PersonStore.FindPersonAsync(recogResult.Person.PersonId.ToString());

                    if (personResult == null)
                    {
                        UserData userData = null;
                        if (recogResult.Person.UserData != null)
                        {
                            userData = JsonConvert.DeserializeObject<UserData>(recogResult.Person.UserData);
                        }
                        Mutant newPerson = new Mutant
                        {
                            Name = recogResult.Person.Name,
                            Id = recogResult.Person.PersonId.ToString(),
                            Data = userData
                        };
                        await PersonStore.AddPersonAsync(newPerson);
                        personResult = newPerson;
                    }

                    retResult.Person = personResult;
                }
            }

            return retResult;
        }
    }

    public class RecognizePersonContext
    {
        public CameraOption CameraOption { get; set; }
        public bool UseCamera { get; set; }
    }

    public class RecognizePersonResult : CommandResult
    {
        public Mutant Person { get; set; }
    }
}