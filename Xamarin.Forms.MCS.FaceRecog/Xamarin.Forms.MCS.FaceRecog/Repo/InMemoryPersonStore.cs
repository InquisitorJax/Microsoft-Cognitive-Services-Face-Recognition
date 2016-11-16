using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.MCS.FaceRecog.Model;

namespace Xamarin.Forms.MCS.FaceRecog.Repo
{
    public class InMemoryPersonStore : IPersonStore
    {
        private readonly Dictionary<string, Mutant> _personCache;

        public InMemoryPersonStore()
        {
            _personCache = new Dictionary<string, Mutant>();
        }

        public Task AddPersonAsync(Mutant person)
        {
            if (_personCache.ContainsKey(person.Id))
            {
                _personCache.Remove(person.Id);
            }
            _personCache.Add(person.Id, person);

            return Task.FromResult(0);
        }

        public Task<Mutant> FindPersonAsync(string personId)
        {
            Mutant person = null;
            if (_personCache.ContainsKey(personId))
            {
                person = _personCache[personId];
            }
            return Task.FromResult(person);
        }
    }
}