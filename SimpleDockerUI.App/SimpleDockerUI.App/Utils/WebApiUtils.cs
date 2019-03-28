using Newtonsoft.Json;
using SimpleDockerUI.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDockerUI.App.Utils
{
    public class WebApiUtils
    {
        public static async Task EnsureLogin(HttpClient client, SiteItem siteItem)
        {
            var url = siteItem.Url + "/api/v1/Login";
            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["password"] = siteItem.Password
            });
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<DockerContainerItem[]> GetContainerItems(HttpClient client, SiteItem siteItem)
        {
            var url = siteItem.Url + "/api/v1/Container";
            var content = await client.GetStringAsync(url);
            var items = JsonConvert.DeserializeObject<DockerContainerItem[]>(content);
            return items;
        }
    }
}
