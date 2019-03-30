using SimpleDockerUI.App.Services;
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
            var result = await Plugin.Fingerprint.CrossFingerprint.Current.AuthenticateAsync("请验证指纹");
            //如果取消
            if (result.Status == Plugin.Fingerprint.Abstractions.FingerprintAuthenticationResultStatus.Canceled)
                return;
            //认证成功，跳转到主页面
            if (result.Authenticated)
            {
                App.Current.MainPage = new MainPage();
                return;
            }
            //如果有错误消息
            if (!string.IsNullOrEmpty(result.ErrorMessage))
                DependencyService.Get<IMessage>().LongAlert(result.ErrorMessage);
        }
    }
}