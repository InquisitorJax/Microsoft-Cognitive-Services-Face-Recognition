using System;
using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Xamarin.Forms.MCS.FaceRecog.FaceApi
{
    public interface IDeletePersonGroupCommand : IAsyncLogicCommand<DeletePersonGroupContext, CommandResult>
    {
    }

    public class DeletePersonGroupCommand : AsyncLogicCommand<DeletePersonGroupContext, CommandResult>, IDeletePersonGroupCommand
    {
        public override async Task<CommandResult> ExecuteAsync(DeletePersonGroupContext request)
        {
            var retResult = new CommandResult();
            try
            {
                var faceServiceClient = FaceClient.Resolve();

                // detect faces in image
                await faceServiceClient.DeletePersonGroupAsync(request.GroupId);
            }
            catch (Exception ex)
            {
                retResult.Notification.Add("Recognize Face Failed! " + ex.Message);
            }
            return retResult;
        }
    }

    public class DeletePersonGroupContext
    {
        public string GroupId { get; set; }
    }
}