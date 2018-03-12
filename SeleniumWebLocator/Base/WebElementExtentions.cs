using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace SeleniumWebLocator.Base
{
    public static class WebElementExtentions
    {
        #region Verify
        public static bool HasClass(this IWebElement webElem, string className)
        {
            var classNameElem = webElem.GetAttribute("class");
            var expectation = classNameElem != null && classNameElem == className.Replace(".", "");
            //expectation.Should().BeTrue("Element '{0}' doesn't have class '{0}' set", webElem.GetTestId(), className);
            return expectation;

        }

        public static bool NotHasClass(this IWebElement webElem, string className)
        {
            return !webElem.HasClass(className);

        }

        public static bool ContainsClass(this IWebElement webElem, string className)
        {
            var classNameElem = webElem.GetAttribute("class");
            var expectation = classNameElem != null && classNameElem.Contains(className.Replace(".", ""));
            //expectation.Should().BeTrue("Element '{0}' doesn't contain class '{0}'", webElem.GetTestId(), className);
            return expectation;

        }

        public static bool NotContainsClass(this IWebElement webElem, string className)
        {
            return !webElem.ContainsClass(className);

        }

        public static bool ContainsText(this IWebElement webElem, string text)
        {
            var textElem = webElem.Text;
            var expectation = textElem != null && textElem.Contains(text);
            return expectation;
        }

        public static bool NotContainsText(this IWebElement webElem, string text)
        {
            return !webElem.ContainsText(text);
        }


        public static bool IsPresent(this IWebElement webElem)
        {
            var expectation = webElem != null && webElem.Text != "DUMMY";
            return expectation;
        }

        public static void IsPresent(this IWebElement webElem, string msg)
        {
            if (webElem == null || !webElem.IsPresent())
                throw new AssertFailedException(msg);

        }

        public static bool IsVisible(this IWebElement webElem)
        {
            var expectation = webElem.IsPresent() && webElem.Displayed;
            return expectation;
        }

        public static bool IsEnabled(this IWebElement webElem)
        {
            var expectation = webElem.IsPresent() && webElem.Enabled;
            return expectation;
        }
        public static bool NotIsEnabled(this IWebElement webElem)
        {
            return !webElem.IsEnabled();
        }

        public static bool NotIsPresent(this IWebElement webElem)
        {
            var expectation = webElem == null || webElem.Text != "DUMMY";
            return expectation;
        }

        public static bool NotIsVisible(this IWebElement webElem)
        {
            var expectation = webElem.IsPresent() && webElem.Displayed;
            return !expectation;
        }


        #endregion

        public static IWebElement Parent(this IWebElement webElem, string selector)
        {
            var by = By.XPath($"parent::{selector}");
            return webElem.FindElement(by);
        }


        public static string GetTestId(this IWebElement webElem)
        {
            var testId = webElem.GetAttribute("id");
            if (string.IsNullOrEmpty(testId))
                return testId;

            testId = webElem.GetAttribute("name");
            if (!string.IsNullOrEmpty(testId))
                return testId;

            testId = webElem.GetAttribute("data-testid");
            if (!string.IsNullOrEmpty(testId))
                return testId;

            testId = webElem.GetAttribute("title");
            if (!string.IsNullOrEmpty(testId))
                return testId;

            testId = webElem.GetAttribute("class");
            if (!string.IsNullOrEmpty(testId))
                return testId;

            testId = webElem.Text;
            if (!string.IsNullOrEmpty(testId))
                return testId.PadRight(20, ' ').Substring(0, 20);

            return "unknown";
        }

        /// <summary>
        /// Click uf Element und explizites Warten 
        /// </summary>
        /// <param name="webElem"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static IWebElement AndWait(this IWebElement webElem, int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return webElem;
        }

    }
}