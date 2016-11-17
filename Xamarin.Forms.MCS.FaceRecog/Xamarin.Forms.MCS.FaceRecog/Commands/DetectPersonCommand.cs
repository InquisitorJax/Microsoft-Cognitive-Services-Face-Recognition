using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms.MCS.FaceRecog.FaceApi;
using Xamarin.Forms.MCS.FaceRecog.Model;

namespace Xamarin.Forms.MCS.FaceRecog.Commands
{

    public interface IDetectPersonCommand : IAsyncLogicCommand<RecognizePersonContext, DetectPersonResult>
    {
    }

    public class DetectPersonCommand : AsyncLogicCommand<RecognizePersonContext, DetectPersonResult>, IDetectPersonCommand
    {
        private IChoosePictureCommand ChoosePicture
        {
            get { return DependencyService.Get<IChoosePictureCommand>(); }
        }

        private ITakePictureCommand TakePicture
        {
            get { return DependencyService.Get<ITakePictureCommand>(); }
        }

        private IDetectFaceCommand DetectFace
        {
            get { return DependencyService.Get<IDetectFaceCommand>(); }
        }

        public override async Task<DetectPersonResult> ExecuteAsync(RecognizePersonContext request)
        {
            var retResult = new DetectPersonResult();

            SelectPictureResult pictureResult;
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
                var faceContext = new DetectFaceContext { FaceImage = pictureResult.Image };
                var recogResult = await DetectFace.ExecuteAsync(faceContext);

                retResult.Notification.AddRange(recogResult.Notification);

                if (retResult.IsValid())
                {
                    retResult.FaceData = recogResult.Face;
                }
            }

            return retResult;
        }
    }

    public class DetectPersonResult : CommandResult
    {
        public FaceData FaceData { get; set; }   
    }

}
