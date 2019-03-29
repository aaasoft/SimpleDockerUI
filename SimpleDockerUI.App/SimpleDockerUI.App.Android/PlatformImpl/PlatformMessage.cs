using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SimpleDockerUI.App;
using SimpleDockerUI.App.Services;

[assembly: Xamarin.Forms.Dependency(typeof(SimpleDockerUI.App.Droid.PlatformImpl.PlatformMessage))]
namespace SimpleDockerUI.App.Droid.PlatformImpl
{
    public class PlatformMessage : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}