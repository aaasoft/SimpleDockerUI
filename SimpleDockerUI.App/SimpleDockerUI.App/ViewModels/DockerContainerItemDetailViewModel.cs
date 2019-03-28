using System;

using SimpleDockerUI.App.Models;

namespace SimpleDockerUI.App.ViewModels
{
    public class DockerContainerItemDetailViewModel : BaseViewModel
    {
        public DockerContainerItem Item { get; set; }
        public DockerContainerItemDetailViewModel(DockerContainerItem item = null)
        {
            Title = item?.Name;
            Item = item;
        }
    }
}
