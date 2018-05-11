using System;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Launcher.Utils;
using Microsoft.AspNetCore.Http;
using Quick.CoreMVC.Api;

namespace Launcher.Api.Container
{
    public class Restart : AbstractMethod<Restart.Parameter>
    {
        public class Parameter
        {            
            public string Id { get; set; }
        }

        public override string Name => "Restart container";

        public override HttpMethod Method => HttpMethod.POST;

        public override Task Invoke(HttpContext context, Parameter parameter)
        {
            bool ret = false;
            DockerClientUtils.UseDockerClient(client =>
            {
                ret = client.Containers.StopContainerAsync(parameter.Id, new ContainerStopParameters()).Result;
                if (!ret)
                    return;
                ret = client.Containers.StartContainerAsync(parameter.Id, new ContainerStartParameters()).Result;
                if (!ret)
                    return;
            });
            if (!ret)
                return Task.FromResult(ApiResult.Error("Retart failed."));
            return Task.FromResult(ret);
        }
    }
}