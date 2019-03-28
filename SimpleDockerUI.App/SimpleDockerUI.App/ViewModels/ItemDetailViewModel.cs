using System;

using SimpleDockerUI.App.Models;

namespace SimpleDockerUI.App.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public SiteItem Item { get; set; }
        public ItemDetailViewModel(SiteItem item = null)
        {
            Title = item?.Name;
            Item = item;
        }
    }
}
