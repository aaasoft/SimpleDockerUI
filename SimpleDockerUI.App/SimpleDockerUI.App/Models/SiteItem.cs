using System;

namespace SimpleDockerUI.App.Models
{
    public class SiteItem : ICheckable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }

        public CheckResult Check()
        {
            Name = Name?.Trim();
            if (string.IsNullOrEmpty(Name))
                return CheckResult.Error("名称不能为空");

            Url = Url?.Trim();
            if (string.IsNullOrEmpty(Url))
                return CheckResult.Error("Url不能为空");
            if (!Uri.TryCreate(Url, UriKind.Absolute, out _))
                return CheckResult.Error("Url参数不合法");

            Password = Password?.Trim();
            if (string.IsNullOrEmpty(Password))
                return CheckResult.Error("密码不能为空");
            return CheckResult.Success();
        }
    }
}