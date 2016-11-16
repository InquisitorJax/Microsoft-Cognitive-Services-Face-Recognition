using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Xamarin.Forms.MCS.FaceRecog.FaceApi
{
    public interface IRecognizeFaceCommand : IAsyncLogicCommand<RecognizeFaceContext, RecognizeFaceResult>
    {
    }

    public class RecognizeFaceCommand : AsyncLogicCommand<RecognizeFaceContext, RecognizeFaceResult>, IRecognizeFaceCommand
    {
        public override async Task<RecognizeFaceResult> ExecuteAsync(RecognizeFaceContext request)
        {
            var retResult = new RecognizeFaceResult();
            try
            {
                var keyProvider = DependencyService.Get<IApiKeyProvider>();
                string faceKey = keyProvider.GetApiKey(ApiKeyType.FaceApi);

                var faceServiceClient = new FaceServiceClient(faceKey);
                using (MemoryStream ms = new MemoryStream(request.FaceImage))
                {
                    // detect faces in image
                    var faces = await faceServiceClient.DetectAsync(ms);
                    var faceIds = faces.Select(face => face.FaceId).ToArray();

                    // Identify the person in the photo, based on the face.
                    try
                    {
                        var results = await faceServiceClient.IdentifyAsync(request.GroupId, faceIds);
                        if (results.Any())
                        {
                            var result = results[0].Candidates[0].PersonId;

                            // Fetch the person from the PersonId and display their name.
                            retResult.Person = await faceServiceClient.GetPersonAsync(request.GroupId, result);
                            if (retResult.Person != null)
                            {
                                retResult.Confidence = results[0].Candidates[0].Confidence;
                            }
                        }
                        else
                        {
                            retResult.Notification.Add("No match found");
                        }
                    }
                    catch (FaceAPIException fex)
                    {
                        if (fex.ErrorCode == FaceApiErrorCode.PersonGroupNotFound)
                        {
                            retResult.Notification.Add("No people have been registered");
                        }
                        else
                        {
                            retResult.Notification.Add("Recognize Face Failed! " + fex.ErrorMessage);
                        }

                        return retResult;
                    }
                }
            }
            catch (Exception ex)
            {
                retResult.Notification.Add("Recognize Face Failed! " + ex.Message);
            }
            return retResult;
        }
    }

    public class RecognizeFaceContext
    {
        public byte[] FaceImage { get; set; }

        public string GroupId { get; set; }
    }

    public class RecognizeFaceResult : CommandResult
    {
        public double Confidence { get; set; }
        public Person Person { get; set; }
    }
}