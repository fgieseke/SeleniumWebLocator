using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebLocator.Base
{
    public static class LocatedWebElementExtentions
    {


        public static bool HasClass(this ILocatedWebElement webElem, string className)
        {
            return webElem.Element.HasClass(className);
        }

        public static bool IsVisible(this ILocatedWebElement webElem)
        {
            return webElem.Element.IsVisible();
        }

        public static bool IsPresent(this ILocatedWebElement webElem)
        {
            return webElem.Element.IsPresent();
        }

        public static bool ContainsClass(this ILocatedWebElement webElem, string className)
        {
            return webElem.Element.ContainsClass(className);
        }

        public static bool ContainsText(this ILocatedWebElement webElem, string text)
        {
            return webElem.Element.ContainsText(text);
        }


        public static bool HasText(this ILocatedWebElement webElem, string text = null)
        {
            var expectation = !string.IsNullOrEmpty(webElem.Element.Text);
            if (text != null)
            {
                expectation = !string.IsNullOrEmpty(webElem.Element.Text) && webElem.Element.Text == text;
            }

            return expectation;
        }

        #region Verify
        public static ILocatedWebElement VerifyHasClass(this ILocatedWebElement webElem, string className, string message = null)
        {
            var expectation = webElem != null && webElem.Element.HasClass(className);
            var msg = message ?? $"Verify element '{webElem?.TestId}' has class '{className}' set";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }

        public static ILocatedWebElement VerifyNotHasClass(this ILocatedWebElement webElem, string className, string message = null)
        {

            var expectation = webElem != null && webElem.Element.NotHasClass(className);
            var msg = message ?? $"Verify element '{webElem?.TestId}' doesn't have class '{className}' set";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }


        public static ILocatedWebElement VerifyContainsText(this ILocatedWebElement webElem, string text, string message = null)
        {
            var success = webElem != null && webElem.ContainsText(text);
            var msg = message ?? $"Verify element '{webElem?.TestId}' contains text '{text}'";
            webElem?.Logger.Log(success, msg);
            return webElem;
        }

        public static ILocatedWebElement VerifyHasText(this ILocatedWebElement webElem, string text = null, string message = null)
        {
            var success = webElem != null && webElem.HasText(text);
            var msg = message ?? $"Verify element '{webElem?.TestId}' has text '{text}'";
            webElem?.Logger.Log(success, msg);

            return webElem;
        }

        public static ILocatedWebElement VerifyNotContainsText(this ILocatedWebElement webElem, string text, string message = null)
        {
            var expectation = webElem != null && webElem.Element.NotContainsText(text);
            var msg = message ?? $"Verify element '{webElem?.TestId}' doesn't contain text '{text}'.";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }

        public static ILocatedWebElement VerifyContainsClass(this ILocatedWebElement webElem, string className, string message = null)
        {
            var expectation = webElem != null && webElem.Element.ContainsClass(className);
            var msg = message ?? $"Verify element '{webElem?.TestId}' contains class '{className}'";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }

        public static ILocatedWebElement VerifyNotContainsClass(this ILocatedWebElement webElem, string className, string message = null)
        {
            var expectation = webElem != null && webElem.Element.NotContainsClass(className);
            var msg = message ?? $"Verify element '{webElem?.TestId}' doesn't contain class '{className}'.";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }

        public static ILocatedWebElement VerifyIsPresent(this ILocatedWebElement webElem, string message = null)
        {
            var expectation = webElem != null && webElem.Element.IsPresent();
            var msg = message ?? $"Verify element '{webElem?.TestId}' is present";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }

        /// <summary>
        /// Asserts Element is present and takes a screenshot of the loaded page 
        /// </summary>
        /// <param name="webElem"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool AssertIsPresent(this ILocatedWebElement webElem, string message = null)
        {
            var msg = message ?? $"Assert element '{webElem?.TestId}' is present";
            if (webElem == null || !webElem.Element.IsPresent())
                throw new AssertFailedException(msg);

            webElem.Logger.AddScreenShot(msg);
            return true;
        }

        public static ILocatedWebElement VerifyIsEnabled(this ILocatedWebElement webElem, string message = null)
        {
            var expectation = webElem != null && webElem.Element.IsEnabled();
            var msg = message ?? $"Verify element '{webElem?.TestId}' is enabled";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }

        public static ILocatedWebElement VerifyIsNotEnabled(this ILocatedWebElement webElem, string message = null)
        {
            var expectation = webElem != null && webElem.Element.NotIsEnabled();
            var msg = message ?? $"Verify element '{webElem?.TestId}' is disabled";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }


        public static ILocatedWebElement VerifyIsVisible(this ILocatedWebElement webElem, string message = null)
        {

            var expectation = webElem != null && webElem.Element.IsVisible();
            var msg = message ?? $"Verify element '{webElem?.TestId}' is visible";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }

        public static ILocatedWebElement VerifyIsNotVisible(this ILocatedWebElement webElem, string message = null)
        {
            var expectation = webElem == null || webElem.Element.NotIsVisible();
            var msg = message ?? $"Verify element '{webElem?.TestId}' is not visible.";
            webElem?.Logger.Log(expectation, msg);
            return webElem;
        }

        //public static bool VerifyIsEnabled(this ILocatedWebElement webElem)
        //{
        //	var expectation = webElem.VerifyIsPresent() && webElem.Enabled;
        //	//expectation.Should().BeTrue("Element '{0}' not enabled", webElem.GetTestId());
        //	return expectation;
        //}
        //public static bool VerifyIsNotEnabled(this ILocatedWebElement webElem)
        //{
        //	return !webElem.VerifyIsEnabled();
        //}

        //public static bool VerifyIsNotPresent(this ILocatedWebElement webElem)
        //{
        //	var expectation = webElem == null || webElem.Text != "DUMMY";
        //	//expectation.Should().BeTrue("Element is present");
        //	return expectation;
        //}




        #endregion

        public static SelectElement AsSelectBox(this ILocatedWebElement element)
        {
            return new SelectElement(element);
        }



    }
}