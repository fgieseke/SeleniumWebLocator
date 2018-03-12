using System.Configuration;

namespace SeleniumWebLocator
{
    public class AppSettings
    {
        public int PageSize => ConfigurationManager.AppSettings["PageSize"] != null
            ? int.Parse(ConfigurationManager.AppSettings["PageSize"])
            : 30;
    }
}