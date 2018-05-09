using System;
using Docker.DotNet;

namespace Launcher.Utils
{
    public class DockerClientUtils
    {
        public static void UseDockerClient(Action<DockerClient> action)
        {
            using(DockerClient client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock"))
                .CreateClient())
                {
                    action(client);
                }
        }
    }
}