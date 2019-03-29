using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace SimpleDockerUI.App.Utils
{
    public class DependencyUtils
    {
        private static PropertyInfo implementorTypeProperty = typeof(DependencyAttribute).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();

        public static void Init(Assembly assembly)
        {
            foreach (var attr in assembly.GetCustomAttributes<DependencyAttribute>())
                register(attr);
        }

        private static void register(DependencyAttribute attr)
        {
            var type = (Type)implementorTypeProperty.GetValue(attr);
            var dependencyServiceType = typeof(DependencyService);
            var registerMethod = dependencyServiceType.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(t => t.Name == nameof(DependencyService.Register));
            var registerGenericMethod = registerMethod.MakeGenericMethod(type);
            registerGenericMethod.Invoke(null, new object[0]);
        }
    }
}
