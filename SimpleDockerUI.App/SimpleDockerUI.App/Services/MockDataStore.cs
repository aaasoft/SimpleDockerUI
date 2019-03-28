using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleDockerUI.App.Models;

namespace SimpleDockerUI.App.Services
{
    public class MockDataStore : IDataStore<SiteItem>
    {
        List<SiteItem> items;

        public MockDataStore()
        {
            items = new List<SiteItem>();
            var mockItems = new List<SiteItem>
            {
                new SiteItem { Id = Guid.NewGuid().ToString(), Name = "First item", Description="This is an item description." },
                new SiteItem { Id = Guid.NewGuid().ToString(), Name = "Second item", Description="This is an item description." },
                new SiteItem { Id = Guid.NewGuid().ToString(), Name = "Third item", Description="This is an item description." },
                new SiteItem { Id = Guid.NewGuid().ToString(), Name = "Fourth item", Description="This is an item description." },
                new SiteItem { Id = Guid.NewGuid().ToString(), Name = "Fifth item", Description="This is an item description." },
                new SiteItem { Id = Guid.NewGuid().ToString(), Name = "Sixth item", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(SiteItem item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(SiteItem item)
        {
            var oldItem = items.Where((SiteItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((SiteItem arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<SiteItem> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<SiteItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}