using System;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebLocator.Base
{

    public class WebDriverFixture : ILocatedWebElementFactory
    {

        private static IWebDriver _instance;
        private static string _webSiteBaseUrl;
        public readonly ElementLocator Locator;


        /// <summary>
        /// Initialisiert WebDriverFixture mit einem WebDriver
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="webSiteBaseUrl"> </param>
        /// <param name="logger"></param>
        /// <param name="defaultTimeOut"></param>
        public WebDriverFixture(IWebDriver driver, string webSiteBaseUrl, IReportLogger logger, int defaultTimeOut = 20)
        {
            _webSiteBaseUrl = webSiteBaseUrl;
            _instance = driver;
            Locator = new ElementLocator(driver, this);
            Logger = logger;
            WaitSeconds = defaultTimeOut;
            Click = new ClickLocator(Locator);

        }


        #region Properties

        public IReportLogger Logger { get; set; }

        /// <summary>
        /// Der Standardwert wie lange auf eine Seite gewartet werden soll. Default : 20
        /// </summary>
        public int WaitSeconds { get; set; }

        public ClickLocator Click { get; set; }

        #endregion


        public LocatedWebElement CreateLocatedWebElement(IWebElement element)
        {
            return LocatedWebElement.CreateLocatedWebElement(element, Locator, Logger, WaitSeconds);
        }


        public void ShutdownBrowser()
        {
            _instance.Close();
            _instance.Dispose();
        }


        /// <summary>
        /// Öffnet den Browser mit angegebener Url
        /// </summary>
        public WebDriverFixture Open(string url)
        {
            _instance.Navigate().GoToUrl($"{_webSiteBaseUrl}{url}");
            _instance.Manage().Window.Maximize();
            return this;
        }


        /// <summary>
        /// Entfernt alle angegebene Cookies. Ist kein Name angegeben, dann werden alle entfernt.
        /// </summary>
        /// <param name="cookies"></param>
        public WebDriverFixture RemoveCookies(params string[] cookies)
        {
            if (cookies.Length > 0)
            {
                foreach (string cookieName in cookies)
                {
                    _instance.Manage().Cookies.DeleteCookieNamed(cookieName);
                }
            }
            else
            {
                _instance.Manage().Cookies.DeleteAllCookies();
            }

            return this;
        }

        public WebDriverFixture MouseClickOn(string selector)
        {
            var builder = new Actions(_instance);
            builder.Click(Locator.FindOrDefault(selector));

            return this;
        }

        ///// <summary>
        ///// Wartet darauf, dass alle abgesetzten Ajax-Aufrufe abgearbeitet werden.
        ///// </summary>
        ///// <param name="waitSeconds">Max. Wartezeit.</param>
        ///// <exception cref="ArgumentException">Wenn waitSeconds keine Zahl darstellt.</exception>
        //public void WaitForAjaxCallWithTimeout(string waitSeconds)
        //{
        //  int timeout;
        //  if (!int.TryParse(waitSeconds, out timeout))
        //  {
        //    _instance.Stop();
        //    throw new ArgumentException("timeOutValue");
        //  }

        //  _instance.WaitForCondition("selenium.browserbot.getUserWindow().$.active == 0", (1000 * timeout).ToString());
        //}



        public WebDriverFixture Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return this;
        }

        public WebDriverFixture SelectDropdownValueAndWait(string selector, string valueOrText, int milliseconds)
        {
            IWebElement select = Locator.FindOrDefault(selector);
            var xpathOption = "//option[contains(text(),'" + valueOrText + "') or @value='" + valueOrText + "']";
            Locator.FindOrDefault(xpathOption, select).Click();
            Wait(milliseconds);
            return this;
        }

        public WebDriverFixture OnElementMouseDown(string selector)
        {
            Actions builder = new Actions(_instance);
            builder.MoveToElement(Locator.FindOrDefault(selector)).Perform();
            builder.Click();
            return this;
        }

        //public void Focus(string locator)
        //{
        //  _instance.Focus(locator);
        //}

        public WebDriverFixture CheckOptionAndWait(string selector, int milliseconds)
        {
            var elem = Locator.FindOrDefault(selector);
            string att = elem.GetAttribute("CHECKED");
            if (string.IsNullOrEmpty(att) || att == "false")
                elem.Click();
            Wait(milliseconds);
            return this;
        }

        public WebDriverFixture UncheckOptionAndWait(string selector, int milliseconds)
        {
            var elem = Locator.FindOrDefault(selector);
            string att = elem.GetAttribute("CHECKED");
            if (string.IsNullOrEmpty(att) || att == "true")
                elem.Click();
            Wait(milliseconds);
            return this;
        }


        public string ReadTextFromElement(string selector)
        {
            return Locator.FindOrDefault(selector).Text;
        }

        public string ReadValueFromElement(string selector)
        {
            return Locator.FindOrDefault(selector).GetAttribute("value");
        }

        #region Verify-Methods

        public bool Verify(bool condition, string reportText = null)
        {
            Logger.Log(condition, reportText);
            return condition;
        }

        public bool VerifyElementIsPresent(string selector, string reportText = null)
        {
            var text = reportText ?? $"Verify ElementIsPresent for element with selector '{selector}'";

            var success = Locator.FindOrDefault(selector).IsPresent();

            Logger.Log(success, text);

            return success;
        }

        public bool AssertElementIsPresent(string selector, string reportText = null)
        {
            var text = reportText ?? $"Assert ElementIsPresent for element with selector '{selector}'";
            var success = VerifyElementIsPresent(selector, reportText);

            Logger.Log(success, text);

            return success;
        }

        public bool VerifyElementIsEnabled(string selector, string reportText = null)
        {
            var text = reportText ?? $"Verify ElementIsEnabled for element with selector '{selector}'";

            var success = Locator.FindOrDefault(selector).IsEnabled();

            Logger.Log(success, text);

            return success;
        }
        public bool VerifyElementHasClass(string selector, string className, string reportText = null)
        {
            var text = reportText ?? $"Verify ElementHasClass '{className}' set for element with selector '{selector}'";

            var success = Locator.FindOrDefault(selector).ContainsClass(className);

            Logger.Log(success, text);

            return success;
        }

        public bool AssertElementIsEnabled(string selector, string reportText = null)
        {
            var text = reportText ?? $"Assert ElementIsEnabled for element with selector '{selector}'";
            var success = VerifyElementIsEnabled(selector, reportText);

            Logger.Log(success, text);

            return success;
        }

        public bool VerifyElementIsVisible(string selector, string reportText = null)
        {
            var text = reportText ?? $"Verify ElementIsVisible for element with selector '{selector}'";

            var success = Locator.FindOrDefault(selector).IsVisible();

            Logger.Log(success, text);

            return success;
        }

        public bool VerifyElementIsNotVisible(string selector, string reportText = null)
        {
            var text = reportText ?? $"Verify ElementIsNotVisible for element with selector '{selector}'";

            var success = !Locator.FindOrDefault(selector).IsVisible();

            Logger.Log(success, text);

            return success;
        }

        public bool AssertElementIsVisible(string selector, string reportText = null)
        {
            var text = reportText ?? $"Assert ElementIsVisible for element with selector '{selector}'";
            var success = VerifyElementIsVisible(selector, reportText);

            Logger.Log(success, text);

            return success;
        }

        public bool VerifyOptionIsChecked(string selector, string reportText = null)
        {

            var text = reportText ?? $"Verify ElementIsVisible for element with selector '{selector}'";

            var att = Locator.FindOrDefault(selector).GetAttribute("CHECKED");

            var success = string.IsNullOrEmpty(att) || (att.ToLower() == "true" || att.ToLower() == "checked");

            Logger.Log(success, text);

            return success;

        }



        //public bool VerifyPageContainsText(string text)
        //{
        //  return _instance.IsTextPresent(text);
        //}

        //public bool VerifyAjaxRequestWasSend()
        //{
        //  string calls = _instance.GetEval("selenium.browserbot.getUserWindow().$.active");
        //  return calls == "1";
        //}

        //public bool VerifyNoAjaxRequestWasSend()
        //{
        //  string calls = _instance.GetEval("selenium.browserbot.getUserWindow().$.active");
        //  return calls == "0";
        //}

        #endregion

        public bool IsElementPresent(string selector)
        {
            return Locator.FindOrDefault(selector).IsPresent();
        }

        public ILocatedWebElement WaitFor(string selector, string textToContain = "", int seconds = -1)
        {
            if (seconds < 0)
            {
                seconds = WaitSeconds;
            }


            try
            {
                var wait = new WebDriverWait(_instance, new TimeSpan(0, 0, seconds));
                var by = ElementLocator.HowToGetElement(selector);
                var elem = wait.Until(ExpectedConditions.ElementIsVisible(by));
                if (string.IsNullOrEmpty(textToContain) || elem.Text.Contains(textToContain))
                {
                    return LocatedWebElement.CreateLocatedWebElement(elem, Locator, Logger, WaitSeconds);
                }
                return null;
            }
            catch (TimeoutException)
            {
                Trace.TraceWarning("Couldn't find element with selector '{0}' within timeout-span .", selector);
                return null;
            }

        }


        public ILocatedWebElement WaitForTextChange(string selector, string message = null, int seconds = -1)
        {
            var wait = new WebDriverWait(_instance, new TimeSpan(0, 0, seconds));
            var by = ElementLocator.HowToGetElement(selector);
            var elem = wait.Until(ExpectedConditions.ElementIsVisible(by));
            var text = elem.Text;
            var oldText = text;
            var waitSum = 0;

            while (oldText == text && waitSum < WaitSeconds * 1000)
            {
                Thread.Sleep(100);
                waitSum += 100;
                elem = wait.Until(ExpectedConditions.ElementIsVisible(by));
                text = elem.Text;
            }
            return LocatedWebElement.CreateLocatedWebElement(elem, Locator, Logger, WaitSeconds);
        }

        /// <summary>
        /// explizites Warten 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public WebDriverFixture AndWait(int seconds)
        {
            Thread.Sleep(seconds * 1000);
            return this;
        }

    }

    public interface ILocatedWebElementFactory
    {
        LocatedWebElement CreateLocatedWebElement(IWebElement element);
    }
}