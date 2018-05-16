using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Launcher.Utils
{
    public class ExceptionUtils
    {
        //忽略的属性名称数组
        private static String[] ignorePropertiyNames = new String[]
        {
            "Message","InnerException","StackTrace","Data"
        };

        public static String GetExceptionMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            Exception tmpEx = ex;
            while (tmpEx != null)
            {
                sb.AppendLine("------------------------->");
                sb.AppendLine(tmpEx.Message);
                tmpEx = tmpEx.InnerException;
            }
            return sb.ToString();
        }

        public static String GetExcpetionString(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            Exception tmpEx = ex;
            while (tmpEx != null)
            {
                sb.AppendLine("-------------------------");
                _GetExceptionString(tmpEx, sb, String.Empty);
                tmpEx = tmpEx.InnerException;
            }
            return sb.ToString();
        }

        private static void _GetExceptionString(Exception ex, StringBuilder sb, String prefix)
        {
            Type exType = ex.GetType();
            sb.AppendLine(prefix + "ExceptionType:" + exType.FullName);
            sb.AppendLine(prefix + "ExceptionMessage:" + ex.Message);
            PropertyInfo[] propertys = exType.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance).Where(t =>
            {
                return !ignorePropertiyNames.Contains(t.Name);
            }).OrderBy(t => { return t.Name; }).ToArray();

            if (propertys.Length > 0)
            {
                sb.AppendLine(prefix + "ExceptionProperties:");
                foreach (PropertyInfo pi in propertys)
                {
                    try
                    {
                        Object propertyValue = pi.GetValue(ex, null);
                        if (propertyValue == null) continue;
                        if (propertyValue is Exception[])
                        {
                            Exception[] subExs = (Exception[])propertyValue;
                            foreach (Exception subEx in subExs)
                            {
                                _GetExceptionString(subEx, sb, prefix + "    ");
                            }
                        }
                        else
                        {
                            sb.AppendLine(String.Format(prefix + "    {0} : {1}", pi.Name, propertyValue.ToString()));
                        }
                    }
                    catch { }
                }
            }
            sb.AppendLine(prefix + "ExceptionStack:" + ex.StackTrace);
        }
    }
}
