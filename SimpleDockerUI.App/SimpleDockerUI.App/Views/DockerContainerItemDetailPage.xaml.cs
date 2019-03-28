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
            this.viewModel = viewModel;
            BindingContext = viewModel;
        }

        public DockerContainerItemDetailPage(SiteItem siteItem, DockerContainerItem dockerContainerItem)
        {
            InitializeComponent();
            viewModel = new DockerContainerItemDetailViewModel(siteItem, dockerContainerItem);
            BindingContext = viewModel;
        }
    }
}