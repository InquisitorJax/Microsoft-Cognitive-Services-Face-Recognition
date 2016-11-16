using Microsoft.ProjectOxford.Face;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms.MCS.FaceRecog.Model;

namespace Xamarin.Forms.MCS.FaceRecog.FaceApi
{
    public interface IFindGroupPeopleCommand : IAsyncLogicCommand<FindGroupPeopleContext, FindGroupPeopleResult>
    {
    }

    public class FindGroupPeopleContext
    {
        public string GroupId { get; set; }
    }

    public class FindGroupPeopleResult : CommandResult
    {
        public FindGroupPeopleResult()
        {
            People = new List<Mutant>();
        }

        public IList<Mutant> People { get; set; }
    }

    internal class FindGroupPeopleCommand : AsyncLogicCommand<FindGroupPeopleContext, FindGroupPeopleResult>, IFindGroupPeopleCommand
    {
        public override async Task<FindGroupPeopleResult> ExecuteAsync(FindGroupPeopleContext request)
        {
            var result = new FindGroupPeopleResult();
            var keyProvider = DependencyService.Get<IApiKeyProvider>();
            string faceKey = keyProvider.GetApiKey(ApiKeyType.FaceApi);

            var faceServiceClient = new FaceServiceClient(faceKey);

            try
            {
                var people = await faceServiceClient.GetPersonsAsync(request.GroupId);

                foreach (var person in people)
                {
                    var p = new Mutant();
                    p.Name = person.Name;
                    p.Id = person.PersonId.ToString();
                    if (person.UserData != null)
                    {
                        p.Data = JsonConvert.DeserializeObject<UserData>(person.UserData);
                    }
                    result.People.Add(p);
                    //TODO: add person to store?
                }
            }
            catch (FaceAPIException fex)
            {
                result.Notification.Add("Fetch People Group Failed! " + fex.ErrorMessage);
            }
            catch (Exception ex)
            {
                result.Notification.Add("Fetch People Group Failed! " + ex.Message);
            }
            return result;
        }
    }
}