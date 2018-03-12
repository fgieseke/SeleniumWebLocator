using System.Collections.Generic;
using OpenQA.Selenium;

namespace SeleniumWebLocator.Base
{
    public interface ILocatedWebElement : IWebElement
    {
        ElementLocator Locator { get; }
        IReportLogger Logger { get; }
        IWebElement Element { get; }
        string TestId { get; set; }
        /// <summary>
        /// Finds an Element relative to this.
        /// </summary>
        /// 
        /// 
        /// <param name="selector"></param>
        /// <returns></returns>
        ILocatedWebElement FindElement(string selector);
        IEnumerable<ILocatedWebElement> FindElements(string selector);

        /// <summary>
        /// Click the element. A screenshot is taken before the click.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        ILocatedWebElement Click(string message);

        /// <summary>
        /// Clicks on the element and waits for the amout of milliseconds and after that takes a screenshot
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="message"></param>
        ILocatedWebElement ClickAndWait(int milliseconds, string message = null);

        ILocatedWebElement ClickAndWaitFor(string selector, string message = null);

        /// <summary>
        /// Click on located Element , wait milliseconds an then wait for element in selector to be present.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="selector"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        ILocatedWebElement ClickAndWaitFor(int milliseconds, string selector, string message = null);

        /// <summary>
        /// Performs a double click on the located element
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        ILocatedWebElement DoubleClick(string message);

        /// <summary>
        /// Performs a double click on the located element and waits for the element with selector to be visible
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        ILocatedWebElement DoubleClickAndWaitFor(string selector, string message = null);

        /// <summary>
        /// Performs a double click on the located element and waits for the element with selector to change its text
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        ILocatedWebElement DoubleClickAndWaitForTextChange(string selector, string message = null);

        /// <summary>
        /// Waits for the selector to return an Element using the searchscope of the current located element.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        ILocatedWebElement WaitFor(string selector, string message = null);

        ILocatedWebElement MouseOver(string message = null, int waitMilliseconds = 0);

        ILocatedWebElement SetValue(string value);
        ILocatedWebElement ScrollTo();
        ILocatedWebElement ClickAndWaitForTextChange(string selector, string message = null);
    }
}