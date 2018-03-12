using OpenQA.Selenium;
using SeleniumWebLocator.Base;

namespace SeleniumWebLocator
{
    public class App : WebDriverFixture
    {


        /// <summary>
        /// Initialisiert WebDriverFixture mit einem WebDriver
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="webSiteBaseUrl"> </param>
        /// <param name="logger"></param>
        /// <param name="defaultTimeOutInSeconds"></param>
        public App(IWebDriver driver, string webSiteBaseUrl, IReportLogger logger, int defaultTimeOutInSeconds)
            : base(driver, webSiteBaseUrl, logger, defaultTimeOutInSeconds)
        {
            Settings = new AppSettings();
        }


        public AppSettings Settings { get; }

        public ILocatedWebElement GetElement(string selector, ISearchContext root = null)
        {
            return Locator.Find(selector, root);
        }

    }
}
