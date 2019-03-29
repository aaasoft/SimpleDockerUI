using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDockerUI.App.Models
{
    public class CheckResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static CheckResult Success() => new CheckResult() { IsSuccess = true };
        public static CheckResult Error(string message) => new CheckResult() { IsSuccess = false, Message = message };
    }
}
