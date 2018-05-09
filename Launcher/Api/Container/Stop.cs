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
    public class Stop : AbstractMethod<Stop.Parameter>
    {
        public class Parameter
        {            
            //容器编号
            public string Id { get; set; }
        }

        public override string Name => "停止容器";

        public override HttpMethod Method => HttpMethod.POST;

        public override Task Invoke(HttpContext context,Parameter parameter)
        {
            bool ret = false;
            DockerClientUtils.UseDockerClient(client =>
            {
                ret = client.Containers.StopContainerAsync(parameter.Id, new ContainerStopParameters()).Result;
            });
            if (!ret)
                return Task.FromResult(ApiResult.Error("Stop failed."));
            return Task.FromResult(ret);
        }
    }
}