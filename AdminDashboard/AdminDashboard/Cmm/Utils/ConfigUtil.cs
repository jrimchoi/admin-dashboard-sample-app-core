using Microsoft.Extensions.Configuration;
using System.Drawing;

namespace DSELN.Cmm.Utils
{

    public static class ConfigUtil
    {
        private static IConfiguration _configuration;
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string getConfigValue(string section, string key)
        {
            return _configuration.GetSection(section).GetValue<string>(key);
        }
        public static string getSectionValue(string section)
        {
            return _configuration.GetSection(section).Value;
        }
    }
}
