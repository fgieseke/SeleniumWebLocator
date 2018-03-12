using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Moq;
using OpenQA.Selenium;
using By = Selenium.WebDriver.Extensions.By;

namespace SeleniumWebLocator.Base
{
    public class ElementLocator
    {
        private static IWebDriver _webDriver;
        private readonly ILocatedWebElementFactory _factory;

        public ElementLocator(IWebDriver webDriver, ILocatedWebElementFactory factory)
        {
            _webDriver = webDriver;
            _factory = factory;
        }

        public IWebDriver Driver => _webDriver;

        public ILocatedWebElement CreateLocatedWebElement(IWebElement webElement)
        {
            return _factory.CreateLocatedWebElement(webElement);
        }

        /// <summary>
        /// Finds an Element by selectors: 
        /// default = id , 
        /// // = xpath, 
        /// # = id, 
        /// css: = css, 
        /// . = single class, 
        /// tag: = tagname,  
        /// text: = text, 
        /// $: = Jquery 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static OpenQA.Selenium.By HowToGetElement(string selector)
        {
            var howToFind = By.Id(selector);
            if (selector.StartsWith(@"//"))
                howToFind = By.XPath(selector);
            else if (selector.StartsWith("#"))
                howToFind = By.Id(selector.Substring(1));
            else if (selector.StartsWith("css:"))
                howToFind = By.CssSelector(selector.Substring(4));
            else if (selector.StartsWith("."))
                howToFind = By.ClassName(selector.Substring(1));
            else if (selector.StartsWith("tag:"))
                howToFind = By.TagName(selector.Substring(4));
            else if (selector.StartsWith("name:"))
                howToFind = By.Name(selector.Substring(5));
            else if (selector.StartsWith("text:"))
                howToFind = By.JQuerySelector($":contains('{selector.Substring(5)}')");
            else if (selector.StartsWith("$:"))
                howToFind = By.JQuerySelector(selector.Substring(2));
            return howToFind;
        }

        private ILocatedWebElement GetWebElement(string[] locators, string ident)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var locator in locators)
            {
                var locatorIdent = string.Format(locator, ident);
                var elem = Find(locatorIdent);
                if (elem != null)
                    return elem;
            }

            return null;
        }


        /// <summary>
        /// Finds an Element by selectors: 
        /// default = id , 
        /// // = xpath, 
        /// # = id, 
        /// css: = css, 
        /// . = single class, 
        /// tag: = tagname,  
        /// text: = text, 
        /// $: = Jquery 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public ILocatedWebElement FindOrDefault(string selector, ISearchContext root = null)
        {
            var by = HowToGetElement(selector);
            var elem = FindBy(by, root);

            return elem ?? CreateDummyElement(_factory);
        }

        /// <summary>
        /// Finds an Element by selectors: 
        /// default = id , 
        /// // = xpath, 
        /// # = id, 
        /// css: = css, 
        /// . = single class, 
        /// tag: = tagname,  
        /// text: = text, 
        /// $: = Jquery 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public ILocatedWebElement Find(string selector, ISearchContext root = null)
        {
            var by = HowToGetElement(selector);
            return FindBy(by, root);
        }

        /// <summary>
        /// Finds an Element by selectors: 
        /// default = id , 
        /// // = xpath, 
        /// # = id, 
        /// css: = css, 
        /// . = single class, 
        /// tag: = tagname,  
        /// text: = text, 
        /// $: = Jquery 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public IEnumerable<ILocatedWebElement> FindAll(string selector, ISearchContext root = null)
        {
            var by = HowToGetElement(selector);
            return FindAllBy(by, root);
        }

        private ILocatedWebElement FindBy(OpenQA.Selenium.By bySelector, ISearchContext root = null, bool throwsException = false)
        {
            try
            {

                if (root == null)
                    root = _webDriver;

                var elem = root.FindElement(bySelector);

                return _factory.CreateLocatedWebElement(elem);
            }
            catch (Exception ex)
            {
                Trace.TraceWarning($"Couldn't find element with selector '{bySelector}': {ex.Message}");
                if (!throwsException)
                    return null;

                throw;
            }
        }

        private IEnumerable<ILocatedWebElement> FindAllBy(OpenQA.Selenium.By bySelector, ISearchContext root = null)
        {
            try
            {
                if (root == null)
                    root = _webDriver;

                var elements = root.FindElements(bySelector);

                var locatedWebElements = elements.Select(webElement => _factory.CreateLocatedWebElement(webElement)).Cast<ILocatedWebElement>().ToList();

                return locatedWebElements;
            }
            catch (Exception)
            {
                Trace.TraceWarning("Couldn't find elements with selector '{0}' .", bySelector);
                return new List<ILocatedWebElement>();
            }
        }


        private static ILocatedWebElement CreateDummyElement(ILocatedWebElementFactory factory)
        {
            var mockElem = new Mock<IWebElement>();
            mockElem.Setup(m => m.Displayed).Returns(false);
            mockElem.Setup(m => m.Enabled).Returns(false);
            mockElem.Setup(m => m.FindElement(It.IsAny<By>())).Returns(mockElem.Object);
            mockElem.Setup(m => m.FindElements(It.IsAny<By>())).Returns(new ReadOnlyCollection<IWebElement>(new List<IWebElement>()));
            mockElem.Setup(m => m.GetAttribute(It.IsAny<string>())).Returns("");
            mockElem.Setup(m => m.GetCssValue(It.IsAny<string>())).Returns("");
            mockElem.Setup(m => m.Location).Returns(new Point(0, 0));
            mockElem.Setup(m => m.Selected).Returns(false);
            mockElem.Setup(m => m.Size).Returns(new Size(0, 0));
            mockElem.Setup(m => m.Text).Returns("DUMMY");
            mockElem.Setup(m => m.TagName).Returns("DUMMY");
            mockElem.Setup(m => m.Click());

            return factory.CreateLocatedWebElement(mockElem.Object);
        }

    }
}