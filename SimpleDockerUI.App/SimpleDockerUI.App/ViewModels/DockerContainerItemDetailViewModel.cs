using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        public Command RefreshCommand { get; set; }
        public Command StartCommand { get; set; }
        public Command StopCommand { get; set; }
        public Command RestartCommand { get; set; }

        private string _BusyText;
        public string BusyText
        {
            get { return _BusyText; }
            set
            {
                _BusyText = value;
                OnPropertyChanged(nameof(BusyText));
            }
        }

        public DockerContainerItemDetailViewModel(SiteItem siteItem, DockerContainerItem dockerContainerItem)
        {
            Title = $"容器[{dockerContainerItem?.Name}] - {siteItem.Name}";
            SiteItem = siteItem;
            DockerContainerItem = dockerContainerItem;
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
            StartCommand = new Command(async () => await ExecuteStartCommand());
            StopCommand = new Command(async () => await ExecuteStopCommand());
            RestartCommand = new Command(async () => await ExecuteRestartCommand());
        }

        async Task ExecuteRefreshCommand()
        {
            IsBusy = true;
            BusyText = "正在刷新...";
            try
            {
                //登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
                var newItem = await WebApiUtils.GetContainerInfo(client, SiteItem, DockerContainerItem.Id);
                JsonConvert.PopulateObject(JsonConvert.SerializeObject(newItem), DockerContainerItem);
                OnPropertyChanged(nameof(DockerContainerItem));
                //退出登录
                await WebApiUtils.Logout(client, SiteItem);

            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("启动容器失败，原因：" + ex.Message);
            }
            IsBusy = false;
        }

        async Task ExecuteStartCommand()
        {
            IsBusy = true;
            BusyText = "正在启动容器...";
            try
            {
                //登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
                await WebApiUtils.StartContainer(client, SiteItem, DockerContainerItem);
                //退出登录
                await WebApiUtils.Logout(client, SiteItem);
                DependencyService.Get<IMessage>().LongAlert("启动容器成功！");
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("启动容器失败，原因：" + ex.Message);
            }
            IsBusy = false;
            //刷新
            await ExecuteRefreshCommand();
        }

        async Task ExecuteStopCommand()
        {
            IsBusy = true;
            BusyText = "正在停止容器...";
            try
            {
                //登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
                await WebApiUtils.StopContainer(client, SiteItem, DockerContainerItem);
                //退出登录
                await WebApiUtils.Logout(client, SiteItem);
                DependencyService.Get<IMessage>().LongAlert("停止容器成功！");
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("停止容器失败，原因：" + ex.Message);
            }
            IsBusy = false;
            //刷新
            await ExecuteRefreshCommand();
        }

        async Task ExecuteRestartCommand()
        {
            IsBusy = true;
            BusyText = "正在重新启动容器...";
            try
            {
                //登录
                await WebApiUtils.EnsureLogin(client, SiteItem);
                await WebApiUtils.RestartContainer(client, SiteItem, DockerContainerItem);
                //退出登录
                await WebApiUtils.Logout(client, SiteItem);
                DependencyService.Get<IMessage>().LongAlert("重新启动容器成功！");
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("重新启动容器失败，原因：" + ex.Message);
            }
            IsBusy = false;
            //刷新
            await ExecuteRefreshCommand();
        }
    }
}
