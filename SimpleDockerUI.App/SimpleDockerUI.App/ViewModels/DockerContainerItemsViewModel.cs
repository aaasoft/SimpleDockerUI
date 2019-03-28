using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleDockerUI.App.ViewModels
{
    public class DockerContainerItemsViewModel : BaseViewModel
    {
        public IDataStore<DockerContainerItem> DataStore { get; private set; }
        public SiteItem SiteItem { get; private set; }
        public ObservableCollection<DockerContainerItem> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public DockerContainerItemsViewModel(SiteItem siteItem)
        {
            this.SiteItem = siteItem;
            DataStore = DependencyService.Get<IDataStore<DockerContainerItem>>() ?? new DockerContainerDataStore(siteItem);
            Title = $"Docker容器 - {siteItem.Name}";
            Items = new ObservableCollection<DockerContainerItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
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

        public override void Dispose()
        {
            base.Dispose();
            DataStore.Dispose();
        }
    }
}
