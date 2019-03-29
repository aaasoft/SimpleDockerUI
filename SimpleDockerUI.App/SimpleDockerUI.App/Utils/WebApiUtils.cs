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

        public static async Task Logout(HttpClient client, SiteItem siteItem)
        {
            var url = siteItem.Url + "/api/v1/Login";
            await client.DeleteAsync(url);
        }

        public static async Task<DockerContainerItem[]> GetContainerItems(HttpClient client, SiteItem siteItem)
        {
            var url = siteItem.Url + "/api/v1/Container";
            var content = await client.GetStringAsync(url);
            var items = JsonConvert.DeserializeObject<DockerContainerItem[]>(content);
            return items;
        }

        public static async Task<DockerContainerItem> GetContainerInfo(HttpClient client, SiteItem siteItem, string id)
        {
            var url = siteItem.Url + "/api/v1/Container/" +id;
            var content = await client.GetStringAsync(url);
            var items = JsonConvert.DeserializeObject<DockerContainerItem>(content);
            return items;
        }

        public static async Task StartContainer(HttpClient client, SiteItem siteItem, DockerContainerItem dockerContainerItem)
        {
            var url = $"{siteItem.Url}/api/v1/Container/{dockerContainerItem.Id}/Start";
            var response = await client.PutAsync(url, null);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(message, ex);
            }
        }

        public static async Task StopContainer(HttpClient client, SiteItem siteItem, DockerContainerItem dockerContainerItem)
        {
            var url = $"{siteItem.Url}/api/v1/Container/{dockerContainerItem.Id}/Stop";
            var response = await client.PutAsync(url, null);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(message, ex);
            }
        }

        public static async Task RestartContainer(HttpClient client, SiteItem siteItem, DockerContainerItem dockerContainerItem)
        {
            var url = $"{siteItem.Url}/api/v1/Container/{dockerContainerItem.Id}/Restart";
            var response = await client.PutAsync(url, null);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(message, ex);
            }
        }
    }
}
