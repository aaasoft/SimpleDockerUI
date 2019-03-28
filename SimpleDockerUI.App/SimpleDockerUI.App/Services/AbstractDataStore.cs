using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDockerUI.App.Services
{
    public abstract class AbstractDataStore<T> : IDataStore<T>
    {
        public virtual Task<bool> AddItemAsync(T item)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public abstract void Dispose();

        public virtual Task<T> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> UpdateItemAsync(T item)
        {
            throw new NotImplementedException();
        }
    }
}
