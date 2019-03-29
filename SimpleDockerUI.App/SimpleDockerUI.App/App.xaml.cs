using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SimpleDockerUI.App.Views;
using SimpleDockerUI.App.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SimpleDockerUI.App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            var fingerprint = Plugin.Fingerprint.CrossFingerprint.Current;

            var isAvailable = await fingerprint.IsAvailableAsync(true);
            //如果指纹识别不可用，则返回
            if (!isAvailable)
                return;

            //进行指纹识别
            var result = await fingerprint.AuthenticateAsync("指纹验证");
            if (result.Authenticated)
                return;
            DependencyService.Get<ILifeCycleManager>().Finish();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
