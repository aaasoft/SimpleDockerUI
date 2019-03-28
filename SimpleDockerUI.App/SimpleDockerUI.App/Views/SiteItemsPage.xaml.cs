using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.Views;
using SimpleDockerUI.App.ViewModels;
using Newtonsoft.Json;

namespace SimpleDockerUI.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SiteItemsPage : ContentPage
    {
        SiteItemsViewModel viewModel;

        public SiteItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SiteItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as SiteItem;
            if (item == null)
                return;

            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            await Navigation.PushAsync(new DockerContainerItemsPage(new DockerContainerItemsViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewSiteItemPage(new SiteItem())));
        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var item = (SiteItem)mi.CommandParameter;
            item = JsonConvert.DeserializeObject<SiteItem>(JsonConvert.SerializeObject(item));
            await Navigation.PushModalAsync(new NavigationPage(new NewSiteItemPage(item)));
        }

        async void DelItem_Clicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var item = (SiteItem)mi.CommandParameter;

            var ret = await DisplayAlert("删除确认", $"确认删除站点[{item.Name}]", "确认", "取消");
            if (!ret)
                return;
            ret = await viewModel.DataStore.DeleteItemAsync(item.Id);
            if (!ret)
            {
                await DisplayAlert("通知", "删除失败", "关闭");
                return;
            }
            if (ret)
                viewModel.Items.Remove(item);
        }
    }
}