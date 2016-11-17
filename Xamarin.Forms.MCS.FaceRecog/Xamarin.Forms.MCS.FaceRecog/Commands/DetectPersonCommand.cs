using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms.MCS.FaceRecog.FaceApi;
using Xamarin.Forms.MCS.FaceRecog.Model;

namespace Xamarin.Forms.MCS.FaceRecog.Commands
{
    public interface IDetectPersonCommand : IAsyncLogicCommand<DetectPersonContext, DetectPersonResult>
    {
    }

    public class DetectPersonCommand : AsyncLogicCommand<DetectPersonContext, DetectPersonResult>, IDetectPersonCommand
    {
        private IChoosePictureCommand ChoosePicture
        {
            get { return DependencyService.Get<IChoosePictureCommand>(); }
        }

        private IDetectFaceCommand DetectFace
        {
            get { return DependencyService.Get<IDetectFaceCommand>(); }
        }

        private ITakePictureCommand TakePicture
        {
            get { return DependencyService.Get<ITakePictureCommand>(); }
        }

        public override async Task<DetectPersonResult> ExecuteAsync(DetectPersonContext request)
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
                var faceContext = new DetectFaceContext
                {
                    FaceImage = pictureResult.Image,
                    DetectFaceAttributes = request.DetectFaceAttributes,
                    DetectFaceId = request.DetectFaceId,
                    DetectFaceLandmarks = request.DetectFaceLandmarks
                };
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

    public class DetectPersonContext : RecognizePersonContext
    {
        public DetectPersonContext()
        {
            DetectFaceAttributes = true;
        }

        public bool DetectFaceAttributes { get; set; }
        public bool DetectFaceId { get; set; }
        public bool DetectFaceLandmarks { get; set; }
    }

    public class DetectPersonResult : CommandResult
    {
        public FaceData FaceData { get; set; }
    }
}