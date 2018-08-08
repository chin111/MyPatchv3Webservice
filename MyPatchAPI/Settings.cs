using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.Configuration;

namespace MyPatchAPI
{
    public class Settings
    {
        private static Configuration configuration;

        private static Configuration Configuration
        {
            get
            {
                if (configuration == null)
                {
                    configuration = WebConfigurationManager.OpenWebConfiguration("~/web.config");
                    return configuration;
                }

                return configuration;
            }
        }

        public static string GetStringSetting(string settingName)
        {
            return Configuration.AppSettings.Settings[settingName].Value;
        }

        public static string GetConnectionString(string connectionName)
        {
            return Configuration.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString;
        }
    }
}