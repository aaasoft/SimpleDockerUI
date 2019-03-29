using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleDockerUI.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DockerContainerItemsPage : ContentPage
    {
        private DockerContainerItemsViewModel viewModel;
        public ObservableCollection<string> Items { get; set; }

        public DockerContainerItemsPage(DockerContainerItemsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as DockerContainerItem;
            if (item == null)
                return;
            await Navigation.PushAsync(new DockerContainerItemDetailPage(viewModel.SiteItem, item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //刷新
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
