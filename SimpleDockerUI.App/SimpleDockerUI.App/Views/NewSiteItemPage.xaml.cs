using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SimpleDockerUI.App.Models;

namespace SimpleDockerUI.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewSiteItemPage : ContentPage
    {
        public SiteItem Item { get; set; }

        public NewSiteItemPage(SiteItem item)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(item.Id))
                Title = "新增站点";
            else
                Title = "编辑站点";
            Item = item;
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Item.Id))
                MessagingCenter.Send(this, "AddItem", Item);
            else
                MessagingCenter.Send(this, "UpdateItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}