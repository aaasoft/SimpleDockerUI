using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace SimpleDockerUI.App.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "关于";
            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/aaasoft/SimpleDockerUI")));
        }

        public ICommand OpenWebCommand { get; }
    }
}