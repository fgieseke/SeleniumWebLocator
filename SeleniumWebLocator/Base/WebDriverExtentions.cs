using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebLocator.Base
{
    public static class WebDriverExtentions
    {
        public static IWebElement WaitFor(this IWebDriver driver, string selector, int waitSeconds = 4)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, waitSeconds));
            var by = ElementLocator.HowToGetElement(selector);
            var elem = wait.Until(ExpectedConditions.ElementExists(by));
            return elem;


        }
        public static IWebElement WaitForIsVisible(this IWebDriver driver, string selector, int waitSeconds = 4)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, waitSeconds));
            var by = ElementLocator.HowToGetElement(selector);
            var elem = wait.Until(ExpectedConditions.ElementIsVisible(by));
            return elem;
        }

        public static IWebElement WaitForText(this IWebDriver driver, string selector, string text, int waitSeconds = 4)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, waitSeconds));
            var by = ElementLocator.HowToGetElement(selector);
            var elem = wait.Until(ExpectedConditions.ElementIsVisible(by));
            wait.Until(ExpectedConditions.TextToBePresentInElement(elem, text));
            return elem;
        }

        public static IWebElement WaitForIsClickable(this IWebDriver driver, string selector, int waitSeconds = 4)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, waitSeconds));
            var by = ElementLocator.HowToGetElement(selector);
            var elem = wait.Until(ExpectedConditions.ElementToBeClickable(by));
            return elem;
        }
    }
}
