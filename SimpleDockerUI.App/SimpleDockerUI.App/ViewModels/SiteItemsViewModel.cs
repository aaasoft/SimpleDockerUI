using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.Views;
using System.Linq;
using SimpleDockerUI.App.Services;

namespace SimpleDockerUI.App.ViewModels
{
    public class SiteItemsViewModel : BaseViewModel
    {
        public IDataStore<SiteItem> DataStore => DependencyService.Get<IDataStore<SiteItem>>() ?? new SiteDataStore();
        public ObservableCollection<SiteItem> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public SiteItemsViewModel()
        {
            Title = "站点";
            Items = new ObservableCollection<SiteItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewSiteItemPage, SiteItem>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as SiteItem;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
            MessagingCenter.Subscribe<NewSiteItemPage, SiteItem>(this, "UpdateItem", async (obj, item) =>
            {
                var newItem = item as SiteItem;
                Items.Remove(Items.First(t => t.Id == newItem.Id));
                Items.Add(newItem);
                await DataStore.UpdateItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}