using Microsoft.ProjectOxford.Face;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms.MCS.FaceRecog.Model;

namespace Xamarin.Forms.MCS.FaceRecog.FaceApi
{
    public interface IDetectFaceCommand : IAsyncLogicCommand<DetectFaceContext, DetectFaceResult>
    {
    }

    public class DetectFaceCommand : AsyncLogicCommand<DetectFaceContext, DetectFaceResult>, IDetectFaceCommand
    {
        public override async Task<DetectFaceResult> ExecuteAsync(DetectFaceContext request)
        {
            var retResult = new DetectFaceResult();
            try
            {
                var keyProvider = DependencyService.Get<IApiKeyProvider>();
                string faceKey = keyProvider.GetApiKey(ApiKeyType.FaceApi);

                var faceServiceClient = new FaceServiceClient(faceKey);
                using (MemoryStream ms = new MemoryStream(request.FaceImage))
                {
                    // detect faces in image
                    var faces = await faceServiceClient.DetectAsync(ms);
                    var face = faces.FirstOrDefault();

                    if (face != null)
                    {
                        retResult.Face = new FaceData();
                        if (face.FaceAttributes != null)
                        {                            
                            retResult.Face.Age = face.FaceAttributes.Age;
                            retResult.Face.Gender = face.FaceAttributes.Gender;
                            retResult.Face.Glasses = face.FaceAttributes.Glasses.ToString();
                        }
                        else
                        {
                            retResult.Notification.Add("Face Attributes could not be found");
                        }
                    }
                    else
                    {
                        retResult.Notification.Add("No face found");
                    }
                }
            }
            catch (Exception ex)
            {
                retResult.Notification.Add("Detect Face Failed! " + ex.Message);
            }
            return retResult;
        }
    }

    public class DetectFaceContext
    {
        public byte[] FaceImage { get; set; }
    }

    public class DetectFaceResult : CommandResult
    {
        public FaceData Face { get; set; }
    }
}