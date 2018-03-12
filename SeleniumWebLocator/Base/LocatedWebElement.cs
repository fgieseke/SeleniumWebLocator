using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Selenium.WebDriver.WaitExtensions;

namespace SeleniumWebLocator.Base
{
    public class LocatedWebElement : ILocatedWebElement
    {
        private readonly int _defaultTimeoutInSeconds;

        private LocatedWebElement(IWebElement element, ElementLocator locator, IReportLogger logger, int defaultTimeoutInSeconds = 4)
        {
            _defaultTimeoutInSeconds = defaultTimeoutInSeconds;
            Element = element;
            Locator = locator;
            Logger = logger;
            TestId = Element.GetTestId();
        }

        public static LocatedWebElement CreateLocatedWebElement(IWebElement element, ElementLocator locator, IReportLogger logger, int defaultTimeoutInSeconds = 4)
        {
            return new LocatedWebElement(element, locator, logger, defaultTimeoutInSeconds);
        }

        #region Implementation of ILocatedWebElement

        public IWebElement Element { get; private set; }
        public string TestId { get; set; }
        public ILocatedWebElement FindElement(string selector)
        {
            return Locator.Find(selector, this);
        }

        public IEnumerable<ILocatedWebElement> FindElements(string selector)
        {
            return Locator.FindAll(selector, this);
        }

        #region clicks
        public ILocatedWebElement Click(string message)
        {
            var msg = message ?? $"Clicked on element '{TestId}'.";
            Logger.Log(ReportLogStatus.Info, msg);
            Logger.AddScreenShot("Before click.");
            Element.Click();
            return this;
        }

        public ILocatedWebElement DoubleClick(string message)
        {
            var msg = message ?? $"Double-Clicked on element '{TestId}'.";
            Logger.Log(ReportLogStatus.Info, msg);
            Logger.AddScreenShot("Before double click.");
            var actionProvider = new Actions(Locator.Driver);
            actionProvider.MoveToElement(Element);

            actionProvider.DoubleClick(Element).Build().Perform();
            return this;
        }

        public ILocatedWebElement ClickAndWait(int milliseconds, string message = null)
        {
            Click(message);
            Thread.Sleep(milliseconds);
            Logger.AddScreenShot("After click and wait.");
            return this;
        }

        public ILocatedWebElement ClickAndWaitFor(string selector, string message = null)
        {

            try
            {
                Click(message);
                var wait = new WebDriverWait(Locator.Driver, new TimeSpan(0, 0, _defaultTimeoutInSeconds));
                var by = ElementLocator.HowToGetElement(selector);
                var elem = wait.Until(ExpectedConditions.ElementExists(by));
                Logger.AddScreenShot("After waiting for element.");
                return new LocatedWebElement(elem, Locator, Logger);
            }
            catch (TimeoutException)
            {
                Logger.Log(ReportLogStatus.Warning, $"Couldn't find element with selector '{selector}' within timeout-span of {_defaultTimeoutInSeconds} sec.");
                return null;
            }
        }
        public ILocatedWebElement DoubleClickAndWaitFor(string selector, string message = null)
        {

            try
            {
                DoubleClick(message);
                
                var wait = new WebDriverWait(Locator.Driver, new TimeSpan(0, 0, _defaultTimeoutInSeconds));
                var by = ElementLocator.HowToGetElement(selector);
                var elem = wait.Until(ExpectedConditions.ElementExists(by));
                Logger.AddScreenShot("After waiting for element.");
                return new LocatedWebElement(elem, Locator, Logger);
            }
            catch (TimeoutException)
            {
                Logger.Log(ReportLogStatus.Warning, $"Couldn't find element with selector '{selector}' within timeout-span of {_defaultTimeoutInSeconds} sec.");
                return null;
            }
        }

        public ILocatedWebElement DoubleClickAndWaitForTextChange(string selector, string message = null)
        {
            var wait = new WebDriverWait(Locator.Driver, new TimeSpan(0, 0, _defaultTimeoutInSeconds));
            var by = ElementLocator.HowToGetElement(selector);
            var elem = wait.Until(ExpectedConditions.ElementExists(by));
            var text = elem.Text;
            var oldText = text;
            var waitSum = 0;

            DoubleClick(message);

            while (oldText == text && waitSum < _defaultTimeoutInSeconds * 1000)
            {
                Thread.Sleep(100);
                waitSum += 100;
                elem = wait.Until(ExpectedConditions.ElementExists(by));
                text = elem.Text;
            }
            return new LocatedWebElement(elem, Locator, Logger, _defaultTimeoutInSeconds);
        }

        public ILocatedWebElement ClickAndWaitFor(int milliseconds, string selector, string message = null)
        {

            try
            {
                Click(message);
                Locator.Driver.Wait(milliseconds);
                var wait = new WebDriverWait(Locator.Driver, new TimeSpan(0, 0, _defaultTimeoutInSeconds));
                var by = ElementLocator.HowToGetElement(selector);
                var elem = wait.Until(ExpectedConditions.ElementExists(by));
                Logger.AddScreenShot("After click.");
                return new LocatedWebElement(elem, Locator, Logger);
            }
            catch (TimeoutException)
            {
                Logger.Log(ReportLogStatus.Warning, $"Couldn't find element with selector '{selector}' within timeout-span of {_defaultTimeoutInSeconds} sec.");
                return null;
            }
        }

        #endregion

        public ILocatedWebElement MouseOver(string message = null, int waitMilliseconds = 0)
        {
            var msg = message ?? $"Mouse over element '{TestId}'.";

            var actions = new Actions(Locator.Driver);
            actions.MoveToElement(Element).Build().Perform();
            if (waitMilliseconds > 0)
            {
                Thread.Sleep(waitMilliseconds);
            }

            Logger.AddScreenShot(msg);
            return this;
        }

        public ILocatedWebElement SetValue(string value)
        {
            Element.SendKeys(value);
            return this;
        }

        public ILocatedWebElement ScrollTo()
        {
            var element = Element;
            var actions = new Actions(Locator.Driver);
            actions.MoveToElement(element);
            actions.Perform();
            return this;
        }

        public ILocatedWebElement ClickAndWaitForTextChange(string selector, string message = null)
        {
            var wait = new WebDriverWait(Locator.Driver, new TimeSpan(0, 0, _defaultTimeoutInSeconds));
            var by = ElementLocator.HowToGetElement(selector);
            var elem = wait.Until(ExpectedConditions.ElementExists(by));
            var text = elem.Text;
            var oldText = text;
            var waitSum = 0;
            Click(message);
            while (oldText == text && waitSum < _defaultTimeoutInSeconds * 1000)
            {
                Thread.Sleep(100);
                waitSum += 100;
                elem = wait.Until(ExpectedConditions.ElementExists(by));
                text = elem.Text;
            }
            return new LocatedWebElement(elem, Locator, Logger, _defaultTimeoutInSeconds);
        }

        public ILocatedWebElement WaitFor(string selector, string message = null)
        {

            try
            {
                var wait = new WebDriverWait(Locator.Driver, new TimeSpan(0, 0, _defaultTimeoutInSeconds));
                var by = ElementLocator.HowToGetElement(selector);
                var elem = wait.Until(driver => Element.FindElement(by));
                return new LocatedWebElement(elem, Locator, Logger);
            }
            catch (TimeoutException)
            {
                Logger.Log(ReportLogStatus.Warning, $"Couldn't find element with selector '{selector}' within timeout-span of {_defaultTimeoutInSeconds} sec.");
                return null;
            }
        }

        public ElementLocator Locator { get; private set; }
        public IReportLogger Logger { get; private set; }

        #endregion

        #region Implementation of ISearchContext

        /// <summary>
        /// Finds the first <see cref="T:OpenQA.Selenium.IWebElement"/> using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>
        /// The first matching <see cref="T:OpenQA.Selenium.IWebElement"/> on the current context.
        /// </returns>
        /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
        public IWebElement FindElement(By by)
        {
            return Element.FindElement(by);
        }

        /// <summary>
        /// Finds all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current context
        ///             using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>
        /// A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1"/> of all <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
        ///             matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Element.FindElements(by);
        }

        #endregion

        #region Implementation of IWebElement

        /// <summary>
        /// Clears the content of this element.
        /// </summary>
        /// <remarks>
        /// If this element is a text entry element, the <see cref="M:OpenQA.Selenium.IWebElement.Clear"/>
        ///             method will clear the value. It has no effect on other elements. Text entry elements
        ///             are defined as elements with INPUT or TEXTAREA tags.
        /// </remarks>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public void Clear()
        {
            Element.Clear();
        }

        /// <summary>
        /// Simulates typing text into the element.
        /// </summary>
        /// <param name="text">The text to type into the element.</param>
        /// <remarks>
        /// The text to be typed may include special characters like arrow keys,
        ///             backspaces, function keys, and so on. Valid special keys are defined in
        ///             <see cref="T:OpenQA.Selenium.Keys"/>.
        /// </remarks>
        /// <seealso cref="T:OpenQA.Selenium.Keys"/><exception cref="T:OpenQA.Selenium.InvalidElementStateException">Thrown when the target element is not enabled.</exception><exception cref="T:OpenQA.Selenium.ElementNotVisibleException">Thrown when the target element is not visible.</exception><exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public void SendKeys(string text)
        {
            Element.SendKeys(text);
        }

        /// <summary>
        /// Submits this element to the web server.
        /// </summary>
        /// <remarks>
        /// If this current element is a form, or an element within a form,
        ///             then this will be submitted to the web server. If this causes the current
        ///             page to change, then this method will block until the new page is loaded.
        /// </remarks>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public void Submit()
        {
            Element.Submit();
        }

        /// <summary>
        /// Clicks this element.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Click this element. If the click causes a new page to load, the <see cref="M:OpenQA.Selenium.IWebElement.Click"/>
        ///             method will attempt to block until the page has loaded. After calling the
        ///             <see cref="M:OpenQA.Selenium.IWebElement.Click"/> method, you should discard all references to this
        ///             element unless you know that the element and the page will still be present.
        ///             Otherwise, any further operations performed on this element will have an undefined.
        ///             behavior.
        /// </para>
        /// <para>
        /// If this element is not clickable, then this operation is ignored. This allows you to
        ///             simulate a users to accidentally missing the target when clicking.
        /// </para>
        /// </remarks>
        /// <exception cref="T:OpenQA.Selenium.ElementNotVisibleException">Thrown when the target element is not visible.</exception><exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public void Click()
        {
            Click($"Clicked on element '{TestId}'...");
        }

        /// <summary>
        /// Gets the value of the specified attribute for this element.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>
        /// The attribute's current value. Returns a <see langword="null"/> if the
        ///             value is not set.
        /// </returns>
        /// <remarks>
        /// The <see cref="M:OpenQA.Selenium.IWebElement.GetAttribute(System.String)"/> method will return the current value
        ///             of the attribute, even if the value has been modified after the page has been
        ///             loaded. Note that the value of the following attributes will be returned even if
        ///             there is no explicit attribute on the element:
        /// <list type="table">
        /// <listheader>
        /// <term>Attribute name</term><term>Value returned if not explicitly specified</term><term>Valid element types</term>
        /// </listheader>
        /// <item>
        /// <description>checked</description><description>checked</description><description>Check Box</description>
        /// </item>
        /// <item>
        /// <description>selected</description><description>selected</description><description>Options in Select elements</description>
        /// </item>
        /// <item>
        /// <description>disabled</description><description>disabled</description><description>Input and other UI elements</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public string GetAttribute(string attributeName)
        {
            return Element.GetAttribute(attributeName);
        }

        /// <summary>
        /// Gets the value of a CSS property of this element.
        /// </summary>
        /// <param name="propertyName">The name of the CSS property to get the value of.</param>
        /// <returns>
        /// The value of the specified CSS property.
        /// </returns>
        /// <remarks>
        /// The value returned by the <see cref="M:OpenQA.Selenium.IWebElement.GetCssValue(System.String)"/>
        ///             method is likely to be unpredictable in a cross-browser environment.
        ///             Color values should be returned as hex strings. For example, a
        ///             "background-color" property set as "green" in the HTML source, will
        ///             return "#008000" for its value.
        /// </remarks>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public string GetCssValue(string propertyName)
        {
            return Element.GetCssValue(propertyName);
        }

        /// <summary>
        /// Gets the tag name of this element.
        /// </summary>
        /// <remarks>
        /// The <see cref="P:OpenQA.Selenium.IWebElement.TagName"/> property returns the tag name of the
        ///             element, not the value of the name attribute. For example, it will return
        ///             "input" for an element specified by the HTML markup &lt;input name="foo" /&gt;.
        /// </remarks>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public string TagName => Element.TagName;

        /// <summary>
        /// Gets the innerText of this element, without any leading or trailing whitespace,
        ///             and with other whitespace collapsed.
        /// </summary>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public string Text => Element.Text;

        /// <summary>
        /// Gets a value indicating whether or not this element is enabled.
        /// </summary>
        /// <remarks>
        /// The <see cref="P:OpenQA.Selenium.IWebElement.Enabled"/> property will generally
        ///             return <see langword="true"/> for everything except explicitly disabled input elements.
        /// </remarks>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public bool Enabled => Element.Enabled;

        /// <summary>
        /// Gets a value indicating whether or not this element is selected.
        /// </summary>
        /// <remarks>
        /// This operation only applies to input elements such as checkboxes,
        ///             options in a select element and radio buttons.
        /// </remarks>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public bool Selected => Element.Selected;

        /// <summary>
        /// Gets a <see cref="T:System.Drawing.Point"/> object containing the coordinates of the upper-left corner
        ///             of this element relative to the upper-left corner of the page.
        /// </summary>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public Point Location => Element.Location;

        /// <summary>
        /// Gets a <see cref="P:OpenQA.Selenium.IWebElement.Size"/> object containing the height and width of this element.
        /// </summary>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public Size Size => Element.Size;

        /// <summary>
        /// Gets a value indicating whether or not this element is displayed.
        /// </summary>
        /// <remarks>
        /// The <see cref="P:OpenQA.Selenium.IWebElement.Displayed"/> property avoids the problem
        ///             of having to parse an element's "style" attribute to determine
        ///             visibility of an element.
        /// </remarks>
        /// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public bool Displayed => Element.Displayed;

        #endregion

    }



}