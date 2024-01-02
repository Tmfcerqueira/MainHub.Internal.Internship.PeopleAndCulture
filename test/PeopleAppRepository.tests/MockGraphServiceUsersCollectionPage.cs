using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace MainHub.Internal.PeopleAndCulture
{
    public class MockGraphServiceUsersCollectionPage : IGraphServiceUsersCollectionPage
    {
        private readonly List<User> _users;

        public MockGraphServiceUsersCollectionPage(List<User> users)
        {
            _users = users;
        }

        public User this[int index]
        {
            get => _users[index];
            set => throw new NotImplementedException();
        }

        public IList<User> CurrentPage => _users;

        public IGraphServiceUsersCollectionRequest NextPageRequest => throw new NotImplementedException();

        public IDictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

        public int Count => _users.Count;

        public bool IsReadOnly => false;

        public void Add(User item)
        {
            _users.Add(item);
        }

        public void Clear()
        {
            _users.Clear();
        }

        public bool Contains(User item)
        {
            return _users.Contains(item);
        }

        public void CopyTo(User[] array, int arrayIndex)
        {
            _users.CopyTo(array, arrayIndex);
        }

        public IEnumerator<User> GetEnumerator()
        {
            return _users.GetEnumerator();
        }

        public int IndexOf(User item)
        {
            return _users.IndexOf(item);
        }

        public void InitializeNextPageRequest(IBaseClient client, string nextPageLinkString)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, User item)
        {
            _users.Insert(index, item);
        }

        public bool Remove(User item)
        {
            return _users.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _users.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _users.GetEnumerator();
        }
    }

}
