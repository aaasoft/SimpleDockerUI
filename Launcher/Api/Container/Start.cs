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
    public class Start : AbstractMethod<Start.Parameter>
    {
        public class Parameter
        {            
            //[FormFieldInfo(Name = "运单编号", NotEmpty = true)]
            //容器编号
            public string Id { get; set; }
        }

        public override string Name => "启动容器";

        public override HttpMethod Method => HttpMethod.POST;

        public override Task Invoke(HttpContext context, Parameter parameter)
        {
            bool ret = false;
            DockerClientUtils.UseDockerClient(client =>
            {
                ret = client.Containers.StartContainerAsync(parameter.Id, new ContainerStartParameters()).Result;
            });
            if (!ret)
                return Task.FromResult(ApiResult.Error("Start failed."));
            return Task.FromResult(ret);
        }
    }
}