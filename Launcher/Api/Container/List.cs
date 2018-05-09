using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Launcher.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quick.CoreMVC.Api;

namespace Launcher.Api.Container
{
    public class List : AbstractMethod
    {
        public override string Name => "获取容器列表";

        public override HttpMethod Method => HttpMethod.GET;

        public override Task Invoke(HttpContext context)
        {
            object list = null;
#if DEBUG
            var content = File.ReadAllText("Launcher/Resource/debug/containerList.json");
            list = JArray.Parse(content);
#else
            DockerClientUtils.UseDockerClient(client =>
            {
                list = client.Containers.ListContainersAsync(new ContainersListParameters() { All = true })
                .Result
                .Select(t => new
                {
                    Id = t.ID,
                    Name = t.Names?.LastOrDefault().Substring(1),
                    t.Names,
                    Port = string.Join(", ", t.Ports?.Select(p => $"{p.IP}:{p.PublicPort}->{p.PrivatePort}/{p.Type}")),
                    t.Ports,
                    t.Command,
                    t.Created,
                    t.Image,
                    t.Status
                });
            });
#endif
            return Task.FromResult(list);
        }
    }
}