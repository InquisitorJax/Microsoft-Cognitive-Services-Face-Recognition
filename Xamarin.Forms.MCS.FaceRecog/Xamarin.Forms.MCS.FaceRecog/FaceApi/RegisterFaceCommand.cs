using Microsoft.ProjectOxford.Face;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms.MCS.FaceRecog.Model;

namespace Xamarin.Forms.MCS.FaceRecog.FaceApi
{
    public interface IRegisterFaceCommand : IAsyncLogicCommand<RegisterFaceContext, CommandResult>
    {
    }

    public class RegisterFaceCommand : AsyncLogicCommand<RegisterFaceContext, CommandResult>, IRegisterFaceCommand
    {
        private IRecognizeFaceCommand Recognize
        {
            get { return DependencyService.Get<IRecognizeFaceCommand>(); }
        }

        public override async Task<CommandResult> ExecuteAsync(RegisterFaceContext request)
        {
            var result = new CommandResult();
            try
            {
                var keyProvider = DependencyService.Get<IApiKeyProvider>();
                string faceKey = keyProvider.GetApiKey(ApiKeyType.FaceApi);

                //TODO: First try recognize person - and return error if person already exists (override flag)
                var recogResult = await Recognize.ExecuteAsync(new RecognizeFaceContext { FaceImage = request.Data.Mug, GroupId = request.GroupId });
                if (recogResult.IsValid())
                {
                    result.Notification.Add("Person already in registery - override if necessary");
                }

                var faceServiceClient = new FaceServiceClient(faceKey);
                try
                {
                    await faceServiceClient.CreatePersonGroupAsync(request.GroupId, request.GroupName);
                }
                catch (FaceAPIException fex)
                {
                    if (fex.ErrorCode != FaceApiErrorCode.PersonGroupExists)
                    {
                        result.Notification.Add("Register Face Failed! " + fex.ErrorMessage);
                        return result;
                    }
                }

                var person = await faceServiceClient.CreatePersonAsync(request.GroupId, request.PersonName);
                using (MemoryStream ms = new MemoryStream(request.Data.Mug))
                {
                    string userData = null; //JsonConvert.SerializeObject(request.Data);
                    await faceServiceClient.AddPersonFaceAsync(request.GroupId, person.PersonId, ms, userData);
                }

                await faceServiceClient.TrainPersonGroupAsync(request.GroupId);
            }
            catch (FaceAPIException fex)
            {
                result.Notification.Add("Register Face Failed! " + fex.ErrorMessage);
            }
            catch (Exception ex)
            {
                result.Notification.Add("Register Face Failed! " + ex.Message);
            }
            return result;
        }
    }

    public class RegisterFaceContext
    {
        public UserData Data { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }

        public string PersonName { get; set; }
    }
}