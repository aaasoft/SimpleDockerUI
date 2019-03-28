using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.Views;
using System.Linq;

namespace SimpleDockerUI.App.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<SiteItem> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "站点";
            Items = new ObservableCollection<SiteItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, SiteItem>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as SiteItem;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
            MessagingCenter.Subscribe<NewItemPage, SiteItem>(this, "UpdateItem", async (obj, item) =>
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