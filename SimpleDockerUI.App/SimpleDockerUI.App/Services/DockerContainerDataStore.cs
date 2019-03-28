using SimpleDockerUI.App.Models;
using SimpleDockerUI.App.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDockerUI.App.Services
{
    public class DockerContainerDataStore : AbstractDataStore<DockerContainerItem>
    {
        private SiteItem siteItem;
        private HttpClient client = new HttpClient();

        public DockerContainerDataStore(SiteItem siteItem)
        {
            this.siteItem = siteItem;
        }

        public override void Dispose()
        {
            client.Dispose();
            client = null;
        }

        public async override Task<IEnumerable<DockerContainerItem>> GetItemsAsync(bool forceRefresh = false)
        {
            //登录
            await WebApiUtils.EnsureLogin(client, siteItem);
            //获取容器列表
            return await WebApiUtils.GetContainerItems(client, siteItem);
        }
    }
}
