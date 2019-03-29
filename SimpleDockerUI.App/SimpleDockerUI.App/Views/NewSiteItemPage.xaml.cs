using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.Services;
using Newtonsoft.Json;

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
            var checkResult = Item.Check();
            if (!checkResult.IsSuccess)
            {
                DependencyService.Get<IMessage>().ShortAlert(checkResult.Message);
                return;
            }
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

        private async void Scan_Clicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();

                if (result != null)
                {
                    var content = result.Text;
                    try
                    {
                        JsonConvert.PopulateObject(content, Item);
                        OnPropertyChanged(nameof(Item));
                    }
                    catch
                    {
                        DependencyService.Get<IMessage>().ShortAlert("二维码数据不正确。");
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.ToString());
            }
        }
    }
}