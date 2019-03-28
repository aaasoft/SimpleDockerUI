using Newtonsoft.Json;
using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDockerUI.App.Services
{
    public class SiteDataStore : IDataStore<SiteItem>
    {
        private string configFileName;
        public List<SiteItem> items;

        public SiteDataStore()
        {
            configFileName = PathUtils.GetConfigFile("config.json");
            if (File.Exists(configFileName))
            {
                try
                {
                    var content = File.ReadAllText(configFileName);
                    items = JsonConvert.DeserializeObject<List<SiteItem>>(content);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.ToString());
                    System.Diagnostics.Debug.Assert(true);
                }
            }
            if (items == null)
                items = new List<SiteItem>();
        }

        private bool SaveToFile()
        {
            try
            {
                var content = JsonConvert.SerializeObject(items);
                File.WriteAllText(configFileName, content);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                System.Diagnostics.Debug.Assert(true);
                return false;
            }
        }

        public async Task<bool> AddItemAsync(SiteItem item)
        {
            if (string.IsNullOrEmpty(item.Id))
                item.Id = Guid.NewGuid().ToString();
            items.Add(item);

            return await Task.Run(() => SaveToFile());
        }

        public async Task<bool> UpdateItemAsync(SiteItem item)
        {
            var oldItem = items.Where((SiteItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.Run(() => SaveToFile());
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((SiteItem arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.Run(() => SaveToFile());
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
