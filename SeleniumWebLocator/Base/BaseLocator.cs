namespace SeleniumWebLocator.Base
{
    public class BaseLocator
    {
        protected readonly ElementLocator Locator;

        public BaseLocator(ElementLocator locator)
        {
            Locator = locator;
        }
    }


}