using System;
using System.Net.Http;
using System.Threading.Tasks;
using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.Services;
using SimpleDockerUI.App.Utils;
using Xamarin.Forms;

namespace SimpleDockerUI.App.ViewModels
{
    public class DockerContainerItemDetailViewModel : BaseViewModel
    {
        private HttpClient client = new HttpClient();
        public SiteItem SiteItem { get; set; }
        public DockerContainerItem DockerContainerItem { get; set; }
        public Command StartCommand { get; set; }
        public Command StopCommand { get; set; }
        public Command RestartCommand { get; set; }

        public DockerContainerItemDetailViewModel(SiteItem siteItem, DockerContainerItem dockerContainerItem)
        {
            Title = $"容器[{dockerContainerItem?.Name}] - {siteItem.Name}";
            SiteItem = siteItem;
            DockerContainerItem = dockerContainerItem;
            StartCommand = new Command(async () => await ExecuteStartCommand());
            StopCommand = new Command(async () => await ExecuteStopCommand());
            RestartCommand = new Command(async () => await ExecuteRestartCommand());
        }

        async Task ExecuteStartCommand()
        {
            IsBusy = true;
            try
            {
                //登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
                await WebApiUtils.StartContainer(client, SiteItem, DockerContainerItem);
                //退出登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("启动容器失败，原因：" + ex.Message);
            }
            IsBusy = false;
        }

        async Task ExecuteStopCommand()
        {
            IsBusy = true;
            try
            {
                //登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
                await WebApiUtils.StopContainer(client, SiteItem, DockerContainerItem);
                //退出登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("停止容器失败，原因：" + ex.Message);
            }
            IsBusy = false;
        }

        async Task ExecuteRestartCommand()
        {
            IsBusy = true;
            try
            {
                //登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
                await WebApiUtils.RestartContainer(client, SiteItem, DockerContainerItem);
                //退出登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("重新启动容器失败，原因：" + ex.Message);
            }
            IsBusy = false;
        }
    }
}
