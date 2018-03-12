
namespace SeleniumWebLocator.Base
{
	public class ClickLocator : BaseLocator
	{

		public ClickLocator(ElementLocator locator)
            : base(locator)
		{
		}

		public ILocatedWebElement OnElement(string selector)
		{
			var elem = Locator.Find(selector);
		    elem?.Click();
		    return elem;
		}

	}


}