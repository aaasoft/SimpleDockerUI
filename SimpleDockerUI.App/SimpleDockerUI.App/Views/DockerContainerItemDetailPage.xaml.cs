using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.ViewModels;

namespace SimpleDockerUI.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DockerContainerItemDetailPage : ContentPage
    {
        DockerContainerItemDetailViewModel viewModel;

        public DockerContainerItemDetailPage(DockerContainerItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public DockerContainerItemDetailPage(DockerContainerItem item)
        {
            InitializeComponent();
            viewModel = new DockerContainerItemDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}