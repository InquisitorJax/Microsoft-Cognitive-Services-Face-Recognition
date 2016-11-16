using System.Threading.Tasks;
using Xamarin.Forms.MCS.FaceRecog.Model;

namespace Xamarin.Forms.MCS.FaceRecog.Repo
{
    public interface IPersonStore
    {
        Task AddPersonAsync(Mutant person);

        Task<Mutant> FindPersonAsync(string personId);
    }
}