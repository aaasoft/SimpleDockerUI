using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleDockerUI.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private bool _IsFingerPrintAvailable = false;
        public bool IsFingerPrintAvailable
        {
            get { return _IsFingerPrintAvailable; }
            set
            {
                _IsFingerPrintAvailable = value;
                OnPropertyChanged(nameof(IsFingerPrintAvailable));
            }
        }

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = this;

            IsFingerPrintAvailable = Plugin.Fingerprint.CrossFingerprint.Current.IsAvailableAsync(true).Result;
        }

        private async void FingerprintLoginButton_Clicked(object sender, EventArgs e)
        {
            //进行指纹识别
            var result = await Plugin.Fingerprint.CrossFingerprint.Current.AuthenticateAsync("指纹验证");
            //认证成功，跳转到主页面
            if (result.Authenticated)
            {
                App.Current.MainPage = new MainPage();
                return;
            }
        }
    }
}