using System;
using System.Collections.Generic;
using System.Linq;

namespace Launcher.Model
{
    /// <summary>
    /// Docker Image
    /// </summary>
    public class Image
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public long SharedSize { get; set; }
        public long Size { get; set; }
        public long VirtualSize { get; set; }
        public string RepoTags { get; set; }
        public string Repo=>RepoTags?.Split(':').FirstOrDefault();
        public string Tag=>RepoTags?.Split(':').LastOrDefault();
        public IDictionary<string, string> Labels { get; set; }
    }
}